using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Ryanair.Reservation.Controllers;
using Ryanair.Reservation.Models.ModelClasses;
using Ryanair.Reservation.Models.ModelRepository;
using Ryanair.Reservation.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ryanair.Reservation.Tests
{
    public class FlightRepositoryTest
    {
        [Fact]
        public void FlightRepositoryTest_SearchFlights_OneWay_Success()
        {
            //Arrange            
            var mockRepo = new Mock<ILogger<FlightRepository>>();
           
            var flightRepo = new FlightRepository(mockRepo.Object);
            //  Act
            int passengers = 2;
            string origin = "DUBLIN";
            string destination = "LONDON";
            DateTime dateOut = new DateTime(2017, 05, 08);
            DateTime dateIn = new DateTime(2017, 05, 08);
            bool roundtrip = false;

            var searchResults= flightRepo.SearchFlightsAvailability(passengers, origin, destination, dateOut, dateIn, roundtrip);
          
           // Assert
            Assert.Equal(2, searchResults.Count());
        }

        [Fact]
        public void FlightRepositoryTest_SearchFlights_RoundTrip_Failure()
        {
            //Arrange            
            var mockRepo = new Mock<ILogger<FlightRepository>>();

            var flightRepo = new FlightRepository(mockRepo.Object);
            //  Act
            int passengers = 2;
            string origin = "DUBLIN";
            string destination = "LONDON";
            DateTime dateOut = new DateTime(2017, 05, 08);
            DateTime dateIn = new DateTime(2017, 05, 08);
            bool roundtrip = true;

            var searchResults = flightRepo.SearchFlightsAvailability(passengers, origin, destination, dateOut, dateIn, roundtrip);

            // Assert
            Assert.True(searchResults==null);
        }

        [Fact]
        public void FlightRepositoryTest_SearchFlights_RoundTrip_Success1()
        {
            //Arrange            
            var mockRepo = new Mock<ILogger<FlightRepository>>();

            var flightRepo = new FlightRepository(mockRepo.Object);
            //  Act
            int passengers = 2;
            string origin = "DUBLIN";
            string destination = "LONDON";
            DateTime dateOut = new DateTime(2017, 05, 08);
            DateTime dateIn = new DateTime(2017, 05, 09);
            bool roundtrip = true;

            var searchResults = flightRepo.SearchFlightsAvailability(passengers, origin, destination, dateOut, dateIn, roundtrip);

            // Assert
            Assert.Equal(4,searchResults.Count());
        }

        [Fact]
        public void FlightRepositoryTest_SearchFlights_RoundTrip_Success2()
        {
            //Arrange            
            var mockRepo = new Mock<ILogger<FlightRepository>>();

            var flightRepo = new FlightRepository(mockRepo.Object);
            //  Act
            int passengers = 2;
            string origin = "DUBLIN";
            string destination = "LONDON";
            DateTime dateOut = new DateTime(2017, 05, 08);
            DateTime dateIn = new DateTime(2017, 05, 10);
            bool roundtrip = true;

            var searchResults = flightRepo.SearchFlightsAvailability(passengers, origin, destination, dateOut, dateIn, roundtrip);

            // Assert
            Assert.Equal(3, searchResults.Count());
        }

        [Fact]
        public void FlightRepositoryTest_SearchFlights_OneWay_Failure()
        {
            //Arrange            
            var mockRepo = new Mock<ILogger<FlightRepository>>();

            var flightRepo = new FlightRepository(mockRepo.Object);
            //  Act
            int passengers = 2;
            string origin = "DUBLIN";
            string destination = "LONDON";
            DateTime dateOut = new DateTime(2017, 05, 10);
            DateTime dateIn = new DateTime(2017, 05, 10);
            bool roundtrip = false;

            var searchResults = flightRepo.SearchFlightsAvailability(passengers, origin, destination, dateOut, dateIn, roundtrip);

            // Assert
            Assert.True(null== searchResults);
        }

        [Fact]
        public void FlightRepositoryTest_SearchFlights_RoundTrip_Failure2()
        {
            //Arrange            
            var mockRepo = new Mock<ILogger<FlightRepository>>();

            var flightRepo = new FlightRepository(mockRepo.Object);
            //  Act
            int passengers = 2;
            string origin = "DUBLIN";
            string destination = "LONDON";
            DateTime dateOut = new DateTime(2017, 05, 11);
            DateTime dateIn = new DateTime(2017, 05, 10);
            bool roundtrip = true;

            var searchResults = flightRepo.SearchFlightsAvailability(passengers, origin, destination, dateOut, dateIn, roundtrip);

            // Assert
            Assert.True(searchResults == null);
        }
    }
}
