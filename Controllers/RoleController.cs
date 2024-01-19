using BookCatalog.Exceptions;
using BookCatalog.Extension;
using BookCatalog.Interfaces;
using BookCatalog.Models;
using BookCatalog.ViewModels;
using BookCatalog.ViewModels.Role;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Controllers
{
    [ApiController]
    public class RoleController(IRoleRepository roleRepository) : ControllerBase
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        [HttpGet("v1/roles")]
        public async Task<IActionResult> ListRoles()
        {
            try
            {
                var roles = await _roleRepository.GetAsync();
                return Ok(new ResultViewModel<List<RoleModel>>(roles));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05RO04 - Falha interna!"));
            }
        }

        [HttpGet("v1/roles-users")]
        public async Task<IActionResult> ListRolesWithUsers()
        {
            try
            {
                var roles = await _roleRepository.GetWithUsersAsync();
                return Ok(new ResultViewModel<List<ListRolesWithUsers>>(roles));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05RO05 - Falha interna!"));
            }
        }

        [HttpGet("v1/roles/{id:int}")]
        public async Task<IActionResult> ListRoleById([FromRoute] int id)
        {
            try
            {
                var role = await _roleRepository.GetByIdAsync(id);

                if (role is null)
                    return NotFound(new ResultViewModel<string>(("07NR02 - Perfil não encontrado!")));

                return Ok(new ResultViewModel<RoleModel>(role));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05RO06 - Falha interna!"));
            }
        }

        [HttpGet("v1/roles-users/{id:int}")]
        public async Task<IActionResult> ListRoleWithUsersById([FromRoute] int id)
        {
            try
            {
                var role = await _roleRepository.GetWithUsersByIdAsync(id);

                if (role is null)
                    return NotFound(new ResultViewModel<string>("07NR03 - Perfil não encontrado!"));

                return Ok(new ResultViewModel<ListRolesWithUsers>(role));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05RO07 - Falha interna!"));
            }
        }

        [HttpPost("v1/roles")]
        public async Task<IActionResult> Create([FromBody] RegisterRoleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrors());
            try
            {
                var role = new RoleModel(model.Name);
                await _roleRepository.PostAsync(role);

                return Created($"/{role.Id}",new ResultViewModel<RoleModel>(role));
            }
            catch(AddRoleException ex)
            {
                return StatusCode(400, new ResultViewModel<string>(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05RO08 - Falha interna!"));
            }
        }

        [HttpPut("v1/roles/{id:int}")]
        public async Task<IActionResult> Update([FromBody] RegisterRoleViewModel model, 
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            try
            {
                var role = await _roleRepository.GetByIdAsync(id);

                if (role == null)
                    return NotFound(new ResultViewModel<string>("07NR04 - Perfil não encontrado!"));

                role.Name = model.Name;
                role.Slug = model.Name.ToString().Replace(" ", "-").ToLower();

                await _roleRepository.PutAsync(role);

                return Ok(new ResultViewModel<RoleModel>(role));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05RO09 - Falha interna!"));
            }
        }

        [HttpDelete("v1/roles/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var role = await _roleRepository.GetByIdAsync(id);

                if (role == null)
                    return NotFound(new ResultViewModel<string>("07NR05 - Perfil não encontrado!"));

                await _roleRepository.DeleteAsync(role);

                return Ok( new ResultViewModel<RoleModel>(role));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("05RO10 - Falha interna!"));
            }
        }

    }
}
