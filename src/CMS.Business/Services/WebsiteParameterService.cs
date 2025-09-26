using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface IWebsiteParameterService
    {
        Task<T> GetParametersByType<T>(string code) where T : class, new();
    }

    public class WebsiteParameterService(IUnitOfWork<CMSContext> unitOfWork) : IWebsiteParameterService
    {       
        public async Task<T> GetParametersByType<T>(string code) where T : class, new()
        {
            T model = new T();

            var parentParameter = await unitOfWork.Repository<WebsiteParameter>()
                .FirstOrDefault(p => p.Code == code && p.ParentId == null);

            if (parentParameter != null)
            {
                var parameters = await unitOfWork.Repository<WebsiteParameter>().Where(x => x.ParentId == parentParameter.Id).ToListAsync();

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
