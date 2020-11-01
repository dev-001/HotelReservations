using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservations.Models;

namespace HotelReservations.Data
{
    public class BookingDataAdapter : IBookingDataAdapter
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IBookingProcessor _bookingProcessor;
        public BookingDataAdapter(IHotelRepository hotelRepository, IBookingProcessor bookingProcessor)
        {
            _hotelRepository = hotelRepository;
            _bookingProcessor = bookingProcessor;
        }

        public async Task<IRoom> BookRoomByNumberAsync(IReservation reservation, int roomNum)
        {
            return await _bookingProcessor.BookRoomByNumberAsync(reservation, roomNum);
        }

        public async Task<IRoom> BookFirstAvailableRoomAsync(IReservation reservation)
        {
            return await _bookingProcessor.BookFirstAvailableRoomAsync(reservation);
        }

        public async Task<ICollection<IRoom>> GetAllAvailbleRooms(IReservation reservation)
        {
            return await _hotelRepository.GetAllAvailbleRooms(reservation);
        }
    }
}
