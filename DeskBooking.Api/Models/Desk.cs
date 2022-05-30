using System.ComponentModel.DataAnnotations;

namespace DeskBooking.Data
{
    public class Desk
    {
        [Key]
        public int DeskId { get; set; }
        public int LocationId { get; set; }
        public string? Note { get; set; }
    }
}
