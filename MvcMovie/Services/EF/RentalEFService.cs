using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;
using System.Text;

namespace MvcMovie.Services.EF
{
    public class RentalEFService
    {
        private readonly MvcMovieContext _context;

        public RentalEFService(MvcMovieContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rental>> GetRentals()
        {
            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine("SELECT");
            //sb.AppendLine("[r].rRentalID");
            //sb.AppendLine("[r].rUserID");
            //sb.AppendLine(",[r].rMovieId");
            //sb.AppendLine(",[r].rRentStart");
            //sb.AppendLine(",[r].rRentEnd");
            ////user
            //sb.AppendLine(",[u].UserID AS uUserID");
            //sb.AppendLine(",[u].LastName AS uLastName");
            //sb.AppendLine(",[u].FirstMidName AS uFirstMidName");
            //sb.AppendLine(",[u].JoinDate AS uJoinDate");
            //sb.AppendLine(",[u].RentalStatus AS uRentalStatus");
            ////movie
            //sb.AppendLine(",[m].Id AS mId");
            //sb.AppendLine(",[m].Title AS mTitle");
            //sb.AppendLine(",[m].ReleaseDate AS mReleaseDate");
            //sb.AppendLine(",[m].Genre AS mGenre");
            //sb.AppendLine(",[m].Price AS mPrice");
            //sb.AppendLine(",[m].Rating AS mRating");
            //sb.AppendLine("FROM dbo.[Rentals] AS [r]");
            //sb.AppendLine("INNER JOIN dbo.[User] AS [u] ON [r].UserID = [u].UserID");
            //sb.AppendLine("INNER JOIN dbo.[Movie] AS [m] ON [r].MovieId = [m].Id");

            //string query = sb.ToString();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT");
            sb.AppendLine("[r].RentalID");
            sb.AppendLine(",[r].UserID");
            sb.AppendLine(",[r].MovieId");
            sb.AppendLine(",[r].RentStart");
            sb.AppendLine(",[r].RentEnd");
            //user
            sb.AppendLine(",[u].UserID");
            sb.AppendLine(",[u].LastName");
            sb.AppendLine(",[u].FirstMidName");
            sb.AppendLine(",[u].JoinDate");
            sb.AppendLine(",[u].RentalStatus");
            //movie
            sb.AppendLine(",[m].Id");
            sb.AppendLine(",[m].Title");
            sb.AppendLine(",[m].ReleaseDate");
            sb.AppendLine(",[m].Genre");
            sb.AppendLine(",[m].Price");
            sb.AppendLine(",[m].Rating");
            sb.AppendLine("FROM dbo.[Rentals] AS [r]");
            sb.AppendLine("INNER JOIN dbo.[User] AS [u] ON [r].UserID = [u].UserID");
            sb.AppendLine("INNER JOIN dbo.[Movie] AS [m] ON [r].MovieId = [m].Id");

            string query = sb.ToString();

            //return (await _context.Rentals.FromSqlRaw(query).ToListAsync()).Select(r => new Rental
            //{
            //    RentalID = r.rRentalID,
            //    UserID = r.rUserID,
            //    MovieId = r.rMovieId,
            //    RentStart = r.rRentStart,
            //    RentEnd = r.rRentEnd,
            //    Movie = new Movie
            //    {
            //        Id = r.mId,
            //        Title = r.mTitle,
            //        ReleaseDate = r.mReleaseDate,
            //        Genre = r.mGenre,
            //        Price = r.mPrice,
            //        Rating = r.mRating
            //    },
            //    User = new User
            //    {
            //        UserID = r.uUserID,
            //        LastName = r.uLastName,
            //        FirstMidName = r.uFirstMidName,
            //        JoinDate = r.uJoinDate,
            //        RentalStatus = r.uRentalStatus
            //    }
            //});

            return (await _context.Rentals.FromSqlRaw(query).Include(r => r.User).Include(r => r.Movie).ToListAsync());
        }
    }

    internal class RentalDTO : IRentalDB, IUserDB, IMovieDB
    {
        public int rRentalID { get; set; }
        public int rUserID { get; set; }

        public int rMovieId { get; set; }
        public DateTime rRentStart { get; set; }
        public DateTime rRentEnd { get; set; }
        public int uUserID { get; set; }
        public string? uLastName { get; set; }
        public string? uFirstMidName { get; set; }
        public DateTime uJoinDate { get; set; }
        public string? uRentalStatus { get; set; }
        public int mId { get; set; }
        public string? mTitle { get; set; }
        public DateTime mReleaseDate { get; set; }
        public string? mGenre { get; set; }
        public decimal mPrice { get; set; }
        public string? mRating { get; set; }
    }
}
