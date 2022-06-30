using System.Linq.Expressions;

namespace ConverterService.Interface
{
    public interface IRepositoryEf<T>
    {
       
        Task CreateAsync(T entity, CancellationToken cancellationToken);

        Task SaveAsync(CancellationToken cancellationToken);

    }
}
