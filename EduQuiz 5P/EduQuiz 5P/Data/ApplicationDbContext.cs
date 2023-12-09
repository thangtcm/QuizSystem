using EduQuiz_5P.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduQuiz_5P.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
            SeedUser(builder);
            SeedUserRole(builder);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<ApplicationRole>().HasData
            (
                new ApplicationRole() { Id = 1, Name = Constants.Roles.Admin, NormalizedName = Constants.Roles.Admin, ConcurrencyStamp = null }
            );
        }

        private void SeedUserRole(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<long>>().HasData(
                new IdentityUserRole<long>
                {
                    UserId = 1,
                    RoleId = 1
                }
            );
        }

        private void SeedUser(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = 1,
                    UserName = "admin",
                    NormalizedUserName = "admin",
                    Email = "admin@tiemkiet.vn",
                    NormalizedEmail = "admin@tiemkiet.vn",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAECAsUeOByw0jsD4x7X0K9WQdxWV/RrvPBnHITnRzdbrhHKzmf35BZDPXJBcVjp5FIQ==", //Admin@123
                    SecurityStamp = "ZD5UZJQK6Q5W6N7O6RBRF6DB2Q2G2AIJ",
                    ConcurrencyStamp = "b19f1b24-5ac9-4c8d-9b7c-5e5d5f5cfb1e",
                    FullName = "Admin",
                    TwoFactorEnabled = false,
                    PhoneNumber = "0923425148",
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                }
            );
        }

    }
}