using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultimediaCenter.ViewModels.Reservations
{
    public class ReservationForUserResponse
    {
        public ApplicationUserViewModel ApplicationUser { get; set; }
        public List<MovieViewModel> Movies { get; set; }
        public int Price { get; set; }
        public bool Paid { get; set; }
    }
}
