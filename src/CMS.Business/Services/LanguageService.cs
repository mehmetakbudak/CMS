using CMS.Business.Exceptions;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Consts;
using CMS.Storage.Dtos.Language;
using CMS.Storage.Dtos.Response;
using CMS.Storage.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface ILanguageService
    {
        IQueryable<LanguageDto> Get();
        List<Language> GetAllFromCache();
        Language GetDefault();
        LanguageDto GetById(int id);
        Task<ServiceResult> Create(LanguageDto languageDto);
        Task<ServiceResult> Update(LanguageDto languageDto);
        Task<ServiceResult> Delete(int id);
    }

    public class LanguageService(
        IUnitOfWork<CMSContext> unitOfWork,
        IMemoryCache memoryCache) : ILanguageService
    {
        public IQueryable<LanguageDto> Get()
        {
            var data = unitOfWork.Repository<Language>()
                .Where(x => !x.Deleted)
                .Select(x => new LanguageDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    IconUrl = x.IconUrl,
                    IsActive = x.IsActive,
                    IsDefault = x.IsDefault
                }).AsQueryable();
            return data;
        }

        public List<Language> GetAllFromCache()
        {
            return memoryCache.GetOrCreate(CacheKeys.ActiveLanguages, entry =>
            {
                return unitOfWork.Repository<Language>()
                    .Where(x => !x.Deleted)
                    .AsNoTracking()
                    .ToList();
            });
        }        

        public Language GetDefault()
        {
            return GetAllFromCache().FirstOrDefault(x => !x.Deleted && x.IsDefault && x.IsActive);
        }

        public LanguageDto GetById(int id)
        {
            var language = GetAllFromCache().FirstOrDefault(x => !x.Deleted && x.Id == id);

            if (language is null)
                throw new NotFoundException("Language.NotFound");

            var result = new LanguageDto()
            {
                Id = language.Id,
                Name = language.Name,
                Code = language.Code,
                IconUrl = language.IconUrl,
                IsActive = language.IsActive,
                IsDefault = language.IsDefault
            };
            return result;
        }

        public async Task<ServiceResult> Create(LanguageDto dto)
        {
            var isExist = GetAllFromCache().Any(x => !x.Deleted && x.Code == dto.Code);

            if (isExist)
                throw new FoundException("Language.AlreadyExist");

            var language = new Language()
            {
                Code = dto.Code,
                IconUrl = dto.IconUrl,
                IsActive = dto.IsActive,
                IsDefault = dto.IsDefault,
                Name = dto.Name,
                Deleted = false
            };
            await unitOfWork.Repository<Language>().Add(language);
            await unitOfWork.Save();
            memoryCache.Remove(CacheKeys.ActiveLanguages);

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> Update(LanguageDto dto)
        {
            var isExist = GetAllFromCache().Any(x => !x.Deleted && x.Id != dto.Id && x.Code == dto.Code);

            if (isExist)
                throw new FoundException("Language.AlreadyExist");

            var language = await unitOfWork.Repository<Language>()
                .FirstOrDefault(x => !x.Deleted && x.Id == dto.Id);

            if (language is null)
                throw new NotFoundException("Language.NotFound");

            language.Code = dto.Code;
            language.IconUrl = dto.IconUrl;
            language.IsDefault = dto.IsDefault;
            language.IsActive = dto.IsActive;
            language.Name = dto.Name;

            unitOfWork.Repository<Language>().Update(language);
            await unitOfWork.Save();
            memoryCache.Remove(CacheKeys.ActiveLanguages);
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var language = await unitOfWork.Repository<Language>()
               .FirstOrDefault(x => !x.Deleted && x.Id == id);

            if (language is null)
                throw new NotFoundException("Language.NotFound");

            language.Deleted = true;
            unitOfWork.Repository<Language>().Update(language);
            await unitOfWork.Save();
            memoryCache.Remove(CacheKeys.ActiveLanguages);

            return ServiceResult.Success();
        }
    }
}
