namespace HotelAgent.Domain.Entities;

public class ReservationNote
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public required string Content { get; set; }
    public required string AddedBy { get; set; } 
    public DateTime CreatedAt { get; set; }

    public Reservation Reservation { get; set; } = null!;
}