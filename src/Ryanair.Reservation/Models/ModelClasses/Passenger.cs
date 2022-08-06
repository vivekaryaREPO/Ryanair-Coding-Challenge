using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ryanair.Reservation.Models.ModelClasses
{
    public class Passenger
    {
        [Required]
        public string name { get; set; }

        [Range(0, 5)]
        public int bags { get; set; }

        [Required]
        [Range(1, 50)]
        public string seat { get; set; }
    }
}
