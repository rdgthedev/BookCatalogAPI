namespace BookCatalog.ViewModels.User
{
    public class EditorUserViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        /// <summary>
        /// Defina esse campo como:
        /// admin, user ou author
        /// </summary>
        public string Role {  get; set; }
    }
}
