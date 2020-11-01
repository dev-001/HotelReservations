using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservations.API.Controllers;
using HotelReservations.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MotelBooking.Tests
{
    [TestClass]
    public class BookingControllerTests
    {
        [TestMethod]
        public async Task Book_Room_Outside_Of_Planning_Period_Size_1()
        {
            IHotelRepository repo = new HotelRepository(1);
            IBookingDataAdapter da = new BookingDataAdapter(repo, new BookingProcessor(repo));

            BookingController controller = new BookingController(da);
            var response = await controller.BookFirstAvailableRoom(-4, 2);

            Assert.AreEqual(((ObjectResult)response).StatusCode, 400);
        }

        [TestMethod]
        public async Task Book_Room_Outside_Of_Planning_Period_Year_Size_1()
        {
            IHotelRepository repo = new HotelRepository(1);
            IBookingDataAdapter da = new BookingDataAdapter(repo, new BookingProcessor(repo));

            BookingController controller = new BookingController(da);
            var response = await controller.BookFirstAvailableRoom(200, 400);

            Assert.AreEqual(((ObjectResult)response).StatusCode, 400);
        }

        [TestMethod]
        public async Task Book_Room_Request_Are_Accepted_Size_3()
        {
            IHotelRepository repo = new HotelRepository(3);
            IBookingDataAdapter da = new BookingDataAdapter(repo, new BookingProcessor(repo));

            BookingController controller = new BookingController(da);
            List<Tuple<int, int>> list = new List<Tuple<int, int>>();
            list.Add(new Tuple<int, int>(0, 5));
            list.Add(new Tuple<int, int>(7, 13));
            list.Add(new Tuple<int, int>(3, 9));
            list.Add(new Tuple<int, int>(5, 7));
            list.Add(new Tuple<int, int>(6, 6));
            list.Add(new Tuple<int, int>(0, 4));
            foreach(var request in list)
            {
                var response = await controller.BookFirstAvailableRoom(request.Item1,request.Item2);

                Assert.AreEqual(((ObjectResult)response).StatusCode, 201);
            }
        }

        [TestMethod]
        public async Task Book_Room_Request_Are_Declined_Size_3()
        {
            IHotelRepository repo = new HotelRepository(3);
            IBookingDataAdapter da = new BookingDataAdapter(repo, new BookingProcessor(repo));

            BookingController controller = new BookingController(da);
            List<Tuple<int, int>> list = new List<Tuple<int, int>>();
            list.Add(new Tuple<int, int>(1, 3));
            list.Add(new Tuple<int, int>(3, 5));
            list.Add(new Tuple<int, int>(1, 9));
            foreach (var request in list)
            {
                var response = await controller.BookFirstAvailableRoom(request.Item1, request.Item2);

                Assert.AreEqual(((ObjectResult)response).StatusCode, 201);
            }

            var badResponse = await controller.BookFirstAvailableRoom(0,15);

            Assert.AreEqual(((ObjectResult)badResponse).StatusCode, 400);
        }

        [TestMethod]
        public async Task Book_Room_Request_Can_Be_Accepted_After_Decline_Size_3()
        {
            IHotelRepository repo = new HotelRepository(3);
            IBookingDataAdapter da = new BookingDataAdapter(repo, new BookingProcessor(repo));

            BookingController controller = new BookingController(da);
            List<Tuple<int, int>> list = new List<Tuple<int, int>>();
            list.Add(new Tuple<int, int>(1, 3));
            list.Add(new Tuple<int, int>(0, 15));
            list.Add(new Tuple<int, int>(1, 9));
            foreach (var request in list)
            {
                var response = await controller.BookFirstAvailableRoom(request.Item1, request.Item2);

                Assert.AreEqual(((ObjectResult)response).StatusCode, 201);
            }

            var badResponse = await controller.BookFirstAvailableRoom(2, 5);

            Assert.AreEqual(((ObjectResult)badResponse).StatusCode, 400);

            var createdResponse = await controller.BookFirstAvailableRoom(4, 9);

            Assert.AreEqual(((ObjectResult)createdResponse).StatusCode, 201);
        }


        [TestMethod]
        public async Task Book_Room_Complex_Requests_Size_2()
        {
            IHotelRepository repo = new HotelRepository(3);
            IBookingDataAdapter da = new BookingDataAdapter(repo, new BookingProcessor(repo));

            BookingController controller = new BookingController(da);
            List<Tuple<int, int, int>> list = new List<Tuple<int, int, int>>();
            list.Add(new Tuple<int, int, int>(1, 3, 201));
            list.Add(new Tuple<int, int, int>(0, 4, 201));
            list.Add(new Tuple<int, int, int>(2, 3, 400));
            list.Add(new Tuple<int, int, int>(5, 5, 201));
            list.Add(new Tuple<int, int, int>(4, 10, 201));
            list.Add(new Tuple<int, int, int>(10, 10, 201));
            list.Add(new Tuple<int, int, int>(6, 7, 201));
            list.Add(new Tuple<int, int, int>(8, 10, 400));
            list.Add(new Tuple<int, int, int>(8, 9, 201));

            foreach (var request in list)
            {
                var response = await controller.BookFirstAvailableRoom(request.Item1, request.Item2);

                Assert.AreEqual(((ObjectResult)response).StatusCode, request.Item3);
            }
        }
    }

}
