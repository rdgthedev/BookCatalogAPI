using BookCatalog.ViewModels.User;

namespace BookCatalog.ViewModels.Role
{
    public class ListRolesWithUsers
    {
        public int Id { get; set; }
        public string TypeRole { get; set; }
        public List<UserViewModel> Users { get; set; }
    }
}
