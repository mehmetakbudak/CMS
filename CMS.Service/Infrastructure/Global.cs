using Microsoft.Extensions.Configuration;

namespace CMS.Service.Infrastructure
{
    public static class Global
    {
        public static string WebUrl { get; set; }
        public static string ApiUrl { get; set; }
        public static string Secret { get; set; }

        public static void Initialize(IConfiguration configuration)
        {
            WebUrl = configuration["WebsiteParameters:WebUrl"];
            ApiUrl = configuration["WebsiteParameters:ApiUrl"];
            Secret = configuration["WebsiteParameters:Secret"];
        }
    }
}
