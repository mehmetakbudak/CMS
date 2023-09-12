using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Consts;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using CMS.Storage.Model;
using CMS.Service.Exceptions;
using CMS.Service.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CMS.Service
{
    public interface ICommentService
    {
        Task<List<CommentModel>> Get();
        Task<List<CommentGetModel>> GetSourceComments(SourceCommentModel model, int? parentId = null, List<CommentGetModel> children = null);
        Task<IQueryable<UserCommentModel>> GetUserComments(int? type = null);
        Task<List<CommentModel>> GetAllByStatus(int status);
        Task<CommentDetailModel> GetDetail(int id);
        Task<ServiceResult> Post(CommentPostModel model);
        Task<ServiceResult> Put(CommentPutModel model);
        Task<ServiceResult> Delete(int id, int? userId= null);
    }

    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;
        private readonly IHttpContextAccessor _httpContext;

        public CommentService(
            IUnitOfWork<CMSContext> unitOfWork,
            IHttpContextAccessor httpContext)
        {
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
        }

        public async Task<List<CommentGetModel>> GetSourceComments(SourceCommentModel model, int? parentId = null, List<CommentGetModel> children = null)
        {
            List<CommentGetModel> comments = new List<CommentGetModel>();

            var list = await _unitOfWork.Repository<Comment>()
                .Where(x => x.SourceType == model.SourceType && !x.Deleted && x.SourceId == model.SourceId && x.Status == CommentStatus.Approved && x.ParentId == parentId)
                .Include(x => x.User)
                .OrderByDescending(x => x.InsertedDate)
                .Select(x => new CommentGetModel()
                {
                    Key = x.Id,
                    Id = x.Id,
                    Description = x.Description,
                    SourceId = x.SourceId,
                    ParentId = x.ParentId,
                    SourceType = x.SourceType,
                    UserFullName = x.User.FullName,
                    InsertedDate = x.InsertedDate
                }).ToListAsync();

            comments.AddRange(list);

            foreach (var comment in list)
            {
                comment.Items = await GetSourceComments(model, comment.Id, list);
            }

            return comments;
        }

        public async Task<IQueryable<UserCommentModel>> GetUserComments(int? type = null)
        {
            var loginUser = _httpContext.HttpContext.User.Parse();

            var data = _unitOfWork.Repository<Comment>()
               .Where(x => !x.Deleted && x.UserId == loginUser.UserId);

            if (type != null)
            {
                data = data.Where(x => x.SourceType == (SourceType)type);
            }
            var blogs = await _unitOfWork.Repository<Blog>()
                .Where(x => !x.Deleted && x.Published).ToListAsync();

            var list = data.OrderByDescending(x => x.InsertedDate)
                .AsEnumerable()
                .Select(x =>
                {
                    var blog = blogs.FirstOrDefault(b => b.Id == x.SourceId);
                    var model = new UserCommentModel()
                    {
                        Description = x.Description,
                        Id = x.Id,
                        InsertedDate = x.InsertedDate,
                        UpdatedDate = x.UpdatedDate,
                        SourceId = x.SourceId,
                        Url = x.SourceType == SourceType.Blog ? $"blog/{blog.Url}/{x.SourceId}" : null,
                        Title = x.SourceType == SourceType.Blog ? blogs.Where(b => b.Id == x.SourceId).Select(b => b.Title).FirstOrDefault() : null,
                        SourceTypeName = EnumHelper.GetDescription(x.SourceType),
                        Status = EnumHelper.GetDescription(x.Status)
                    };
                    return model;
                }).AsQueryable();

            return list;
        }

        public async Task<List<CommentModel>> GetAllByStatus(int status)
        {
            var list = await _unitOfWork.Repository<Comment>()
                .Where(x => !x.Deleted && x.Status == (CommentStatus)status)
                .OrderByDescending(x => x.InsertedDate)
                .Select(x => new CommentModel()
                {
                    Description = x.Description,
                    Id = x.Id,
                    SourceType = x.SourceType,
                    InsertedDate = x.InsertedDate,
                    ParentId = x.ParentId,
                    SourceId = x.SourceId,
                    Source = EnumHelper.GetDescription(x.SourceType),
                    Status = EnumHelper.GetDescription(x.Status),
                    UpdatedDate = x.UpdatedDate,
                    UserFullName = x.User.FullName
                }).ToListAsync();

            return list;
        }

        public async Task<CommentDetailModel> GetDetail(int id)
        {
            var comment = await _unitOfWork.Repository<Comment>()
                .FirstOrDefault(x => !x.Deleted && x.Id == id);

            if (comment == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            var model = new CommentDetailModel();

            if (comment.ParentId.HasValue)
            {
                var parentComment = await _unitOfWork.Repository<Comment>()
                    .Where(x => x.Id == comment.ParentId && !x.Deleted)
                    .Include(x => x.User).FirstOrDefaultAsync();

                if (parentComment != null)
                {
                    model.ParentId = parentComment.Id;
                    model.ParentDescription = parentComment.Description;
                }
            }

            model.CommentStatus = comment.Status;
            model.Description = comment.Description;
            model.UpdatedDate = comment.UpdatedDate;
            model.UserFullName = comment.User.FullName;
            model.Status = EnumHelper.GetDescription(comment.Status);
            model.InsertedDate = comment.InsertedDate;
            model.Source = EnumHelper.GetDescription(comment.SourceType);

            if (comment.SourceType == SourceType.Blog)
            {
                var blog = await _unitOfWork.Repository<Blog>()
                    .FirstOrDefault(x => x.Id == comment.SourceId && !x.Deleted);

                if (blog != null)
                {
                    model.SourceTitle = blog.Title;
                    model.SourceUrl = $"/blog/{blog.Url}/{blog.Id}";
                }
            }

            return model;
        }

        public async Task<ServiceResult> Post(CommentPostModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var loginUser = _httpContext.HttpContext.User.Parse();

            var entity = new Comment()
            {
                Deleted = false,
                Description = model.Description,
                InsertedDate = DateTime.Now,
                ParentId = model.ParentId,
                SourceType = model.SourceType,
                Status = CommentStatus.WaitingforApproval,
                UserId = loginUser.UserId,
                SourceId = model.SourceId
            };

            await _unitOfWork.Repository<Comment>().Add(entity);

            await _unitOfWork.Save();

            result.Message = "Yorumunuz başarıyla kaydedildi. Onay sürecinden sonra yayınlanacaktır.";

            return result;
        }

        public async Task<ServiceResult> Put(CommentPutModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Put };

            var comment = await _unitOfWork.Repository<Comment>()
                .FirstOrDefault(x => !x.Deleted && x.Id == model.Id);

            if (comment == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            comment.Status = model.CommentStatus;
            comment.UpdatedDate = DateTime.Now;

            await _unitOfWork.Save();

            return result;
        }

        public async Task<ServiceResult> Delete(int id, int? userId = null)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK, Message = AlertMessages.Delete };

            var query = _unitOfWork.Repository<Comment>()
                .Where(x => !x.Deleted && x.Id == id);

            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            var comment = query.FirstOrDefault();

            if (comment == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }

            comment.Deleted = true;
            await _unitOfWork.Save();

            return result;
        }

        public async Task<List<CommentModel>> Get()
        {
            return await _unitOfWork.Repository<Comment>().Where(x => !x.Deleted)
                 .Include(x => x.User)
                 .Select(x => new CommentModel()
                 {
                     Description = x.Description,
                     Id = x.Id,
                     StatusId = (int)x.Status,
                     SourceType = x.SourceType,
                     InsertedDate = x.InsertedDate,
                     ParentId = x.ParentId,
                     SourceId = x.SourceId,
                     Source = EnumHelper.GetDescription(x.SourceType),
                     Status = EnumHelper.GetDescription(x.Status),
                     UpdatedDate = x.UpdatedDate,
                     UserFullName = x.User.FullName
                 }).ToListAsync();
        }
    }
}
