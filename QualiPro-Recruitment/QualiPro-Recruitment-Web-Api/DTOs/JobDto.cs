using QualiPro_Recruitment_Data.Models;
using System.ComponentModel.DataAnnotations;

namespace QualiPro_Recruitment_Web_Api.DTOs
{
    public class JobDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? YearsOfExperience { get; set; }
        public string? Languages { get; set; }
        public string? EducationLevel { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int ContractTypeId { get; set; }
        public int UserId { get; set; }
        public int JobProfileId { get; set; }

    }
}



