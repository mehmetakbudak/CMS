using System.ComponentModel;

namespace CMS.Storage.Enum
{
    public enum MethodType
    {
        [Description("GET")]
        GET = 1,
        [Description("POST")]
        POST,
        [Description("PUT")]
        PUT,
        [Description("DELETE")]
        DELETE
    }
}
