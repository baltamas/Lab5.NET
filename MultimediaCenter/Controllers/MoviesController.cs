﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultimediaCenter.Data;
using MultimediaCenter.Models;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using MultimediaCenter.ViewModels;
using MultimediaCenter.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MultimediaCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MoviesController(IMoviesService moviesService, UserManager<ApplicationUser> userManager)
        {
            _moviesService = moviesService;
            _userManager = userManager;
        }

        /// <summary>
        /// Gets a list of Movies
        /// </summary>
        /// <response code="200">Gets a list of Movies</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieViewModel>>> GetMovies()
        {
            var moviesServiceResult = await _moviesService.GetMovies();

            return Ok(moviesServiceResult.ResponseOk);
        }

        /// <summary>
        /// Get a movie by id
        /// </summary>
        /// <response code="200">Get a movie by id</response>
        /// <response code="404">Movie not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieViewModel>> GetMovie(int id)
        {
            var moviesServiceResult = await _moviesService.GetMovie(id);
            if (moviesServiceResult.ResponseError != null)
            {
                return BadRequest(moviesServiceResult.ResponseError);
            }

            return Ok(moviesServiceResult.ResponseOk);
        }

        /// <summary>
        /// Based on the given movied id, it returns a comment to the specified movie
        /// </summary>
        /// <param name="id">The id of the movie</param>
        /// <returns>A list of comments for a movie</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}/Comments")]
        public async Task<ActionResult<IEnumerable<MovieWithCommentsViewModels>>> GetCommentsForMovie(int id)
        {
            var moviesServiceResult = await _moviesService.GetCommentsForMovie(id);

            return Ok(moviesServiceResult.ResponseOk);
        }

        /// <summary>
        /// Filter movies by date added
        /// </summary>
        /// <response code="200">Filter movies by date added</response>
        /// <response code="400">Unable to get the movie</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        [Route("filter")]
        public async Task<ActionResult<IEnumerable<MovieViewModel>>> FilterMoviesByDateAdded(DateTime? fromDate, DateTime? toDate)
        {
            var moviesServiceResult = await _moviesService.FilterMoviesByDateAdded(fromDate, toDate);
            if (moviesServiceResult.ResponseError != null)
            {
                return BadRequest(moviesServiceResult.ResponseError);
            }

            return Ok(moviesServiceResult.ResponseOk);
        }

        /// <summary>
        /// Add a new movie
        /// </summary>
        /// <response code="201">Adds a new movie</response>
        /// <response code="400">Unable to add the movie</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // POST: api/Movies
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        public async Task<ActionResult<MovieViewModel>> PostMovie([FromBody] MovieViewModel movieRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            var moviesServiceResult = await _moviesService.PostMovie(movieRequest);
            if (moviesServiceResult.ResponseError != null)
            {
                return BadRequest(moviesServiceResult.ResponseError);
            }

            var movie = moviesServiceResult.ResponseOk;

            return CreatedAtAction("GetMovie", new { id = movie.Id }, "New movie successfully created");
        }

        /// <summary>
        /// Add a new comment to movie
        /// </summary>
        /// <response code="201">Adds a new comment to a movie</response>
        /// <response code="404">Movie not found</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("{id}/Comments")]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        public async Task<ActionResult> PostCommentForMovie(int movieId, CommentViewModel commentRequest)
        {
            try
            {
                var user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            var moviesServiceResult = await _moviesService.PostCommentForMovie(movieId, commentRequest);
            if (moviesServiceResult.ResponseError != null)
            {
                return BadRequest(moviesServiceResult.ResponseError);
            }

            var movie = moviesServiceResult.ResponseOk;

            return CreatedAtAction("GetMovieWithComments", new { id = movie.Id }, "New comment successfully added");
        }

        /// <summary>
        /// Amend a movie
        /// </summary>
        /// <response code="204">Amend a movie</response>
        /// <response code="400">Unable to amend the movie due to validation error</response>
        /// <response code="404">Movie not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        public async Task<IActionResult> PutMovie(int id, MovieViewModel movieRequest)
        {
            try
            {
                var user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            var moviesServiceResult = await _moviesService.PutMovie(id, movieRequest);
            if (moviesServiceResult.ResponseError != null)
            {
                return BadRequest(moviesServiceResult.ResponseError);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete a movie by id
        /// </summary>
        /// <response code="204">Delete a movie</response>
        /// <response code="404">Movie not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            try
            {
                var user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            if (!_moviesService.MovieExists(id))
            {
                return NotFound();
            }

            var moviesServiceResult = await _moviesService.DeleteMovie(id);

            if (moviesServiceResult.ResponseError != null)
            {
                return BadRequest(moviesServiceResult.ResponseError);
            }

            return NoContent();
        }
    }
}