using System.Threading.Tasks;

namespace DyadApp.API.Data
{
    public interface IRepository<T>  where T : class
    {
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}