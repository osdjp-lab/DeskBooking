using System.ComponentModel.DataAnnotations;

namespace DeskBooking.Data
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }
        public int DeskId { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
