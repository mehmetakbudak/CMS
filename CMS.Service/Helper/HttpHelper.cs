﻿using CMS.Storage.Model;

namespace CMS.Service.Helper
{
    public static class HttpHelper
    {
        public static ServiceResult Result(ServiceResult model)
        {
            return new ServiceResult
            {
                Data = model.Data,
                StatusCode = model.StatusCode
            };
        }
    }
}
