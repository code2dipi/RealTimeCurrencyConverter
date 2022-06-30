using System.Linq.Expressions;
using ConverterEntityFramework;
using ConverterService.Interface;
using Microsoft.EntityFrameworkCore;

namespace ConverterService.Repository
{
    public abstract class RepositoryEf<T> : IRepositoryEf<T> where T : class
    {
        protected RepositoryEf(Context context)
        {
            Context = context;
        }

        public Context Context { get; set; }


        public async Task CreateAsync(T entity, CancellationToken cancellationToken)
        {
            await Context.Set<T>().AddAsync(entity, cancellationToken);
        }

   
        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }

      

    }
}
