using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface ITagService
    {
        Task<List<SourceTag>> GetSouceTags(SourceType sourceType, int? top = null);
    }

    public class TagService : ITagService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public TagService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SourceTag>> GetSouceTags(SourceType sourceType, int? top = null)
        {
            var tags = _unitOfWork.Repository<SourceTag>()
                .Where(x => x.SourceType == sourceType)
                .OrderByDescending(x => x.Tag).AsQueryable();

            if (top.HasValue)
            {
                tags = tags.Take(top.Value);
            }

            var list = await tags.ToListAsync();

            return list;
        }
    }
}
