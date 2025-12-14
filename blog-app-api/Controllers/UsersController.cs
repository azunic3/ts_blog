using BlogAppAPI.Models.DTO;
using BlogAppAPI.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BlogAppAPI.Controllers
{
    [ApiController]
    [Route("api/Users")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [SwaggerOperation("Get All Users")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                return Ok(users);
            }
            catch
            {
                return StatusCode(500, new { error = "Error retrieving users. Please try again later." });
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("Delete User by Id")]
        public async Task<IActionResult> Delete(string id)
        {
            var exists = await _userRepository.ExistsAsync(id);
            if (!exists)
                return NotFound(new ErrorResponseDto("User not found."));

            await _userRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
