using MultimediaCenter.ErrorHandling;
using MultimediaCenter.Models;
using MultimediaCenter.ViewModels.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultimediaCenter.Services.Interfaces
{
    public interface IReservationsService
    {
        Task<ServiceResponse<Reservation, IEnumerable<EntityError>>> PlaceReservation(NewReservationRequest newReservationRequest, ApplicationUser user);

        Task<ServiceResponse<ReservationForUserResponse, IEnumerable<EntityError>>> GetAll(ApplicationUser user);

        Task<ServiceResponse<Reservation, IEnumerable<EntityError>>> UpdateReservation(int id, NewReservationRequest updateReservationRequest, ApplicationUser user);

        Task<ServiceResponse<bool, IEnumerable<EntityError>>> DeleteReservation(int id);

        bool ReservationExists(int id);
    }
}
