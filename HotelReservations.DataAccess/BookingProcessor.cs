using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using HotelReservations.Models;

namespace HotelReservations.Data
{
    public class BookingProcessor : IBookingProcessor
    {
        private readonly IHotelRepository _hotelRepository;

        public BookingProcessor(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        // make assumption here that user can request a specific room number
        public async Task<IRoom> BookRoomByNumberAsync(IReservation reservation, int roomNum)
        {
            var room = await _hotelRepository.GetRoomByNumber(roomNum);

            if (!room.IsRoomAvailableForBooking(reservation))
                throw new Exception($"Room {roomNum} is not available for booking");

            await _hotelRepository.AddBookedRoom(reservation, roomNum);
            return room;
        }

        public async Task<IRoom> BookFirstAvailableRoomAsync(IReservation reservation)
        {
            var rooms = await _hotelRepository.GetAllAvailbleRooms(reservation);

            if (rooms == null || rooms.Count == 0)
                throw new Exception($"There is no room available for booking on requested days {reservation.StartDay} to {reservation.EndDay}");
            var reservationRoom = GetBestRoomToBook(rooms, reservation);
            await _hotelRepository.AddBookedRoom(reservation, reservationRoom.Number);
            return reservationRoom;
        }

        private IRoom GetBestRoomToBook(ICollection<IRoom> rooms, IReservation reservation)
        {
            if(rooms.Count == 1 || rooms.Sum(x => x.Reservations.Count) == 0)
                return rooms.FirstOrDefault();
            var orderedList = rooms
                .Where(x => x.Reservations.Min(y => y.EndDay < reservation.EndDay))
                .Select(x => new { room = x, endDate = Math.Abs(x.Reservations.Min(y => y.EndDay) - reservation.EndDay)})
                .OrderBy(x => x.endDate);
            return orderedList.FirstOrDefault().room;
        }

        public async Task<IRoom> GetRoomByNumber(int roomNum)
        {
            return await _hotelRepository.GetRoomByNumber(roomNum);
        }
    }
}
