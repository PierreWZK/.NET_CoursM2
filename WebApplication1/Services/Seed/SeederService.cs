using WebApplication1.Data;
using WebApplication1.Data.Seed;

namespace WebApplication1.Services.Seed
{
    public class SeederService : ISeederService
    {
        private readonly AppDbContext _context;

        public SeederService(AppDbContext context)
        {
            _context = context;
        }

        public void SeedDatabase()
        {
            _context.Database.EnsureCreated();
            ArticleSeeder.Seed(_context);
        }
    }
}
