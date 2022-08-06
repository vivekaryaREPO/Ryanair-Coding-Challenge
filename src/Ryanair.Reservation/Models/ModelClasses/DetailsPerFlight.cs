using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ryanair.Reservation.Models.ModelClasses
{
    public class DetailsPerFlight
    {
        [Required]
        public string key { get; set; }
        [Required]
        public IEnumerable<Passenger> passengers { get; set; }
    }
}