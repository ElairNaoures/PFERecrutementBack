using System;
using System.Collections.Generic;

namespace QualiPro_Recruitment_Data.Models;

public partial class TabJobApplication
{
    public int Id { get; set; }

    public int? CondidatId { get; set; }

    public int? JobId { get; set; }

    public DateTime? MeetingDate { get; set; }

    public int? HeadToHeadInterviewNote { get; set; }

    public bool? Deleted { get; set; }

    public int? Score { get; set; }

    public virtual TabCondidat? Condidat { get; set; }

    public virtual TabJob? Job { get; set; }

    public virtual ICollection<TabQuizEvaluation> TabQuizEvaluations { get; set; } = new List<TabQuizEvaluation>();
}
