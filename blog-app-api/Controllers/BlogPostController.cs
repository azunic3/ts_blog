using BlogAppAPI.Data;
using BlogAppAPI.Models.Domain;
using BlogAppAPI.Models.DTO;
using BlogAppAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BlogAppAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogRepository _blogPostRepository;

        public BlogPostController(IBlogRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        [HttpGet("Visible")]
        [SwaggerOperation("Get Visible Blog Posts by Page")]
        public async Task<IActionResult> GetVisible(int page = 1)
        {
            var blogPosts = await _blogPostRepository.GetVisible(page);
            var numberOfBlogPosts = _blogPostRepository.CountVisible();

            var result = new BlogPostGetResponseDto
            {
                Page = page,
                BlogPosts = blogPosts,
            };

            if (blogPosts.Count < 6 || numberOfBlogPosts == page * 6)
            {
                result.Page = -1;
            }

            return Ok(result);
        }

        [HttpGet]
        [SwaggerOperation("Get Blog Posts by Page")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(int page = 1)
        {
            var blogPosts = await _blogPostRepository.Get(page);
            var numberOfBlogPosts = _blogPostRepository.Count();

            var result = new BlogPostGetResponseDto
            {
                Page = page,
                BlogPosts = blogPosts,
            };

            if (blogPosts.Count < 6 || numberOfBlogPosts == page * 6)
            {
                result.Page = -1;
            }

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation("Get Blog Post by Id")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var blogPost = await _blogPostRepository.Get(id);
            if (blogPost == null)
            {
                return NotFound(new ErrorResponseDto("Blog post not found."));
            }
            
            return Ok(blogPost);
        }

        [HttpGet("{urlHandle}")]
        [SwaggerOperation("Get Blog Post by UrlHandle")]
        public async Task<IActionResult> GetByUrlHandle(string urlHandle)
        {
            var blogPost = await _blogPostRepository.GetByUrl(urlHandle);
            if (blogPost == null)
            {
                return NotFound(new ErrorResponseDto("Blog post not found."));
            }

            return Ok(blogPost);
        }

        [HttpPost]
        [SwaggerOperation("Create Blog Post")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(BlogPostCreateDto blogPost)
        {
            if (blogPost == null)
            {
                return BadRequest(new ErrorResponseDto("Data is empty."));
            }

            await _blogPostRepository.Create(blogPost);

            var response = new BlogPostResponseDto
            {
                StatusCode = 201,
                Message = "Created",
            };

            return StatusCode(201, response);
        }

        [HttpPut("{id}")]
        [SwaggerOperation("Update Blog Post by Id")]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, BlogPostCreateDto blogPost)
        {
            await _blogPostRepository.Update(id, blogPost);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("Delete Blog Post by Id")]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingBlogPost = await _blogPostRepository.Get(id);
            if (existingBlogPost == null)
            {
                return NotFound(new ErrorResponseDto("Blog post not found."));
            }

            await _blogPostRepository.Delete(id);
            return NoContent();
        }
    }
}
