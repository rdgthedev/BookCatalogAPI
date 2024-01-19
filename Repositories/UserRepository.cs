using BookCatalog.Data;
using BookCatalog.Interfaces;
using BookCatalog.Models;
using BookCatalog.ViewModels.User;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Repositories
{
    public class UserRepository(DataContext dataContext, IUserRoleService  userRoleService) : IUserRepository
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly IUserRoleService _userRoleService = userRoleService;

        public async Task<List<UserModel>> GetAsync()
        {
            var users = await _dataContext.Users.AsNoTracking()
                .Include(x => x.Roles)
                .ToListAsync();
            return users;
        }

        public async Task<UserModel> GetByIdAsync(int id)
        {
            var user = await _dataContext
                    .Users
                    .AsNoTracking()
                    .Include(x => x.Roles)
                    .FirstOrDefaultAsync(user => user.Id == id);

            return user!;
        }

        public async Task<UserModel> GetByEmailAsync(string email)
        {
            var user = await _dataContext.Users
                .Include(r => r.Roles)
                .FirstOrDefaultAsync(x => x.Email == email);
            return user!;
        }

        public async Task<UserModel> PostAsync(UserModel user, EditorUserViewModel model)
        {
            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();
            var userWithRole = await _userRoleService.AddRole(user, model.Role, dataContext);
            return userWithRole;
        }

        public async Task<UserModel> PutAsync(UserModel user)
        {
            _dataContext.Users.Update(user);
            await _dataContext.SaveChangesAsync();
            return user;
        }
        public async Task<UserModel> Delete(UserModel user)
        {
            _dataContext.Users.Remove(user);
            await _dataContext.SaveChangesAsync();
            return user;
        }

        public async Task<UserModel> AddRoleToUser(UserModel user, string role)
        {
            var userWithRole = await _userRoleService.AddRole(user, role, dataContext);
            return userWithRole;
        }
    }
}
