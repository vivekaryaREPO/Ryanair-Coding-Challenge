using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ryanair.Reservation.Models.ModelClasses
{
    public class Flight 
    {
        public string key { get; set; }

        public  DateTime time { get; set; }

        [Required]
        public ushort? numberOfPassengers { get; } = 50;

        [Required]
        public string? origin { get; set; }

        [Required]
        public string? destination { get; set; }

        public DateTime dateOut { get; set; }
        public DateTime? dateIn { get; set; }
        public bool? roundtrip { get; set; }
    }
}
