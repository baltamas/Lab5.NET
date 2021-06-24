using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultimediaCenter.Data
{
    public class SeedReservations
    {
        private static Random random = new Random();

        public static void Seed(ApplicationDbContext context, int count)
        {
            context.Database.EnsureCreated();
            var usersCount = context.ApplicationUsers.Count();
            var moviesCount = context.Movies.Count();

            for (int i = 0; i < count; ++i)
            {
                var user = context.ApplicationUsers.Skip(random.Next(1, usersCount)).Take(1).First();
                var movies = context.Movies.Skip(random.Next(1, moviesCount)).Take(3).ToList();

                var favourites = new Models.Reservation
                {
                    Price = generateRandomInt(10, 860),
                    Paid = generateRandomBoolean(),
                    ApplicationUser = user,
                    Movies = movies,
                    
                };


                context.Reservations.Add(favourites);
            }

            context.SaveChanges();
        }

        private static int generateRandomInt(int min, int max)
        {
            return random.Next(min, max);
        }

        private static bool generateRandomBoolean()
        {
            return random.Next(0, 2) >0;
        }
    }
}

