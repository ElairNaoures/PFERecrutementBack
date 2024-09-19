namespace QualiPro_Recruitment_Web_Api.DTOs
{
    public class QuestionOptionDto
    {
        public int Id { get; set; }
        public string QuestionOptionName { get; set; } = null!;
        public int? QuestionId { get; set; }
    }
}
