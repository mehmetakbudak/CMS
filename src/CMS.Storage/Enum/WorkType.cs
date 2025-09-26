using System.ComponentModel;

namespace CMS.Storage.Enum
{
    public enum WorkType
    {
        [Description("WorkType.FullTime")]
        FullTime = 1,
        [Description("WorkType.PartTime")]
        PartTime,
        [Description("WorkType.Intern")]
        Intern,
        [Description("WorkType.Freelance")]
        Freelance
    }
}
