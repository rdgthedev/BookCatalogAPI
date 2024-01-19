using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json.Serialization;

namespace BookCatalog.Models
{
    public class CategoryModel
    {
        public CategoryModel(string title)
        {
            Title = title;
            Slug = title.Replace(" ", "-").ToLower();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public List<BookModel> Books { get; set; }
    }
}
