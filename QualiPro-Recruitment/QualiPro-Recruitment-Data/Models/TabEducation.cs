using System;
using System.Collections.Generic;

namespace QualiPro_Recruitment_Data.Models;

public partial class TabEducation
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Establishment { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Description { get; set; }

    public int? CondidatId { get; set; }

    public virtual TabCondidat? Condidat { get; set; }
}
