using BlogAppAPI.Models.DTO;
using BlogAppAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BlogAppAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BlogPostController : ControllerBase
	{
		private readonly IBlogRepository _blogPostRepository;

		public BlogPostController(IBlogRepository blogPostRepository)
		{
			_blogPostRepository = blogPostRepository;
		}

		[HttpGet("Visible")]
		[AllowAnonymous]
		[SwaggerOperation("Get Visible Blog Posts by Page")]
		public async Task<IActionResult> GetVisible(int page = 1)
		{
			var blogPosts = await _blogPostRepository.GetVisible(page);
			var numberOfBlogPosts = _blogPostRepository.CountVisible();

			var result = new BlogPostGetResponseDto
			{
				Page = page,
				BlogPosts = blogPosts
			};

			if (blogPosts.Count < 6 || numberOfBlogPosts == page * 6)
				result.Page = -1;

			return Ok(result);
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		[SwaggerOperation("Get All Blog Posts (Admin)")]
		public async Task<IActionResult> GetAll(int page = 1)
		{
			var blogPosts = await _blogPostRepository.Get(page);
			var numberOfBlogPosts = _blogPostRepository.Count();

			var result = new BlogPostGetResponseDto
			{
				Page = page,
				BlogPosts = blogPosts
			};

			if (blogPosts.Count < 6 || numberOfBlogPosts == page * 6)
				result.Page = -1;

			return Ok(result);
		}

		[HttpGet("{id:guid}")]
		[AllowAnonymous]
		[SwaggerOperation("Get Blog Post by Id")]
		public async Task<IActionResult> GetById(Guid id)
		{
			var blogPost = await _blogPostRepository.Get(id);
			if (blogPost == null)
				return NotFound(new ErrorResponseDto("Blog post not found."));

			if (!blogPost.IsVisible)
			{
				if (!User.Identity?.IsAuthenticated ?? true)
					return Unauthorized();

				var isAdmin = User.IsInRole("Admin");
				var username = User.Identity?.Name;

				if (!isAdmin && blogPost.Author != username)
					return Forbid();
			}

			return Ok(blogPost);
		}

		[HttpGet("by-url/{urlHandle}")]
		[AllowAnonymous]
		[SwaggerOperation("Get Blog Post by UrlHandle")]
		public async Task<IActionResult> GetByUrlHandle(string urlHandle)
		{
			var blogPost = await _blogPostRepository.GetByUrl(urlHandle);
			if (blogPost == null)
				return NotFound(new ErrorResponseDto("Blog post not found."));

			return Ok(blogPost);
		}

		[HttpPost]
		[Authorize]
		[SwaggerOperation("Create Blog Post")]
		public async Task<IActionResult> Create([FromBody] BlogPostCreateDto blogPost)
		{
			if (blogPost == null)
				return BadRequest(new ErrorResponseDto("Data is empty."));

			var username = User.Identity?.Name;
			if (string.IsNullOrWhiteSpace(username))
				return Unauthorized(new ErrorResponseDto("Missing user identity."));

			blogPost.Author = username;

			await _blogPostRepository.Create(blogPost);

			return StatusCode(201, new BlogPostResponseDto
			{
				StatusCode = 201,
				Message = "Created"
			});
		}

		[HttpPut("{id:guid}")]
		[Authorize]
		[SwaggerOperation("Update Blog Post by Id")]
		public async Task<IActionResult> Update(Guid id, [FromBody] BlogPostCreateDto blogPost)
		{
			var existing = await _blogPostRepository.Get(id);
			if (existing == null)
				return NotFound(new ErrorResponseDto("Blog post not found."));

			var isAdmin = User.IsInRole("Admin");
			var username = User.Identity?.Name;

			if (!isAdmin && existing.Author != username)
				return Forbid();

			blogPost.Author = existing.Author;

			await _blogPostRepository.Update(id, blogPost);
			return NoContent();
		}

		[HttpDelete("{id:guid}")]
		[Authorize]
		[SwaggerOperation("Delete Blog Post by Id")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var existing = await _blogPostRepository.Get(id);
			if (existing == null)
				return NotFound(new ErrorResponseDto("Blog post not found."));

			var isAdmin = User.IsInRole("Admin");
			var username = User.Identity?.Name;

			if (!isAdmin && existing.Author != username)
				return Forbid();

			await _blogPostRepository.Delete(id);
			return NoContent();
		}

		[HttpGet("mine")]
		[Authorize]
		[SwaggerOperation("Get My Blog Posts by Page")]
		public async Task<IActionResult> GetMine(int page = 1)
		{
			var username = User.Identity?.Name;
			if (string.IsNullOrWhiteSpace(username))
				return Unauthorized(new ErrorResponseDto("Missing user identity."));

			var isAdmin = User.IsInRole("Admin");

			var blogPosts = await _blogPostRepository.GetMine(page, username, isAdmin);
			var numberOfBlogPosts = await _blogPostRepository.CountMine(username, isAdmin);

			var result = new BlogPostGetResponseDto
			{
				Page = page,
				BlogPosts = blogPosts
			};

			if (blogPosts.Count < 6 || numberOfBlogPosts == page * 6)
				result.Page = -1;

			return Ok(result);
		}
	}
}