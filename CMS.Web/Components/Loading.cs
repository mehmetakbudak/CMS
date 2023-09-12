using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.Web.Components
{
    [ViewComponent]
    public class Loading : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
