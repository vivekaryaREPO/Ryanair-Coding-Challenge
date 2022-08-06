using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ryanair.Reservation.DTOs
{
    public class FlightDTO
    {
        public string key { get; set; }

        public DateTime time { get; set; }

        public string origin { get; set; }
        public string destination { get; set; }

    }
}
