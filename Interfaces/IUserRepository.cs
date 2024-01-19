using BookCatalog.Models;
using BookCatalog.ViewModels;
using BookCatalog.ViewModels.User;

namespace BookCatalog.Interfaces
{
    public interface IUserRepository
    {
        Task<List<UserModel>> GetAsync();
        Task<UserModel> GetByIdAsync(int id);
        Task<UserModel> GetByEmailAsync(string name);
        Task<UserModel> PostAsync(UserModel model, EditorUserViewModel user);
        Task<UserModel> PutAsync(UserModel model);
        Task<UserModel> Delete(UserModel model);
        Task<UserModel> AddRoleToUser(UserModel model, string role);

    }
}
