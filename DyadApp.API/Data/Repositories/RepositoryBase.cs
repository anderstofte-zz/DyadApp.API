using System.Threading;
using System.Threading.Tasks;

namespace DyadApp.API.Data.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        public RepositoryBase(DyadAppContext context)
        {
            Context = context;
        }

        private DyadAppContext Context { get; }
        public Task CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            Context.Set<T>().Add(entity);
            return Context.SaveChangesAsync(cancellationToken);
        }
    }
}