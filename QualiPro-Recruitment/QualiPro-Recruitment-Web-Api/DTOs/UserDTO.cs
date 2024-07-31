namespace QualiPro_Recruitment_Web_Api.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Country { get; set; }

        public string? PhoneNumber { get; set; }

         public DateTime? Birthdate { get; set; }

        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
    }
}
