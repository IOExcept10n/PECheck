using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Section> Sections { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<StudentSection> StudentSections { get; set; }
        public DbSet<Normative> Normatives { get; set; }
        public DbSet<NormativeResult> NormativeResults { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints
            modelBuilder.Entity<StudentSection>()
                .HasKey(ss => new { ss.StudentId, ss.SectionId, ss.SemesterId });

            modelBuilder.Entity<StudentSection>()
                .HasOne(ss => ss.Student)
                .WithMany(s => s.StudentSections)
                .HasForeignKey(ss => ss.StudentId);

            modelBuilder.Entity<StudentSection>()
                .HasOne(ss => ss.Section)
                .WithMany(s => s.StudentSections)
                .HasForeignKey(ss => ss.SectionId);

            modelBuilder.Entity<StudentSection>()
                .HasOne(ss => ss.Semester)
                .WithMany(s => s.StudentSections)
                .HasForeignKey(ss => ss.SemesterId);

            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.StudentSection)
                .WithMany(ss => ss.Attendances)
                .HasForeignKey(a => new { a.StudentId, a.SectionId, a.SemesterId });

            modelBuilder.Entity<NormativeResult>()
                .HasOne(nr => nr.StudentSection)
                .WithMany(ss => ss.NormativeResults)
                .HasForeignKey(nr => new { nr.StudentId, nr.SectionId, nr.SemesterId });

            modelBuilder.Entity<NormativeResult>()
                .HasOne(nr => nr.Normative)
                .WithMany(n => n.NormativeResults)
                .HasForeignKey(nr => nr.NormativeId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.StudentSection)
                .WithMany(ss => ss.Reviews)
                .HasForeignKey(r => new { r.StudentId, r.SectionId, r.SemesterId });

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.StudentSection)
                .WithMany(ss => ss.Payments)
                .HasForeignKey(p => new { p.StudentId, p.SectionId, p.SemesterId });

            modelBuilder.Entity<Section>()
                .HasOne(s => s.Teacher)
                .WithMany(t => t.Sections)
                .HasForeignKey(s => s.TeacherId);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Section)
                .WithMany(s => s.Schedules)
                .HasForeignKey(s => s.SectionId);

            // Configure identity tables with custom names
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        }
    }
}