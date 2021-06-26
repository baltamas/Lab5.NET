using MultimediaCenter.ErrorHandling;
using MultimediaCenter.Models;
using MultimediaCenter.Services;
using MultimediaCenter.ViewModels;
using MultimediaCenter.ViewModels.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultimediaCenter.Services
{
    public interface IMoviesService
    {
        public Task<ServiceResponse<PaginatedResultSet<Movie>, IEnumerable<EntityError>>> GetMovies(int? page = 1, int? perPage = 20);

        Task<ServiceResponse<MovieViewModel, string>> GetMovie(int id);

        public Task<ServiceResponse<PaginatedResultSet<Comment>, IEnumerable<EntityError>>> GetCommentsForMovie(int id, int? page = 1, int? perPage = 20);

        public Task<ServiceResponse<PaginatedResultSet<Movie>, IEnumerable<EntityError>>> FilterMoviesByDateAdded(string fromDate, string toDate, int? page = 1, int? perPage = 10);

        Task<ServiceResponse<Movie, IEnumerable<EntityError>>> PostMovie(MovieViewModel movieRequest);

        Task<ServiceResponse<Comment, IEnumerable<EntityError>>> PostCommentForMovie(int movieId, CommentViewModel commentRequest);

        Task<ServiceResponse<Movie, IEnumerable<EntityError>>> PutMovie(int id, MovieViewModel movieRequest);

        Task<ServiceResponse<bool, IEnumerable<EntityError>>> DeleteMovie(int id);

        bool MovieExists(int id);
    }
}