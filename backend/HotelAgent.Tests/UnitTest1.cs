using HotelAgent.Domain.Entities;
using HotelAgent.Domain.Enums;

namespace HotelAgent.Tests;

public class ReservationTests
{
    [Fact]
    public void NumberOfNights_CalculatesCorrectly()
    {
        var reservation = new Reservation
        {
            ConfirmationNumber = "TEST-001",
            CheckInDate = new DateOnly(2026, 5, 1),
            CheckOutDate = new DateOnly(2026, 5, 4),
            RequestedRoomType = RoomType.Standard,
            RatePerNight = 150m,
            NumberOfGuests = 2,
            Status = ReservationStatus.Confirmed
        };

        Assert.Equal(3, reservation.NumberOfNights);
    }

    [Fact]
    public void TotalAmount_AppliesDiscount()
    {
        var reservation = new Reservation
        {
            ConfirmationNumber = "TEST-002",
            CheckInDate = new DateOnly(2026, 5, 1),
            CheckOutDate = new DateOnly(2026, 5, 3),
            RequestedRoomType = RoomType.Deluxe,
            RatePerNight = 200m,
            DiscountAmount = 50m,
            NumberOfGuests = 1,
            Status = ReservationStatus.Confirmed
        };

        // 2 nights * $200 = $400, minus $50 discount = $350
        Assert.Equal(350m, reservation.TotalAmount);
    }

    [Fact]
    public void Guest_FullName_Concatenates()
    {
        var guest = new Guest
        {
            FirstName = "Priya",
            LastName = "Patel",
            Email = "priya@example.com"
        };

        Assert.Equal("Priya Patel", guest.FullName);
    }

}
