using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Entity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface IUserTokenService
    {
        Task<UserToken> GetByUserId(int userId);
    }

    public class UserTokenService(IUnitOfWork<CMSContext> unitOfWork) : IUserTokenService
    {    
        public async Task<UserToken> GetByUserId(int userId)
        {
            var data = await unitOfWork.Repository<UserToken>()                
                .Where(x => x.UserId == userId)
                .Include(x => x.User)
                .FirstOrDefaultAsync();

            return data;
        }
    }
}
