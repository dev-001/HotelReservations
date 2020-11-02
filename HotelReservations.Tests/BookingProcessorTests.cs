using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservations.Data;
using HotelReservations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MotelBooking.Tests
{
    [TestClass]
    public class BookingProcessorTests
    {
        [TestMethod]
        public async Task Book_Room_Outside_Of_Planning_Period_Size_1()
        {
            IHotelRepository repo = new HotelRepository(1);
            IBookingProcessor processor = new BookingProcessor(repo);
            try
            {
                await processor.BookFirstAvailableRoomAsync(new Reservation { StartDay = -4, EndDay = 2 });
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public async Task Book_Room_Outside_Of_Planning_Period_Year_Size_1()
        {
            IHotelRepository repo = new HotelRepository(1);
            IBookingProcessor processor = new BookingProcessor(repo);

            try
            {
                await processor.BookFirstAvailableRoomAsync(new Reservation { StartDay = 200, EndDay = 400 });
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public async Task Book_Room_Request_Are_Accepted_Size_3()
        {
            IHotelRepository repo = new HotelRepository(3);
            IBookingProcessor processor = new BookingProcessor(repo);

            List<Tuple<int, int>> list = new List<Tuple<int, int>>();
            list.Add(new Tuple<int, int>(0, 5));
            list.Add(new Tuple<int, int>(7, 13));
            list.Add(new Tuple<int, int>(3, 9));
            list.Add(new Tuple<int, int>(5, 7));
            list.Add(new Tuple<int, int>(6, 6));
            list.Add(new Tuple<int, int>(0, 4));
            foreach (var request in list)
            {
                try
                {
                    await processor.BookFirstAvailableRoomAsync(new Reservation { StartDay = request.Item1, EndDay = request.Item2 });
                    Assert.IsTrue(true);
                }
                catch (Exception ex)
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public async Task Book_Room_Request_Are_Declined_Size_3()
        {
            IHotelRepository repo = new HotelRepository(3);
            IBookingProcessor processor = new BookingProcessor(repo);

            List<Tuple<int, int>> list = new List<Tuple<int, int>>();
            list.Add(new Tuple<int, int>(1, 3));
            list.Add(new Tuple<int, int>(3, 5));
            list.Add(new Tuple<int, int>(1, 9));
            foreach (var request in list)
            {
                try
                {
                    await processor.BookFirstAvailableRoomAsync(new Reservation { StartDay = request.Item1, EndDay = request.Item2 });
                    Assert.IsTrue(true);
                }
                catch (Exception)
                {
                    Assert.Fail();
                }
            }

            try
            {
                await processor.BookFirstAvailableRoomAsync(new Reservation { StartDay = 0, EndDay = 15 });
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public async Task Book_Room_Request_Can_Be_Accepted_After_Decline_Size_3()
        {
            IHotelRepository repo = new HotelRepository(3);
            IBookingProcessor processor = new BookingProcessor(repo);

            List<Tuple<int, int>> list = new List<Tuple<int, int>>();
            list.Add(new Tuple<int, int>(1, 3));
            list.Add(new Tuple<int, int>(0, 15));
            list.Add(new Tuple<int, int>(1, 9));
            foreach (var request in list)
            {
                try
                {
                    await processor.BookFirstAvailableRoomAsync(new Reservation { StartDay = request.Item1, EndDay = request.Item2 });
                    Assert.IsTrue(true);
                }
                catch (Exception)
                {
                    Assert.Fail();
                }
            }
            try
            {
                await processor.BookFirstAvailableRoomAsync(new Reservation { StartDay = 2, EndDay = 5 });
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
            try
            {
                await processor.BookFirstAvailableRoomAsync(new Reservation { StartDay = 4, EndDay = 9 });
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task Book_Room_Complex_Requests_Size_2()
        {
            IHotelRepository repo = new HotelRepository(2);
            IBookingProcessor processor = new BookingProcessor(repo);

            List<Tuple<int, int, bool>> list = new List<Tuple<int, int, bool>>();
            list.Add(new Tuple<int, int, bool>(1, 3, true));
            list.Add(new Tuple<int, int, bool>(0, 4, true));
            list.Add(new Tuple<int, int, bool>(2, 3, false));
            list.Add(new Tuple<int, int, bool>(5, 5, true));
            list.Add(new Tuple<int, int, bool>(4, 10, true));
            list.Add(new Tuple<int, int, bool>(10, 10, true));
            list.Add(new Tuple<int, int, bool>(6, 7, true));
            list.Add(new Tuple<int, int, bool>(8, 10, false));
            list.Add(new Tuple<int, int, bool>(8, 9, true));

            foreach (var request in list)
            {
                try
                {
                    await processor.BookFirstAvailableRoomAsync(new Reservation { StartDay = request.Item1, EndDay = request.Item2 });
                    Assert.IsTrue(request.Item3);
                }
                catch (Exception)
                {
                    Assert.IsFalse(request.Item3);
                }
            }
        }
    }
}
