using BookCatalog.Data;
using BookCatalog.Exceptions;
using BookCatalog.Interfaces;
using BookCatalog.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Services
{
    public class UserRoleService : IUserRoleService
    {
        public async Task<UserModel> AddRole(UserModel user, string role, DataContext dataContext)
        {
            if (user == null)
                throw new NotFoundException("09NFU33 - Usuário não encontrado!");

            var resultUser = await dataContext.Users
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            var resultRole = dataContext.Roles.FirstOrDefault(r => r.Name == role);

            if (resultRole == null)
            {
                var newRole = new RoleModel(role);
                await dataContext.Roles.AddAsync(newRole);
                await dataContext.SaveChangesAsync();
                resultRole = newRole;
            }
            var spUserWithRole = "EXEC spUserWithRole @UserId, @RoleId";

            List<SqlParameter> parameters = [new("@RoleId", resultRole.Id), new("@UserId", user.Id)];

            try
            {
                await dataContext.Database.ExecuteSqlRawAsync(spUserWithRole, parameters);
                await dataContext.SaveChangesAsync();

                resultUser = await dataContext.Users
                    .Include(x => x.Roles)
                    .FirstOrDefaultAsync(u => u.Id == user.Id);
                return resultUser!;
            }
            catch { throw new Exception(); }
        }
    }
}
