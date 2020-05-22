using System.Threading;
using System.Threading.Tasks;

namespace DyadApp.API.Data.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        Task CreateAsync(T entity, CancellationToken cancellationToken = default);
    }
}