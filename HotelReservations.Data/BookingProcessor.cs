using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using HotelReservations.Models;
using System.Runtime.CompilerServices;

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
            if (rooms.Count == 1 || rooms.Sum(x => x.Reservations.Count) == 0)
                return rooms.FirstOrDefault();
            IRoom roomCandidate = null;
            foreach(var room in rooms)
            {
                // if there is a room that has ending reservation day before new one we will take that room to maximize space 
                foreach (var r in room.Reservations)
                {
                    int daysLeft = r.EndDay - reservation.StartDay;
                    if(daysLeft == -1)
                    {
                        roomCandidate = room;
                        break;
                    }
                }

                if(roomCandidate == room)
                {
                    // means we found perfect candidate
                    break;
                }

                if (roomCandidate == null)
                {
                    roomCandidate = room;
                    continue;
                }
            }
            return roomCandidate;
        }

        public async Task<IRoom> GetRoomByNumber(int roomNum)
        {
            return await _hotelRepository.GetRoomByNumber(roomNum);
        }
    }
}
