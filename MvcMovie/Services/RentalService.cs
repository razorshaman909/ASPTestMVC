using MvcMovie.Models;
using Dapper;
using System.Linq;
using System.Data;
using System.Text;

namespace MvcMovie.Services
{
    public class RentalService
    {
        private readonly IDbConnection _connection;

        public RentalService(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Rental>> GetRentals()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT");
            sb.AppendLine("[r].RentalID");
            sb.AppendLine(",[r].UserID");
            sb.AppendLine(",[r].MovieId");
            sb.AppendLine(",[r].RentStart");
            sb.AppendLine(",[r].RentEnd");
            //splitting point for dapper
            sb.AppendLine(",[u].UserID AS SplittingPoint");
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

            return await _connection.QueryAsync<Rental, User, Movie, Rental>(
                query,
                (rental, user, movie) =>
                {
                    rental.User = user;
                    rental.Movie = movie;

                    return rental;
                },
                splitOn: "SplittingPoint"
            );
        }
    }
}
