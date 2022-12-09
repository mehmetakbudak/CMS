using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Service
{
    public interface IWebsiteParameterService
    {
        Task<T> GetParametersByType<T>(string code) where T : class, new();
    }

    public class WebsiteParameterService : IWebsiteParameterService
    {
        private readonly IUnitOfWork<CMSContext> _unitOfWork;

        public WebsiteParameterService(IUnitOfWork<CMSContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<T> GetParametersByType<T>(string code) where T : class, new()
        {
            T model = new T();

            var parentParameter = await _unitOfWork.Repository<WebsiteParameter>()
                .FirstOrDefault(p => p.Code == code && p.ParentId == null);

            if (parentParameter != null)
            {
                var parameters = await _unitOfWork.Repository<WebsiteParameter>().Where(x => x.ParentId == parentParameter.Id).ToListAsync();

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
