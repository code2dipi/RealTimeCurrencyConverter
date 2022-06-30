using ConverterEntityFramework;
using ConverterService.Interface;
using ConverterService.Repository;
using CurrencyConverterWebApi.BackgroundService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Context>(o => o.UseSqlServer("Server=Dipi;Database=CurrencyConverter;Trusted_Connection=True",
    o => o.MigrationsAssembly(typeof(Context).Assembly.FullName)));
builder.Services.AddSingleton<IConverterRepo, ConverterRepo>();
builder.Services.AddTransient<IConvertCurrencyRepo, ConvertCurrencyRepo>();
builder.Services.AddHostedService<BackGroundRunner>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
