using ConverterEntityFramework;
using ConverterEntityFramework.Entities;
using ConverterService.Interface;
using ConverterService.Repository;
using Newtonsoft.Json.Linq;

namespace CurrencyConverterWebApi.BackgroundService
{
    public class BackGroundRunner : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConvertCurrencyRepo _convertCurrencyRepo;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public BackGroundRunner(IServiceProvider serviceProvider, IServiceScopeFactory scopeFactory)
        {
            _serviceProvider = serviceProvider;
            _serviceScopeFactory = scopeFactory;
            _convertCurrencyRepo = new ConvertCurrencyRepo(_serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<Context>());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var converterRepo = _serviceProvider.GetRequiredService<IConverterRepo>();
                while (true)
                {
                    var latestRates = await converterRepo.GetCurrentCurrency("NOK");

                    Console.WriteLine($"{DateTime.UtcNow}-->> starting to fetch new rates...");

                    var ent = new ConverterEntity
                    {
                        DateTime = latestRates.Date.GetValueOrDefault(),
                        Value = JObject.FromObject(latestRates.Rates).ToString()
                    };

                    await _convertCurrencyRepo.AddLatestRates(ent, stoppingToken);

                    Console.WriteLine($"{DateTime.UtcNow}-->> New rates saved to DB.");

                    await Task.Delay(TimeSpan.FromHours(24), stoppingToken).ConfigureAwait(false); 
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
