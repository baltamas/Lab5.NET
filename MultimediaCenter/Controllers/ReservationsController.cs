using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MultimediaCenter.Data;
using MultimediaCenter.Models;
using MultimediaCenter.ViewModels.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MultimediaCenter.Controllers
{
    [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReservationsController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationsController(ApplicationDbContext context, ILogger<ReservationsController> logger,
            IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }
        [HttpPost]
        [HttpPost]
        public async Task<ActionResult> PlaceReservation(NewReservationRequest newReservationRequest)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            List<Movie> reservedMovies = new List<Movie>();
            newReservationRequest.ReservedMovieIds.ForEach(pid =>
            {
                var movieWithId = _context.Movies.Find(pid);
                if (movieWithId != null)
                {
                    reservedMovies.Add(movieWithId);
                }
            });

            if (reservedMovies.Count == 0)
            {
                return BadRequest("The movie doesn't exist!");
            }

            var reservation = new Reservation
            {
                ApplicationUser = user,
                Price = newReservationRequest.Price,
                Paid = newReservationRequest.Paid,
                Movies = reservedMovies
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationForUserResponse>>> GetAll()
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (user == null)
            {
                return NotFound();
            }

            var result = _context.Reservations.Where(f => f.ApplicationUser.Id == user.Id).Include(f => f.Movies).ToList();
            return _mapper.Map<List<Reservation>, List<ReservationForUserResponse>>(result);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateReservation(UpdateReservationForUser updateReservationRequest)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            Reservation reservations = _context.Reservations.Where(r => r.Id == updateReservationRequest.Id && r.ApplicationUser.Id == user.Id).Include(r => r.Movies).FirstOrDefault();

            if (reservations == null)
            {
                return BadRequest("There is no reservation with this ID.");
            }

            updateReservationRequest.MovieIds.ForEach(mid =>
            {
                var movie = _context.Movies.Find(mid);
                if (movie != null && !reservations.Movies.Contains(movie))
                {
                    reservations.Movies.ToList().Add(movie);
                }
            });

            _context.Entry(reservations).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservations(int id)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var reservations = _context.Reservations.Where(f => f.ApplicationUser.Id == user.Id && f.Id == id).FirstOrDefault();

            if (reservations == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservations);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }



}


