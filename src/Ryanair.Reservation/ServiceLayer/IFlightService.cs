using Ryanair.Reservation.DTOs;
using Ryanair.Reservation.Models.ModelClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ryanair.Reservation.ServiceLayer
{
    public interface IFlightService
    {

        IEnumerable<FlightDTO> SearchFlightsAvailability(int passengers, string origin, string destination, DateTime dateOut, DateTime dateIn, bool roundtrip);

    }
}
