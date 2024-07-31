namespace QualiPro_Recruitment_Web_Api.DTOs
{
    public class EducationDto
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Establishment { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? Description { get; set; }

        public int? CondidatId { get; set; }
    }
}
