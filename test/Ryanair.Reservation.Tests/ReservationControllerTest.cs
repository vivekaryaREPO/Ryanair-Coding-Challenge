using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Ryanair.Reservation.Controllers;
using Ryanair.Reservation.DTOs;
using Ryanair.Reservation.Models.ModelClasses;
using Ryanair.Reservation.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ryanair.Reservation.Tests
{
    public class ReservationControllerTest
    {
        [Fact]
        public void ReservationControllerTest_SuccessfulSearch()
        {
            //Arrange
            int passengers = 2;
            string origin = "Dublin";
            string destination = "London";
            DateTime dateOut = new DateTime(2017, 05, 08);
            DateTime dateIn = new DateTime(2017, 05, 08);
            bool roundtrip = true;
            var mockRepo = new Mock<ILogger<ReservationController>>();
            var flightServiceRepo = new Mock<IFlightService>();
            var reservationServiceRepo = new Mock<IReservationService>();
            FlightDTO f1 = new FlightDTO();
            f1.origin = "Dublin";
            f1.destination = "London";

            FlightDTO f2 = new FlightDTO();
            f2.origin = "Dublin";
            f2.origin = "Dublin";
            f2.destination = "London";
            var flights = new List<FlightDTO>() { f1,f2 };
            flightServiceRepo.Setup(f => f.SearchFlightsAvailability(passengers, origin, destination, dateOut, dateIn, roundtrip)).Returns(flights);
 
            var controller = new ReservationController(mockRepo.Object, flightServiceRepo.Object, reservationServiceRepo.Object);

          //  Act
            var result= controller.Flight(passengers, origin, destination, dateOut, dateIn, roundtrip);
            var res = result.Result as OkObjectResult;
            var availableFlights = res.Value as IEnumerable<FlightDTO>;

            // Assert
            Assert.Equal(availableFlights, flights);
            Assert.Equal(2, availableFlights.Count());
            Assert.True(result.Result is OkObjectResult);
        }

        [Fact]
        public void ReservationControllerTest_EmptySearchResponse()
        {
            //Arrange
            int passengers = 2;
            string origin = "Dublin";
            string destination = "London";
            DateTime dateOut = new DateTime(2017, 05, 08);
            DateTime dateIn = new DateTime(2017, 05, 08);
            bool roundtrip = true;
            var mockRepo = new Mock<ILogger<ReservationController>>();
            var impapper = new Mock<IMapper>();
            var flightServiceRepo = new Mock<IFlightService>();
            var reservationServiceRepo = new Mock<IReservationService>();
            FlightDTO f1 = new FlightDTO();
            f1.origin = "Dublin";
            f1.destination = "London";

            FlightDTO f2 = new FlightDTO();
            f2.origin = "Dublin";
            f2.origin = "Dublin";
            f2.destination = "London";
            var flights = new List<FlightDTO>() { };
            flightServiceRepo.Setup(f => f.SearchFlightsAvailability(passengers, origin, destination, dateOut, dateIn, roundtrip)).Returns(flights);

            var controller = new ReservationController(mockRepo.Object, flightServiceRepo.Object, reservationServiceRepo.Object);

            //  Act
            var result = controller.Flight(passengers, origin, destination, dateOut, dateIn, roundtrip);

            // Assert
            Assert.True(result.Result is NotFoundObjectResult);
        }

        [Fact]
        public void ReservationControllerTest_ExceptionResponse()
        {
            //Arrange
            int passengers = 2;
            string origin = "Dublin";
            string destination = "London";
            DateTime dateOut = new DateTime(2017, 05, 08);
            DateTime dateIn = new DateTime(2017, 05, 08);
            bool roundtrip = true;
            var mockRepo = new Mock<ILogger<ReservationController>>();
            var flightServiceRepo = new Mock<IFlightService>();
            var reservationServiceRepo = new Mock<IReservationService>();
            var impapper = new Mock<IMapper>();
            FlightDTO f1 = new FlightDTO();
            f1.origin = "Dublin";
            f1.destination = "London";

            FlightDTO f2 = new FlightDTO();
            f2.origin = "Dublin";
            f2.origin = "Dublin";
            f2.destination = "London";
            var flights = new List<FlightDTO>() { f1, f2 };
            flightServiceRepo.Setup(f => f.SearchFlightsAvailability(passengers, origin, destination, dateOut, dateIn, roundtrip)).Throws(new ArgumentNullException());

            var controller = new ReservationController(mockRepo.Object, flightServiceRepo.Object, reservationServiceRepo.Object);

            //  Act
            var result = controller.Flight(passengers, origin, destination, dateOut, dateIn, roundtrip);
            var errorResult = result.Result as ObjectResult;
            // Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, errorResult.StatusCode);
        }

        [Fact]
        public void ReservationControllerTest_CreateReservation_Success()
        {
            var reservationService = new Mock<IReservationService>();
            Reservations reservation = new Reservations();
            ReservationsDTO reservationsDTO = new ReservationsDTO();
            reservationsDTO.creditCard = "1234";
            reservationsDTO.email = "testEmail@gmail.com";
            var mockRepo = new Mock<ILogger<ReservationController>>();
            var flightServiceRepo = new Mock<IFlightService>();
            var impapper = new Mock<IMapper>();
            reservation.creditCard = "1234";
            reservation.email = "testEmail@gmail.com";
            string reservationNumber = "ABC100";
            reservation.reservationNumber = reservationNumber;
            reservationService.Setup(f => f.AddReservation(reservationsDTO)).Returns(reservation);

            var controller = new ReservationController(mockRepo.Object, flightServiceRepo.Object, reservationService.Object);

            //  Act
            var result = controller.Reservation(reservationsDTO);
            var res = result.Result as OkObjectResult;
            Assert.Equal(reservationNumber, res.Value);
        }

        [Fact]
        public void ReservationControllerTest_NullReservationNumber()
        {
            var impapper = new Mock<IMapper>();
            var reservationService = new Mock<IReservationService>();
            Reservations reservation = new Reservations();

            ReservationsDTO reservationsDTO = new ReservationsDTO();
            reservationsDTO.creditCard = "1234";
            reservationsDTO.email = "testEmail@gmail.com";
            var mockRepo = new Mock<ILogger<ReservationController>>();
            var flightServiceRepo = new Mock<IFlightService>();
            Error error = new Error();
            error.errorMessage = "error in creating reservation";
            reservation.creditCard = "1234";
            reservation.email = "testEmail@gmail.com";
            reservation.error = error;
            reservation.reservationNumber = null;
            reservationService.Setup(f => f.AddReservation(reservationsDTO)).Returns(reservation);

            var controller = new ReservationController(mockRepo.Object, flightServiceRepo.Object, reservationService.Object);

            //  Act
            var result = controller.Reservation(reservationsDTO);
          
            // Assert
            Assert.True(result.Result is BadRequestObjectResult);
        }

        [Fact]
        public void ReservationControllerTest_ProcessingException()
        {
            var impapper = new Mock<IMapper>();
            var reservationService = new Mock<IReservationService>();
            ReservationsDTO reservation = new ReservationsDTO();
            var mockRepo = new Mock<ILogger<ReservationController>>();
            var flightServiceRepo = new Mock<IFlightService>();
            Error error = new Error();
            error.errorMessage = "error in creating reservation";
            reservation.creditCard = "1234";
            reservation.email = "testEmail@gmail.com";
            reservationService.Setup(f => f.AddReservation(reservation)).Throws(new NullReferenceException());

            var controller = new ReservationController(mockRepo.Object, flightServiceRepo.Object, reservationService.Object);

            //  Act
            var result = controller.Reservation(reservation);
            var errorResult = result.Result as ObjectResult;
            // Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, errorResult.StatusCode);
        }

        [Fact]
        public void ReservationControllerTest_ReservationRetrieval_Success()
        {
            var impapper = new Mock<IMapper>();
            var reservationService = new Mock<IReservationService>();
            Reservations reservation = new Reservations();
            var mockRepo = new Mock<ILogger<ReservationController>>();
            var flightServiceRepo = new Mock<IFlightService>();
            ReservationsDTO reservationsDTO = new ReservationsDTO();
            reservationsDTO.creditCard = "1234";
            reservationsDTO.email = "testEmail@gmail.com";
            reservation.creditCard = "1234";
            reservation.email = "testEmail@gmail.com";
            string reservationNumber = "ABC100";
            reservation.reservationNumber = "ABC100";
            DetailsPerFlight f1 = new DetailsPerFlight();
            f1.key = "1";
            DetailsPerFlight f2 = new DetailsPerFlight();
            f2.key = "2";
            var flights = new List<DetailsPerFlight>() { f1, f2 };
            reservation.flights = flights;
            reservationService.Setup(f => f.GetReservation(reservationNumber)).Returns(reservationsDTO);

            var controller = new ReservationController(mockRepo.Object, flightServiceRepo.Object, reservationService.Object);

            //  Act
            var result = controller.Reservation(reservationNumber);
            var res = result.Result as OkObjectResult;
            // Assert
            Assert.Equal(reservationsDTO, res.Value);
            Assert.True(result.Result is OkObjectResult);
        }

       
        [Fact]
        public void ReservationControllerTest_ReservationRetrieval_processingException()
        {
            var impapper = new Mock<IMapper>();
            var reservationService = new Mock<IReservationService>();
            Reservations reservation = new Reservations();
            var mockRepo = new Mock<ILogger<ReservationController>>();
            var flightServiceRepo = new Mock<IFlightService>();

            reservation.creditCard = "1234";
            reservation.email = "testEmail@gmail.com";
            string reservationNumber = null;
            reservation.reservationNumber = reservationNumber;
            DetailsPerFlight f1 = new DetailsPerFlight();
            f1.key = "1";
            DetailsPerFlight f2 = new DetailsPerFlight();
            f2.key = "2";
            var flights = new List<DetailsPerFlight>() { f1, f2 };
            reservation.flights = flights;
            reservationService.Setup(f => f.GetReservation(reservationNumber)).Throws(new NullReferenceException());

            var controller = new ReservationController(mockRepo.Object, flightServiceRepo.Object, reservationService.Object);

            //  Act
            var result = controller.Reservation(reservationNumber);
            // Assert
            var errorResult = result.Result as ObjectResult;
            // Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, errorResult.StatusCode);
        }
    }
}
