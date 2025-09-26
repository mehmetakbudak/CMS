using Microsoft.Extensions.Configuration;

namespace CMS.Business.Infrastructure
{
    public static class Global
    {
        public static string WebUrl { get; set; }
        public static string ApiUrl { get; set; }
        public static string Secret { get; set; }
        public static string Issuer { get; set; }
        public static int AccessTokenExpiration { get; set; }
        public static int RefreshTokenExpiration { get; set; }

        public static void Initialize(IConfiguration configuration)
        {
            WebUrl = configuration["WebsiteParameters:WebUrl"];
            ApiUrl = configuration["WebsiteParameters:ApiUrl"];
            Secret = configuration["Jwt:Secret"];
            Issuer = configuration["Jwt:Issuer"];
            AccessTokenExpiration =  configuration.GetValue<int>("Jwt:AccessTokenExpiration");
            RefreshTokenExpiration = configuration.GetValue<int>("Jwt:RefreshTokenExpiration");
        }
    }
}
