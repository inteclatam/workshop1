using Intec.Workshop1.Customers.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Intec.Workshop1.Customers.Infrastructure.Configuration;

public class ContactInformationConfigurationMapping : IEntityTypeConfiguration<ContactInformation>
{
    public void Configure(EntityTypeBuilder<ContactInformation> builder)
    {
        // Config tabla
        builder.ToTable("ContactInformation","customers");


        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .ValueGeneratedNever();
        // l value object Email
        builder.OwnsOne(c => c.Email, emailBuilder =>
        {
            emailBuilder.Property(e => e.Value)
                .HasColumnName("Email")
                .HasMaxLength(255)
                .IsRequired();

            emailBuilder.HasIndex(e => e.Value)
                .IsUnique();
        });

        
        // value object PhoneNumber
        builder.OwnsOne(c => c.PhoneNumber, phoneBuilder =>
        {
            phoneBuilder.Property(p => p.Number)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(20)
                .IsRequired();

            phoneBuilder.Property(p => p.Prefix)
                .HasColumnName("PhonePrefix")
                .HasMaxLength(10)
                .IsRequired();

            phoneBuilder.Property(p => p.Value)
                .HasColumnName("PhoneValue")
                .HasMaxLength(30)
                .IsRequired();
        });

        builder.Property(c => c.IsVerified)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(c => c.IsPrimary)
            .IsRequired()
            .HasDefaultValue(false);

        // Configure foreign key with same conversion as Customer.Id
        builder.Property(c => c.CustomerId)
            .HasConversion(new CustomerIdConverter())
            .IsRequired();

        builder.HasIndex(c => c.CustomerId);
        builder.HasIndex(c => c.IsPrimary);
    }
}