using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcMovie.Data;
using System;
using System.Linq;

namespace MvcMovie.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new MvcMovieContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<MvcMovieContext>>()))
        {
            // Look for any movies.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }
            context.Users.AddRange(
                new User
                {

                    LastName = "Alexander",
                    FirstMidName = "Carson",
                    JoinDate = DateTime.Parse("2019-09-01"),
                },
                new User
                {

                    LastName = "Alonso",
                    FirstMidName = "Meredith",
                    JoinDate = DateTime.Parse("2017-09-01"),
                },
                new User
                {

                    LastName = "Anand",
                    FirstMidName = "Arturo",
                    JoinDate = DateTime.Parse("2018-09-01"),
                },
                new User
                {

                    LastName = "Barzdukas",
                    FirstMidName = "Gytis",
                    JoinDate = DateTime.Parse("2017-09-01"),
                }
            );
            context.SaveChanges();
        }
    }
}