using BookCatalog.Models;

namespace BookCatalog.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(UserModel user);
    }
}
