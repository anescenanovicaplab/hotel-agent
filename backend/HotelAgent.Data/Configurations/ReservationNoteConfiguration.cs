using HotelAgent.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelAgent.Data.Configurations;

public class ReservationNoteConfiguration : IEntityTypeConfiguration<ReservationNote>
{
    public void Configure(EntityTypeBuilder<ReservationNote> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Content)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(n => n.AddedBy)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne(n => n.Reservation)
            .WithMany(r => r.Notes)
            .HasForeignKey(n => n.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(n => n.ReservationId);
    }
}