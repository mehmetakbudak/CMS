using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Model.Enum;
using System.Collections.Generic;
using System.Linq;

namespace CMS.Service
{
    public interface ITagService
    {
        List<SourceTag> GetSouceTags(SourceType sourceType, int? top = null);
    }

    public class TagService : ITagService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public TagService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<SourceTag> GetSouceTags(SourceType sourceType, int? top = null)
        {
            var tags = _unitOfWork.Repository<SourceTag>()
                .Where(x => x.SourceType == sourceType)
                .OrderByDescending(x => x.Tag).AsQueryable();

            if (top.HasValue)
            {
                tags = tags.Take(top.Value);
            }

            return tags.ToList();
        }
    }
}
