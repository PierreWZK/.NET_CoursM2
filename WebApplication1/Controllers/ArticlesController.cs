using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data.Entity;
using WebApplication1.Services.Article;
using WebApplication1.Services.Model;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _service;

        public ArticlesController(IArticleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleEntity>>> GetAll()
        {
            var articles = await _service.GetAllAsync();
            return Ok(articles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleEntity>> GetById(int id)
        {
            var article = await _service.GetByIdAsync(id);
            if (article == null)
                return NotFound();

            return Ok(article);
        }

        [HttpPost]
        public async Task<ActionResult<ArticleEntity>> Create(ArticleModel model)
        {
            var newArticle = new ArticleEntity
            {
                Title = model.Title,
                Content = model.Content,
            };

            var created = await _service.CreateAsync(newArticle);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
