using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace PBL3.Database
{
    public partial class PBL3Context : DbContext
    {
        public PBL3Context()
        {
        }

        public PBL3Context(DbContextOptions<PBL3Context> options)
            : base(options)
        {
        }

        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<CityPost> CityPosts { get; set; }
        public virtual DbSet<CompanyCity> CompanyCities { get; set; }
        public virtual DbSet<CompanyFeedback> CompanyFeedbacks { get; set; }
        public virtual DbSet<CompanyPost> CompanyPosts { get; set; }
        public virtual DbSet<CompanyProfile> CompanyProfiles { get; set; }
        public virtual DbSet<CompanySkill> CompanySkills { get; set; }
        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<PostSubmit> PostSubmits { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<Skillpost> Skillposts { get; set; }
        public virtual DbSet<StudentProfile> StudentProfiles { get; set; }
        public virtual DbSet<StudentSkill> StudentSkills { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("DataBasePBL3");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("cities");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CityName)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.Createdate).HasColumnType("datetime");

                entity.Property(e => e.Updatedate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CityPost>(entity =>
            {
                entity.ToTable("cityPosts");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.Createdate).HasColumnType("datetime");

                entity.Property(e => e.PostId).HasColumnName("PostID");

                entity.Property(e => e.Updatedate).HasColumnType("datetime");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.CityPosts)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_cityPosts_city");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.CityPosts)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_cityPosts_post");
            });

            modelBuilder.Entity<CompanyCity>(entity =>
            {
                entity.ToTable("companyCities");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.Createdate).HasColumnType("datetime");

                entity.Property(e => e.Updatedate).HasColumnType("datetime");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.CompanyCities)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_comCty_cties");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyCities)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_comCty_comPs");
            });

            modelBuilder.Entity<CompanyFeedback>(entity =>
            {
                entity.ToTable("companyFeedbacks");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.Createdate).HasColumnType("datetime");

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.Updatedate).HasColumnType("datetime");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyFeedbacks)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_pComm_comPs");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.CompanyFeedbacks)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_pComm_stuPs");
            });

            modelBuilder.Entity<CompanyPost>(entity =>
            {
                entity.ToTable("companyPosts");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.Createdate).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Updatedate).HasColumnType("datetime");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyPosts)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_comPosts_comPs");
            });

            modelBuilder.Entity<CompanyProfile>(entity =>
            {
                entity.ToTable("companyProfiles");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CompanyName).IsRequired();

                entity.Property(e => e.Createdate).HasColumnType("datetime");

                entity.Property(e => e.Updatedate).HasColumnType("datetime");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.CompanyProfile)
                    .HasForeignKey<CompanyProfile>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_comP_acc");
            });

            modelBuilder.Entity<CompanySkill>(entity =>
            {
                entity.ToTable("companySkills");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.Createdate).HasColumnType("datetime");

                entity.Property(e => e.SkillId).HasColumnName("SkillID");

                entity.Property(e => e.Updatedate).HasColumnType("datetime");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanySkills)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_comSk_comPs");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.CompanySkills)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_comSk_sks");
            });

            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.ToTable("faculties");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createdate).HasColumnType("datetime");

                entity.Property(e => e.FacultyName).IsRequired();

                entity.Property(e => e.Updatedate).HasColumnType("datetime");
            });

            modelBuilder.Entity<PostSubmit>(entity =>
            {
                entity.ToTable("postSubmits");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createdate).HasColumnType("datetime");

                entity.Property(e => e.PostId).HasColumnName("PostID");

                entity.Property(e => e.StudentCvpath)
                    .IsRequired()
                    .HasColumnName("StudentCVPath");

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.Updatedate).HasColumnType("datetime");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostSubmits)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_postsub_composts");

                entity.HasOne(d => d.PostNavigation)
                    .WithMany(p => p.PostSubmits)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_postsub_stuPs");
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.ToTable("skills");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createdate).HasColumnType("datetime");

                entity.Property(e => e.SkillName).IsRequired();

                entity.Property(e => e.Updatedate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Skillpost>(entity =>
            {
                entity.ToTable("skillposts");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createdate).HasColumnType("datetime");

                entity.Property(e => e.PostId).HasColumnName("PostID");

                entity.Property(e => e.SkillId).HasColumnName("SkillID");

                entity.Property(e => e.Updatedate).HasColumnType("datetime");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Skillposts)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_skillPosts_post");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.Skillposts)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_skillPosts_skill");
            });

            modelBuilder.Entity<StudentProfile>(entity =>
            {
                entity.ToTable("studentProfiles");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Createdate).HasColumnType("datetime");

                entity.Property(e => e.ForeignLanguage).HasMaxLength(50);

                entity.Property(e => e.Gpa).HasColumnName("GPA");

                entity.Property(e => e.Mssv).HasColumnName("MSSV");

                entity.Property(e => e.Updatedate).HasColumnType("datetime");

                entity.HasOne(d => d.CityNavigation)
                    .WithMany(p => p.StudentProfiles)
                    .HasForeignKey(d => d.City)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_stuP_cities");

                entity.HasOne(d => d.FacultyNavigation)
                    .WithMany(p => p.StudentProfiles)
                    .HasForeignKey(d => d.Faculty)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_stuP_facul");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.StudentProfile)
                    .HasForeignKey<StudentProfile>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_stuP_acc");
            });

            modelBuilder.Entity<StudentSkill>(entity =>
            {
                entity.ToTable("studentSkills");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createdate).HasColumnType("datetime");

                entity.Property(e => e.SkillId).HasColumnName("SkillID");

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.Updatedate).HasColumnType("datetime");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.StudentSkills)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_stuSk_sks");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentSkills)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_stuSk_stuPs");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AvtLocation)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Bio).HasMaxLength(200);

                entity.Property(e => e.Cicno)
                    .HasMaxLength(20)
                    .HasColumnName("CICNo");

                entity.Property(e => e.Createdate).HasColumnType("datetime");

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.GivenName).HasMaxLength(200);

                entity.Property(e => e.LastLogin)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash).IsRequired();

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.SubName).HasMaxLength(200);

                entity.Property(e => e.Updatedate).HasColumnType("datetime");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_acc_accR");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("userRoles");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createdate).HasColumnType("datetime");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Updatedate).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
