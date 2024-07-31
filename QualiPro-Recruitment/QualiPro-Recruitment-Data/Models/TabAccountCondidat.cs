using System;
using System.Collections.Generic;

namespace QualiPro_Recruitment_Data.Models;

public partial class TabAccountCondidat
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public bool? Blocked { get; set; }

    public int? CondidatId { get; set; }

    public virtual TabCondidat? Condidat { get; set; }
}
