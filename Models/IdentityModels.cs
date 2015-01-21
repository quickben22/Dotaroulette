using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace WebApplication5.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual UserProfileInfo UserProfileInfo { get; set; }
    }



    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
        public DbSet<UserProfileInfo> UserProfileInfo { get; set; }
        public DbSet<Position> Pozitions { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<MemberAlert> MemberAlerts { get; set; }
        public DbSet<MemberRequests> MemberRequestsSet{ get; set; }
        public DbSet<MemberGroups> MemberGroupsSet { get; set; }
        public DbSet<MemberGroupmembers> MemberGroupmembersSet { get; set; }
    }
}