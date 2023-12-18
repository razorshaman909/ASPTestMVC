using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovie.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string? LastName { get; set; }
        public string? FirstMidName { get; set; }
        public DateTime JoinDate { get; set; }
        public string? RentalStatus { get; set; }
        public ICollection<Rental>? Rentals { get; set; }

    }

}
