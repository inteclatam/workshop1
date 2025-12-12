using Intec.Workshop1.Customers.Application.Features.CreateCustomer;
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

app.MapCreateCustomerEndpoint();
app.Run();

