using BookCatalog.Models;
using BookCatalog.ViewModels.Role;

namespace BookCatalog.Interfaces
{
    public interface IRoleRepository
    {
        public Task<List<RoleModel>> GetAsync();
        public Task<List<ListRolesWithUsers>> GetWithUsersAsync();
        public Task<RoleModel> GetByIdAsync(int id);
        public Task<ListRolesWithUsers> GetWithUsersByIdAsync(int id);
        public Task<RoleModel> PostAsync(RoleModel model);
        public Task<RoleModel> PutAsync(RoleModel model);
        public Task<RoleModel> DeleteAsync(RoleModel model);
    }
}
