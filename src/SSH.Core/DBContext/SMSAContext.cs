using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using SSH.Core.Entity;

namespace SSH.Core.DBContext
{
    public class SSHContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserLogin, ApplicationUserRole, IdentityUserClaim>
    {
        public SSHContext() : base("DefaultConnectionString")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Audit> Audit { get; set; }

        public DbSet<AuditDetail> AuditDetail { get; set; }
        
        public DbSet<EmailNotification> EmailNotification { get; set; }
        
        public DbSet<LOV> LOV { get; set; }
        
        public DbSet<Permission> Permission { get; set; }

        public DbSet<UserRolePermission> UserRolePermission { get; set; }

        public DbSet<UserSession> UserSession { get; set; }

        public DbSet<OTP> OTP { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
