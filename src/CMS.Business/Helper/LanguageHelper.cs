using CMS.Business.Services;
using CMS.Storage.Dtos.Language;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace CMS.Business.Helper;

public interface ILanguageHelper
{
    string Translate(string code, string defaultText = null);
    List<DictionaryDto> GetDictionary();
}

public class LanguageHelper(
    IHttpContextAccessor httpContextAccessor,
    ILanguageValueService languageValueService,
    ILanguageService languageService) : ILanguageHelper
{
    public string Translate(string code, string defaultText = null)
    {
        var lang = httpContextAccessor.HttpContext?.Request.Headers["Language"].ToString();

        if (string.IsNullOrEmpty(lang))
        {
            var defaultLanguage = languageService.GetDefault();

            if (defaultLanguage != null)
                lang = defaultLanguage.Code;
            else
                return defaultText ?? code;
        }
        var items = GetDictionary().FirstOrDefault(x => x.Code == lang);

        if (items != null && items.Items.ContainsKey(code))
            return items.Items[code];

        return defaultText ?? code;
    }

    public List<DictionaryDto> GetDictionary()
    {
        var list = new List<DictionaryDto>();

        var languages = languageService.GetAllFromCache().Where(x => x.IsActive).ToList();

        var languageValues = languageValueService.GetAllFromCache()
            .Where(x => x.IsActive)
            .ToList();

        foreach (var language in languages)
        {
            var item = new DictionaryDto
            {
                Code = language.Code,
                Name = language.Name,
                Items = languageValues
                        .Where(x => x.LanguageId == language.Id)
                        .ToDictionary(x => x.LanguageCode.Code, x => x.Value)
            };
            list.Add(item);
        }
        return list;
    }
}