using QualiPro_Recruitment_Data.Models;

namespace QualiPro_Recruitment_Web_Api.Models
{
    public class UserAccountRoleModel
    {
        public int UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Country { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? Birthdate { get; set; }

        // account
        public int AccountId { get; set; }

        public string? Email { get; set; }
        public string? Password { get; set; }

        public bool? Blocked { get; set; }
        // role
        public int? RoleId { get; set; }

        public string? RoleName { get; set; }


        


    }
}
