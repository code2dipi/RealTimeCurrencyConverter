using ConverterService.DTO;
using ConverterService.Interface;
using ConverterService.Service;
using Newtonsoft.Json.Linq;

namespace ConverterService.Repository
{
    public class ConverterRepo : IConverterRepo
    {
        private static readonly string Access_Key = "9V7n3Hb2JUyYXa4x9e51Gvqv6nfoPhBh";
        private static readonly Uri BaseUrl = new Uri("https://api.apilayer.com/fixer");
        private readonly IHttpHelpers _httpHelpers;

        public ConverterRepo()
        {
            var httpClient = new HttpClient()
            {
                BaseAddress = BaseUrl
            };
            _httpHelpers = new HttpHelpers(httpClient);
        }

        public async Task<ApiResponse> GetCurrencyCode()
        {
            try
            {

                var relativeUrl = $"symbols?apikey={Access_Key}";
                var result = await _httpHelpers.Get<ApiResponse>(relativeUrl);

                
                if (result is null)
                {
                    throw new ArgumentNullException($"No data found");
                }

                return result;
            }
            catch (Exception e)
            {
                //write log 
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<double> ConvertCurrency(
            string firstCode,
            string secondCode,
            double amount,
            DateTime? dateTime = null)
        {
            try
            {

                var relativeUrl = string.Empty;

                if (dateTime == null)
                {
                    relativeUrl = $"latest?apikey={Access_Key}&base={firstCode}&symbols={secondCode}";
                }

                if (dateTime is not null)
                {
                    relativeUrl = $"{dateTime?.ToString("yyyy-MM-dd")}?apikey={Access_Key}&base={firstCode}&symbols={secondCode}";
                }

                if (string.IsNullOrEmpty(relativeUrl))
                {
                    throw new ArgumentNullException($"No URl Found.");
                }

                var result = await _httpHelpers.Get<ApiResponse>(relativeUrl);

                if (result is null)
                {
                    throw new ArgumentNullException($"No data found");
                }

                if (!result.Success && string.IsNullOrEmpty(result?.Rates?.Keys.FirstOrDefault(x => x.ToLower() == secondCode.ToLower())))
                {
                    var d = JObject.FromObject(result.Data).ToString();
                    throw new ArgumentNullException($"Unable to Fetch; message= {d}");
                }

                var rate = result?.Rates?.FirstOrDefault(x => x.Key.ToLower() == secondCode.ToLower());
                var value = rate.GetValueOrDefault().Value;

                Console.WriteLine(dateTime == null
                    ? $"{DateTime.UtcNow}-->> Conversion rate from '{firstCode}' to '{secondCode}' is {value}."
                    : $"{dateTime}-->> Conversion rate from '{firstCode}' to '{secondCode}' is {value}.");

                return value * amount;

            }
            catch (Exception e)
            {
                //write log 
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ApiResponse?> GetCurrentCurrency(
            string baseCode)
        {
            try
            {

                var relativeUrl = $"latest?apikey={Access_Key}&base={baseCode}";


                var result = await _httpHelpers.Get<ApiResponse>(relativeUrl);

                if (result is null)
                {
                    throw new ArgumentNullException($"No data found");
                }

                if (!result.Success)
                {
                    var d = JObject.FromObject(result.Data).ToString();
                    throw new ArgumentNullException($"Unable to Fetch; message= {d}");
                }

                var rate = result?.Rates;

                return result;

            }
            catch (Exception e)
            {
                //write log 
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
