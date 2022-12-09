using System.ComponentModel;

namespace CMS.Storage.Enum
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
