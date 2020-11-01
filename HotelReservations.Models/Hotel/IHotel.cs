using System.Collections.Generic;

namespace HotelReservations.Models
{
    public interface IHotel
    {
        int Size { get; set; }
        ICollection<IRoom> Rooms { get; }
    }
}