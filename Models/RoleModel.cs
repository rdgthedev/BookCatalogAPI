namespace BookCatalog.Models
{
    public class RoleModel
    {
        public RoleModel(string name)
        {
            Name = name;
            Slug = Name.ToString().ToLower();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public List<UserModel> Users { get; set; }
    }
}
