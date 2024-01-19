namespace BookCatalog.Models
{
    public class UserModel
    {
        public UserModel(string name, string email)
        {
            Name = name;
            Email = email;
            LastLoggedIn = DateTime.Now;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime LastLoggedIn { get; set; }

        public List<RoleModel> Roles { get; set; }
    }
}
