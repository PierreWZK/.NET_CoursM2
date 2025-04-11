using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Data.Entity;
using WebApplication1.Data.Entity.Identity;

namespace WebApplication1.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ArticleEntity> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ArticleEntity>();

            modelBuilder.Entity<InterventionEntity>()
                .HasMany(i => i.Technicians)
                .WithMany(u => u.Interventions)
                .UsingEntity(j => j.ToTable("InterventionTechnicians"));

            modelBuilder.Entity<InterventionEntity>()
                .HasMany(i => i.Materials)
                .WithMany(m => m.Interventions)
                .UsingEntity(j => j.ToTable("InterventionMaterials"));
        }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        
        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<MaterialEntity> Materials { get; set; }
        public DbSet<ServiceTypeEntity> ServiceTypes { get; set; }
        public DbSet<InterventionEntity> Interventions { get; set; }

    }
}
