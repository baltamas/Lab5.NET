using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultimediaCenter.ViewModels.Reservations
{
    public class NewReservationRequest
    {
        public List<int> ReservedMovieIds{ get; set; }
        public int Price { get; set; }
        public bool Paid { get; set; }

    }
}
