using System;
using System.Collections.Generic;

namespace QualiPro_Recruitment_Data.Models;

public partial class Notification
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CondidatId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool IsRead { get; set; }

    public virtual TabCondidat Condidat { get; set; } = null!;

    public virtual TabUser User { get; set; } = null!;
}
