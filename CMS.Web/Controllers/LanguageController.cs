using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace CMS.Web.Controllers
{
    public class LanguageController : Controller
    {
        private readonly RequestLocalizationOptions _localizationOptions;
        public LanguageController(IOptions<RequestLocalizationOptions> localizationOptions)
        {
            _localizationOptions = localizationOptions.Value;
        }

        [HttpGet("Language")]
        public IActionResult Get()
        {
            IRequestCultureFeature requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();

            var allCultures = _localizationOptions.SupportedCultures
                .Select(culture => new
                {
                    Name = culture.Name,
                    Text = culture.DisplayName
                }).ToList();

            return Ok(allCultures);
        }

        [HttpGet("Language/CurrentLanguage")]
        public IActionResult GetCurrentLanguage()
        {
            var requestCultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
            var currentCulture = requestCultureFeature.RequestCulture.Culture;
            return Ok(currentCulture);
        }

        [HttpGet("Language/ChangeLanguage")]
        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddYears(1) });
            return Ok();
        }
    }
}
