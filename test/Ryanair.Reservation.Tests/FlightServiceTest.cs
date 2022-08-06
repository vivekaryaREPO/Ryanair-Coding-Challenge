using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Ryanair.Reservation.Controllers;
using Ryanair.Reservation.DTOs;
using Ryanair.Reservation.Models.IModelRepository;
using Ryanair.Reservation.Models.ModelClasses;
using Ryanair.Reservation.Models.ModelRepository;
using Ryanair.Reservation.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ryanair.Reservation.Tests
{
    public class FlightServiceTest
    {
        [Fact]
        public void FlightServiceTest_SearchFlightss()
        {
            //Arrange            
            var mockRepo = new Mock<IFlightRepository>();
            var impapper = new Mock<IMapper>();
            var flightService = new FlightService(mockRepo.Object, impapper.Object);
            //  Act
            int passengers = 2;
            string origin = "DUBLIN";
            string destination = "LONDON";
            DateTime dateOut = new DateTime(2017, 05, 08);
            DateTime dateIn = new DateTime(2017, 05, 08);
            bool roundtrip = false;


            Flight f1 = new Flight();
            f1.origin = "DUBLIN";
            f1.destination = "LONDON";

            Flight f2 = new Flight();
            f2.origin = "DUBLIN";
            f2.destination = "LONDON";
            var flights = new List<Flight>() {f1,f2 };

            FlightDTO f11 = new FlightDTO();
            f1.origin = "DUBLIN";
            f1.destination = "LONDON";

            FlightDTO f22 = new FlightDTO();
            f2.origin = "DUBLIN";
            f2.destination = "LONDON";
            var flightsDTO = new List<FlightDTO>() { f11, f22 };

            mockRepo.Setup(f => f.SearchFlightsAvailability(passengers, origin, destination, dateOut, dateIn, roundtrip)).Returns(flights);
            impapper.Setup(f => f.Map<IEnumerable<FlightDTO>>(flights)).Returns(flightsDTO);

            var searchResults= flightService.SearchFlightsAvailability(passengers, origin, destination, dateOut, dateIn, roundtrip);
          
           // Assert
            Assert.Equal(2, searchResults.Count());
        }
    }
}
