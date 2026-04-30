using HotelAgent.Api.Dtos;
using HotelAgent.Api.Mappers;
using HotelAgent.Data;
using HotelAgent.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelAgent.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly HotelDbContext _db;
    private readonly ILogger<ReservationsController> _logger;

    public ReservationsController(HotelDbContext db, ILogger<ReservationsController> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// Returns reservations arriving today that are still in Confirmed status.
    /// </summary>
    [HttpGet("today-arrivals")]
    public async Task<ActionResult<IReadOnlyList<ReservationListItemDto>>> GetTodayArrivals(
        CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        _logger.LogInformation("Fetching today's arrivals for {Date}", today);

        var reservations = await _db.Reservations
            .Include(r => r.Guest)
            .Include(r => r.Room)
            .Where(r => r.CheckInDate == today && r.Status == ReservationStatus.Confirmed)
            .OrderBy(r => r.Guest.LastName)
            .ToListAsync(cancellationToken);

        var dtos = reservations.Select(r => r.ToListItemDto()).ToList();

        return Ok(dtos);
    }
}