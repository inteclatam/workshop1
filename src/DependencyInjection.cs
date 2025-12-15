using Intec.Workshop1.Customers.Infrastructure;
using Intec.Workshop1.Customers.Infrastructure.SnowflakeId;
using Microsoft.EntityFrameworkCore;
using Serilog.Core;

namespace Intec.Workshop1.Customers;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CustomersDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );

    // Configure IdGenerator (Snowflake)
        var workerId = configuration.GetValue<ushort>("IdGenerator:WorkerId");
        var datacenterId = configuration.GetValue<ushort>("IdGenerator:DatacenterId");


        var idGeneratorOptions = new IdGeneratorOptions
        {
            WorkerId = workerId,
            DatacenterId = datacenterId
        };
        services.AddSingleton<IIdGeneratorPool>(sp => new DefaultIdGeneratorPool(idGeneratorOptions));
        services.AddSingleton<IIdGenerator, SnowflakeIdGenerator>();

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}