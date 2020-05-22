
using System.Threading.Tasks;

namespace DyadApp.API.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DyadAppContext Context { get; }

        public Repository(DyadAppContext context)
        {
            Context = context;
        }

        public async Task CreateAsync(T entity)
        {
            Context.Set<T>().Add(entity);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            Context.Set<T>().Update(entity);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            Context.Set<T>().Remove(entity);
            await Context.SaveChangesAsync();
        }
    }
}