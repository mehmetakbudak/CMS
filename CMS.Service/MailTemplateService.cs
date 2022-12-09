using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Entity;
using CMS.Storage.Enum;
using CMS.Storage.Model;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface IMailTemplateService
    {
        Task<TemplateResponseModel> GetTemplateByType<T>(T data, TemplateType type);
    }

    public class MailTemplateService : IMailTemplateService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public MailTemplateService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<TemplateResponseModel> GetTemplateByType<T>(T data, TemplateType type)
        {
            TemplateResponseModel result = null;

            var mailTemplate = await _unitOfWork.Repository<MailTemplate>()
                .FirstOrDefault(x => x.TemplateType == type);

            if (mailTemplate != null)
            {
                var properties = data.GetType().GetProperties();

                var body = mailTemplate.Body;

                foreach (var property in properties)
                {
                    body = body.Replace("{{" + property.Name + "}}", property.GetValue(data).ToString());
                }

                result = new TemplateResponseModel()
                {
                    Subject = mailTemplate.Title,
                    Body = body
                };
            }
            return result;
        }
    }
}
