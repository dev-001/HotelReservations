using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservations.Models;

namespace HotelReservations.Data
{
    /// <summary>
    /// This class will be used as a fake database / repository to hold all our booking data
    /// </summary>
    public class HotelRepository : IHotelRepository
    {
        private IHotel Hotel { get; set; }
        public HotelRepository(int size)
        {
            Initialize(size);
        }

        private void Initialize(int size)
        {
            Hotel = new Hotel();
            Hotel.Size = size;

            // generate rooms for hotel
            for (var i = 1; i <= size; i++)
            {
                Hotel.Rooms.Add(new Room(i));
            }
        }

        public async Task<ICollection<IRoom>> GetAllAvailbleRooms(IReservation reservation)
        {
            var rooms = Hotel.Rooms.Where(x => x.IsRoomAvailableForBooking(reservation));
            return await Task.Run(() => rooms.ToHashSet());
        }

        

        public async Task<IRoom> GetRoomByNumber(int roomNum)
        {
            var room = Hotel.Rooms.SingleOrDefault(x => x.Number == roomNum);
            return await Task.Run(() => room);
        }

        public async Task AddBookedRoom(IReservation reservation, int roomNum)
        {
            var room = Hotel.Rooms.SingleOrDefault(x => x.Number == roomNum);
            if (room == null)
                throw new Exception($"Room {roomNum} not found");
            if (reservation.StartDay < 0 || reservation.EndDay > 365 || reservation.StartDay > reservation.EndDay)
                throw new Exception($"Reservation is outside of planning period, requested dates {reservation.StartDay} to {reservation.EndDay}");
            await Task.Run(() => room.Reservations.Add(reservation));
        }
    }
}
