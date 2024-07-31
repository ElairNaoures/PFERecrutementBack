using System;
using System.Collections.Generic;

namespace QualiPro_Recruitment_Data.Models;

public partial class TabAccount
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public bool? Blocked { get; set; }

    public int? UserId { get; set; }

    public virtual TabUser? User { get; set; }
}
