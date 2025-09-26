using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using CMS.Storage.Dtos.Mail;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface IMailTemplateService
    {
        Task<TemplateResponseDto> GetTemplateByType<T>(T data, TemplateType type);
    }

    public class MailTemplateService(IUnitOfWork<CMSContext> unitOfWork) : IMailTemplateService
    {        
        public async Task<TemplateResponseDto> GetTemplateByType<T>(T data, TemplateType type)
        {
            TemplateResponseDto result = null;

            var mailTemplate = await unitOfWork.Repository<MailTemplate>()
                .FirstOrDefault(x => x.TemplateType == type);

            if (mailTemplate != null)
            {
                var properties = data.GetType().GetProperties();

                var body = mailTemplate.Body;

                foreach (var property in properties)
                {
                    body = body.Replace("{{" + property.Name + "}}", property.GetValue(data).ToString());
                }

                result = new TemplateResponseDto()
                {
                    Subject = mailTemplate.Title,
                    Body = body
                };
            }
            return result;
        }
    }
}
