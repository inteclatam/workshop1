using Intec.Workshop1.Customers.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Intec.Workshop1.Customers.Infrastructure.Configuration;

public class ContactInformationConfigurationMapping:IEntityTypeConfiguration<ContactInformation>
{
    public void Configure(EntityTypeBuilder<ContactInformation> builder)
    {
        builder.HasKey(c => c.Id);
         builder.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(50);
         builder.Property(c => c.Email).IsRequired().HasMaxLength(50);
         builder.HasIndex(c => c.Email).IsUnique();
         builder.Property(c => c.IsVerified).HasConversion<string>();
//         builder.OwnsOne()
    }
}
public class CustomerConfigurationMapping:IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
    }
}