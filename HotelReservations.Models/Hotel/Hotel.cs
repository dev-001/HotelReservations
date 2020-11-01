using System.Collections.Generic;

namespace HotelReservations.Models
{
    public class Hotel : IHotel
    {
        public Hotel()
        {
            Rooms = new HashSet<IRoom>();
        }
        public int Size { get; set; }
        public ICollection<IRoom> Rooms { get; private set; }
    }
}
