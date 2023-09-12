using System.ComponentModel;

namespace CMS.Storage.Enum
{
    public enum WorkType
    {
        [Description("Tam zamanlı")]
        FullTime,
        [Description("Yarı zamanlı")]
        PartTime,
        [Description("Stajyer")]
        Intern,
        [Description("Serbest Çalışan")]
        Freelance
    }
}
