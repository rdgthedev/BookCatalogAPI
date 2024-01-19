using BookCatalog.Models;
using BookCatalog.ViewModels.Category;

namespace BookCatalog.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<CategoryModel>> GetAsync();
        Task<List<ListCategoriesViewModel>> GetWithBooksAsync();
        Task<CategoryModel> GetByIdAsync(int id);
        Task<ListCategoriesViewModel> GetCategoryWithBooksByIdAsync(int id);
        Task<CategoryModel> PostAsync(CategoryModel model);
        Task<CategoryModel> PutAsync(CategoryModel model);
        Task<CategoryModel> Delete(CategoryModel model);
        Task<CategoryModel> GetByTitle(string title);
    }
}
