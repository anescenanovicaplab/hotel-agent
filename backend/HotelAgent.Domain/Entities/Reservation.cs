using HotelAgent.Domain.Enums;

namespace HotelAgent.Domain.Entities;

public class Reservation
{
    public int Id { get; set; }
    public required string ConfirmationNumber { get; set; }

    public int GuestId { get; set; }
    public Guest Guest { get; set; } = null!;

    public int? RoomId { get; set; }
    public Room? Room { get; set; }

    public RoomType RequestedRoomType { get; set; }

    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }

    public decimal RatePerNight { get; set; }
    public decimal? DiscountAmount { get; set; }

    public ReservationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<ReservationNote> Notes { get; set; } = new();

    public int NumberOfNights =>
        CheckOutDate.DayNumber - CheckInDate.DayNumber;

    public decimal TotalAmount =>
        (RatePerNight * NumberOfNights) - (DiscountAmount ?? 0);
}