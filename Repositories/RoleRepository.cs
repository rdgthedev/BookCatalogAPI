using BookCatalog.Data;
using BookCatalog.Exceptions;
using BookCatalog.Interfaces;
using BookCatalog.Models;
using BookCatalog.ViewModels.Role;
using BookCatalog.ViewModels.User;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Repositories
{
    public class RoleRepository(DataContext dataContext) : IRoleRepository
    {
        private readonly DataContext _dataContext = dataContext;

        public async Task<List<RoleModel>> GetAsync()
            => await _dataContext.Roles.ToListAsync();

        public async Task<List<ListRolesWithUsers>> GetWithUsersAsync()
        {
            var rolesWithUsers =
                await _dataContext.Roles
                .AsNoTracking()
                .Include(role => role.Users)
                .Select(role => new ListRolesWithUsers
                {
                    Id = role.Id,
                    TypeRole = role.Name,
                    Users = role.Users.Select(user => new UserViewModel
                    {
                        Name = user.Name,
                        Email = user.Email,
                    }).ToList()
                })
                .ToListAsync();
            return rolesWithUsers;
        }

        public async Task<RoleModel> GetByIdAsync(int id)
        {
            var category = await _dataContext.Roles.FirstOrDefaultAsync(role => role.Id == id);
            return category!;
        }


        public async Task<ListRolesWithUsers> GetWithUsersByIdAsync(int id)
        {
            var rolesWithUsers =
                await _dataContext.Roles
                .AsNoTracking()
                .Include(role => role.Users)
                .Select(role => new ListRolesWithUsers
                {
                    Id = role.Id,
                    TypeRole = role.Name,
                    Users = role.Users.Select(user => new UserViewModel
                    {
                        Name = user.Name,
                        Email = user.Email,
                    }).ToList()
                })
                .FirstOrDefaultAsync(role => role.Id == id);

            return rolesWithUsers!;
        }

        public async Task<RoleModel> PostAsync(RoleModel model)
        {
            var role = await _dataContext.Roles.FirstOrDefaultAsync(r => r.Name == model.Name);

            if (role == null)
            {
                await _dataContext.AddAsync(model);
                await _dataContext.SaveChangesAsync();
                return model;
            }
            throw new AddRoleException("01CR23 - Este perfil já existe.");
        }

        public async Task<RoleModel> PutAsync(RoleModel model)
        {

            _dataContext.Roles.Update(model);
            await _dataContext.SaveChangesAsync();
            return model;
        }
        public async Task<RoleModel> DeleteAsync(RoleModel model)
        {
            _dataContext.Roles.Remove(model);
            await _dataContext.SaveChangesAsync();
            return model;
        }
    }
}
