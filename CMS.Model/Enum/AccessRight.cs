using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Model.Enum
{
    public enum AccessRightCategoryType
    {
        Frontend = 1,
        Admin,
        Api,
        Page
    }

    public enum HttpStatusType
    {
        GET = 1,
        POST,
        PUT,
        DELETE
    }
}
