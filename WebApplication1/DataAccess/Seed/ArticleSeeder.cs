using WebApplication1.Data.Entity;

namespace WebApplication1.Data.Seed
{
    public class ArticleSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Articles.Any())
                return;

            context.Articles.AddRange(new List<ArticleEntity>
            {
                new ArticleEntity { Title = "Titre 11", Content = "Contenu 11111" },
                new ArticleEntity { Title = "Titre 22", Content = "Contenu 22222" }
            });

            context.SaveChanges();
        }
    }
}
