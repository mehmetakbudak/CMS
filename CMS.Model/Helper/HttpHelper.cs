using CMS.Model.Model;

namespace CMS.Model.Helper
{
    public static class HttpHelper
    {
        public static ServiceReturnModel Result(ServiceResult model)
        {
            return new ServiceReturnModel
            {
                Data = model.Data,
                Exceptions = model.Exceptions,
                StatusCode = model.StatusCode
            };
        }
    }
}
