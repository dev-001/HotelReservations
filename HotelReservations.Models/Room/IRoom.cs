using System.Collections.Generic;

namespace HotelReservations.Models
{
    public interface IRoom
    {
        int Number { get; set; }
        ICollection<IReservation> Reservations { get; }
        bool IsRoomAvailableForBooking(IReservation reservationRequest);
    }
}