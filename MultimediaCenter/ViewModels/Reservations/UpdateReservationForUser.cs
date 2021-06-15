using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultimediaCenter.ViewModels.Reservations
{
    public class UpdateReservationForUser
    {
        public int Id { get; set; }
        public ApplicationUserViewModel User { get; set; }
        public List<int> MovieIds { get; set; }
        public bool Price { get; set; }
        public int Paid { get; set; }
    }
}
