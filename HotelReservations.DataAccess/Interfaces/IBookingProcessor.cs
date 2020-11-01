using System.Threading.Tasks;
using HotelReservations.Models;

namespace HotelReservations.Data
{
    public interface IBookingProcessor
    {
        Task<IRoom> BookFirstAvailableRoomAsync(IReservation reservation);
        Task<IRoom> BookRoomByNumberAsync(IReservation reservation, int roomNum);
        Task<IRoom> GetRoomByNumber(int roomNum);
    }
}