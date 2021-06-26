using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MultimediaCenter.Data;
using MultimediaCenter.Models;
using MultimediaCenter.Services.Interfaces;
using MultimediaCenter.ViewModels.Pagination;
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
        private readonly IReservationsService _reservationsService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationsController(IReservationsService reservationsService, UserManager<ApplicationUser> userManager)
        {
            _reservationsService = reservationsService;
            _userManager = userManager;
        }
        
        /// <summary>
        /// Adds a new reservation
        /// </summary>
        /// <response code="201">Adds a new reservation</response>
        /// <response code="400">Unable to add the reservation</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
        [HttpPost]
        public async Task<ActionResult> PlaceReservation(NewReservationRequest newReservationRequest)
        {
            var user = new ApplicationUser();
            try
            {
                user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            var reservationServiceResult = await _reservationsService.PlaceReservation(newReservationRequest, user);
            if (reservationServiceResult.ResponseError != null)
            {
                return BadRequest(reservationServiceResult.ResponseError);
            }

            var reservation = reservationServiceResult.ResponseOk;

            return CreatedAtAction("GetReservations", new { id = reservation.Id }, "New reservation successfully added");
        }


        /// <summary>
        /// Get all reservations
        /// </summary>
        /// <response code="200">Get All Reservations</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
        [HttpGet]
        public async Task<ActionResult<PaginatedResultSet<Reservation>>> GetAll(int? page = 1, int? perPage = 20)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (user == null)
            {
                return NotFound();
            }

            var serviceResponse = await _reservationsService.GetAll(user.Id, page, perPage);
            return serviceResponse.ResponseOk;
        }

        /// <summary>
        /// Edit a reservation
        /// </summary>
        /// <response code="204">Update a reservation</response>
        /// <response code="400">Unable to update the reservation</response>
        /// <response code="404">Reservation not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservation(int id, NewReservationRequest updateReservationRequest)
        {
            var user = new ApplicationUser();
            try
            {
                user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("You have to login!");
            }

            var reservationServiceResult = await _reservationsService.UpdateReservation(id, updateReservationRequest, user);
            if (reservationServiceResult.ResponseError != null)
            {
                return BadRequest(reservationServiceResult.ResponseError);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete a reservation by the given id
        /// </summary>
        /// <response code="204">Deletes a reservation</response>
        /// <response code="404">Reservation not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            try
            {
                var user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("You have to login!");
            }

            if (!_reservationsService.ReservationExists(id))
            {
                return NotFound();
            }

            var reservationServiceResult = await _reservationsService.DeleteReservation(id);
            if (reservationServiceResult.ResponseError != null)
            {
                return BadRequest(reservationServiceResult.ResponseError);
            }

            return NoContent();
        }
    }

}


