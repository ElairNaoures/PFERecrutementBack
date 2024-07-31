using QualiPro_Recruitment_Web_Api.DTOs;

namespace QualiPro_Recruitment_Web_Api.Models
{
    public class AuthModel
    {
        public bool? Success { get; set; }
        public string? Message { get; set; }
        public string? AccessToken { get; set; }
        public UserAccountRoleModel? UserInfo { get; set; }
        public CondidatAccountRoleModel? CondidatInfo { get; set; }
    }
}
