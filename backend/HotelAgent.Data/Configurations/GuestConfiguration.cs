using HotelAgent.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelAgent.Data.Configurations;

public class GuestConfiguration : IEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(g => g.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(g => g.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(g => g.Phone)
            .HasMaxLength(50);

        builder.Property(g => g.Notes)
            .HasMaxLength(2000);

        builder.HasIndex(g => g.Email);
        builder.HasIndex(g => g.LastName);
    }
}