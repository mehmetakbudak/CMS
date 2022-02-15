using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using CMS.Model.Enum;
using CMS.Model.Model;

namespace CMS.Service
{
    public interface IMailTemplateService
    {
        TemplateResponseModel GetTemplateByType<T>(T data, TemplateType type);
    }

    public class MailTemplateService : IMailTemplateService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public MailTemplateService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public TemplateResponseModel GetTemplateByType<T>(T data, TemplateType type)
        {
            TemplateResponseModel result = null;

            var mailTemplate = _unitOfWork.Repository<MailTemplate>().FirstOrDefault(x => x.TemplateType == type);

            if (mailTemplate != null)
            {
                var properties = typeof(T).GetProperties();

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
