namespace BookCatalog.ViewModels.Category
{
    public class ListCategoriesViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<BookViewModel> Books { get; set; }
    }
}
