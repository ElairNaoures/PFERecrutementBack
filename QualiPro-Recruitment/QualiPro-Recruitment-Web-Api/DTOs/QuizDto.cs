namespace QualiPro_Recruitment_Web_Api.DTOs
{
    public class QuizDto
    {
        public int Id { get; set; }

        public string QuizName { get; set; } = null!;
        public int JobProfileId { get; set; }
        public int QuestionId { get; set; }
        public int QuizEvaluationId { get; set; }
        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();


    }
}



