using WebApplication1.Data.Entity;

namespace WebApplication1.Data.Dao
{
    public interface IArticleDataAccess
    {
        Task<IEnumerable<ArticleEntity>> GetAllAsync();
        Task<ArticleEntity?> GetByIdAsync(int id);
        Task<ArticleEntity> CreateAsync(ArticleEntity article);
        Task<bool> DeleteAsync(int id);
    }
}
