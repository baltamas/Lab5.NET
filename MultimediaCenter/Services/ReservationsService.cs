using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MultimediaCenter.Data;
using MultimediaCenter.ErrorHandling;
using MultimediaCenter.Models;
using MultimediaCenter.Services.Interfaces;
using MultimediaCenter.ViewModels.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultimediaCenter.Services
{
    public class ReservationsService : IReservationsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationsService(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ServiceResponse<ReservationForUserResponse, IEnumerable<EntityError>>> GetAll(ApplicationUser user)
        {
            var reservationsFromDb = await _context.Reservations
                .Where(o => o.ApplicationUser.Id == user.Id)
                .Include(o => o.Movies)
                .FirstOrDefaultAsync();

            var reservationsForUserResponse = _mapper.Map<ReservationForUserResponse>(reservationsFromDb);

            var serviceResponse = new ServiceResponse<ReservationForUserResponse, IEnumerable<EntityError>>();
            serviceResponse.ResponseOk = reservationsForUserResponse;

            return serviceResponse;
        }

        public async Task<ServiceResponse<Reservation, IEnumerable<EntityError>>> PlaceReservation(NewReservationRequest newReservationRequest, ApplicationUser user)
        {
            var serviceResponse = new ServiceResponse<Reservation, IEnumerable<EntityError>>();

            var reservedMovies = new List<Movie>();
            newReservationRequest.ReservedMovieIds.ForEach(rid =>
            {
                var movieWithId = _context.Movies.Find(rid);
                if (movieWithId != null)
                {
                    reservedMovies.Add(movieWithId);
                }
            });

            var reservation = new Reservation
            {
                ApplicationUser = user,
                Price = newReservationRequest.Price,
                Paid = newReservationRequest.Paid,
                Movies = reservedMovies
            };

            _context.Reservations.Add(reservation);

            try
            {
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = reservation;
            }
            catch (Exception e)
            {
                var errors = new List<EntityError>();
                errors.Add(new EntityError { ErrorType = e.GetType().ToString(), Message = e.Message });
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<Reservation, IEnumerable<EntityError>>> UpdateReservation(int id, NewReservationRequest updateReservationRequest, ApplicationUser user)
        {
            var serviceResponse = new ServiceResponse<Reservation, IEnumerable<EntityError>>();

            var reservedMovies = new List<Movie>();
            updateReservationRequest.ReservedMovieIds.ForEach(rid =>
            {
                var movieWithId = _context.Movies.Find(rid);
                if (movieWithId != null)
                {
                    reservedMovies.Add(movieWithId);
                }
            });

            var reservation = new Reservation
            {
                Id = id,
                ApplicationUser = user,
                Price = updateReservationRequest.Price,
                Paid = updateReservationRequest.Paid,
                Movies = reservedMovies
            };

            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = reservation;
            }
            catch (Exception e)
            {
                var errors = new List<EntityError>();
                errors.Add(new EntityError { ErrorType = e.GetType().ToString(), Message = e.Message });
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<bool, IEnumerable<EntityError>>> DeleteReservation(int id)
        {
            var serviceResponse = new ServiceResponse<bool, IEnumerable<EntityError>>();

            try
            {
                var reservation = await _context.Reservations.FindAsync(id);
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = true;
            }
            catch (Exception e)
            {
                var errors = new List<EntityError>();
                errors.Add(new EntityError { ErrorType = e.GetType().ToString(), Message = e.Message });
                serviceResponse.ResponseError = errors;
            }

            return serviceResponse;
        }

        public bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
