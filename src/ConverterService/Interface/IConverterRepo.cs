using ConverterService.DTO;

namespace ConverterService.Interface
{
    public interface IConverterRepo
    {
        Task<ApiResponse> GetCurrencyCode();

        Task<double> ConvertCurrency(
            string firstCode,
            string secondCode,
            double amount,
            DateTime? dateTime = null);

        Task<ApiResponse?> GetCurrentCurrency(
            string baseCode);


    }
}
