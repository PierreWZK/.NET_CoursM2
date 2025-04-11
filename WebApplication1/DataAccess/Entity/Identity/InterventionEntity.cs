using WebApplication1.Data.Entity.Identity;

namespace WebApplication1.Data.Entity
{
    public class InterventionEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public int ClientId { get; set; }
        public required ClientEntity Client { get; set; }

        public int ServiceTypeId { get; set; }
        public required ServiceTypeEntity ServiceType { get; set; }

        public ICollection<ApplicationUser> Technicians { get; set; } = new List<ApplicationUser>();
        public ICollection<MaterialEntity> Materials { get; set; } = new List<MaterialEntity>();
    }
}
