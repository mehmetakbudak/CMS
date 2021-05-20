using Microsoft.Extensions.Configuration;

namespace CMS.Service.Helper
{
    public static class Global
    {
        public static string Secret { get; set; }


        public static void Initialize(IConfiguration configuration)
        {
            Secret = configuration["Secret"];
        }
    }
}
