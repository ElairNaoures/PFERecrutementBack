using System;
using System.Collections.Generic;

namespace QualiPro_Recruitment_Data.Models;

public partial class TabQuiz
{
    public int Id { get; set; }

    public string QuizName { get; set; } = null!;

    public bool? Deleted { get; set; }

    public virtual ICollection<TabProfileJob> TabProfileJobs { get; set; } = new List<TabProfileJob>();

    public virtual ICollection<TabQuestion> TabQuestions { get; set; } = new List<TabQuestion>();

    public virtual ICollection<TabQuizEvaluation> TabQuizEvaluations { get; set; } = new List<TabQuizEvaluation>();
}
