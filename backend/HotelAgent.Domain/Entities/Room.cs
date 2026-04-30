using HotelAgent.Domain.Enums;

namespace HotelAgent.Domain.Entities;

public class Room
{
    public int Id { get; set; }
    public required string RoomNumber { get; set; }
    public RoomType Type { get; set; }
    public RoomStatus Status { get; set; }
    public decimal BaseRate { get; set; }
    public int MaxOccupancy { get; set; }

    public List<Reservation> Reservations { get; set; } = new();
}