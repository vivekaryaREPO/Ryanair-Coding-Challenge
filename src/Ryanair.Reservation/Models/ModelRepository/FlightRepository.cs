using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ryanair.Reservation.Models.IModelRepository;
using Ryanair.Reservation.Models.ModelClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ryanair.Reservation.Models.ModelRepository
{
    public class FlightRepository : IFlightRepository
    {
        private readonly ILogger<FlightRepository> logger;
        public List<Flight> flights { get; set; }
        public FlightRepository(ILogger<FlightRepository> logger)
        {
            this.logger = logger;
            string flightsJson = System.IO.File.ReadAllText("InitialState.json");
            this.flights = JsonConvert.DeserializeObject<List<Flight>>(flightsJson);
        }

        public IEnumerable<Flight> SearchFlightsAvailability(int passengers, string origin, string destination, DateTime dateOut, DateTime dateIn, bool roundtrip)
        {
            List<Flight> flightResults = null;
            IEnumerable<Flight> flightOut = flights.Where(flight => (DateTime.Equals(flight.time.Date, dateOut.Date) && (flight.origin.Equals(origin) && flight.destination.Equals(destination))));
            if (roundtrip)
            {
                logger.LogInformation("searching flight availability for roundtrip");
                IEnumerable<Flight> flightIn = flights.Where(flight => (DateTime.Equals(flight.time.Date, dateIn.Date) && (flight.origin.Equals(destination) && flight.destination.Equals(origin))));
                if (null!= flightOut && flightOut.Count()>0 && null!= flightIn && flightIn.Count()>0)
                {
                    flightResults = new List<Flight>();
                    flightResults.AddRange(flightOut.ToList());
                    flightResults.AddRange(flightIn.ToList());
                    logger.LogInformation("round trip available with requested parameters");
                }
            }
            else
            {
                logger.LogInformation("searching flight availability for one way trip");
                if (null != flightOut && flightOut.Count() > 0)
                {
                    flightResults = new List<Flight>();
                    flightResults.AddRange(flightOut.ToList());
                    logger.LogInformation("one way trip available with requested parameters");
                }
            }
            return flightResults;
        }

    }
}
