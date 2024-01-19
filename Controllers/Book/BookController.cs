using BookCatalog.Exceptions;
using BookCatalog.Extension;
using BookCatalog.Interfaces;
using BookCatalog.Models;
using BookCatalog.Repositories;
using BookCatalog.ViewModels;
using BookCatalog.ViewModels.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Controllers.Book
{
    [ApiController]
    public class BookController(IBookRepository bookRepository) : ControllerBase
    {
        private readonly IBookRepository _bookRepository = bookRepository;

        [HttpGet("v1/books")]
        [Authorize(Roles = "Admin, User, Author")]
        public async Task<IActionResult> ListBooks()
        {
            try
            {
                var books = await _bookRepository.GetAsync();
                return Ok(new ResultViewModel<List<BookModel>>(books));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05B04 - Falha interna!"));
            }
        }

        [HttpGet("v1/books/{id:int}")]
        [Authorize(Roles = "Admin, User, Author")]
        public async Task<IActionResult> ListBookById(
            [FromRoute] int id)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(id);

                if (book == null)
                    return NotFound(new ResultViewModel<string>("05BN05 - Usuário não encontrado!"));

                return Ok(new ResultViewModel<BookModel>(book!));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05B05 - Falha interna!"));
            }
        }

        [HttpPost("v1/books")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateBook(
            [FromBody] EditorBookViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<ModelStateDictionary>(ModelState.GetErrors()));

            try
            {
                var book = await _bookRepository.PostAsync(model);
                return Created($"/{book.Id}", new ResultViewModel<BookModel>(book));
            }
            catch (DbUpdateException)
            {
                return StatusCode(400, new ResultViewModel<string>("05BR13 - Não foi possível incluir um usuário!"));
            }
            catch (NotFoundException ex)
            {
                return StatusCode(404, new ResultViewModel<string>(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, new ResultViewModel<string>(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05B06 - Falha interna!"));
            }
        }

        [HttpPut("v1/books/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBook(
           [FromBody] EditorBookViewModel model,
           [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<ModelStateDictionary>(ModelState.GetErrors()));

            try
            {
                var book = await _bookRepository.GetByIdAsync(id);

                book.Title = model.Title;
                book.Slug = model.Title.Replace(" ", "-").ToLower();

                await _bookRepository.PutAsync(book);

                if (book == null)
                    return NotFound(new ResultViewModel<string>("05BN12 - Usuário não encontrado!"));

                return Ok(new ResultViewModel<BookModel>(book!));
            }
            catch (DbUpdateException)
            {
                return StatusCode(400, "05BD14 - Não foi possível alterar um usuário!");
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05B07 - Falha interna!"));
            }
        }

        [HttpDelete("v1/books/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(
            [FromRoute] int id)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(id);

                if (book == null)
                    return NotFound(new ResultViewModel<string>("05BN11 - Usuário não encontrado!"));

                await _bookRepository.Delete(book!);
                return Ok(new ResultViewModel<BookModel>(book!));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05B08 - Falha interna!"));
            }
        }
    }
}
