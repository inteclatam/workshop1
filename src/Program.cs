using Intec.Workshop1.Customers;
using Intec.Workshop1.Customers.Application.Features.CreateCustomer;
using Intec.Workshop1.Customers.Infrastructure.Configuration;
using Intec.Workshop1.Customers.Infrastructure.SnowflakeId;
using Intec.Workshop1.Customers.Infrastructure.Exceptions;
using Scalar.AspNetCore;
using Serilog;
using Spectre.Console;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?? "Development";
var builder = WebApplication.CreateBuilder(args);
var configuration=builder.Configuration
    .AddJsonFile("appsettings.json",optional:false,reloadOnChange:true)
    .AddEnvironmentVariables().Build();

DisplayHeader(environment);

void DisplayHeader(string environmentName,string applicationName="Customers Api")
{
    AnsiConsole.Write(
        new FigletText(applicationName)
            .LeftJustified()
            .Color(Color.Green));
}

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);

// Exception Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Logging
builder.Host.UseSerilog(
    (context, loggerConfiguration)
        => loggerConfiguration.ReadFrom.Configuration(context.Configuration));
var app = builder.Build();

// Exception handling middleware
app.UseExceptionHandler();



app.MapOpenApi();

//if (app.Environment.IsDevelopment())
//{
    app.MapScalarApiReference();
//}
//app.UseHttpsRedirection();

app.MapCreateCustomerEndpoint();



app.Run();

