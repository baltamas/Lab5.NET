using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MultimediaCenter.Models;
using MultimediaCenter.ViewModels;
using MultimediaCenter.ViewModels.Reservations;

namespace MultimediaCenter
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieViewModel>().ReverseMap();
            CreateMap<Comment, CommentViewModel>().ReverseMap();
            CreateMap<Movie, MovieWithCommentsViewModels>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserViewModel>().ReverseMap();
            CreateMap<Reservation, ReservationForUserResponse>().ReverseMap();

        }
    }
}
