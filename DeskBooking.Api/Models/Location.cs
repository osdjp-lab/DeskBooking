using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace DeskBooking.Data
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
    }
}