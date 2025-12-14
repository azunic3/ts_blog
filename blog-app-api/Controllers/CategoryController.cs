using BlogAppAPI.Models.Domain;
using BlogAppAPI.Models.DTO;
using BlogAppAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BlogAppAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryRepository _categoryRepository;

		public CategoryController(ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		[HttpGet]
		[AllowAnonymous]
		[SwaggerOperation("Get All Categories")]
		public async Task<IActionResult> Get()
		{
			var categories = await _categoryRepository.Get();
			return Ok(categories);
		}

		[HttpGet("{id:guid}")]
		[AllowAnonymous]
		[SwaggerOperation("Get Category by Id")]
		public async Task<IActionResult> GetById(Guid id)
		{
			var category = await _categoryRepository.Get(id);
			if (category == null)
				return NotFound(new ErrorResponseDto("Category not found."));

			return Ok(category);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[SwaggerOperation("Create Category")]
		public async Task<IActionResult> Create([FromBody] CategoryCreateDto category)
		{
			if (string.IsNullOrWhiteSpace(category?.Name) || string.IsNullOrWhiteSpace(category?.UrlHandle))
				return BadRequest(new ErrorResponseDto("Name or UrlHandle is empty."));

			var newCategory = new Category
			{
				Name = category.Name,
				UrlHandle = category.UrlHandle
			};

			await _categoryRepository.Create(newCategory);

			return StatusCode(201, new CategoryResponseDto
			{
				StatusCode = 201,
				Message = "Created",
				Data = newCategory
			});
		}

		[HttpPut("{id:guid}")]
		[Authorize(Roles = "Admin")]
		[SwaggerOperation("Update Category by Id")]
		public async Task<IActionResult> Update(Guid id, [FromBody] CategoryCreateDto category)
		{
			var existingCategory = await _categoryRepository.Get(id);
			if (existingCategory == null)
				return NotFound(new ErrorResponseDto("Category not found."));

			if (string.IsNullOrWhiteSpace(category?.Name) || string.IsNullOrWhiteSpace(category?.UrlHandle))
				return BadRequest(new ErrorResponseDto("Name or UrlHandle is empty."));

			existingCategory.Name = category.Name;
			existingCategory.UrlHandle = category.UrlHandle;

			await _categoryRepository.Update(existingCategory);
			return NoContent();
		}

		[HttpDelete("{id:guid}")]
		[Authorize(Roles = "Admin")]
		[SwaggerOperation("Delete Category by Id")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var existingCategory = await _categoryRepository.Get(id);
			if (existingCategory == null)
				return NotFound(new ErrorResponseDto("Category not found."));

			await _categoryRepository.Delete(id);
			return NoContent();
		}
	}
}