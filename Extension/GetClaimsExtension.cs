using BookCatalog.Models;
using System.Security.Claims;

namespace BookCatalog.Extension
{
    public static class GetClaimsExtension
    {
        public static IEnumerable<Claim> GetClaims(this UserModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
            };

            user.Roles.ForEach(x => claims.Add(new Claim(ClaimTypes.Role, x.Name.ToString())));
            return claims;
        }
    }
}
