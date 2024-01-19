namespace BookCatalog.ViewModels
{
    public class ResultViewModel<T> where T : class
    {
        public T Data { get; private set; }
        public List<string> Errors { get; private set; } = new();

        public ResultViewModel(T data)
            => Data = data;

        public ResultViewModel(T data, List<string> errors)
        {
            Data = data;
            Errors = errors;
        }

        public ResultViewModel(List<string> errors)
            => Errors = errors;

        public ResultViewModel(string error)
            => Errors.Add(error);
    }
}
