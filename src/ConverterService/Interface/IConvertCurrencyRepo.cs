using ConverterEntityFramework.Entities;

namespace ConverterService.Interface
{
    public interface IConvertCurrencyRepo : IRepositoryEf<ConverterEntity>
    {
        Task AddLatestRates(ConverterEntity converterEntity, CancellationToken cancellationToken);
    }
}
