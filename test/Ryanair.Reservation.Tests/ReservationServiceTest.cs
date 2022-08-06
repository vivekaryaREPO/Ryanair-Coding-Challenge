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
    public class ReservationServiceTest
    {
        [Fact]
        public void ReservationServiceTest_AddReservation()
        {
            //Arrange            
            var mockRepo = new Mock<IReservationRepository>();
            var impapper = new Mock<IMapper>();
            var reservationService = new ReservationService(mockRepo.Object, impapper.Object);

            ReservationsDTO d = new ReservationsDTO();
            d.email = "testemail.com";
            Reservations d1 = new Reservations();
            d1.email = "testemail.com";

            mockRepo.Setup(f => f.AddReservation(d1)).Returns(d1);
            impapper.Setup(f => f.Map<Reservations>(d)).Returns(d1);

            var searchResults = reservationService.AddReservation(d);

            // Assert
            Assert.True(searchResults!=null);
        }

        [Fact]
        public void ReservationServiceTest_GetReservation()
        {
            //Arrange            
            var mockRepo = new Mock<IReservationRepository>();
            var impapper = new Mock<IMapper>();
            var reservationService = new ReservationService(mockRepo.Object, impapper.Object);

            ReservationsDTO d = new ReservationsDTO();
            d.email = "testemail.com";
            string reservationNumber = "ABC123";
            Reservations d1 = new Reservations();
            d1.email = "testemail.com";

            mockRepo.Setup(f => f.GetReservation(reservationNumber)).Returns(d1);
            impapper.Setup(f => f.Map<ReservationsDTO>(d1)).Returns(d);

            var searchResults = reservationService.GetReservation(reservationNumber);

            // Assert
            Assert.True(searchResults != null);
        }
    }
}
