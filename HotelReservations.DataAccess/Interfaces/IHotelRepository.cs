using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservations.Models;

namespace HotelReservations.Data
{
    public interface IHotelRepository
    {
        Task AddBookedRoom(IReservation reservation, int number);
        Task<ICollection<IRoom>> GetAllAvailbleRooms(IReservation reservation);
        Task<IRoom> GetRoomByNumber(int roomNum);
    }
}