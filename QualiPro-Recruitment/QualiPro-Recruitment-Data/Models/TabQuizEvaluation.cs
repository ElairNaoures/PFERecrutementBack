using System;
using System.Collections.Generic;

namespace QualiPro_Recruitment_Data.Models;

public partial class TabQuizEvaluation
{
    public int Id { get; set; }

    public int? JobApplicationId { get; set; }

    public int? QuizId { get; set; }

    public int? QuestionId { get; set; }

    public int? QuestionOptionId { get; set; }

    public int? IdReponse { get; set; }

    public virtual TabJobApplication? JobApplication { get; set; }

    public virtual TabQuestion? Question { get; set; }

    public virtual TabQuestionOption? QuestionOption { get; set; }

    public virtual TabQuiz? Quiz { get; set; }
}
