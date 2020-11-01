namespace HotelReservations.Models
{
    public class Reservation : IReservation
    {
        public int StartDay { get; set; }
        public int EndDay { get; set; }
    }
}
