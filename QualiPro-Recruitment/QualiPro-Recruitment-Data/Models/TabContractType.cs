using System;
using System.Collections.Generic;

namespace QualiPro_Recruitment_Data.Models;

public partial class TabContractType
{
    public int Id { get; set; }

    public string Designation { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public bool? Deleted { get; set; }

    public virtual ICollection<TabJob> TabJobs { get; set; } = new List<TabJob>();
}
