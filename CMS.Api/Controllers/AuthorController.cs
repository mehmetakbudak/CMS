using CMS.Model.Model;
using CMS.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService authorService;
        public AuthorController(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var list = authorService.GetAll();
            return Ok(list);
        }

        [HttpPost]
        public IActionResult Post([FromBody] AuthorModel model)
        {
            var result = authorService.Post(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        public IActionResult Put([FromBody] AuthorModel model)
        {
            var result = authorService.Put(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = authorService.Delete(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
