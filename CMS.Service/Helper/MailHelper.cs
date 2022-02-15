using CMS.Model.Consts;
using CMS.Model.Model;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CMS.Service.Helper
{
    public interface IMailHelper
    {
        Task<ServiceResult> Send(MailModel model);

    }

    public class MailHelper : IMailHelper
    {
        private readonly IWebsiteParameterService _websiteParameterService;

        public MailHelper(IWebsiteParameterService websiteParameterService)
        {
            _websiteParameterService = websiteParameterService;
        }

        public async Task<ServiceResult> Send(MailModel model)
        {
            var result = new ServiceResult { StatusCode = (int)HttpStatusCode.OK };
            try
            {
                var emailSetting = _websiteParameterService.GetParametersByType<EmailSettingModel>(WebsiteParameterTypes.EmailSettings);

                Int32.TryParse(emailSetting.Port, out int port);

                using (var client = new SmtpClient(emailSetting.Host, port))
                {
                    client.UseDefaultCredentials = true;
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(emailSetting.EmailAddress, emailSetting.Password);

                    MailMessage message = new MailMessage();

                    message.From = new MailAddress(emailSetting.EmailAddress);
                    message.To.Add(model.EmailAddress);
                    message.Body = model.Body;
                    message.Subject = model.Subject;
                    message.IsBodyHtml = true;
                    await Task.Run(() => client.Send(message));
                }
            }
            catch
            {
                result.StatusCode = (int)HttpStatusCode.InternalServerError;
                result.Message = "Mail gönderilirken hata oluştu.";
            }
            return result;
        }
    }
}
