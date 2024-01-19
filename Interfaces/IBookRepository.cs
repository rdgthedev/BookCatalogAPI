using BookCatalog.Models;
using BookCatalog.ViewModels.Book;

namespace BookCatalog.Interfaces
{
    public interface IBookRepository
    {
        Task<List<BookModel>> GetAsync();
        Task<BookModel> GetByIdAsync(int id);
        Task<BookModel> PostAsync(EditorBookViewModel model);
        Task<BookModel> PutAsync(BookModel model);
        Task<BookModel> Delete(BookModel model);
    }
}
