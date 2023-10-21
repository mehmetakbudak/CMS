using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using CMS.Storage.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface ITagService
    {
        Task<Tag> GetByUrl(string url);
        Task<List<BlogTagCountModel>> GetSourceTags(SourceType sourceType, int? top = null);
    }

    public class TagService : ITagService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public TagService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Tag> GetByUrl(string url)
        {
            return await _unitOfWork.Repository<Tag>().FirstOrDefault(x => x.Url == url);
        }

        public async Task<List<BlogTagCountModel>> GetSourceTags(SourceType sourceType, int? top = null)
        {
            var tags = _unitOfWork.Repository<Tag>()
                .Where()
                .Include(x => x.SourceTags)
                .Select(x => new BlogTagCountModel
                {
                    Count = x.SourceTags.Count(a => a.SourceType == sourceType && a.SourceId == x.Id),
                    Name = x.Name,
                    Url = x.Url
                })
                .OrderByDescending(x => x.Count)
                .AsQueryable();

            if (top.HasValue)
            {
                tags = tags.Take(top.Value);
            }
            var list = await tags.ToListAsync();
            return list;
        }
    }
}
