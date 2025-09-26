using CMS.Storage.Dtos.Dashboard;
using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace CMS.Business.Services
{
    public interface IDashboardService
    {
        Task<DashboardCountDto> GetCount();
    }

    public class DashboardService(IDbConnection dbConnection) : IDashboardService
    {
        public async Task<DashboardCountDto> GetCount()
        {
            var userCount = await dbConnection.QueryFirstAsync<int>("select count(*) from Users where Deleted=0");
            var blogCount = await dbConnection.QueryFirstAsync<int>("select count(*) from Blogs where Deleted=0");
            var commentCount = await dbConnection.QueryFirstAsync<int>("select count(*) from Comments where Deleted=0");
            var contactMessageCount = await dbConnection.QueryFirstAsync<int>("select count(*) from Contacts");
            var homepageSliderCount = await dbConnection.QueryFirstAsync<int>("select count(*) from HomepageSliders where Deleted=0");
            var jobCount = await dbConnection.QueryFirstAsync<int>("select count(*) from Jobs where Deleted=0");
            var pageCount = await dbConnection.QueryFirstAsync<int>("select count(*) from Pages where Deleted=0");
            var roleCount = await dbConnection.QueryFirstAsync<int>("select count(*) from Roles where Deleted=0");
            var serviceCount = await dbConnection.QueryFirstAsync<int>("select count(*) from [Services] where Deleted=0");
            var taskCount = await dbConnection.QueryFirstAsync<int>("select count(*) from Tasks where Deleted=0");
            var testimonialCount = await dbConnection.QueryFirstAsync<int>("select count(*) from Testimonials where Deleted=0");

            return new()
            {
                UserCount = userCount,
                BlogCount = blogCount,
                CommentCount = commentCount,
                ContactMessageCount = contactMessageCount,
                HomepageSliderCount = homepageSliderCount,
                JobCount = jobCount,
                PageCount = pageCount,
                RoleCount = roleCount,
                ServiceCount = serviceCount,
                TaskCount = taskCount,
                TestimonialCount = testimonialCount
            };
        }
    }
}
