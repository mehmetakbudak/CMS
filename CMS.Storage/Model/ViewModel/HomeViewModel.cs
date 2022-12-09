using System.Collections.Generic;

namespace CMS.Storage.Model.ViewModel
{
    public class HomeViewModel
    {
        public List<HomepageSliderModel> HomepageSliders { get; set; }
        public List<ServiceModel> Services { get; set; }
        public List<BlogModel> Blogs { get; set; }
        public List<TestimonialModel> Testimonials { get; set; }
        public List<ClientModel> Clients { get; set; }
    }
}
