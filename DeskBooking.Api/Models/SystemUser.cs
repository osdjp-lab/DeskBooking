using System.ComponentModel.DataAnnotations;

namespace DeskBooking.Data
{
    public class SystemUser
    {
        [Key]
        public int SystemUserId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Boolean IsAdmin { get; set; }

    }
}
