using CMS.Model.Model;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        [ProducesResponseType(typeof(BlogDetailViewModel), 200)] //OK
        public IActionResult Get(int id)
        {
            var model = _blogService.GetDetailById(id);
            return Ok(model);
        }

        /// <summary>
        /// Arama metnine göre blog listesi döner.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpGet("GetBlogs")]
        [ProducesResponseType(typeof(List<BlogViewModel>), 200)] //OK
        public IActionResult GetBlogs(string? text)
        {
            var list = _blogService.GetBlogs(text);
            return Ok(list);
        }

        /// <summary>
        /// Kategoriye göre blog listesi döner.
        /// </summary>
        /// <param name="blogCategoryUrl"></param>
        /// <returns></returns>
        [HttpGet("GetBlogsByCategoryUrl/{blogCategoryUrl}")]
        [ProducesResponseType(typeof(BlogCategoryViewModel), 200)] //OK
        public IActionResult GetBlogsByCategoryUrl(string blogCategoryUrl)
        {
            var list = _blogService.GetBlogsByCategoryUrl(blogCategoryUrl);
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
        [ProducesResponseType(typeof(List<MostReadBlogViewModel>), 200)] //OK
        public IActionResult MostRead()
        {
            var list = _blogService.MostRead();
            return Ok(list);
        }

        /// <summary>
        /// Blog kategorisine göre çok okunan blog listesini döner.
        /// </summary>
        /// <param name="blogCategoryUrl"></param>
        /// <returns></returns>
        [HttpGet("MostReadByBlogCategory/{blogCategoryUrl}")]
        [ProducesResponseType(typeof(List<MostReadBlogViewModel>), 200)] //OK
        public IActionResult MostReadByBlogCategoryId(string blogCategoryUrl)
        {
            var list = _blogService.MostRead(blogCategoryUrl);
            return Ok(list);
        }
    }
}
