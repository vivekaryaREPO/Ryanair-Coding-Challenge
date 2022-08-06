using Ryanair.Reservation.Models.ModelClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ryanair.Reservation.DTOs
{
    public class ReservationsDTO
    {
        public string reservationNumber { get; set; }
        public string email { get; set; }
        public string creditCard { get; set; }
        public IEnumerable<DetailsPerFlight> flights { get; set; }
    }
}
