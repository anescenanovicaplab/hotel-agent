using HotelAgent.Api.Dtos;
using HotelAgent.Domain.Entities;

namespace HotelAgent.Api.Mappers;

public static class ReservationMapper
{
    public static ReservationListItemDto ToListItemDto(this Reservation reservation)
    {
        return new ReservationListItemDto(
            ConfirmationNumber: reservation.ConfirmationNumber,
            GuestName: reservation.Guest.FullName,
            IsVip: reservation.Guest.IsVip,
            RequestedRoomType: reservation.RequestedRoomType.ToString(),
            AssignedRoomNumber: reservation.Room?.RoomNumber,
            CheckInDate: reservation.CheckInDate,
            CheckOutDate: reservation.CheckOutDate,
            NumberOfGuests: reservation.NumberOfGuests,
            NumberOfNights: reservation.NumberOfNights,
            TotalAmount: reservation.TotalAmount,
            Status: reservation.Status.ToString()
        );
    }
}