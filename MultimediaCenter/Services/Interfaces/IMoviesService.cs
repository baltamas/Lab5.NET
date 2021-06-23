using MultimediaCenter.ErrorHandling;
using MultimediaCenter.Models;
using MultimediaCenter.Services;
using MultimediaCenter.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultimediaCenter.Services
{
    public interface IMoviesService
    {
        Task<ServiceResponse<IEnumerable<MovieViewModel>, IEnumerable<EntityError>>> GetMovies();

        Task<ServiceResponse<MovieViewModel, string>> GetMovie(int id);

        Task<ServiceResponse<IEnumerable<MovieWithCommentsViewModels>, IEnumerable<EntityError>>> GetCommentsForMovie(int id);

        Task<ServiceResponse<IEnumerable<MovieViewModel>, IEnumerable<EntityError>>> FilterMoviesByDateAdded(DateTime? fromDate, DateTime? toDate);

        Task<ServiceResponse<Movie, IEnumerable<EntityError>>> PostMovie(MovieViewModel movieRequest);

        Task<ServiceResponse<Comment, IEnumerable<EntityError>>> PostCommentForMovie(int movieId, CommentViewModel commentRequest);

        Task<ServiceResponse<Movie, IEnumerable<EntityError>>> PutMovie(int id, MovieViewModel movieRequest);

        Task<ServiceResponse<bool, IEnumerable<EntityError>>> DeleteMovie(int id);

        bool MovieExists(int id);
    }
}