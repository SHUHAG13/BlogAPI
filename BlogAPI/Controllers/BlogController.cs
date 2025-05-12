using BlogAPI.Interfaces;
using BlogAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IRepository<Blog> _blogRepo;

        public BlogController(IRepository<Blog> blogReop)
        {
            _blogRepo = blogReop;
        }


        [HttpGet]
        public async Task<IActionResult>GetAllBlogs()
        {

            var blogList = await _blogRepo.GetAll();
            return Ok(blogList);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult>GetById([FromRoute]int id)
        {
            var blog = await _blogRepo.GetById(id);
            return Ok(blog);
        }

        [HttpPost]
        public async Task<IActionResult> AddBlog([FromBody]Blog model)
        {
            await _blogRepo.AddAsync(model);
            await _blogRepo.SaveChangesAsync();
            return Ok(model);
        }
        
        
        [HttpPut("{id:int}")] 
        public async Task<IActionResult> UpdateBlog([FromRoute] int id,[FromBody]Blog model)
        {
            var blog = await _blogRepo.GetById(id);
            blog.Description = model.Description;
            blog.Title = model.Title;
            blog.IsFeatured = model.IsFeatured;
            blog.Image = model.Image;
            blog.Content = model.Content;
            _blogRepo.Updates(blog);
            await _blogRepo.SaveChangesAsync();
            return Ok(model);
        }

        [HttpDelete]
        public async Task<IActionResult>DeleteBlog(int id)
        {
            await _blogRepo.DeleteAsync(id);
            
            return Ok($"Deleted with {id} successfully");
        }


        [HttpGet("{featured}")]
        public async Task<IActionResult>GetFeaturedBlogs()
        {
            var blogs = await _blogRepo.GetAll(x => x.IsFeatured == true);
            return Ok(blogs);
        }
    }
}
