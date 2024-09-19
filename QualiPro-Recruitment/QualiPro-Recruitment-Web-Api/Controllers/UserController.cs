using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;
using QualiPro_Recruitment_Web_Api.Models;
using QualiPro_Recruitment_Web_Api.Repositories.AuthRepo;
using QualiPro_Recruitment_Web_Api.Repositories.UserRepo;

namespace QualiPro_Recruitment_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;


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

        [HttpPost("add-user")]

        public async Task<IActionResult> AddUser(UserAccountRoleModel userAccountRoleModel)
        {
            try
            {
                if (userAccountRoleModel != null && userAccountRoleModel.Email != null && userAccountRoleModel.Password != null)
                {
                    AuthModel result = await _authRepository.SignUp(userAccountRoleModel);

                    if (result.Success == true && result.Success.Value && result.AccessToken != null && result.UserInfo != null)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





        //private string HashPassword(string password)
        //{
        //    return BCrypt.Net.BCrypt.HashPassword(password);
        //}



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromForm] UserDTO userDTO, IFormFile imageFile)
        {
            if (id != userDTO.Id)
            {
                return BadRequest();
            }
            if (imageFile != null)
            {
                var imageFilePath = Path.Combine("UploadedImages", imageFile.FileName);
                using (var stream = new FileStream(imageFilePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                userDTO.ImageFileName = imageFile.FileName;
            }
            await _userRepository.UpdateUser(userDTO);
            return NoContent();


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
