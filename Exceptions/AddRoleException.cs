namespace BookCatalog.Exceptions
{
    public class AddRoleException : Exception
    {
        public AddRoleException() { }
        public AddRoleException(string message) : base(message) { }
        public AddRoleException(string message, Exception inner) : base(message, inner) { }
    }
}
