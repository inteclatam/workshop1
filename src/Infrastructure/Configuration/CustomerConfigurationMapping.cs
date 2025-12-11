using Intec.Workshop1.Customers.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Intec.Workshop1.Customers.Infrastructure.Configuration;

public class CustomerConfigurationMapping : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        // Config tabla
        builder.ToTable("Customers");

        builder.ToTable("Customers","customers");
        // clave primaria (CustomerId es un value object)
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => new CustomerId(value))
            .ValueGeneratedNever();

        // Config del value object CustomerName
        builder.OwnsOne(c => c.Name, nameBuilder =>
        {
            nameBuilder.Property(n => n.FullName)
                .HasColumnName("FullName")
                .HasMaxLength(200)
                .IsRequired();
        });

        // audit
        builder.Property(c => c.Created)
            .IsRequired();

        builder.Property(c => c.CreatedBy);

        builder.Property(c => c.LastModified);

        builder.Property(c => c.LastModifiedBy);

        //  soft delete
        builder.Property(c => c.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(c => c.Deleted);

        builder.Property(c => c.DeletedBy);

        // Configuración de la relación 1:n ContactInformation
        builder.HasMany(c => c.ContactInformations)
            .WithOne()
            .HasForeignKey(ci => ci.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.IsDeleted);

        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}