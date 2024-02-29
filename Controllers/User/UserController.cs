using BookCatalog.Exceptions;
using BookCatalog.Extension;
using BookCatalog.Interfaces;
using BookCatalog.Models;
using BookCatalog.Repositories;
using BookCatalog.ViewModels;
using BookCatalog.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
using System.Runtime.CompilerServices;

namespace BookCatalog.Controllers.User
{
    [ApiController]
    public class UserController(IUserRepository userRepository, IUserRoleService userRoleService) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUserRoleService _userRoleService = userRoleService;

        [HttpGet("v1/users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListUsers()
        {
            try
            {
                var users = await _userRepository.GetAsync();
                return Ok(new ResultViewModel<List<UserModel>>(users));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna!"));
            }
        }

        [HttpGet("v1/users/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListUserById(
            [FromRoute] int id)
        {
            UserModel user;
            try
            {
                user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                    return NotFound(new ResultViewModel<string>("05U10 - Usuário não encontrado!"));

                return Ok(new ResultViewModel<UserModel>(user!));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05X05 - Falha interna!"));
            }
        }

        [HttpPost("v1/users")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser(
            [FromBody] EditorUserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<ModelStateDictionary>(ModelState.GetErrors()));

            var user = new UserModel(model.Name, model.Email);
            var password = PasswordGenerator.Generate(25, true, true);
            var passwordHash = PasswordHasher.Hash(password);
            user.PasswordHash = passwordHash;

            try
            {
                await _userRepository.PostAsync(user, model);
                user = await _userRepository.GetByIdAsync(user.Id);
            }
            catch (DbUpdateException)
            {
                return StatusCode(400, "05U13 - Não foi possível incluir um usuário!");
            }
            catch (AddRoleException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05X06 - Falha interna!"));
            }

            return Created($"/{user.Id}", new ResultViewModel<dynamic>(new
            {
                User = user
            }));
        }

        [HttpPost("v1/users/add-role/{id:int}")]
        public async Task<IActionResult> AddRoleToExistingUser([FromRoute] int id, [FromBody] string role)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            try
            {
                var resultUser = await _userRepository.GetByIdAsync(id);
                var user = await _userRepository.AddRoleToUser(resultUser, role);

                return Ok(new ResultViewModel<UserModel>(user));
            }
            catch (NotFoundException ex)
            {

                return StatusCode(404, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05X43 - Falha interna!"));
            }
        }

        [HttpPut("v1/users/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(
            [FromBody] EditorUserViewModel model,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<ModelStateDictionary>(ModelState.GetErrors()));

            try
            {
                var user = await _userRepository.GetByIdAsync(id);

                user.Name = model.Name;
                user.Email = model.Email;
                user.PasswordHash = user.PasswordHash;

                await _userRepository.PutAsync(user);

                if (user == null)
                    return NotFound(new ResultViewModel<string>("05U12 - Usuário não encontrado!"));

                return Ok(new ResultViewModel<UserModel>(user!));
            }
            catch (DbUpdateException)
            {
                return StatusCode(400, "05U13 - Não foi possível alterar um usuário!");
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05X07 - Falha interna!"));
            }
        }

        [HttpDelete("v1/users/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(
            [FromRoute] int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);

                if (user == null)
                    return NotFound(new ResultViewModel<string>("05U11 - Usuário não encontrado!"));

                await _userRepository.Delete(user!);
                return Ok(new ResultViewModel<UserModel>(user!));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05X08 - Falha interna!"));
            }
        }
    }
}