using AutoMapper;
using Ryanair.Reservation.DTOs;
using Ryanair.Reservation.Models.IModelRepository;
using Ryanair.Reservation.Models.ModelClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ryanair.Reservation.ServiceLayer
{
    public class FlightService: IFlightService
    {
        private readonly IFlightRepository flightRepository;

        private readonly IMapper mapper;
        public FlightService(IFlightRepository flightRepository,
                                     IMapper mapper)
        {
            this.flightRepository = flightRepository;
            this.mapper = mapper;
        }

        public IEnumerable<FlightDTO> SearchFlightsAvailability(int passengers, string origin, string destination, DateTime dateOut, DateTime dateIn, bool roundtrip)
        {
            IEnumerable<FlightDTO> res = null;
            IEnumerable <Flight> flights =  flightRepository.SearchFlightsAvailability(passengers, origin, destination, dateOut, dateIn, roundtrip);
                if (null!=flights)
                {
                    res = mapper.Map<IEnumerable<FlightDTO>>(flights);
                }
            return res;
        }
    }
}
