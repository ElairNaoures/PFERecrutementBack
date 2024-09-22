namespace QualiPro_Recruitment_Web_Api.DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string QuestionName { get; set; } = null!;
        public int? QuizId { get; set; }
        public int? Coefficient { get; set; }
        public int? CorrectQuestionOptionId { get; set; }
        public string CorrectQuestionOptionName { get; set; } = string.Empty; 

        public List<QuestionOptionDto> QuestionOptions { get; set; } = new List<QuestionOptionDto>();


    }
}
