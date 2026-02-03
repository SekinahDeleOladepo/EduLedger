using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduLedger.Data
{
    public class EduLedgerDBContext :  IdentityDbContext<ApplicationUser> 
    {
        public EduLedgerDBContext(DbContextOptions<EduLedgerDBContext> options) : base(options)
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

            // 4️⃣ ApplicationUser ↔ Course (Many-to-Many)
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Courses)
                .WithMany(c => c.Users)
                .UsingEntity(j => j.ToTable("UserCourses"));
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<ClassLevel> ClassLevels { get; set; }
        public DbSet<Course> Courses { get; set; }

    }
}
