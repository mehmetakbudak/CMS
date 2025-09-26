using CMS.Business.Exceptions;
using CMS.Business.Extensions;
using CMS.Business.Helper;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos;
using CMS.Storage.Dtos.Comment;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface ICommentService
    {
        IQueryable<CommentDto> Get();
        Task<List<CommentListDto>> GetSourceComments(SourceCommentDto dto, int? parentId = null, List<CommentListDto> children = null);
        Task<IQueryable<UserCommentDto>> GetUserComments(int? type = null);
        Task<List<CommentDto>> GetAllByStatus(int status);
        Task<CommentDetailDto> GetDetail(int id);
        Task<ServiceResult> Create(CommentCreateDto dto);
        Task<ServiceResult> Update(CommentUpdateDto dto);
        Task<ServiceResult> Delete(int id, int? userId = null);
    }

    public class CommentService(
            IUnitOfWork<CMSContext> unitOfWork,
            IHttpContextAccessor httpContext) : ICommentService
    {
        public IQueryable<CommentDto> Get()
        {
            return unitOfWork.Repository<Comment>().Where(x => !x.Deleted)
                 .Include(x => x.User)
                 .Select(x => new CommentDto()
                 {
                     Description = x.Description,
                     Id = x.Id,
                     StatusId = (int)x.Status,
                     SourceType = x.SourceType,
                     InsertedDate = x.InsertedDate,
                     ParentId = x.ParentId,
                     SourceId = x.SourceId,
                     Source = x.SourceType.GetDescription(),
                     Status = x.Status.GetDescription(),
                     UpdatedDate = x.UpdatedDate,
                     UserFullName = x.User.FullName
                 }).AsQueryable();
        }

        public async Task<List<CommentListDto>> GetSourceComments(SourceCommentDto dto, int? parentId = null, List<CommentListDto> children = null)
        {
            var comments = new List<CommentListDto>();

            var list = await unitOfWork.Repository<Comment>()
                .Where(x => x.SourceType == dto.SourceType && !x.Deleted && x.SourceId == dto.SourceId && x.Status == CommentStatus.Approved && x.ParentId == parentId)
                .Include(x => x.User)
                .OrderByDescending(x => x.InsertedDate)
                .Select(x => new CommentListDto()
                {
                    Id = x.Id,
                    Description = x.Description,
                    SourceId = x.SourceId,
                    ParentId = x.ParentId,
                    SourceType = x.SourceType,
                    UserFullName = x.User.FullName,
                    InsertedDate = x.InsertedDate,
                }).ToListAsync();

            comments.AddRange(list);

            foreach (var comment in list)
            {
                comment.Children = await GetSourceComments(dto, comment.Id, list);
            }

            return comments;
        }

        public async Task<IQueryable<UserCommentDto>> GetUserComments(int? type = null)
        {
            var loginUser = httpContext.HttpContext.User.Parse();

            var data = unitOfWork.Repository<Comment>()
               .Where(x => !x.Deleted && x.UserId == loginUser.UserId);

            if (type != null)
            {
                data = data.Where(x => x.SourceType == (SourceType)type);
            }
            var blogs = await unitOfWork.Repository<Blog>()
                .Where(x => !x.Deleted && x.Published).ToListAsync();

            var list = data.OrderByDescending(x => x.InsertedDate)
                .AsEnumerable()
                .Select(x =>
                {
                    var blog = blogs.FirstOrDefault(b => b.Id == x.SourceId);
                    var model = new UserCommentDto()
                    {
                        Description = x.Description,
                        Id = x.Id,
                        InsertedDate = x.InsertedDate,
                        UpdatedDate = x.UpdatedDate,
                        SourceId = x.SourceId,
                        Url = x.SourceType == SourceType.Blog ? $"blog/{blog.Url}/{x.SourceId}" : null,
                        Title = x.SourceType == SourceType.Blog ? blogs.Where(b => b.Id == x.SourceId).Select(b => b.Title).FirstOrDefault() : null,
                        SourceTypeName = x.SourceType.GetDescription(),
                        Status = x.Status.GetDescription()
                    };
                    return model;
                }).AsQueryable();

            return list;
        }

        public async Task<List<CommentDto>> GetAllByStatus(int status)
        {
            var list = await unitOfWork.Repository<Comment>()
                .Where(x => !x.Deleted && x.Status == (CommentStatus)status)
                .OrderByDescending(x => x.InsertedDate)
                .Select(x => new CommentDto()
                {
                    Description = x.Description,
                    Id = x.Id,
                    SourceType = x.SourceType,
                    InsertedDate = x.InsertedDate,
                    ParentId = x.ParentId,
                    SourceId = x.SourceId,
                    Source = x.SourceType.GetDescription(),
                    Status = x.Status.GetDescription(),
                    UpdatedDate = x.UpdatedDate,
                    UserFullName = x.User.FullName
                }).ToListAsync();

            return list;
        }

        public async Task<CommentDetailDto> GetDetail(int id)
        {
            var comment = await unitOfWork.Repository<Comment>()
                .FirstOrDefault(x => !x.Deleted && x.Id == id);

            if (comment is null)
                throw new NotFoundException();

            var model = new CommentDetailDto();

            if (comment.ParentId.HasValue)
            {
                var parentComment = await unitOfWork.Repository<Comment>()
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
            model.Status = comment.Status.GetDescription();
            model.InsertedDate = comment.InsertedDate;
            model.Source = comment.SourceType.GetDescription();

            if (comment.SourceType == SourceType.Blog)
            {
                var blog = await unitOfWork.Repository<Blog>()
                    .FirstOrDefault(x => x.Id == comment.SourceId && !x.Deleted);

                if (blog != null)
                {
                    model.SourceTitle = blog.Title;
                    model.SourceUrl = $"/blog/{blog.Url}/{blog.Id}";
                }
            }

            return model;
        }

        public async Task<ServiceResult> Create(CommentCreateDto dto)
        {
            var loginUser = httpContext.HttpContext.User.Parse();

            var comment = new Comment()
            {
                Deleted = false,
                Description = dto.Description,
                InsertedDate = DateTime.Now,
                ParentId = dto.ParentId,
                SourceType = dto.SourceType,
                Status = CommentStatus.WaitingforApproval,
                UserId = loginUser.UserId,
                SourceId = dto.SourceId
            };

            if (loginUser.UserType == (int)UserType.SuperAdmin)            
                comment.Status = CommentStatus.Approved;            

            await unitOfWork.Repository<Comment>().Add(comment);

            await unitOfWork.Save();

            return ServiceResult.Success(200, "Yorumunuz başarıyla kaydedildi. Onay sürecinden sonra yayınlanacaktır");
        }

        public async Task<ServiceResult> Update(CommentUpdateDto dto)
        {
            var comment = await unitOfWork.Repository<Comment>()
                .FirstOrDefault(x => !x.Deleted && x.Id == dto.Id);

            if (comment is null)
                throw new NotFoundException("Comment.Notfound");

            comment.Status = dto.CommentStatus;
            comment.UpdatedDate = DateTime.Now;

            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }

        public async Task<ServiceResult> Delete(int id, int? userId = null)
        {
            var query = unitOfWork.Repository<Comment>()
                 .Where(x => !x.Deleted && x.Id == id);

            if (userId != null)
                query = query.Where(x => x.UserId == userId);

            var comment = query.FirstOrDefault();

            if (comment is null)
                throw new NotFoundException("Comment.Notfound");

            comment.Deleted = true;
            await unitOfWork.Save();

            return ServiceResult.Success(200);
        }
    }
}
