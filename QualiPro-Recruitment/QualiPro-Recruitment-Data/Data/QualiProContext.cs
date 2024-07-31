using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Models;

namespace QualiPro_Recruitment_Data.Data;

public partial class QualiProContext : DbContext
{
    public QualiProContext()
    {
    }

    public QualiProContext(DbContextOptions<QualiProContext> options)
        : base(options)
    {
    }

    public virtual DbSet<NewTabContractType> NewTabContractTypes { get; set; }

    public virtual DbSet<ProfessionalExperience> ProfessionalExperiences { get; set; }

    public virtual DbSet<TabAccount> TabAccounts { get; set; }

    public virtual DbSet<TabAccountCondidat> TabAccountCondidats { get; set; }

    public virtual DbSet<TabCondidat> TabCondidats { get; set; }

    public virtual DbSet<TabContractType> TabContractTypes { get; set; }

    public virtual DbSet<TabEducation> TabEducations { get; set; }

    public virtual DbSet<TabJob> TabJobs { get; set; }

    public virtual DbSet<TabModule> TabModules { get; set; }

    public virtual DbSet<TabModuleRole> TabModuleRoles { get; set; }

    public virtual DbSet<TabRole> TabRoles { get; set; }

    public virtual DbSet<TabSkill> TabSkills { get; set; }

    public virtual DbSet<TabUser> TabUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-APR3B4P\\PFE;Database=qualipro_recruitment;Trusted_Connection=True;Encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NewTabContractType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__new_tab___3213E83F5023A1BA");

            entity.ToTable("new_tab_contract_type");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Designation)
                .HasMaxLength(255)
                .HasColumnName("designation");
        });

        modelBuilder.Entity<ProfessionalExperience>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__professi__3213E83F659E3AD6");

            entity.ToTable("professional_experience");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Company)
                .HasMaxLength(255)
                .HasColumnName("company");
            entity.Property(e => e.CondidatId).HasColumnName("condidat_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Condidat).WithMany(p => p.ProfessionalExperiences)
                .HasForeignKey(d => d.CondidatId)
                .HasConstraintName("FK__professio__condi__5EBF139D");
        });

        modelBuilder.Entity<TabAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tab_acco__3213E83FDAD5A188");

            entity.ToTable("tab_account");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Blocked).HasColumnName("blocked");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.TabAccounts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__tab_accou__user___267ABA7A");
        });

        modelBuilder.Entity<TabAccountCondidat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tab_acco__3213E83FCE597FCE");

            entity.ToTable("tab_account_condidat");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Blocked).HasColumnName("blocked");
            entity.Property(e => e.CondidatId).HasColumnName("condidat_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");

            entity.HasOne(d => d.Condidat).WithMany(p => p.TabAccountCondidats)
                .HasForeignKey(d => d.CondidatId)
                .HasConstraintName("FK__tab_accou__condi__693CA210");
        });

        modelBuilder.Entity<TabCondidat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tab_cond__3213E83F4A944C0A");

            entity.ToTable("tab_condidat");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Birthdate)
                .HasColumnType("datetime")
                .HasColumnName("birthdate");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("country");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone_number");
            entity.Property(e => e.Summary)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("summary");
        });

        modelBuilder.Entity<TabContractType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tab_cont__3213E83FA4719E5F");

            entity.ToTable("tab_contract_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Designation)
                .HasMaxLength(255)
                .HasColumnName("designation");
        });

        modelBuilder.Entity<TabEducation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tab_educ__3213E83FCC163CBF");

            entity.ToTable("tab_education");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CondidatId).HasColumnName("condidat_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.Establishment)
                .HasMaxLength(255)
                .HasColumnName("establishment");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Condidat).WithMany(p => p.TabEducations)
                .HasForeignKey(d => d.CondidatId)
                .HasConstraintName("FK__tab_educa__condi__6477ECF3");
        });

        modelBuilder.Entity<TabJob>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tab_job__3213E83F800A05C6");

            entity.ToTable("tab_job");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContractTypeId).HasColumnName("contract_type_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EducationLevel)
                .HasMaxLength(255)
                .HasColumnName("education_level");
            entity.Property(e => e.ExpirationDate)
                .HasColumnType("datetime")
                .HasColumnName("expiration_date");
            entity.Property(e => e.Languages)
                .HasMaxLength(255)
                .HasColumnName("languages");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.YearsOfExperience)
                .HasMaxLength(50)
                .HasColumnName("years_of_experience");

            entity.HasOne(d => d.ContractType).WithMany(p => p.TabJobs)
                .HasForeignKey(d => d.ContractTypeId)
                .HasConstraintName("FK__tab_job__contrac__403A8C7D");

            entity.HasOne(d => d.User).WithMany(p => p.TabJobs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__tab_job__user_id__3F466844");
        });

        modelBuilder.Entity<TabModule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tab_modu__3213E83F0D2F8FE5");

            entity.ToTable("tab_module");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ModuleName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("module_name");
        });

        modelBuilder.Entity<TabModuleRole>(entity =>
        {
            entity.HasKey(e => new { e.ModuleId, e.RoleId }).HasName("PK__tab_modu__CD4D900FB5014DAE");

            entity.ToTable("tab_module_role");

            entity.Property(e => e.ModuleId).HasColumnName("module_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.AllowAdd).HasColumnName("allow_add");
            entity.Property(e => e.AllowDelete).HasColumnName("allow_delete");
            entity.Property(e => e.AllowUpdate).HasColumnName("allow_update");
            entity.Property(e => e.AllowView).HasColumnName("allow_view");

            entity.HasOne(d => d.Module).WithMany(p => p.TabModuleRoles)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tab_modul__modul__2E1BDC42");

            entity.HasOne(d => d.Role).WithMany(p => p.TabModuleRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tab_modul__role___2F10007B");
        });

        modelBuilder.Entity<TabRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tab_role__3213E83FCEA8784A");

            entity.ToTable("tab_role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<TabSkill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tab_skil__3213E83FA2C51495");

            entity.ToTable("tab_skill");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.SoftSkill).HasColumnName("soft_skill");
            entity.Property(e => e.TechnicalSkill).HasColumnName("technical_skill");
            entity.Property(e => e.ToolsSkill).HasColumnName("tools_skill");
        });

        modelBuilder.Entity<TabUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tab_user__3213E83F82ACE118");

            entity.ToTable("tab_user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Birthdate)
                .HasColumnType("datetime")
                .HasColumnName("birthdate");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("country");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone_number");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.TabUsers)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
