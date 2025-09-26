using System;
using System.Net;
using System.Net.Mail;
using CMS.Storage.Consts;
using CMS.Business.Services;
using System.Threading.Tasks;
using CMS.Storage.Dtos.Mail;
using CMS.Storage.Dtos.WebsiteParameter;
using CMS.Storage.Dtos.Response;

namespace CMS.Business.Helper
{
    public interface IMailHelper
    {
        Task<ServiceResult> Send(MailDto model);

        Task<ServiceResult> SendWithTemplate(MailWithTemplateDto model);
    }

    public class MailHelper(
            IWebsiteParameterService websiteParameterService,
            IMailTemplateService mailTemplateService) : IMailHelper
    {       
        public async Task<ServiceResult> Send(MailDto model)
        {
            try
            {
                var emailSetting = await websiteParameterService.GetParametersByType<EmailSettingDto>(WebsiteParameterTypes.EmailSettings);

                Int32.TryParse(emailSetting.Port, out int port);

                using (var client = new SmtpClient(emailSetting.Host, port))
                {
                    client.UseDefaultCredentials = false;
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
                return ServiceResult.Fail(500, "Mail gönderilirken hata oluştu");
            }
            return ServiceResult.Success(200);
        }

        public async Task<ServiceResult> SendWithTemplate(MailWithTemplateDto model)
        {
            try
            {
                var emailSetting = await websiteParameterService.GetParametersByType<EmailSettingDto>(WebsiteParameterTypes.EmailSettings);

                var data = model.Data;

                var mailTemplate = await mailTemplateService.GetTemplateByType(data, model.TemplateType);

                if (mailTemplate != null)
                {
                    if (string.IsNullOrEmpty(model.Subject))
                    {
                        model.Subject = mailTemplate.Subject;
                    }
                    if (string.IsNullOrEmpty(model.Body))
                    {
                        model.Body = mailTemplate.Body;
                    }
                }

                Int32.TryParse(emailSetting.Port, out int port);

                using (var client = new SmtpClient(emailSetting.Host, port))
                {
                    client.UseDefaultCredentials = false;
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
                return ServiceResult.Fail(500, "Mail gönderilirken hata oluştu");
            }
            return ServiceResult.Success(200);
        }
    }
}
