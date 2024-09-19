using System;
using System.Collections.Generic;

namespace QualiPro_Recruitment_Data.Models;

public partial class TabRole
{
    public int Id { get; set; }

    public string? RoleName { get; set; }

    public bool? Deleted { get; set; }

    public virtual ICollection<TabModuleRole> TabModuleRoles { get; set; } = new List<TabModuleRole>();

    public virtual ICollection<TabUser> TabUsers { get; set; } = new List<TabUser>();
}
