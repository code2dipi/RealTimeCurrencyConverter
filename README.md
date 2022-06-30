# ConsoleCurrencyConverter
- ConsoleCurrency Converter Application was developed in Visual studio 2022 using C#,EntityFrameworkCore, ASP.NET Core web APP. 
- The goal is to convert currency in real time.
- The project will integrate the fixer API to fetch real time rates of all currencies .

# Three are 4 different projects located inside this project:
- ConsoleCurrencyConverter: This will communicate with ConverterService and and output the real time conversion from one currency to another.
- ConvertEntityFramework:  This will use ConverterService and store all currencies rate and send to MS-SQLSERVER Database every 24 hrs. 
- ConverterService: This is a main business logic which is responsible to call fixer API and communicate with others projects.
 -CurrencyConverterWebApi: This is simple REST API which is based up on Console CurrencyConverter.Web UI has been built on the top of it using Swagger doc.
 
 

