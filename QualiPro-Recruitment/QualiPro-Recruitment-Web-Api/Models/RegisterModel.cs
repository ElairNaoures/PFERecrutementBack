using QualiPro_Recruitment_Web_Api.DTOs;

namespace QualiPro_Recruitment_Web_Api.Models
{
    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public UserDTO User { get; set; }
    }
}
