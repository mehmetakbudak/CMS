using Microsoft.Extensions.Configuration;

namespace CMS.Service.Infrastructure
{
    public static class Global
    {
        public static string Secret { get; set; }

        public static string UIUrl { get; set; }

        public static string ApiUrl { get; set; }


        public static void Initialize(IConfiguration configuration)
        {
            Secret = configuration["WebsiteParameters:Secret"];
            UIUrl = configuration["WebsiteParameters:UIUrl"];
            ApiUrl = configuration["WebsiteParameters:ApiUrl"];
        }
    }
}
