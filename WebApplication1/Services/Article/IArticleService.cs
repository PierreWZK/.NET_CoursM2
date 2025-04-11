using WebApplication1.Data.Entity;

namespace WebApplication1.Services.Article
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticleEntity>> GetAllAsync();
        Task<ArticleEntity?> GetByIdAsync(int id);
        Task<ArticleEntity> CreateAsync(ArticleEntity article);
        Task<bool> DeleteAsync(int id);
    }
}
