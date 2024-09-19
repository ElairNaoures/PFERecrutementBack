namespace QualiPro_Recruitment_Web_Api.DTOs
{
    public class QuizEvaluationDto
    {
        public int Id { get; set; }
        public int? JobApplicationId { get; set; }
        public int? QuizId { get; set; }
        public int? QuestionId { get; set; }
        public int? QuestionOptionId { get; set; }
        public int? IdReponse { get; set; }
        //public string? CorrectQuestionOptionName { get; set; }
        //public string? ReponseOptionName { get; set; }
        //public string? QuestionName { get; set; }
        

    }
}
