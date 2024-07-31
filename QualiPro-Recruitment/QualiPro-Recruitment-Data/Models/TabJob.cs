using System;
using System.Collections.Generic;

namespace QualiPro_Recruitment_Data.Models;

public partial class TabJob
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? UserId { get; set; }

    public string? YearsOfExperience { get; set; }

    public string? Languages { get; set; }

    public string? EducationLevel { get; set; }

    public int? ContractTypeId { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual TabContractType? ContractType { get; set; }

    public virtual TabUser? User { get; set; }
}
