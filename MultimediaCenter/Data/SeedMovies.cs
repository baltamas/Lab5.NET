using MultimediaCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultimediaCenter.Data
{
    public class SeedMovies
    {
        private static string Characters = "ABCDEFGHIJKLMNOPRSTUVWXYZ 0123456789 abcdefghijklmnopqrstuvwxyz";
        private static Random random = new Random();

        public static void Seed(ApplicationDbContext context, int count)
        {
            context.Database.EnsureCreated();

            var movies = new List<Movie>();
            for (int i = 0; i < count; ++i)
            {
                var movie = new Models.Movie
                {
                    Title = generateRandomString(3, 20),
                    Description = generateRandomString(5, 50),
                    Director = generateRandomString(3, 10),
                    Genre = generateRandomString(5, 10),
                    Duration = generateRandomInt(10, 400),
                    ReleaseYear = generateRandomInt(1920, DateTime.Now.Year),
                    Watched = generateRandomBoolean(),
                };

                if (movie.Watched)
                {
                    movie.Rating = generateRandomInt(1, 10);
                }

                context.Movies.Add(movie);
                movies.Add(movie);
            }

            context.SaveChanges();
        }

        private static bool generateRandomBoolean()
        {
            return random.Next(0, 3000) < 1500;
        }

        private static int generateRandomInt(int min, int max)
        {
            return random.Next(min, max);
        }

        private static string generateRandomString(int min, int max)
        {
            string str = "";

            for (int j = 0; j < random.Next(min, max); ++j)
            {
                str += Characters[random.Next(Characters.Length)];
            }

            return str;
        }

        
    }
}

