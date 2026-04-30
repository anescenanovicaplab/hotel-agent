namespace HotelAgent.Domain.Entities;

public class  Guest
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public bool IsVip { get; set; }
    public string? Notes { get; set; }

    public List<Reservation> Reservations { get; set; } = new();

    public string FullName => $"{FirstName} {LastName}";
}