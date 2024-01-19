using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Runtime.CompilerServices;

namespace BookCatalog.Extension
{
    public static  class ModelStateExtension
    {
        public static List<string> GetErrors(this ModelStateDictionary modelState)
        {
            var errors = new List<string>();

            foreach (var item in modelState.Values)
                errors.AddRange(item.Errors.Select(error => error.ErrorMessage));
            return errors;
        }
    }
}
