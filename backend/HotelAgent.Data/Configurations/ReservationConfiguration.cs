using HotelAgent.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelAgent.Data.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.ConfirmationNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(r => r.RatePerNight)
            .HasPrecision(10, 2);

        builder.Property(r => r.DiscountAmount)
            .HasPrecision(10, 2);

        builder.Property(r => r.RequestedRoomType)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(r => r.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Relationships
        builder.HasOne(r => r.Guest)
            .WithMany(g => g.Reservations)
            .HasForeignKey(r => r.GuestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Room)
            .WithMany(room => room.Reservations)
            .HasForeignKey(r => r.RoomId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(r => r.ConfirmationNumber).IsUnique();
        builder.HasIndex(r => r.CheckInDate);
        builder.HasIndex(r => r.Status);
    }
}