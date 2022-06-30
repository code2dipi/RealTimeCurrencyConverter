using ConverterService.Interface;
using ConverterService.Repository;
using Newtonsoft.Json.Linq;

var converterRepo = (IConverterRepo)new ConverterRepo();
if (converterRepo == null)
{
    throw new ArgumentNullException($"unable to load Repo");
}

var currencyKeys = await converterRepo.GetCurrencyCode();

if (!currencyKeys.Success)
{
    var str = JObject.FromObject(currencyKeys.Data).ToString();

    Console.WriteLine($"Unable to load data from fixer.io");

    Console.WriteLine(str);
    Console.ReadLine();
    return;
}

var codes = currencyKeys.Symbols.Select(x => x.Key.ToLower());
var runService = true;

do
{
    Console.WriteLine($"----------------------------------");
    runService = await RunService();
    Console.WriteLine($"----------------------------------");
} while (runService);

async Task<bool>  RunService()
{
    try
    {
        Console.WriteLine($"Choose service to run: {Environment.NewLine} 1. Enter '1' for Real Time calculations. {Environment.NewLine} 2. Enter '2' for historical calculations.");
        var options = Console.ReadLine();
        if (!int.TryParse(options, out int optionsNumber))
        {
            Console.WriteLine("Invalid options");
        }

        switch (optionsNumber)
        {
            case 1:
                await OptionOne();
                break;
            case 2:
                await OptionTwo();
                break;
        }

        Console.WriteLine("Choose something....");
        return true;
    }
    catch (Exception e)
    {
        Console.WriteLine($"Something went wrong try again !!!. ErrorMessage: '{e.Message}'.");
        return false;
    }
   
}

async Task OptionOne()
{
    var firstOutputMessage = "Enter valid First currency code e.g(NOK,USD):";
    var firstCodeValue = GetCurrencyInputData(firstOutputMessage);

    var secondFirsMessage = "Enter valid Second currency code e.g(NOK,USD):";
    var secondCodeValue = GetCurrencyInputData(secondFirsMessage);


    var amountOutputMessage = "Enter a amount";
    var amountToConvert = GetAmountInputData(amountOutputMessage);
    Console.WriteLine($"Converting currency rate of {firstCodeValue} to {secondCodeValue} of {amountToConvert}.");

    var convertedAmount = await converterRepo.ConvertCurrency(firstCodeValue, secondCodeValue, amountToConvert);

    Console.WriteLine($"Currency rate of {firstCodeValue} to {secondCodeValue} of {amountToConvert} amount is = {convertedAmount}.");
    Console.WriteLine($"-----------------------------------------------");
}

async Task OptionTwo()
{
    var firstOutputMessage = "Enter valid First currency code e.g(NOK,USD):";
    var firstCodeValue = GetCurrencyInputData(firstOutputMessage);

    var secondFirsMessage = "Enter valid Second currency code e.g(NOK,USD):";
    var secondCodeValue = GetCurrencyInputData(secondFirsMessage);

    var dateTimeMessage = "Enter date time(YYYY-MM-DD):";
    var dateTimeValue = GetDateTimeData(dateTimeMessage);

    var amountOutputMessage = "Enter a amount";
    var amountToConvert = GetAmountInputData(amountOutputMessage);
    Console.WriteLine($"Converting currency rate of {firstCodeValue} to {secondCodeValue} of {amountToConvert} in DateTime={dateTimeValue}.");

    var convertedAmount = await converterRepo.ConvertCurrency(firstCodeValue, secondCodeValue, amountToConvert, dateTimeValue);

    Console.WriteLine($"Currency rate of {firstCodeValue} to {secondCodeValue} of {amountToConvert} amount is = {convertedAmount}.");
    Console.WriteLine($"-----------------------------------------------");
}


string GetCurrencyInputData(string outputMsg)
{
        var validCode = false;
        var enterText = string.Empty;
   
    while (!validCode)
    {
        Console.WriteLine(outputMsg);
        var inputText = string.Empty;
        enterText = Console.ReadLine();

        if (string.IsNullOrEmpty(enterText) || !codes.Contains(enterText.ToLower()))
        {
            PrintMessage($"Invalid currency code.");
            validCode = false;
        }
        else
        {
            validCode = true;
            break;
        }
    }

    return enterText;
}

double GetAmountInputData(string outputMsg)
{
    var firstAmountString=string.Empty;
    var validAmount = false;
    var firstAmount = 0.0;
    while (!validAmount)
    {
        firstAmount = 0.0;
        Console.WriteLine("Enter a amount");
        firstAmountString = Console.ReadLine();
        if (!double.TryParse(firstAmountString, out  firstAmount))
        {
            Console.WriteLine("Invalid amount");
            validAmount = false;
        }
        else
        {
            validAmount = true;
        }

    }
   

    return firstAmount;
}

DateTime GetDateTimeData(string outputMsg)
{
    var dateTime=string.Empty;
    var validDateTime = false;
    var datetimeValue = DateTime.UtcNow; 
    while (!validDateTime)
    {
        datetimeValue = DateTime.UtcNow;
        Console.WriteLine(outputMsg);
        dateTime = Console.ReadLine();
        

        if (dateTime is null || !DateTime.TryParse(dateTime, out datetimeValue))
        {
            Console.WriteLine("Invalid DateTime");
          validDateTime= false;
        }
        else
        {
            validDateTime = true;
        }
    }
   

    return datetimeValue;
}

void PrintMessage(string message)
{
    Console.WriteLine(message);
}

Console.ReadLine();