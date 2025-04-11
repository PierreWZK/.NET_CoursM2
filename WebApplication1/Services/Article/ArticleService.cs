using WebApplication1.Data;
using WebApplication1.Data.Dao;
using WebApplication1.Data.Entity;

namespace WebApplication1.Services.Article
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleDataAccess _dataAccess;

        public ArticleService(IArticleDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public Task<IEnumerable<ArticleEntity>> GetAllAsync() => _dataAccess.GetAllAsync();
        public Task<ArticleEntity?> GetByIdAsync(int id) => _dataAccess.GetByIdAsync(id);
        public Task<ArticleEntity> CreateAsync(ArticleEntity article) => _dataAccess.CreateAsync(article);
        public Task<bool> DeleteAsync(int id) => _dataAccess.DeleteAsync(id);
    }
}
