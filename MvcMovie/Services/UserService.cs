using MvcMovie.Models;
using System.Data;
using Dapper;
using System.Text;
using System.Collections.Generic;

namespace MvcMovie.Services
{
    public class UserService
    {
        private readonly IDbConnection _connection;

        public UserService(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT");
            sb.AppendLine("[u].UserID");
            sb.AppendLine(",[u].LastName");
            sb.AppendLine(",[u].FirstMidName");
            sb.AppendLine(",[u].JoinDate");
            sb.AppendLine(",[u].RentalStatus");
            //rentals
            sb.AppendLine(",[r].RentalID As Id");
            sb.AppendLine(",[r].UserID");
            sb.AppendLine(",[r].MovieId");
            sb.AppendLine(",[r].RentStart");
            sb.AppendLine(",[r].RentEnd");
            sb.AppendLine("FROM dbo.[User] AS [u]");
            sb.AppendLine("LEFT JOIN dbo.[Rentals] AS [r] ON [u].UserID = [r].UserID");

            string query = sb.ToString();
          
            var userMap = new Dictionary<int, User>();
            await _connection.QueryAsync<User, Rental, User>(  
                query,
                (user, rental) =>
                {
                    rental.UserID = user.UserID;

                    if (userMap.TryGetValue(user.UserID, out User existingUser))
                    {
                        user = existingUser;
                    }
                    else
                    {
                        user.Rentals = new List<Rental>();
                        userMap.Add(user.UserID, user);
                    }

                    user.Rentals.Add(rental);
                    return user;
                }
            );
            return userMap.Values.ToList();
         }
    }
}

/*  
            IEnumerable<Rental> rentalData = await _connection.QueryAsync<Rental, User, Rental>(
                query,
                (user, rental) =>
                {
                    user.UserID = rental.UserID;
                    rental.User = user;

                    return rental;
                }
            );

            List<User> userList = new List<User>();

            foreach (Rental rental in rentalData) {
                User user = userList.FirstOrDefault(u => u.UserID == rental.UserID);

                if (user != null && user.UserID > 0)
                {
                    user.Rentals.Add(rental);
                }
                else
                {
                    User newUser = new User()
                    {
                        UserID = rental.User.UserID,
                        LastName = rental.User.LastName,
                        FirstMidName = rental.User.FirstMidName,
                        JoinDate = rental.User.JoinDate,
                        RentalStatus = rental.User.RentalStatus,
                        Rentals = new List<Rental>() { rental }
                    };
                    userList.Add(newUser);
                }
            }

            return userList;
                  }
    }
}
*/


