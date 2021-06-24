using Microsoft.AspNetCore.Identity;
using MultimediaCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultimediaCenter.Data
{
    public class SeedUsers
    {
        private static string Characters = "ABCDEFGHIJKLMNOPRSTUVWXYZ 0123456789 abcdefghijklmnopqrstuvwxyz";
        private static Random random = new Random();
        public static void Seed(ApplicationDbContext context, UserManager<ApplicationUser> userManager, int count)
        {

            context.Database.EnsureCreated();

            for (int i = 0; i < count; ++i)
            {
                var email = generateRandomString(3, 10) + "@" + generateRandomString(2, 3);
                var user = new ApplicationUser
                {

                    Email = email,
                    UserName = email,
                };

                user.PasswordHash = userManager.PasswordHasher.HashPassword(user, "ASDfwbckw(#");
                context.ApplicationUsers.Add(user);
                context.SaveChanges();
            }
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

