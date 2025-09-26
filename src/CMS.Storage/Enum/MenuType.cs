using System.ComponentModel;

namespace CMS.Storage.Enum
{
    public enum MenuType
    {
        [Description("Frontend")]
        FrontEnd = 1,
        [Description("Admin")]
        Admin,
        [Description("Custom")]
        Custom
    }
}
