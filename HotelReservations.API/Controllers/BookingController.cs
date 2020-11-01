using System;
using System.Threading.Tasks;
using HotelReservations.Data;
using HotelReservations.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservations.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : Controller
    {
        private readonly IBookingDataAdapter _bookingDataAdapter;

        public BookingController(IBookingDataAdapter bookingDataAdapter)
        {
            _bookingDataAdapter = bookingDataAdapter;
        }

        [HttpPost]
        [Route("BookRoomByNumber")]
        public async Task<IActionResult> BookRoomByNumber(int startDate, int endDate, int roomNum)
        {
            try
            {
                var room = await _bookingDataAdapter.BookRoomByNumberAsync(new Reservation { StartDay = startDate, EndDay = endDate }, roomNum);
                return Created("api/Booking/BookRoom", $"Successfully booked room {room.Number}.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("BookFirstAvailableRoom")]
        public async Task<IActionResult> BookFirstAvailableRoom(int startDate, int endDate)
        {
            try
            {
                var room = await _bookingDataAdapter.BookFirstAvailableRoomAsync(new Reservation { StartDay = startDate, EndDay = endDate });
                return Created("api/Booking/BookRoom", $"Successfully booked room {room.Number}.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
