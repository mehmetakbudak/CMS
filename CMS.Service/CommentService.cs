using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Consts;
using CMS.Model.Entity;
using CMS.Model.Enum;
using CMS.Model.Model;
using CMS.Service.Exceptions;
using CMS.Service.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CMS.Service
{
    public interface ICommentService
    {
        List<CommentGetModel> GetSourceComments(SourceCommentModel model, int? parentId = null, List<CommentGetModel> children = null);
        List<UserCommentModel> GetUserComments(int type);
        List<CommentModel> GetAllByStatus(int status);
        CommentDetailModel GetDetail(int id);
        ServiceResult Post(CommentPostModel model);
        ServiceResult Put(CommentPutModel model);
        ServiceResult Delete(int id, int? userId = null);
    }

    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public CommentService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<CommentGetModel> GetSourceComments(SourceCommentModel model, int? parentId = null, List<CommentGetModel> children = null)
        {
            List<CommentGetModel> comments = new List<CommentGetModel>();

            var list = _unitOfWork.Repository<Comment>()
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
                }).ToList();

            comments.AddRange(list);

            foreach (var comment in list)
            {
                comment.Children = GetSourceComments(model, comment.Id, list);
            }

            return comments;
        }

        public List<UserCommentModel> GetUserComments(int type)
        {
            var data = _unitOfWork.Repository<Comment>()
               .Where(x => !x.Deleted && x.UserId == AuthTokenContent.Current.UserId);

            if (type != 0)
            {
                data = data.Where(x => x.SourceType == (SourceType)type);
            }

            var list = data.OrderByDescending(x => x.InsertedDate)
               .Select(x => new UserCommentModel()
               {
                   Description = x.Description,
                   Id = x.Id,
                   InsertedDate = x.InsertedDate,
                   UpdatedDate = x.UpdatedDate,
                   Source = EnumHelper.GetDescription(x.SourceType),
                   Status = EnumHelper.GetDescription(x.Status)
               }).ToList();

            return list;
        }

        public List<CommentModel> GetAllByStatus(int status)
        {
            var list = _unitOfWork.Repository<Comment>()
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
                }).ToList();

            return list;
        }

        public CommentDetailModel GetDetail(int id)
        {
            var comment = _unitOfWork.Repository<Comment>().FirstOrDefault(x => !x.Deleted && x.Id == id);
            if (comment == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }
            var model = new CommentDetailModel();

            if (comment.ParentId.HasValue)
            {
                var parentComment = _unitOfWork.Repository<Comment>()
                    .Where(x => x.Id == comment.ParentId && !x.Deleted)
                    .Include(x => x.User).FirstOrDefault();

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
                var blog = _unitOfWork.Repository<Blog>().FirstOrDefault(x => x.Id == comment.SourceId && !x.Deleted);
                if (blog != null)
                {
                    model.SourceTitle = blog.Title;
                    model.SourceUrl = $"/blog/{blog.Url}/{blog.Id}";
                }
            }

            return model;
        }

        public ServiceResult Post(CommentPostModel model)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };
            var entity = new Comment()
            {
                Deleted = false,
                Description = model.Description,
                InsertedDate = DateTime.Now,
                ParentId = model.ParentId,
                SourceType = model.SourceType,
                Status = CommentStatus.WaitingforApproval,
                UserId = AuthTokenContent.Current.UserId,
                SourceId = model.SourceId
            };
            _unitOfWork.Repository<Comment>().Add(entity);
            _unitOfWork.Save();
            result.Message = AlertMessages.Post;
            return result;
        }

        public ServiceResult Put(CommentPutModel model)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var comment = _unitOfWork.Repository<Comment>().FirstOrDefault(x => !x.Deleted && x.Id == model.Id);
            if (comment == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }
            comment.Status = model.CommentStatus;
            comment.UpdatedDate = DateTime.Now;
            _unitOfWork.Save();
            return result;
        }

        public ServiceResult Delete(int id, int? userId = null)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };

            var data = _unitOfWork.Repository<Comment>().Where(x => !x.Deleted && x.Id == id);

            if (userId.HasValue)
            {
                data = data.Where(x => x.UserId == AuthTokenContent.Current.UserId);
            }
            var comment = data.FirstOrDefault();

            if (comment == null)
            {
                throw new NotFoundException(AlertMessages.NotFound);
            }
            comment.Deleted = true;
            _unitOfWork.Save();
            result.Message = AlertMessages.Delete;
            return result;
        }
    }
}
