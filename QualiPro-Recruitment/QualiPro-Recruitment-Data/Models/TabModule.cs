using System;
using System.Collections.Generic;

namespace QualiPro_Recruitment_Data.Models;

public partial class TabModule
{
    public int Id { get; set; }

    public string? ModuleName { get; set; }

    public virtual ICollection<TabModuleRole> TabModuleRoles { get; set; } = new List<TabModuleRole>();
}
