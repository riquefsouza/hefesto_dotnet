using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace hefesto.admin.Models
{
    public partial class dbhefestoContext : DbContext
    {
        public dbhefestoContext(DbContextOptions<dbhefestoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdmMenu> AdmMenus { get; set; }
        public virtual DbSet<AdmPage> AdmPages { get; set; }
        public virtual DbSet<AdmPageProfile> AdmPageProfiles { get; set; }
        public virtual DbSet<AdmParameter> AdmParameters { get; set; }
        public virtual DbSet<AdmParameterCategory> AdmParameterCategories { get; set; }
        public virtual DbSet<AdmProfile> AdmProfiles { get; set; }
        public virtual DbSet<AdmUser> AdmUsers { get; set; }
        public virtual DbSet<AdmUserProfile> AdmUserProfiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseNpgsql("Host=localhost;Database=dbhefesto;Username=postgres;Password=abcd1234");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "C.UTF-8");

            modelBuilder.Entity<AdmMenu>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("adm_menu_pkey");

                entity.ToTable("adm_menu");

                entity.HasIndex(e => e.Description, "adm_menu_uk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("mnu_seq");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("mnu_description");

                entity.Property(e => e.Order).HasColumnName("mnu_order");

                entity.Property(e => e.IdPage).HasColumnName("mnu_pag_seq");

                entity.Property(e => e.IdMenuParent).HasColumnName("mnu_parent_seq");

                entity.HasOne(d => d.AdmPage)
                    .WithMany(p => p.AdmMenus)
                    .HasForeignKey(d => d.IdPage)
                    .HasConstraintName("adm_menu_page_fkey");

                entity.HasOne(d => d.AdmMenuParent)
                    .WithMany(p => p.InverseAdmMenuParent)
                    .HasForeignKey(d => d.IdMenuParent)
                    .HasConstraintName("adm_menu_parent_fkey");

                entity.Ignore(e => e.Url);   
            });

            modelBuilder.Entity<AdmPage>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("adm_page_pkey");

                entity.ToTable("adm_page");

                entity.HasIndex(e => e.Description, "adm_page_description_uk")
                    .IsUnique();

                entity.HasIndex(e => e.Url, "adm_page_url_uk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("pag_seq");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("pag_description");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("pag_url");
            });

            modelBuilder.Entity<AdmPageProfile>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("adm_page_profile_pkey");

                entity.ToTable("adm_page_profile");

                entity.HasIndex(e => new { e.IdPage, e.IdProfile }, "adm_page_profile_uk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("pgl_seq");

                entity.Property(e => e.IdPage).HasColumnName("pgl_pag_seq");

                entity.Property(e => e.IdProfile).HasColumnName("pgl_prf_seq");

                entity.HasOne(d => d.AdmPage)
                    .WithMany(p => p.AdmPageProfiles)
                    .HasForeignKey(d => d.IdPage)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("adm_pgl_page_fkey");

                entity.HasOne(d => d.AdmProfile)
                    .WithMany(p => p.AdmPageProfiles)
                    .HasForeignKey(d => d.IdProfile)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("adm_pgl_profile_fkey");
            });

            modelBuilder.Entity<AdmParameter>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("adm_parameter_pkey");

                entity.ToTable("adm_parameter");

                entity.HasIndex(e => e.Description, "adm_parameter_uk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("par_seq");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("par_code");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("par_description");

                entity.Property(e => e.IdParameterCategory).HasColumnName("par_pmc_seq");

                entity.Property(e => e.Value)
                    .HasMaxLength(4000)
                    .HasColumnName("par_value");

                entity.HasOne(d => d.AdmParameterCategory)
                    .WithMany(p => p.AdmParameters)
                    .HasForeignKey(d => d.IdParameterCategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("adm_parameter_pmc_fkey");
            });

            modelBuilder.Entity<AdmParameterCategory>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("adm_parameter_category_pkey");

                entity.ToTable("adm_parameter_category");

                entity.HasIndex(e => e.Description, "adm_pmc_uk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("pmc_seq");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("pmc_description");

                entity.Property(e => e.Order).HasColumnName("pmc_order");
            });

            modelBuilder.Entity<AdmProfile>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("adm_profile_pkey");

                entity.ToTable("adm_profile");

                entity.HasIndex(e => e.Description, "adm_profile_uk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("prf_seq");

                entity.Property(e => e.Administrator)
                    .HasMaxLength(1)
                    .HasColumnName("prf_administrator")
                    .HasDefaultValueSql("'N'::bpchar");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("prf_description");

                entity.Property(e => e.General)
                    .HasMaxLength(1)
                    .HasColumnName("prf_general")
                    .HasDefaultValueSql("'N'::bpchar");
            });

            modelBuilder.Entity<AdmUser>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("adm_user_pkey");

                entity.ToTable("adm_user");

                entity.HasIndex(e => e.Email, "adm_user_email_uk")
                    .IsUnique();

                entity.HasIndex(e => e.Login, "adm_user_login_uk")
                    .IsUnique();

                entity.HasIndex(e => e.Name, "adm_user_name_uk")
                    .IsUnique();

                entity.HasIndex(e => e.Password, "adm_user_password_uk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("usu_seq");

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .HasColumnName("usu_active")
                    .HasDefaultValueSql("'N'::bpchar");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("usu_email");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("usu_login");

                entity.Property(e => e.Name)
                    .HasMaxLength(64)
                    .HasColumnName("usu_name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("usu_password");
            });

            modelBuilder.Entity<AdmUserProfile>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("adm_user_profile_pkey");

                entity.ToTable("adm_user_profile");

                entity.HasIndex(e => new { e.IdProfile, e.IdUser }, "adm_user_profile_uk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("usp_seq");

                entity.Property(e => e.IdProfile).HasColumnName("usp_prf_seq");

                entity.Property(e => e.IdUser).HasColumnName("usp_use_seq");

                entity.HasOne(d => d.AdmProfile)
                    .WithMany(p => p.AdmUserProfiles)
                    .HasForeignKey(d => d.IdProfile)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("adm_usp_page_fkey");

                entity.HasOne(d => d.AdmUser)
                    .WithMany(p => p.AdmUserProfiles)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("adm_usp_profile_fkey");
            });

            modelBuilder.Entity<SequenceValue>().HasNoKey();

            modelBuilder.HasSequence("adm_menu_seq");

            modelBuilder.HasSequence("adm_page_profile_seq");

            modelBuilder.HasSequence("adm_page_seq");

            modelBuilder.HasSequence("adm_parameter_category_seq");

            modelBuilder.HasSequence("adm_parameter_seq");

            modelBuilder.HasSequence("adm_profile_seq");

            modelBuilder.HasSequence("adm_user_profile_seq");

            modelBuilder.HasSequence("adm_user_seq");         

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
