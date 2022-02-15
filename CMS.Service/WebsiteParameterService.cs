using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using System.Linq;

namespace CMS.Service
{
    public interface IWebsiteParameterService
    {
        T GetParametersByType<T>(string code) where T : class, new();
    }

    public class WebsiteParameterService : IWebsiteParameterService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public WebsiteParameterService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public T GetParametersByType<T>(string code) where T : class, new()
        {
            T model = new T();

            var parentParameter = _unitOfWork.Repository<WebsiteParameter>().FirstOrDefault(p => p.Code == code && p.ParentId == null);

            if (parentParameter != null)
            {
                var parameters = _unitOfWork.Repository<WebsiteParameter>().Where(x => x.ParentId == parentParameter.Id).ToList();
                var properties = typeof(T).GetProperties();

                foreach (var property in properties)
                {
                    var parameter = parameters.FirstOrDefault(x => x.Code == property.Name);
                    if (parameter != null)
                    {
                        property.SetValue(model, parameter.Value);
                    }
                }
            }
            return model;
        }
    }
}
