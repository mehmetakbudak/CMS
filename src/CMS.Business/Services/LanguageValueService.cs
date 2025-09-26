using System;
using System.Linq;
using CMS.Data.Context;
using CMS.Storage.Entity;
using CMS.Storage.Consts;
using MassTransit.Testing;
using CMS.Data.Repository;
using System.Threading.Tasks;
using CMS.Business.Exceptions;
using CMS.Storage.Dtos.Language;
using CMS.Storage.Dtos.Response;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CMS.Business.Services
{
    public interface ILanguageValueService
    {
        Task<LanguageValueWithHeaderDto> Get(LanguageValueFilterDto dto);
        List<LanguageValue> GetAllFromCache();
        Task<LanguageValueItemDto> GetById(int id);
        Task<LanguageValueDto> GetByCodeWithLanguageId(string code, int languageId);
        Task<ServiceResult> Create(LanguageValueItemDto dto);
        Task<ServiceResult> Update(LanguageValueItemDto dto);
        Task<ServiceResult> Delete(int id);
    }

    public class LanguageValueService(
        IMemoryCache memoryCache,
        ILanguageService languageService,
        IUnitOfWork<CMSContext> unitOfWork) : ILanguageValueService
    {
        public async Task<LanguageValueWithHeaderDto> Get(LanguageValueFilterDto dto)
        {
            var result = new LanguageValueWithHeaderDto();

            var languages = languageService.GetAllFromCache();

            result.Header.Add("Code");
            languages.ForEach(x => result.Header.Add(x.Name));

            var query = unitOfWork.Repository<LanguageValue>()
                .Where(x => !x.Deleted)
                .Include(x => x.LanguageCode)
                .AsQueryable();

            if (dto.LanguageValues != null && dto.LanguageValues.Any())
            {
                var filteredCodeIdSets = dto.LanguageValues.Where(x => !string.IsNullOrEmpty(x.Value))
                        .Select(f => query.Where(lv => lv.LanguageId == f.LanguageId && lv.Value.ToLower().Contains(f.Value.ToLower()))
                                          .Select(lv => lv.LanguageCodeId)
                ).ToList();

                if (filteredCodeIdSets != null && filteredCodeIdSets.Count > 0)
                {
                    var matchingCodeIds = filteredCodeIdSets.Aggregate((prev, next) => prev.Intersect(next));
                    query = query.Where(lv => matchingCodeIds.Contains(lv.LanguageCodeId));
                }
            }

            if (!string.IsNullOrEmpty(dto.Code))
            {
                query = query.Where(x => x.LanguageCode.Code.ToLower().Contains(dto.Code.ToLower()));
            }

            var data = query.GroupBy(x => x.LanguageCode);

            var items = data.AsEnumerable().Select(g =>
            {
                var item = new LanguageValueItemDto
                {
                    Code = g.Key.Code,
                    Id = g.Key.Id
                };
                foreach (var language in languages)
                {
                    item.Values.Add(new LanguageValueDto
                    {
                        LanguageId = language.Id,
                        Value = g.FirstOrDefault(l => l.LanguageId == language.Id)?.Value
                    });
                }
                return item;
            })
            .OrderByDescending(x => x.Id)
            .Skip(dto.Skip)
            .Take(dto.Take)
            .ToList();

            result.TotalCount = await data.CountAsync();
            result.Items = items;

            return result;
        }

        public List<LanguageValue> GetAllFromCache()
        {
            return memoryCache.GetOrCreate(CacheKeys.ActiveLanguageValues, entry =>
            {
                return unitOfWork.Repository<LanguageValue>()
                   .Where(x => !x.Deleted)
                   .Include(x => x.LanguageCode)
                   .ToList();
            });
        }

        public async Task<LanguageValueItemDto> GetById(int id)
        {
            var languages = languageService.GetAllFromCache();

            var languageValues = await unitOfWork.Repository<LanguageValue>()
                .Where(x => !x.Deleted && !x.Language.Deleted && !x.LanguageCode.Deleted && x.LanguageCodeId == id)
                .Include(x => x.LanguageCode)
                .ToListAsync();

            var result = languageValues.GroupBy(x => x.LanguageCode).Select(x =>
            {
                var item = new LanguageValueItemDto
                {
                    Id = x.Key.Id,
                    Code = x.Key.Code
                };
                foreach (var language in languages)
                {
                    item.Values.Add(new LanguageValueDto
                    {
                        LanguageId = language.Id,
                        Value = languageValues.FirstOrDefault(l => l.LanguageId == language.Id && l.LanguageCodeId == x.Key.Id)?.Value
                    });
                }
                return item;
            }).FirstOrDefault();

            return result;
        }

        public async Task<LanguageValueDto> GetByCodeWithLanguageId(string code, int languageId)
        {
            var languageValue = await unitOfWork.Repository<LanguageValue>()
                .FirstOrDefault(x => !x.Deleted && !x.Language.Deleted && !x.LanguageCode.Deleted && x.LanguageCode.Code == code && x.LanguageId == languageId);

            if (languageValue is null)
                throw new NotFoundException("Notfound.LanguageValue");

            var result = new LanguageValueDto
            {
                LanguageId = languageValue.LanguageId,
                Value = languageValue.Value
            };

            return result;
        }

        public async Task<ServiceResult> Create(LanguageValueItemDto dto)
        {
            var isExistCode = await unitOfWork.Repository<LanguageCode>()
                .Any(x => !x.Deleted && x.Code == dto.Code);

            if (isExistCode)
                throw new FoundException("AlreadyExist.LanguageCode");

            var languages = languageService.GetAllFromCache();

            var strategy = unitOfWork.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                try
                {
                    await unitOfWork.CreateTransaction();

                    var languageCode = new LanguageCode
                    {
                        Code = dto.Code,
                        IsActive = true,
                        Deleted = false
                    };

                    await unitOfWork.Repository<LanguageCode>().Add(languageCode);
                    await unitOfWork.Save();

                    foreach (var item in dto.Values)
                    {
                        var isExistLanguage = languages.Any(x => x.Id == item.LanguageId);

                        if (!isExistLanguage)
                            continue;

                        var languageValue = new LanguageValue
                        {
                            LanguageId = item.LanguageId,
                            LanguageCodeId = languageCode.Id,
                            Value = item.Value,
                            Deleted = false,
                            IsActive = true
                        };
                        await unitOfWork.Repository<LanguageValue>().Add(languageValue);
                    }
                    await unitOfWork.Save();
                    await unitOfWork.Commit();
                    memoryCache.Remove(CacheKeys.ActiveLanguageValues);
                }
                catch (Exception ex)
                {
                    await unitOfWork.Rollback();
                    throw new Exception(ex.Message);
                }
            });
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> Update(LanguageValueItemDto dto)
        {
            var languageCode = await unitOfWork.Repository<LanguageCode>()
                .Where(x => !x.Deleted && x.Id == dto.Id)
                .Include(x => x.LanguageValues)
                .FirstOrDefaultAsync();

            if (languageCode is null)
                throw new NotFoundException("Notfound");

            var isExistCode = await unitOfWork.Repository<LanguageCode>()
               .Any(x => !x.Deleted && x.Id != dto.Id && x.Code == dto.Code);

            if (isExistCode)
                throw new FoundException("AlreadyExist.LanguageCode");

            languageCode.Code = dto.Code;

            unitOfWork.Repository<LanguageCode>().Update(languageCode);

            var languages = languageService.GetAllFromCache();

            var languageValues = languageCode.LanguageValues.ToList();

            unitOfWork.Repository<LanguageValue>().DeleteRange(languageValues);

            foreach (var item in dto.Values)
            {
                var isExistLanguage = languages.Any(x => x.Id == item.LanguageId);

                if (!isExistLanguage)
                    continue;

                await unitOfWork.Repository<LanguageValue>().Add(new LanguageValue
                {
                    LanguageId = item.LanguageId,
                    LanguageCodeId = languageCode.Id,
                    Value = item.Value,
                    Deleted = false,
                    IsActive = true
                });
            }
            await unitOfWork.Save();
            memoryCache.Remove(CacheKeys.ActiveLanguageValues);
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var languageCode = await unitOfWork.Repository<LanguageCode>()
                .Where(x => !x.Deleted && x.Id == id)
                .Include(x => x.LanguageValues)
                .FirstOrDefaultAsync();

            if (languageCode is null)
                throw new NotFoundException("Notfound.LanguageValue");

            languageCode.Deleted = true;

            var languageValues = languageCode.LanguageValues.ToList();

            languageValues.ForEach(x =>
            {
                x.Deleted = true;
            });

            unitOfWork.Repository<LanguageCode>().Update(languageCode);
            if (languageValues.Count > 0)
            {
                unitOfWork.Repository<LanguageValue>().UpdateRange(languageValues);
            }
            await unitOfWork.Save();
            memoryCache.Remove(CacheKeys.ActiveLanguageValues);

            return ServiceResult.Success();
        }
    }
}
