using System;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data.Entity;

namespace WebApplication1.Data.Dao
{
    public class ArticleDataAccess : IArticleDataAccess
    {
        private readonly AppDbContext _context;

        public ArticleDataAccess(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ArticleEntity>> GetAllAsync()
        {
            return await _context.Articles.ToListAsync();
        }

        public async Task<ArticleEntity?> GetByIdAsync(int id)
        {
            return await _context.Articles.FindAsync(id);
        }

        public async Task<ArticleEntity> CreateAsync(ArticleEntity article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return article;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null) return false;

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
