using System.ComponentModel;

namespace CMS.Model.Enum
{
    public enum AccessRightType
    {
        Menu = 1,
        Operation
    }

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
