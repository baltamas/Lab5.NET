using Microsoft.EntityFrameworkCore;
using MultimediaCenter.Data;
using MultimediaCenter.Services;
using NUnit.Framework;
using System;

namespace UnitTests
{
    public class Tests
    {
        public class Test_MoviesService
        {
            private ApplicationDbContext _context;
            [SetUp]
            public void Setup()
            {
                Console.WriteLine("In setup.");
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDB")
                    .Options;

                _context = new ApplicationDbContext(options, new OperationalStoreOptionsForTests());

                _context.Movies.Add(new MultimediaCenter.Models.Movie { Title = "p1 test", Description = "fsd ds fsd fsd", Rating = 6 });
                _context.Movies.Add(new MultimediaCenter.Models.Movie { Title = "p1 test", Description = "fsd ds fsd fsd", Rating = 3 });
                _context.Movies.Add(new MultimediaCenter.Models.Movie { Title = "p2 test", Description = "fsd ds fsd fsd", Rating = 10 });
                _context.SaveChanges();
            }

            [TearDown]
            public void Teardown()
            {
                Console.WriteLine("In teardown");

                foreach (var movie in _context.Movies)
                {
                    _context.Remove(movie);
                }
                _context.SaveChanges();
            }

            [Test]
            public void TestGetRating()
            {
                var service = new MovieService(_context);
                Assert.AreEqual(2, service.GetAllAboveRating(10).Count);
            }


        }
    }
}