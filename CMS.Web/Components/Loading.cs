using Microsoft.AspNetCore.Mvc;

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
