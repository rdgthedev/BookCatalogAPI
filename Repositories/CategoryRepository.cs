using BookCatalog.Data;
using BookCatalog.Exceptions;
using BookCatalog.Interfaces;
using BookCatalog.Models;
using BookCatalog.ViewModels;
using BookCatalog.ViewModels.Category;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _dataContext;
        public CategoryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public async Task<List<CategoryModel>> GetAsync()
        {
            var categories = await _dataContext.Categories
                .AsNoTracking()
                .ToListAsync();

            return categories;
        }

        public async Task<List<ListCategoriesViewModel>> GetWithBooksAsync()
        {
            var categories = await _dataContext.Categories
                .AsNoTracking()
                .Include(c => c.Books)
                .Select(c => new ListCategoriesViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    Books = c.Books.Select(x => new BookViewModel
                    {
                        BookId = x.Id,
                        BookTitle = x.Title,
                        BookDescription = x.Description,
                    }).ToList()!
                })
                .OrderBy(x => x.Id)
                .ToListAsync();

            return categories;
        }

        public async Task<CategoryModel> GetByIdAsync(int id)
        {
            var category = await _dataContext.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return category!;
        }

        public async Task<ListCategoriesViewModel> GetCategoryWithBooksByIdAsync(int id)
        {
            var category = await _dataContext.Categories
                .AsNoTracking()
                .Include(c => c.Books)
                .Select(c => new ListCategoriesViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    Books = c.Books.Select(x => new BookViewModel
                    {
                        BookId = x.Id,
                        BookTitle = x.Title,
                        BookDescription = x.Description,
                    }).ToList()!
                })
                .FirstOrDefaultAsync(x => x.Id == id);
            return category!;
        }

        public async Task<CategoryModel> PostAsync(CategoryModel category)
        {
            await _dataContext.Categories.AddAsync(category);
            await _dataContext.SaveChangesAsync();
            return category;
        }

        public async Task<CategoryModel> PutAsync(CategoryModel category)
        {

            _dataContext.Categories.Update(category);
            await _dataContext.SaveChangesAsync();
            return category;
        }

        public async Task<CategoryModel> Delete(CategoryModel category)
        {
            _dataContext.Categories.Remove(category);
            await _dataContext.SaveChangesAsync();
            return category;
        }

        public async Task<CategoryModel> GetByTitle(string title)
        {
            var category = await _dataContext.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Title == title);

            return category;
        }
    }
}
