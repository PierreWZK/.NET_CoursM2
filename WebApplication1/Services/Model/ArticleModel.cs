using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Services.Model
{
    public class ArticleModel
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
