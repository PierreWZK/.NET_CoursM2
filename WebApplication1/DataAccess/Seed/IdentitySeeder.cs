using Microsoft.AspNetCore.Identity;
using WebApplication1.Data.Entity.Identity;
using WebApplication1.Data.Entity;
using WebApplication1.Data;

namespace WebApplication1.DataAccess.Seed
{
    public class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<AppDbContext>();

            // Roles
            foreach (var role in new[] { "admin", "technician" })
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = role });
                }
            }

            // Admin
            var adminEmail = "admin@example.com";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail, DisplayName = "Admin" };
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "admin");
            }

            // Technicians
            var tech1 = await userManager.FindByEmailAsync("tech1@example.com");
            if (tech1 == null)
            {
                tech1 = new ApplicationUser { UserName = "tech1@example.com", Email = "tech1@example.com", DisplayName = "Tech One" };
                await userManager.CreateAsync(tech1, "Tech123!");
                await userManager.AddToRoleAsync(tech1, "technician");
            }

            var tech2 = await userManager.FindByEmailAsync("tech2@example.com");
            if (tech2 == null)
            {
                tech2 = new ApplicationUser { UserName = "tech2@example.com", Email = "tech2@example.com", DisplayName = "Tech Two" };
                await userManager.CreateAsync(tech2, "Tech123!");
                await userManager.AddToRoleAsync(tech2, "technician");
            }

            // Intervention dependencies
            if (!context.Clients.Any())
            {
                var client = new ClientEntity { Nom = "Jean Pierre", Adresse = "29 rue du Bignon" };
                var type = new ServiceTypeEntity { Nom = "Électricité" };
                var mat1 = new MaterialEntity { Nom = "Tournevis" };
                var mat2 = new MaterialEntity { Nom = "Câble" };

                context.Clients.Add(client);
                context.ServiceTypes.Add(type);
                context.Materials.AddRange(mat1, mat2);
                await context.SaveChangesAsync();

                var intervention = new InterventionEntity
                {
                    Client = client,
                    ServiceType = type,
                    Date = DateTime.UtcNow.AddDays(1),
                    Technicians = new List<ApplicationUser> { tech1, tech2 },
                    Materials = new List<MaterialEntity> { mat1, mat2 }
                };

                context.Interventions.Add(intervention);
                await context.SaveChangesAsync();
            }
        }
    }
}
