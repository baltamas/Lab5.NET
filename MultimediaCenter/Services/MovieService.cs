﻿using AutoMapper;
using MultimediaCenter.Data;
using MultimediaCenter.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultimediaCenter.ErrorHandling;
using MultimediaCenter.Models;
using MultimediaCenter.Services;
using MultimediaCenter.ViewModels.Pagination;

namespace MultimediaCenter.Services
{
    public class MovieService : IMoviesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MovieService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<PaginatedResultSet<Movie>, IEnumerable<EntityError>>> GetMovies(int? page = 1, int? perPage = 20)
        {
            var movies = await _context.Movies
                .Skip((page.Value - 1) * perPage.Value)
                .Take(perPage.Value)
                .ToListAsync();

            var count = await getMoviesCount();

            var resultSet = new PaginatedResultSet<Movie>(movies, page.Value, count, perPage.Value);

            var serviceResponse = new ServiceResponse<PaginatedResultSet<Movie>, IEnumerable<EntityError>>();
            serviceResponse.ResponseOk = resultSet;
            return serviceResponse;
        }

        public async Task<int> getMoviesCount()
        {
            return await _context.Movies.CountAsync();
        }

        public async Task<int> getCommentsCount(int movieId)
        {
            return await _context.Comments.Where(c => c.MovieId == movieId).CountAsync();
        }

        public async Task<ServiceResponse<PaginatedResultSet<Movie>, IEnumerable<EntityError>>> FilterMoviesByDateAdded(string fromDate, string toDate, int? page = 1, int? perPage = 10)
        {
            var startDateDt = DateTime.Parse(fromDate);
            var endDateDt = DateTime.Parse(toDate);

            var movies = await _context.Movies
                .Where(m => m.DateAdded >= startDateDt && m.DateAdded <= endDateDt)
                .OrderByDescending(m => m.ReleaseYear)
                .Skip((page.Value - 1) * perPage.Value)
                .Take(perPage.Value)
                .ToListAsync();

            var count = await getMoviesCount();

            var resultSet = new PaginatedResultSet<Movie>(movies, page.Value, count, perPage.Value);

            var serviceResponse = new ServiceResponse<PaginatedResultSet<Movie>, IEnumerable<EntityError>>();
            serviceResponse.ResponseOk = resultSet;
            return serviceResponse;
        }

        public async Task<ServiceResponse<MovieViewModel, string>> GetMovie(int id)
        {
            var serviceResponse = new ServiceResponse<MovieViewModel, string>();
            var movie = await _context.Movies
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            if (movie == null)
            {
                serviceResponse.ResponseError = "No movie found";
                return serviceResponse;
            }

            var movieResponse = _mapper.Map<MovieViewModel>(movie);
            serviceResponse.ResponseOk = movieResponse;
            return serviceResponse;
        }

        public async Task<ServiceResponse<PaginatedResultSet<Comment>, IEnumerable<EntityError>>> GetCommentsForMovie(int id, int? page = 1, int? perPage = 10)
        {
            var comments = await _context.Comments
                .Where(c => c.MovieId == id)
                .Skip((page.Value - 1) * perPage.Value)
                .Take(perPage.Value)
                .ToListAsync();

            var count = await _context.Comments.Where(c => c.MovieId == id).CountAsync();

            var resultSet = new PaginatedResultSet<Comment>(comments, page.Value, count, perPage.Value);

            var serviceResponse = new ServiceResponse<PaginatedResultSet<Comment>, IEnumerable<EntityError>>();
            serviceResponse.ResponseOk = resultSet;
            return serviceResponse;
        }

        public async Task<ServiceResponse<IEnumerable<MovieViewModel>, IEnumerable<EntityError>>> FilterMoviesByDateAdded(DateTime? fromDate, DateTime? toDate)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<MovieViewModel>, IEnumerable<EntityError>>();
            var errors = new List<EntityError>();

            if (!fromDate.HasValue || !toDate.HasValue)
            {
                errors.Add(new EntityError { ErrorType = "", Message = "Both dates are required" });
                serviceResponse.ResponseError = errors;
                return serviceResponse;
            }

            if (fromDate >= toDate)
            {
                errors.Add(new EntityError { ErrorType = "", Message = "fromDate is not before toDate" });
                serviceResponse.ResponseError = errors;
                return serviceResponse;
            }

            var filteredMovies = await _context.Movies
                .Where(m => m.DateAdded >= fromDate && m.DateAdded <= toDate)
                .OrderByDescending(m => m.ReleaseYear)
                .Include(m => m.Comments)
                .Select(m => _mapper.Map<MovieViewModel>(m))
                .ToListAsync();

            serviceResponse.ResponseOk = filteredMovies;

            return serviceResponse;
        }

        public async Task<ServiceResponse<Movie, IEnumerable<EntityError>>> PostMovie(MovieViewModel movieRequest)
        {
            var movie = _mapper.Map<Movie>(movieRequest);
            _context.Movies.Add(movie);

            var serviceResponse = new ServiceResponse<Movie, IEnumerable<EntityError>>();

            try
            {
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = movie;
            }
            catch (Exception e)
            {
                var errors = new List<EntityError>();
                errors.Add(new EntityError { ErrorType = e.GetType().ToString(), Message = e.Message });
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<Comment, IEnumerable<EntityError>>> PostCommentForMovie(int movieId, CommentViewModel commentRequest)
        {
            var serviceResponse = new ServiceResponse<Comment, IEnumerable<EntityError>>();

            var commentDB = _mapper.Map<Comment>(commentRequest);

            var movie = await _context.Movies
                .Where(m => m.Id == movieId)
                .Include(m => m.Comments)
                .FirstOrDefaultAsync();

            try
            {
                movie.Comments.Add(commentDB);
                _context.Entry(movie).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = commentDB;
            }
            catch (Exception e)
            {
                var errors = new List<EntityError>();
                errors.Add(new EntityError { ErrorType = e.GetType().ToString(), Message = e.Message });
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<Movie, IEnumerable<EntityError>>> PutMovie(int id, MovieViewModel movieRequest)
        {
            var serviceResponse = new ServiceResponse<Movie, IEnumerable<EntityError>>();

            var movie = _mapper.Map<Movie>(movieRequest);

            try
            {

                _context.Entry(movie).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = movie;
            }
            catch (Exception e)
            {
                var errors = new List<EntityError>();
                errors.Add(new EntityError { ErrorType = e.GetType().ToString(), Message = e.Message });
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<bool, IEnumerable<EntityError>>> DeleteMovie(int id)
        {
            var serviceResponse = new ServiceResponse<bool, IEnumerable<EntityError>>();

            try
            {
                var movie = await _context.Movies.FindAsync(id);
                _context.Movies.Remove(movie);
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

        public bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}