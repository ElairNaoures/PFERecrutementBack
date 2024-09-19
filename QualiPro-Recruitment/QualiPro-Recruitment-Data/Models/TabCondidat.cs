using System;
using System.Collections.Generic;

namespace QualiPro_Recruitment_Data.Models;

public partial class TabCondidat
{
    public int Id { get; set; }

    public string? Summary { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Country { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime? Birthdate { get; set; }

    public string? ImageFileName { get; set; }

    public string? CvFileName { get; set; }

    public bool? Deleted { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<ProfessionalExperience> ProfessionalExperiences { get; set; } = new List<ProfessionalExperience>();

    public virtual ICollection<TabAccountCondidat> TabAccountCondidats { get; set; } = new List<TabAccountCondidat>();

    public virtual ICollection<TabEducation> TabEducations { get; set; } = new List<TabEducation>();

    public virtual ICollection<TabJobApplication> TabJobApplications { get; set; } = new List<TabJobApplication>();
}
