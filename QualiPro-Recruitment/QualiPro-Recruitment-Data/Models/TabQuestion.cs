using System;
using System.Collections.Generic;

namespace QualiPro_Recruitment_Data.Models;

public partial class TabQuestion
{
    public int Id { get; set; }

    public string QuestionName { get; set; } = null!;

    public int? QuizId { get; set; }

    public int? Coefficient { get; set; }

    public int? CorrectQuestionOptionId { get; set; }

    public bool? Deleted { get; set; }

    public virtual TabQuestionOption? CorrectQuestionOption { get; set; }

    public virtual TabQuiz? Quiz { get; set; }

    public virtual ICollection<TabQuestionOption> TabQuestionOptions { get; set; } = new List<TabQuestionOption>();

    public virtual ICollection<TabQuizEvaluation> TabQuizEvaluations { get; set; } = new List<TabQuizEvaluation>();
}
