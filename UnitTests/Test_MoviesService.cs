using Microsoft.EntityFrameworkCore;
using MultimediaCenter.Data;
using MultimediaCenter.Services;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

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
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDB")
                    .Options;

                _context = new ApplicationDbContext(options, new OperationalStoreOptionsForTests());

                _context.Movies.Add(new MultimediaCenter.Models.Movie { });
                _context.Movies.Add(new MultimediaCenter.Models.Movie { });
                _context.SaveChanges();
            }

            [TearDown]
            public void Teardown()
            {
                foreach (var movie in _context.Movies)
                {
                    _context.Remove(movie);
                }
                _context.SaveChanges();
            }

            [Test]
            public async Task TestGetMovies()
            {
                var service = new MovieService(_context);
                var moviesResponse = await service.GetMovies();
                var moviesCount = moviesResponse.ResponseOk.ToString();
                Assert.AreEqual(2, moviesCount);
            }
        }
    }
}