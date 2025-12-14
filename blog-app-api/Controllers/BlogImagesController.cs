using BlogAppAPI.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using BlogAppAPI.Models.Domain;
using BlogAppAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace BlogAppAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BlogImagesController : ControllerBase
	{
		private readonly IBlogImageRepository _blogImageRepository;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public BlogImagesController(IBlogImageRepository blogImageRepository,IWebHostEnvironment webHostEnvironment)
		{
			_blogImageRepository = blogImageRepository;
			_webHostEnvironment = webHostEnvironment;
		}

		[Authorize(Roles = "Admin")]
		[HttpGet]
		[SwaggerOperation("Get All Images")]
		public async Task<ActionResult<IEnumerable<BlogImageDto>>> GetAllImages()
		{
			var images = await _blogImageRepository.GetAllImages();

			var imagesDto = images.Select(image => new BlogImageDto
			{
				Id = image.Id,
				FileName = image.FileName,
				FileExtension = image.FileExtension,
				Title = image.Title,
				Url = image.Url,
				DateCreated = image.DateCreated
			}).ToList();

			return Ok(imagesDto);
		}

		[Authorize]
		[HttpPost("Upload")]
		[SwaggerOperation("Upload Image")]
		public async Task<IActionResult> UploadImage(
			[FromForm] IFormFile file,
			[FromForm] string fileName,
			[FromForm] string title)
		{
			if (file == null || file.Length == 0)
				return BadRequest("No file uploaded.");

			if (string.IsNullOrWhiteSpace(fileName))
				return BadRequest("File name is required.");

			if (string.IsNullOrWhiteSpace(title))
				return BadRequest("Title is required.");

			ValidateFileMethod(file);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var targetFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
			if (!Directory.Exists(targetFolder))
				Directory.CreateDirectory(targetFolder);

			var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
			var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{fileExtension}";
			var fullPath = Path.Combine(targetFolder, uniqueFileName);

			using (var stream = new FileStream(fullPath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			var fileUrl = $"{Request.Scheme}://{Request.Host}/images/{uniqueFileName}";

			var blogImage = new BlogImage
			{
				Id = Guid.NewGuid(),
				FileName = uniqueFileName,
				FileExtension = fileExtension,
				Title = title,
				Url = fileUrl,
				DateCreated = DateTime.UtcNow
			};

			await _blogImageRepository.SaveImage(blogImage);

			return Ok(new { filePath = fileUrl });
		}

		private void ValidateFileMethod(IFormFile file)
		{
			var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
			var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

			if (!allowedExtensions.Contains(ext))
				ModelState.AddModelError("file", "Unsupported file format.");

			if (file.Length > 10 * 1024 * 1024)
				ModelState.AddModelError("file", "File size can't be more than 10MB.");
		}
	}
}