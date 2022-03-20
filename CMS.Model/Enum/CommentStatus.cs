using System.ComponentModel;

namespace CMS.Model.Enum
{
    public enum CommentStatus
    {
        [Description("Onay Bekliyor")]
        WaitingforApproval = 1,
        [Description("Onaylandı")]
        Approved,
        [Description("Reddedildi")]
        Rejected
    }
}
