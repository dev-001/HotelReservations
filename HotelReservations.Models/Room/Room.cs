using System.Collections.Generic;
using System.Linq;

namespace HotelReservations.Models
{
    public class Room : IRoom
    {
        public Room(int number)
        {
            Number = number;
            Reservations = new HashSet<IReservation>();
        }

        public int Number { get; set; }
        public ICollection<IReservation> Reservations { get; private set; }

        public bool IsRoomAvailableForBooking(IReservation reservationRequest)
        {
            if (Reservations.Count == 0)
                return true;
            // inbound
            if (Reservations.Any(x => reservationRequest.StartDay >= x.StartDay && reservationRequest.StartDay <= x.EndDay))
                return false;
            if (Reservations.Any(x => reservationRequest.EndDay >= x.StartDay && reservationRequest.EndDay <= x.EndDay))
                return false;
            // outbound
            if (Reservations.Any(x => reservationRequest.StartDay <= x.StartDay && reservationRequest.EndDay <= x.EndDay && reservationRequest.EndDay >= x.StartDay))
                return false;
            if (Reservations.Any(x => reservationRequest.StartDay >= x.StartDay && reservationRequest.StartDay <= x.EndDay && reservationRequest.EndDay > x.EndDay))
                return false;
            if (Reservations.Any(x => reservationRequest.StartDay <= x.StartDay && reservationRequest.EndDay >= x.EndDay))
                return false;

            return true;
        }
    }
}
