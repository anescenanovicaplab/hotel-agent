using HotelAgent.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelAgent.Data;

public class HotelDbContext : DbContext
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Guest> Guests { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<Reservation> Reservations { get; set; } = null!;
    public DbSet<ReservationNote> ReservationNotes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HotelDbContext).Assembly);
    }
}