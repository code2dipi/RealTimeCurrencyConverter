using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace ConverterService.DTO
{
    public class ApiResponse
    {
        [JsonConstructor]
        public ApiResponse(string @base,
            bool success,
            IDictionary<string, string>? symbols,
            IDictionary<string, double>? rates,
            IDictionary<string, string>? error,
            DateTime? date)
        {
            Base = @base;
            Success = success;
            Symbols = symbols;
            Rates = rates;
            Error = error;
            Date = date;
        }

        [JsonProperty(PropertyName = "success")]
        public bool Success { get; }

        [JsonProperty(PropertyName = "base")]
        public string Base { get; }

        [JsonProperty(PropertyName = "date")]
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
        public DateTime? Date { get; }

        [JsonProperty(PropertyName = "rates")]
        public IDictionary<string, double>? Rates { get; set; } = new Dictionary<string, double>();

        [JsonProperty(PropertyName = "error")]
        public IDictionary<string, string>? Error { get; set; } = new Dictionary<string, string>();

        [JsonProperty(PropertyName = "symbols")]
        public IDictionary<string, string>? Symbols { get; set; } = new Dictionary<string, string>();

        [JsonExtensionData]
        public IDictionary<string, JToken> Data { get; set; } = new Dictionary<string, JToken>();
    }

    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}
