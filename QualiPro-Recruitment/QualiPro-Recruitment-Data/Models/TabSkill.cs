using System;
using System.Collections.Generic;

namespace QualiPro_Recruitment_Data.Models;

public partial class TabSkill
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool? TechnicalSkill { get; set; }

    public bool? SoftSkill { get; set; }

    public bool? ToolsSkill { get; set; }
}
