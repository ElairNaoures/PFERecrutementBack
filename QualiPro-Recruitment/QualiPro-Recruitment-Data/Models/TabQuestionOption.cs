using System;
using System.Collections.Generic;

namespace QualiPro_Recruitment_Data.Models;

public partial class TabQuestionOption
{
    public int Id { get; set; }

    public string QuestionOptionName { get; set; } = null!;

    public int? QuestionId { get; set; }

    public virtual TabQuestion? Question { get; set; }

    public virtual ICollection<TabQuestion> TabQuestions { get; set; } = new List<TabQuestion>();

    public virtual ICollection<TabQuizEvaluation> TabQuizEvaluations { get; set; } = new List<TabQuizEvaluation>();
}
