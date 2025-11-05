using BlogAppAPI.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlogAppAPI.Models.Domain;
using BlogAppAPI.Models.DTO;
using Blog.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogImagesController : ControllerBase
    {
        private readonly IBlogImageRepository _blogImageRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _targetFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        public BlogImagesController(IBlogImageRepository blogImageRepository, IWebHostEnvironment webHostEnvironment)
        {
            _blogImageRepository = blogImageRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: api/images
        [HttpGet]
        //[Authorize(Roles = "Admin")]
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

        [HttpPost("Upload")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation("Upload Image")]
        public async Task<IActionResult> UploadImage(IFormFile file, [FromForm] string fileName, [FromForm] string title)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            if (string.IsNullOrEmpty(fileName))
                return BadRequest("File name is required.");

            if (string.IsNullOrEmpty(title))
                return BadRequest("Title is required.");

            if (!Directory.Exists(_targetFolder))
            {
                Directory.CreateDirectory(_targetFolder);
            }

            var fileExtension = Path.GetExtension(file.FileName);
            ValidateFileMethod(file);
            if (ModelState.IsValid)
            {

                // Generate a unique filename with the provided fileName
                var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{fileExtension}";

                var filePath = Path.Combine(_targetFolder, uniqueFileName);

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Generate the file URL
                var fileUrl = $"{Request.Scheme}://{Request.Host}/images/{uniqueFileName}";

                // Create a new BlogImage entry
                var blogImage = new BlogImage
                {
                    Id = Guid.NewGuid(),
                    FileName = uniqueFileName,
                    FileExtension = fileExtension,
                    Title = title,
                    Url = fileUrl,
                    DateCreated = DateTime.Now
                };

                // Save the BlogImage entry to the database
                await _blogImageRepository.SaveImage(blogImage);

                return Ok(new { filePath = fileUrl });
            }
            return BadRequest(ModelState);
        }

        private void ValidateFileMethod(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format.");
            }
            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size can't be more than 10MB.");
            }
        }
    }


}

