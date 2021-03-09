using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Model.Helper;
using CMS.Model.Model;
using CMS.Model.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net;

namespace CMS.Service
{
    public interface ILoginService
    {
        ServiceResult Post(LoginModel login);
    }

    public class LoginService : ILoginService
    {
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork<CMSContext> unitOfWork;
        private readonly IJWTService jwtService;

        public LoginService(IConfiguration configuration,
            IUnitOfWork<CMSContext> unitOfWork,
            IJWTService jwtService)
        {
            this.configuration = configuration;
            this.unitOfWork = unitOfWork;
            this.jwtService = jwtService;
        }

        public ServiceResult Post(LoginModel model)
        {
            ServiceResult serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var hassPassword = Security.MD5Crypt(model.Password);
                var user = unitOfWork.Repository<User>()
                    .Find(x => x.EmailAddress == model.EmailAddress && x.Password == hassPassword && !x.Deleted);

                if (user == null)
                {
                    serviceResult.StatusCode = HttpStatusCode.BadRequest;
                    serviceResult.Exceptions.Add("Email adresi veya şifre hatalıdır.");
                    return serviceResult;
                }

                if (!user.IsActive)
                {
                    serviceResult.StatusCode = HttpStatusCode.BadRequest;
                    serviceResult.Exceptions.Add("Hesabınız aktif değildir.");
                    return serviceResult;
                }

                var userFullName = user.Name + " " + user.Surname;

                var data = new LoginReturnModel
                {
                    UserFullName = userFullName,
                    UserId = user.Id,
                    UserType = (int)user.UserType
                };
                serviceResult.Data = data;
            }
            catch (Exception ex)
            {
                serviceResult.Exceptions.Add(ex.Message);
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
            }
            return serviceResult;
        }
    }
}
