using System.Text.Json.Serialization;

namespace WebApplication1.Data.Entity
{
    public class ClientEntity
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Adresse { get; set; } = string.Empty;

        public ICollection<InterventionEntity> Interventions { get; set; } = new List<InterventionEntity>();
    }
}
