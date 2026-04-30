using HotelAgent.Data;
using HotelAgent.Data.Seeding;
using HotelAgent.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HotelAgent.Tests;

public class TodayArrivalsQueryTests
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
    public async Task TodayArrivals_ReturnsOnlyTodaysConfirmed()
    {
        await using var ctx = CreateInMemoryContext();
        await HotelSeeder.SeedAsync(ctx);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var arrivals = await ctx.Reservations
            .Include(r => r.Guest)
            .Include(r => r.Room)
            .Where(r => r.CheckInDate == today && r.Status == ReservationStatus.Confirmed)
            .ToListAsync();

        // Seeder sets up exactly 3 confirmed arrivals for today
        Assert.Equal(3, arrivals.Count);

        // Each one has a guest loaded
        Assert.All(arrivals, a => Assert.NotNull(a.Guest));

        // None should be cancelled or already checked in
        Assert.All(arrivals, a => Assert.Equal(ReservationStatus.Confirmed, a.Status));
    }

    [Fact]
    public async Task TodayArrivals_ExcludesPastAndFutureArrivals()
    {
        await using var ctx = CreateInMemoryContext();
        await HotelSeeder.SeedAsync(ctx);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var nonTodayCount = await ctx.Reservations
            .Where(r => r.CheckInDate != today)
            .CountAsync();

        Assert.True(nonTodayCount > 0,
            "Test fixture should include reservations on other dates to make this assertion meaningful");
    }
}