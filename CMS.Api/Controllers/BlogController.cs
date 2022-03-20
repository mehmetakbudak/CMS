using CMS.Model.Model;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CMS.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        /// <summary>
        /// Id si verilen blog detay bilgilerini döner.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BlogDetailModel), 200)] //OK
        public IActionResult Get(int id)
        {
            var model = _blogService.GetDetailById(id);
            return Ok(model);
        }

        /// <summary>
        /// Kategoriye ve arama metnine göre blog listesi döner.
        /// </summary>
        /// <param name="categoryUrl"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpGet("GetByText")]
        [ProducesResponseType(typeof(BlogCategoryModel), 200)] //OK
        public IActionResult GetByText(string categoryUrl, string? text)
        {
            var list = _blogService.GetBlogList(categoryUrl, text);
            return Ok(list);
        }

        /// <summary>
        /// Seçilen blog bilgisinin görülme sayısını arttırır.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("Seen/{id}")]
        public IActionResult Seen(int id)
        {
            var result = _blogService.Seen(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Çok okunan blog listesini döner.
        /// </summary>
        /// <returns></returns>
        [HttpGet("MostRead")]
        [ProducesResponseType(typeof(List<BlogGetModel>), 200)] //OK
        public IActionResult MostRead()
        {
            var list = _blogService.MostRead();
            return Ok(list);
        }

        /// <summary>
        /// Blog kategorisine göre çok okunan blog listesini döner.
        /// </summary>
        /// <param name="blogCategoryId"></param>
        /// <returns></returns>
        [HttpGet("MostReadByBlogCategoryId/{blogCategoryId}")]
        [ProducesResponseType(typeof(List<BlogGetModel>), 200)] //OK
        public IActionResult MostReadByBlogCategoryId(int blogCategoryId)
        {
            var list = _blogService.MostRead(blogCategoryId);
            return Ok(list);
        }
    }
}
