using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservations.Models;

namespace HotelReservations.Data
{
    public interface IBookingDataAdapter
    {
        Task<IRoom> BookFirstAvailableRoomAsync(IReservation reservation);
        Task<IRoom> BookRoomByNumberAsync(IReservation reservation, int roomNum);
        Task<ICollection<IRoom>> GetAllAvailbleRooms(IReservation reservation);
    }
}