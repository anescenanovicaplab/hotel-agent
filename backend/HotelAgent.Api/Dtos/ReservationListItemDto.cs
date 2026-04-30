namespace HotelAgent.Api.Dtos;

public record ReservationListItemDto(
    string ConfirmationNumber,
    string GuestName,
    bool IsVip,
    string RequestedRoomType,
    string? AssignedRoomNumber,
    DateOnly CheckInDate,
    DateOnly CheckOutDate,
    int NumberOfGuests,
    int NumberOfNights,
    decimal TotalAmount,
    string Status
);