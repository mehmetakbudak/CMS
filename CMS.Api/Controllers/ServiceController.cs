using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IService_Service _serviceService;
        
        public ServiceController(IService_Service serviceService)
        {
            _serviceService = serviceService;
        }

        /// <summary>
        /// Hizmetler listesini döner.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Model.Entity.Service>), 200)] //OK
        public IActionResult Get()
        {
            var result = _serviceService.GetAll().Where(x => x.IsActive).ToList();
            return Ok(result);
        }
    }
}
