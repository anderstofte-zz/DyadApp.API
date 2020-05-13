using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DyadApp.API.Extensions
{
    public static class ModelStateExtensions
    {
        public static string FirstError(this ModelStateDictionary dictionary)
        {
            return dictionary.Select(x => x.Value.Errors.Select(error => error.ErrorMessage).First()).First();
        }
    }
}