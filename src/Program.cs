using Intec.Workshop1.Customers.Infrastructure;
using Intec.Workshop1.Customers.Infrastructure.SnowflakeId;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure IdGenerator (Snowflake)
var workerId = builder.Configuration.GetValue<ushort>("IdGenerator:WorkerId");
var datacenterId = builder.Configuration.GetValue<ushort>("IdGenerator:DatacenterId");


var idGeneratorOptions = new IdGeneratorOptions
{
    WorkerId = workerId,
    DatacenterId = datacenterId
    };


builder.Services.AddSingleton<IIdGeneratorPool>(sp => new DefaultIdGeneratorPool(idGeneratorOptions));
builder.Services.AddSingleton<IIdGenerator, SnowflakeIdGenerator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapPost("/customers", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


