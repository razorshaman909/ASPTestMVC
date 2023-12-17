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
            //user
            sb.AppendLine(",[u].UserID As Id"); // put "Id" as alias for dapper to know this is the splitting point
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
                }
            );
        }

        public async Task<IEnumerable<Rental>> GetRentals(int? id)
        {
            
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT");
            sb.AppendLine("[r].RentalID");
            sb.AppendLine(",[r].UserID");
            sb.AppendLine(",[r].MovieId");
            sb.AppendLine(",[r].RentStart");
            sb.AppendLine(",[r].RentEnd");
            //user
            sb.AppendLine(",[u].UserID As Id"); // put "Id" as alias for dapper to know this is the splitting point
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
            sb.AppendLine("WHERE [r].[RentalID] = @RentID");

            string query = sb.ToString();

            
            return await _connection.QueryAsync<Rental, User, Movie, Rental>(
                query,
                (rental, user, movie) =>
                {
                    rental.User = user;
                    rental.Movie = movie;

                    return rental;
                },
                new {RentID = id}
            );
        }

        public async Task<int> AddRental(Rental rental)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO dbo.[Rentals]");
            sb.AppendLine("(");
            sb.AppendLine("UserID,");
            sb.AppendLine("MovieId,");
            sb.AppendLine("RentStart,");
            sb.AppendLine("RentEnd");
            sb.AppendLine(")");
            sb.AppendLine("VALUES");
            sb.AppendLine("(");
            sb.AppendLine("@UserID,");
            sb.AppendLine("@MovieId,");
            sb.AppendLine("@RentStart,");
            sb.AppendLine("@RentEnd");
            sb.AppendLine(")");
            sb.AppendLine("SELECT SCOPE_IDENTITY();");

            string query = sb.ToString();

            return await _connection.ExecuteAsync(query, new
            {
                UserID = rental.UserID,
                MovieId = rental.MovieId,
                RentStart = rental.RentStart,
                RentEnd = rental.RentEnd,
            });
        }
    }
}
