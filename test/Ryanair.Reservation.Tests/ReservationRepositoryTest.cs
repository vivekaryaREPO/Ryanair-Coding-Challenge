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
    public class ReservationRepositoryTest
    {
        [Fact]
        public void ReservationRepositoryTest_GetReservations()
        {
            //Arrange            
            var mockRepo = new Mock<ILogger<ReservationRepository>>();
            DetailsPerFlight f1 = new DetailsPerFlight();
            f1.key = "1";
            DetailsPerFlight f2 = new DetailsPerFlight();
            f2.key = "2";
            var flights = new List<DetailsPerFlight>() { f1, f2 };
            Reservations reservation = new Reservations();
            reservation.creditCard = "1234";
            reservation.email = "testEmail@gmail.com";
            string reservationNumber = "ABC123";
            reservation.reservationNumber = reservationNumber;
            reservation.flights = flights;
            var reservationRepo = new ReservationRepository(mockRepo.Object);
            var reservations = new List<Reservations>() { reservation };
            reservationRepo.reservations = reservations;

            //  Act
            var reservationsResult= reservationRepo.GetReservation(reservationNumber);
          
           // Assert
            Assert.Equal(reservationNumber, reservationsResult.reservationNumber);
            Assert.Equal(2, reservationsResult.flights.Count());
        }

        [Fact]
        public void ReservationRepositoryTest_AddReservation_Success_ExistingFlight()
        {
            //Arrange            
            var mockRepo = new Mock<ILogger<ReservationRepository>>();
            Dictionary<string, int> flightBagCapacity = new Dictionary<string, int>();
            flightBagCapacity.Add("f1",10);
            flightBagCapacity.Add("f2", 20);
            DetailsPerFlight f1 = new DetailsPerFlight();
            f1.key = "f1";
            Passenger p1 = new Passenger();
            p1.bags = 5;
            var passengers = new List<Passenger>() { p1 };
            f1.passengers = passengers;
            DetailsPerFlight f2 = new DetailsPerFlight();
            f2.key = "f2";
            Passenger p2 = new Passenger();
            p2.bags = 5;
            passengers = new List<Passenger>() { p2 };
            f2.passengers = passengers;
            var flights = new List<DetailsPerFlight>() { f1, f2 };
            Reservations reservationRequest = new Reservations();
            reservationRequest.creditCard = "1234";
            reservationRequest.email = "testEmail@gmail.com";
            string reservationNumber = "ABC123";
            reservationRequest.reservationNumber = reservationNumber;
            reservationRequest.reservationNumber = null;
            reservationRequest.flights = flights;
            var reservationRepo = new ReservationRepository(mockRepo.Object);
            reservationRepo.flightBagCapacity = flightBagCapacity;

            //  Act
            var reservationsResult = reservationRepo.AddReservation(reservationRequest);

            // Assert
            Assert.True(reservationsResult.reservationNumber!=null);
            Assert.True(reservationsResult.error==null);
            Assert.True(reservationRepo.flightBagCapacity["f1"]==15);
            Assert.True(reservationRepo.flightBagCapacity["f2"]== 25);
        }

        [Fact]
        public void ReservationRepositoryTest_AddReservation_Success_NewFlight()
        {
            //Arrange            
            var mockRepo = new Mock<ILogger<ReservationRepository>>();
            Dictionary<string, int> flightBagCapacity = new Dictionary<string, int>();
            DetailsPerFlight f1 = new DetailsPerFlight();
            f1.key = "f1";
            Passenger p1 = new Passenger();
            p1.bags = 5;
            var passengers = new List<Passenger>() { p1 };
            f1.passengers = passengers;
            DetailsPerFlight f2 = new DetailsPerFlight();
            f2.key = "f2";
            Passenger p2 = new Passenger();
            p2.bags = 5;
            passengers = new List<Passenger>() { p2 };
            f2.passengers = passengers;
            var flights = new List<DetailsPerFlight>() { f1, f2 };
            Reservations reservationRequest = new Reservations();
            reservationRequest.creditCard = "1234";
            reservationRequest.email = "testEmail@gmail.com";
            string reservationNumber = "ABC123";
            reservationRequest.reservationNumber = reservationNumber;
            reservationRequest.reservationNumber = null;
            reservationRequest.flights = flights;
            var reservationRepo = new ReservationRepository(mockRepo.Object);
            reservationRepo.flightBagCapacity = flightBagCapacity;

            //  Act
            var reservationsResult = reservationRepo.AddReservation(reservationRequest);

            // Assert
            Assert.True(reservationsResult.reservationNumber != null);
            Assert.True(reservationsResult.error == null);
            Assert.True(reservationRepo.flightBagCapacity["f1"] == 5);
            Assert.True(reservationRepo.flightBagCapacity["f2"] == 5);
        }

        [Fact]
        public void ReservationRepositoryTest_AddReservation_Failure_AdditionalBags()
        {
            //Arrange            
            var mockRepo = new Mock<ILogger<ReservationRepository>>();
            Dictionary<string, int> flightBagCapacity = new Dictionary<string, int>();
            flightBagCapacity.Add("f1", 46);
            flightBagCapacity.Add("f2", 25);
            DetailsPerFlight f1 = new DetailsPerFlight();
            f1.key = "f1";
            Passenger p1 = new Passenger();
            p1.bags = 5;
            var passengers = new List<Passenger>() { p1 };
            f1.passengers = passengers;
            DetailsPerFlight f2 = new DetailsPerFlight();
            f2.key = "f2";
            Passenger p2 = new Passenger();
            p2.bags = 5;
            passengers = new List<Passenger>() { p2 };
            f2.passengers = passengers;
            var flights = new List<DetailsPerFlight>() { f1, f2 };
            Reservations reservationRequest = new Reservations();
            reservationRequest.creditCard = "1234";
            reservationRequest.email = "testEmail@gmail.com";
            reservationRequest.reservationNumber = null;
            reservationRequest.flights = flights;
            var reservationRepo = new ReservationRepository(mockRepo.Object);
            reservationRepo.flightBagCapacity = flightBagCapacity;

            //  Act
            var reservationsResult = reservationRepo.AddReservation(reservationRequest);

            // Assert
            Assert.True(reservationsResult.reservationNumber == null);
            Assert.True(reservationsResult.error != null);
            Assert.True(reservationRepo.flightBagCapacity["f1"] == 46);
            Assert.True(reservationRepo.flightBagCapacity["f2"] == 25);
        }
    }
}
