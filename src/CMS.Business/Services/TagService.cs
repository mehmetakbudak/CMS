using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Dtos.Blog;
using CMS.Storage.Dtos.Tag;
using CMS.Storage.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface ITagService
    {
        IQueryable<Tag> Get();
        Task<Tag> GetByUrl(string url);
        Task<List<BlogTagCountDto>> GetSourceTags(GetSourceTagDto dto);
    }

    public class TagService(IUnitOfWork<CMSContext> unitOfWork) : ITagService
    {
        public IQueryable<Tag> Get()
        {
            return unitOfWork.Repository<Tag>().Where();
        }

        public async Task<Tag> GetByUrl(string url)
        {
            return await unitOfWork.Repository<Tag>().FirstOrDefault(x => x.Url == url);
        }

        public async Task<List<BlogTagCountDto>> GetSourceTags(GetSourceTagDto dto)
        {
            var tags = unitOfWork.Repository<Tag>()
                .Where()
                .Include(x => x.SourceTags)
                .Select(x => new BlogTagCountDto
                {
                    Count = x.SourceTags.Count(a => a.SourceType == dto.SourceType && a.SourceId == x.Id),
                    Name = x.Name,
                    Url = x.Url
                })
                .OrderByDescending(x => x.Count)
                .AsQueryable();

            if (dto != null && dto.Top.HasValue)
            {
                tags = tags.Take(dto.Top.Value);
            }
            var list = await tags.ToListAsync();
            return list;
        }
    }
}
