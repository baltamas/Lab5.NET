using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultimediaCenter.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<Movie> Movies { get; set; }
        public int Price { get; set; }
        public bool Paid { get; set; }
    }
}
