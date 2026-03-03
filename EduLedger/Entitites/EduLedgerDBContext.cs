using EduLedger.Entitites.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduLedger.Data
{
    public class EduLedgerDBContext : IdentityDbContext<ApplicationUser>
    {
        public EduLedgerDBContext(DbContextOptions<EduLedgerDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1️⃣ ApplicationUser ↔ UserProfile (One-to-One)
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.UserProfile)
                .WithOne(p => p.ApplicationUser)
                .HasForeignKey<UserProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 2️⃣ ClassLevel ↔ ApplicationUser (One-to-Many)
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.ClassLevel)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.ClassLevelId)
                .OnDelete(DeleteBehavior.SetNull);

            // 3️⃣ ClassLevel ↔ Course (One-to-Many)
            builder.Entity<Course>()
                .HasOne(c => c.ClassLevel)
                .WithMany(cl => cl.Courses)
                .HasForeignKey(c => c.ClassLevelId)
                .OnDelete(DeleteBehavior.SetNull);

            // 4️⃣ Instructor ↔ Course (One-to-Many)
            builder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany(u => u.CoursesTaught)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            // 5️⃣ Student ↔ Course (Many-to-Many)
            builder.Entity<Course>()
                .HasMany(c => c.Students)
                .WithMany(u => u.EnrolledCourses)
                .UsingEntity(j => j.ToTable("StudentCourses"));

            // 6️⃣ AcademicRecord ↔ Student (One-to-Many)
            builder.Entity<AcademicRecord>()
                .HasOne(ar => ar.Student)
                .WithMany(u => u.AcademicRecords)
                .HasForeignKey(ar => ar.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // 7️⃣ AcademicRecord ↔ Course (One-to-Many)
            builder.Entity<AcademicRecord>()
                .HasOne(ar => ar.Course)
                .WithMany(c => c.AcademicRecords)
                .HasForeignKey(ar => ar.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // 8️⃣ Prevent duplicate academic records per term
            builder.Entity<AcademicRecord>()
                .HasIndex(ar => new { ar.StudentId, ar.CourseId, ar.Session, ar.Term })
                .IsUnique();
        }

        public DbSet<AcademicRecord> AcademicRecords { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<ClassLevel> ClassLevels { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}
