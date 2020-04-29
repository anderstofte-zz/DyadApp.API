using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DyadApp.API.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DyadAppContext Context { get; }

        public Repository(DyadAppContext context)
        {
            Context = context;
        }

        public Task CreateAsync(T entity, CancellationToken cancellationToken = default)
        { 
            Context.Set<T>().Add(entity);
            return Context.SaveChangesAsync(cancellationToken);
        }

        public T GetById(int id)
        {
            return Context.Set<T>().Find(id);
        }


        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType { get; }
        public Expression Expression { get; }
        public IQueryProvider Provider { get; }
    }
}