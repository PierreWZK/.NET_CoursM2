using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Data;
using WebApplication1.Data.Entity;
using WebApplication1.Data.Entity.Identity;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InterventionController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public InterventionController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var isAdmin = await _userManager.IsInRoleAsync(user, "admin");

            var query = _context.Interventions
                .Include(i => i.Client)
                .Include(i => i.ServiceType)
                .Include(i => i.Technicians)
                .Include(i => i.Materials)
                .AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(i => i.Technicians.Any(t => t.Id == user.Id));
            }

            var results = await query
                .Select(i => new
                {
                    i.Id,
                    i.Date,
                    ClientName = i.Client.Nom,
                    ServiceTypeName = i.ServiceType.Nom,
                    Technicians = i.Technicians.Select(t => t.DisplayName),
                    Materials = i.Materials.Select(m => m.Nom)
                })
                .ToListAsync();

            return Ok(results);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Create(InterventionCreateModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (model.Date < DateTime.UtcNow)
            {
                return BadRequest(new
                {
                    Title = "Invalid date",
                    Detail = "La date de l'intervention ne peut pas être dans le passé.",
                    Status = 400
                });
            }

            var client = await _context.Clients.FindAsync(model.ClientId);
            var serviceType = await _context.ServiceTypes.FindAsync(model.ServiceTypeId);
            var technicians = await _context.Users.Where(u => model.TechnicianIds.Contains(u.Id)).ToListAsync();
            var materials = await _context.Materials.Where(m => model.MaterialIds.Contains(m.Id)).ToListAsync();

            if (client == null || serviceType == null || technicians.Count != model.TechnicianIds.Count || materials.Count != model.MaterialIds.Count)
            {
                return BadRequest("Certains éléments liés sont introuvables (Client, Techniciens, Matériaux, ServiceType).");
            }

            var intervention = new InterventionEntity
            {
                Date = model.Date,
                Client = client,
                ServiceType = serviceType,
                Technicians = technicians,
                Materials = materials
            };

            _context.Interventions.Add(intervention);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = intervention.Id }, intervention);
        }
    }

    public class InterventionCreateModel
    {
        public DateTime Date { get; set; }
        public int ClientId { get; set; }
        public int ServiceTypeId { get; set; }
        public List<string> TechnicianIds { get; set; } = new();
        public List<int> MaterialIds { get; set; } = new();
    }
}
