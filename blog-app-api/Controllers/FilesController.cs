using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
	private readonly IWebHostEnvironment _env;

	public FilesController(IWebHostEnvironment env)
	{
		_env = env;
	}

	[Authorize]
	[HttpPost("upload")]
	public async Task<IActionResult> Upload(IFormFile file)
	{
		if (file == null || file.Length == 0)
			return BadRequest("No file");

		var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
		var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

		if (!allowedExtensions.Contains(extension))
			return BadRequest("Unsupported file type");

		if (file.Length > 10 * 1024 * 1024)
			return BadRequest("File too large");

		var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
		if (!Directory.Exists(uploadsPath))
			Directory.CreateDirectory(uploadsPath);

		var fileName = $"{Guid.NewGuid()}{extension}";
		var filePath = Path.Combine(uploadsPath, fileName);

		using (var stream = new FileStream(filePath, FileMode.Create))
		{
			await file.CopyToAsync(stream);
		}

		var url = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
		return Ok(new { url });
	}
}