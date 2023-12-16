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

    public interface IUserDB
    {
        public int uUserID { get; set; }
        public string? uLastName { get; set; }
        public string? uFirstMidName { get; set; }
        public DateTime uJoinDate { get; set; }
        public string? uRentalStatus { get; set; }
    }
}
