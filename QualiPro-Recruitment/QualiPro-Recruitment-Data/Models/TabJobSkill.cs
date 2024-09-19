using System;
using System.Collections.Generic;

namespace QualiPro_Recruitment_Data.Models;

public partial class TabJobSkill
{
    public int Id { get; set; }

    public int JobId { get; set; }

    public int SkillsId { get; set; }

    public virtual TabJob Job { get; set; } = null!;

    public virtual TabSkill Skills { get; set; } = null!;
}
