using System.Runtime.InteropServices;
using dotenv.net;
using Intec.Workshop1.Customers;
using Intec.Workshop1.Customers.Application.Features.CreateCustomer;
using Intec.Workshop1.Customers.Application.Features.GetCustomerById;
using Intec.Workshop1.Customers.Application.Features.GetAllCustomers;
using Intec.Workshop1.Customers.Application.Features.UpdateCustomer;
using Intec.Workshop1.Customers.Application.Features.ChangeCustomerName;
using Intec.Workshop1.Customers.Application.Features.DeleteCustomer;
using Intec.Workshop1.Customers.Infrastructure;
using Intec.Workshop1.Customers.Infrastructure.Configuration;
using Intec.Workshop1.Customers.Infrastructure.SnowflakeId;
using Intec.Workshop1.Customers.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SpectreConsole;
using Spectre.Console;


DotEnv.Load();


var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?? "Development";
var builder = WebApplication.CreateBuilder(args);
var configuration=builder.Configuration
    .AddJsonFile("appsettings.json",optional:false,reloadOnChange:true)
    .AddEnvironmentVariables().Build();
var applicationName=configuration.GetValue<string>("APP_NAME");
var kestrelPort = configuration.GetValue<int>("KESTREL_PORT");

DisplayHeader(environment,applicationName!,kestrelPort.ToString());
DisplaySystemInfo();
DisplayEnvironmentVariablesTable();
ConfigureKestrel(builder, kestrelPort);
void DisplayHeader(string environmentName,string appName,string kPort)
{
    AnsiConsole.Write(
        new FigletText(appName)
            .LeftJustified()
            .Color(Color.Green));
    AnsiConsole.WriteLine();
    AnsiConsole.Write(new Markup($"[bold yellow]Environment:[/] [yellow]{environmentName}[/]"));
    
    AnsiConsole.WriteLine();
}
void DisplayRule(string? ruleTitle, string body, bool endRule = true, bool success = true)
{
    var emoji = success ? ":check_mark:" : ":cross_mark:";
    var color = success ? "green" : "red";
    var rule = !string.IsNullOrEmpty(ruleTitle) ? new Rule($"[{color}]{ruleTitle}[/]") : new Rule();
    rule.Justification = Justify.Center;
    AnsiConsole.Write(rule);
    AnsiConsole.MarkupLine($"{emoji}   {body}");
    if (endRule) AnsiConsole.Write(new Rule());
}
void DisplayEnvironmentVariablesTable()
{
    var table = new Table();
    table.AddColumn("Key");
    table.AddColumn("Value");

    var environmentVariables = Environment.GetEnvironmentVariables();
    foreach (var key in environmentVariables.Keys)
    {
        table.AddRow(key.ToString(), environmentVariables[key]?.ToString() ?? "null");
    }

    AnsiConsole.Write(table);
}
void DisplaySystemInfo()
{
    var os = RuntimeInformation.OSDescription;
    var timeZone = TimeZoneInfo.Local;
    //var currentTime = TimeUtil.ConvertUtcToTimeZone(DateTime.UtcNow, "America/Bogota");

    var table = new Table();
    table.AddColumn("System Info");
    table.AddColumn("Details");

    table.AddRow("Operating System", os);
    table.AddRow("Time Zone", timeZone.DisplayName);
    table.AddRow("Current Date Time", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));

    AnsiConsole.Write(table);
}
void ConfigureKestrel(WebApplicationBuilder contextBuilder, int kestrelPort)
{

    contextBuilder.WebHost.ConfigureKestrel(options =>
    {
        
        options.ListenAnyIP(kestrelPort, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http2 | HttpProtocols.Http1;
            
        });
    });

    var message = $"Using Kestrel On Port: [bold yellow]{kestrelPort}[/]";
    DisplayRule("Kestrel", message, false, kestrelPort > 0);

}

builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);

// Exception Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", applicationName)
    .Enrich.WithProperty("Environment", environment)
    .Enrich.WithProperty("Version", "1.0.0")
    .WriteTo.Console()
    .WriteTo.SpectreConsole(outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();

builder.Host.UseSerilog();


Log.Information("Hello, {Name}!", Environment.UserName);

var app = builder.Build();

// Exception handling middleware
app.UseExceptionHandler();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CustomersDbContext>();
    await DatabaseSeederRuntime.SeedAsync(db);
}


app.MapOpenApi();

//if (app.Environment.IsDevelopment())
//{
    app.MapScalarApiReference();
//}
//app.UseHttpsRedirection();

app.MapCreateCustomerEndpoint();
app.MapGetCustomerByIdEndpoint();
app.MapGetAllCustomersEndpoint();
app.MapUpdateCustomerEndpoint();
app.MapChangeCustomerNameEndpoint();
app.MapDeleteCustomerEndpoint();


app.Run();

