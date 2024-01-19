using BookCatalog.Data;
using BookCatalog.Models;

namespace BookCatalog.Interfaces
{
    public interface IUserRoleService
    {
        public Task<UserModel> AddRole(UserModel user, string role, DataContext dataContext);

        //public Task<UserModel> AddRoleToOldUser(UserModel user, string role);
    }
}
