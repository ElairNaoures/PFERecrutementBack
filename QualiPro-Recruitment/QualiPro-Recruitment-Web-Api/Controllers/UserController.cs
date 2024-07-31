using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;
using QualiPro_Recruitment_Web_Api.Repositories.UserRepo;

namespace QualiPro_Recruitment_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var ListUsers = await _userRepository.GetAllUsers();
            return Ok(ListUsers);

        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserId(int userId)
        {
            var User = await _userRepository.GetUserById(userId);
            return Ok(User);


        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserDTO userInput)
        {
            TabUser User = await _userRepository.AddUser(userInput);
            return Ok(User);


        }


        
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int userId, [FromBody] TabUser userInput)
        {
            var User = await _userRepository.UpdateUser(userId,userInput);
            return Ok(User);


        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var deletedUser = await _userRepository.DeleteUser(userId);
            if (deletedUser == null)
            {
                return NotFound("User not found");
            }
            return Ok("User deleted successfully");
        }

    }
}
