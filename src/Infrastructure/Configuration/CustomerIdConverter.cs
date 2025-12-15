using Intec.Workshop1.Customers.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Intec.Workshop1.Customers.Infrastructure.Configuration;

public class CustomerIdConverter : ValueConverter<CustomerId, long>
{
    public CustomerIdConverter():base(
        id => id.Value,
        value => new CustomerId(value)
    )
    {
        
    }
    
}