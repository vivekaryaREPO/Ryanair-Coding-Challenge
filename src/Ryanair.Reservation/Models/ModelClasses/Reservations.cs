using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ryanair.Reservation.Models.ModelClasses
{
    public class Reservations
    {
       public string? reservationNumber { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        public string creditCard { get; set; }

        [Required]
        public IEnumerable<DetailsPerFlight> flights { get; set; }
        public Error error { get; set; }
    }
}
