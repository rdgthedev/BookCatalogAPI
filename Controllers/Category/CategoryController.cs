using BookCatalog.Exceptions;
using BookCatalog.Extension;
using BookCatalog.Interfaces;
using BookCatalog.Models;
using BookCatalog.ViewModels;
using BookCatalog.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BookCatalog.Controllers.Category
{
    [ApiController]
    public class CategoryController(ICategoryRepository repositoryCategory) : ControllerBase
    {
        private readonly ICategoryRepository _repositoryCategory = repositoryCategory;

        [HttpGet("v1/categories")]
        [Authorize(Roles = "Admin, User, Author")]
        public async Task<IActionResult> ListCategories()
        {
            try
            {
                var categories = await _repositoryCategory.GetAsync();
                return Ok(new ResultViewModel<dynamic>(new
                {
                    nomeUser = User.Identity?.Name,
                    categories
                }));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05P04 - Falha interna!"));
            }
        }


        [HttpGet("v1/categories-books")]
        [Authorize(Roles = "Admin, User, Author")]
        public async Task<IActionResult> ListCategoriesWithBooks()
        {
            try
            {
                var categories = await _repositoryCategory.GetWithBooksAsync();
                return Ok(new ResultViewModel<List<ListCategoriesViewModel>>(categories));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05P04 - Falha interna!"));
            }
        }

        [HttpGet("v1/categories/{id:int}")]
        [Authorize(Roles = "Admin, User, Author")]
        public async Task<IActionResult> ListCategoryById(
            [FromRoute] int id)
        {
            try
            {
                var category = await _repositoryCategory.GetByIdAsync(id);

                if (category == null)
                    return NotFound(new ResultViewModel<string>("05C05 - Categoria não encontrada!"));

                return Ok(new ResultViewModel<CategoryModel>(category!));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05P05 - Falha interna!"));
            }
        }

        [HttpGet("v1/categories/{title}")]
        [Authorize(Roles = "Admin, User, Author")]
        public async Task<IActionResult> ListCategoryByTitle(
            [FromRoute] string title)
        {
            try
            {
                var category = await _repositoryCategory.GetByTitle(title);

                if (category == null)
                    return NotFound(new ResultViewModel<string>("05C05 - Categoria não encontrada!"));

                return Ok(new ResultViewModel<CategoryModel>(category!));
            }
            catch (NotFoundException ex)
            {
                return StatusCode(404, new ResultViewModel<string>(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05P05 - Falha interna!"));
            }
        }

        [HttpPost("v1/categories")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory(
            [FromBody] EditorCategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<ModelStateDictionary>(ModelState.GetErrors()));

            var category = new CategoryModel(model.Title);

            try
            {
                await _repositoryCategory.PostAsync(category);
            }
            catch (DbUpdateException)
            {
                return StatusCode(400, "05I13 - Não foi possível incluir uma categoria!");
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05P06 - Falha interna!"));
            }

            return Created($"/{category.Id}", category);
        }

        [HttpPut("v1/categories/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(
            [FromBody] EditorCategoryViewModel model,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<ModelStateDictionary>(ModelState.GetErrors()));

            try
            {
                var category = await _repositoryCategory.GetByIdAsync(id);

                if (category == null)
                    return NotFound(new ResultViewModel<string>("05C12 - Categoria não encontrada!"));

                category.Title = model.Title;
                category.Slug = model.Title.Replace(" ", "-").ToLower();

                await _repositoryCategory.PutAsync(category);
                return Ok(new ResultViewModel<CategoryModel>(category!));
            }
            catch (DbUpdateException)
            {
                return StatusCode(400, "05I14 - Não foi possível alterar uma categoria!");
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05P07 - Falha interna!"));
            }
        }

        [HttpDelete("v1/categories/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(
            [FromRoute] int id)
        {
            try
            {
                var category = await _repositoryCategory.GetByIdAsync(id);

                if (category == null)
                    return NotFound(new ResultViewModel<string>("05C11 - Categoria não encontrada!"));

                await _repositoryCategory.Delete(category!);
                return Ok(new ResultViewModel<CategoryModel>(category!));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05P08 - Falha interna!"));
            }
        }
    }
}

