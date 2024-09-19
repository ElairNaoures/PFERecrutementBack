using System;
using System.Collections.Generic;

namespace QualiPro_Recruitment_Data.Models;

public partial class TabProfileJob
{
    public int Id { get; set; }

    public string ProfileName { get; set; } = null!;

    public int? QuizId { get; set; }

    public virtual TabQuiz? Quiz { get; set; }

    public virtual ICollection<TabJob> TabJobs { get; set; } = new List<TabJob>();
}
