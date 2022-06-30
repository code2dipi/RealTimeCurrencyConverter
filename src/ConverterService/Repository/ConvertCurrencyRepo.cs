using ConverterEntityFramework;
using ConverterEntityFramework.Entities;
using ConverterService.Interface;
using Microsoft.EntityFrameworkCore;

namespace ConverterService.Repository
{
    public class ConvertCurrencyRepo : RepositoryEf<ConverterEntity>, IConvertCurrencyRepo
    {
        public ConvertCurrencyRepo(Context context) : base(context)
        {
        }

        public async Task AddLatestRates(ConverterEntity converterEntity, CancellationToken cancellationToken)
        {
            await CreateAsync(converterEntity, cancellationToken);
            await SaveAsync(cancellationToken); 
        }

    }
}
