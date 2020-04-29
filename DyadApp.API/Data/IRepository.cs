using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DyadApp.API.Data
{
    public interface IRepository<T> : IQueryable<T> where T : class
    {
        T GetById(int id);
    }
}