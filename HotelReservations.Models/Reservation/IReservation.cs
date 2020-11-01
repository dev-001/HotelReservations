namespace HotelReservations.Models
{
    public interface IReservation
    {
        int StartDay { get; set; }
        int EndDay { get; set; }
    }
}