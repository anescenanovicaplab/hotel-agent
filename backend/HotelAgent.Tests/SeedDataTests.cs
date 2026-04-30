using HotelAgent.Data;
using HotelAgent.Data.Seeding;
using HotelAgent.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HotelAgent.Tests;

public class SeedDataTests
{
    private static HotelDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<HotelDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        var context = new HotelDbContext(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task Seeder_CreatesExpectedCounts()
    {
        await using var ctx = CreateInMemoryContext();
        await HotelSeeder.SeedAsync(ctx);

        Assert.Equal(23, await ctx.Rooms.CountAsync());
        Assert.Equal(10, await ctx.Guests.CountAsync());
        Assert.True(await ctx.Reservations.CountAsync() >= 10);
        Assert.True(await ctx.ReservationNotes.CountAsync() >= 3);
    }

    [Fact]
    public async Task Seeder_IsIdempotent()
    {
        await using var ctx = CreateInMemoryContext();
        await HotelSeeder.SeedAsync(ctx);
        var firstCount = await ctx.Reservations.CountAsync();

        await HotelSeeder.SeedAsync(ctx);
        var secondCount = await ctx.Reservations.CountAsync();

        Assert.Equal(firstCount, secondCount);
    }

    [Fact]
    public async Task Seeder_HasReservationsArrivingToday()
    {
        await using var ctx = CreateInMemoryContext();
        await HotelSeeder.SeedAsync(ctx);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var arrivingToday = await ctx.Reservations
            .Where(r => r.CheckInDate == today && r.Status == ReservationStatus.Confirmed)
            .CountAsync();

        Assert.True(arrivingToday > 0, "Demo expects reservations arriving today");
    }
}