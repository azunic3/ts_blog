using BlogAppAPI.Data;
using BlogAppAPI.Models.Domain;
using BlogAppAPI.Models.DTO;
using BlogAppAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

// From the controller, I define the routes, and call the repository CRUD Methods. 
namespace BlogAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [SwaggerOperation("Get All Categories")]
        public async Task<IActionResult> Get()       {
            try
            {
                var categories = await _categoryRepository.Get();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error retrieving blog posts. Please try again later." });
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Get Category by Id")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _categoryRepository.Get(id);
            if (category == null)
            {
                return NotFound(new ErrorResponseDto("Category not found."));
            }

            return Ok(category);
        }

        [HttpPost]
        [SwaggerOperation("Create Category")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CategoryCreateDto category)
        {
            if (category.Name == "" || category.UrlHandle == "")
            {
                return BadRequest(new ErrorResponseDto("Name or UrlHandle is empty."));
            }

            var newCategory = new Category
            {
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            await _categoryRepository.Create(newCategory);

            var response = new CategoryResponseDto
            {
                StatusCode = 201,
                Message = "Created",
                Data = newCategory
            };

            return StatusCode(201, response);
        }

        [HttpPut("{id}")]
        [SwaggerOperation("Update Category by Id")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, CategoryCreateDto category)
        {
            var existingCategory = await _categoryRepository.Get(id);
            if (existingCategory == null)
            {
                return NotFound(new ErrorResponseDto("Category not found."));
            }

            if (category.Name == "" || category.UrlHandle == "")
            {
                return BadRequest(new ErrorResponseDto("Name or UrlHandle is empty."));
            }

            existingCategory.Name = category.Name;
            existingCategory.UrlHandle = category.UrlHandle;

            await _categoryRepository.Update(existingCategory);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("Delete Category by Id")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingCategory = await _categoryRepository.Get(id);
            if (existingCategory == null)
            {
                return NotFound(new ErrorResponseDto("Category not found."));
            }

            await _categoryRepository.Delete(id);
            return NoContent();
        }
    }
}
