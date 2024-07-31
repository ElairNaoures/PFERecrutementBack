using System;
using System.Collections.Generic;

namespace QualiPro_Recruitment_Data.Models;

public partial class TabUser
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Country { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime? Birthdate { get; set; }

    public int? RoleId { get; set; }

    public virtual TabRole? Role { get; set; }

    public virtual ICollection<TabAccount> TabAccounts { get; set; } = new List<TabAccount>();

    public virtual ICollection<TabJob> TabJobs { get; set; } = new List<TabJob>();
}
