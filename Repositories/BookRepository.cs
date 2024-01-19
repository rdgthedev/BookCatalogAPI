using BookCatalog.Data;
using BookCatalog.Exceptions;
using BookCatalog.Interfaces;
using BookCatalog.Models;
using BookCatalog.ViewModels.Book;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Repositories
{

    public class BookRepository(DataContext dataContext) : IBookRepository
    {
        private readonly DataContext _dataContext = dataContext;

        public async Task<List<BookModel>> GetAsync()
        { 
            var books = await _dataContext.Books
                 .AsNoTracking()
                 .Include(c => c.Category)
                 .ToListAsync();

            return books;
        }

        public async Task<BookModel> GetByIdAsync(int id)
        {
            var book = await _dataContext.Books
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return book!;
        }

        public async Task<BookModel> PostAsync(EditorBookViewModel model)
        {
            var resultBook = await BookWithCategory(model);

            await _dataContext.Books.AddAsync(resultBook);
            await _dataContext.SaveChangesAsync();
            return resultBook;
        }

        public async Task<BookModel> PutAsync(BookModel book)
        {
            _dataContext.Books.Update(book);
            await _dataContext.SaveChangesAsync();
            return book;
        }

        public async Task<BookModel> Delete(BookModel book)
        {
            _dataContext.Books.Remove(book);
            await _dataContext.SaveChangesAsync();
            return book;
        }

        public async Task<BookModel> BookWithCategory(EditorBookViewModel model)
        {
            CategoryRepository categoryRepository = new(_dataContext);

            BookModel book = new(model.Title, model.Description, model.CreatedAt);

            var category = await categoryRepository.GetByTitle(model.TitleCategory);

            if (category is null)
            {
                var newCategory = new CategoryModel(model.TitleCategory);
                var  createdCategory =  await categoryRepository.PostAsync(newCategory);
                book.CategoryId = createdCategory.Id;
                return book;
            }

            book.CategoryId = category!.Id;
            return book;
        }
    }
}