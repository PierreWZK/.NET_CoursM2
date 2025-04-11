namespace WebApplication1.Data.Entity
{
    public class ServiceTypeEntity
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;

        public ICollection<InterventionEntity> Interventions { get; set; } = new List<InterventionEntity>();
    }
}
