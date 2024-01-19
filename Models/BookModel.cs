using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text.Json.Serialization;

namespace BookCatalog.Models
{
    public class BookModel
    {
        public BookModel(string title, string description, DateTime createdAt, int categoryId = 0)
        {
            Title = title;
            Description = description;
            Slug = title.Replace(" ", "-").ToLower();
            CreatedAt = createdAt;
            CategoryId = categoryId;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt {  get; set; }
        public string Slug { get; set; }
        public int CategoryId { get; set; }
        public CategoryModel Category { get; set; }
    }
}
