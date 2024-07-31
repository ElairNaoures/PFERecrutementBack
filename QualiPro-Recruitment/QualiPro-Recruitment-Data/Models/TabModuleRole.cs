using System;
using System.Collections.Generic;

namespace QualiPro_Recruitment_Data.Models;

public partial class TabModuleRole
{
    public int ModuleId { get; set; }

    public int RoleId { get; set; }

    public bool? AllowAdd { get; set; }

    public bool? AllowUpdate { get; set; }

    public bool? AllowDelete { get; set; }

    public bool? AllowView { get; set; }

    public virtual TabModule Module { get; set; } = null!;

    public virtual TabRole Role { get; set; } = null!;
}
