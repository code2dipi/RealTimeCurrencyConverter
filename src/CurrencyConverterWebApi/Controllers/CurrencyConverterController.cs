using System.Net;
using ConverterService.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverterWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyConverterController : ControllerBase
    {
        private readonly IConverterRepo _converterRepo;

        public CurrencyConverterController(IConverterRepo converterRepo)
        {
            this._converterRepo = converterRepo;
        }

        [HttpGet("ConvertInRealTime")]
        [ProducesResponseType(typeof(double), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]

        public async Task<IActionResult> ConvertInRealTime(
            [FromQuery] string code1 = "NOK",
            [FromQuery] string code2 = "EUR",
            [FromQuery] double amount = 10.0)
        {
            var codes = await GetCodes();
            if (!codes.Contains(code1.ToLower()))
            {
                return BadRequest($"{code1} is Invalid.");
            }

            if (!codes.Contains(code2.ToLower()))
            {
                return BadRequest($"{code2} is Invalid.");
            }

            var convertData = await _converterRepo.ConvertCurrency(code1, code2, amount);

            return Ok(convertData);
        }

        [HttpGet("ConvertInHistoricalTime")]
        [ProducesResponseType(typeof(double), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ConvertInHistoricalTime(
            [FromQuery] DateTime dateTime,
            [FromQuery] string code1 = "NOK",
            [FromQuery] string code2 = "EUR",
            [FromQuery] double amount = 10.0)
        {
            var codes = await GetCodes();
            if (!codes.Contains(code1.ToLower()))
            {
                return BadRequest($"{code1} is Invalid.");
            }

            if (!codes.Contains(code2.ToLower()))
            {
                return BadRequest($"{code2} is Invalid.");
            }
            
            var convertData = await _converterRepo.ConvertCurrency(code1, code2, amount, dateTime);

            return Ok(convertData);
        }


        private async Task<IEnumerable<string>> GetCodes()
        {
            var currencyKeys = await _converterRepo.GetCurrencyCode();
            if (!currencyKeys.Success)
            {
                throw new ArgumentNullException($"Unable to load codes from API.");
            }

            return currencyKeys.Symbols.Select(x => x.Key.ToLower());
        }
    }
}
