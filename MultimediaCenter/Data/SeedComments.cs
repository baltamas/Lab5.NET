using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultimediaCenter.Data
{
    public class SeedComments
    {
        private static string Characters = "ABCDEFGHIJKLMNOPRSTUVWXYZ 0123456789 abcdefghijklmnopqrstuvwxyz";
        private static Random random = new Random();

        public static void Seed(ApplicationDbContext context, int count)
        {
            context.Database.EnsureCreated();
            var moviesCount = context.Movies.Count();

            for (int i = 0; i < count; ++i)
            {
                var movie = context.Movies.Skip(random.Next(1, moviesCount)).Take(1).First();

                var comment = new Models.Comment
                {
                    Content = generateRandomString(3, 50),
                    Stars = generateRandomInt(1, 10),
                    Movie = movie
                };


                context.Comments.Add(comment);
            }

            context.SaveChanges();
        }

        private static string generateRandomString(int min, int max)
        {
            string title = "";

            for (int j = 0; j < random.Next(min, max); ++j)
            {
                title += Characters[random.Next(Characters.Length)];
            }

            return title;
        }

        private static int generateRandomInt(int min, int max)
        {
            return random.Next(min, max);
        }

    }
}

