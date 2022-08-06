using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ryanair.Reservation.DTOs;
using Ryanair.Reservation.Models.IModelRepository;
using Ryanair.Reservation.Models.ModelClasses;
using Ryanair.Reservation.Models.ModelRepository;
using Ryanair.Reservation.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ryanair.Reservation.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ReservationController:ControllerBase
    {
        private readonly ILogger<ReservationController> logger;
        private readonly IFlightService flightService;
        private readonly IReservationService reservationService;
        public ReservationController(ILogger<ReservationController> logger,
                                     IFlightService flightService,
                                     IReservationService reservationService)
        {
            this.flightService = flightService;
            this.reservationService = reservationService;
            this.logger = logger;
        }
        //passengers=3&origin=DUBLIN&destination=LONDON&dateOut=2017-05-08&dateIn=2017-05-10&roundTrip=true

        [HttpGet("Flight")]
        public ActionResult<IEnumerable<FlightDTO>> Flight(int passengers,string origin,string destination, DateTime dateOut, DateTime dateIn,bool roundtrip)
        {
            try
            { 
                logger.LogInformation("searching flight availability based on multiple parameters");
                IEnumerable<FlightDTO> result = flightService.SearchFlightsAvailability(passengers, origin, destination, dateOut, dateIn, roundtrip);
                if(result==null || result.Count()==0)
                {
                    logger.LogWarning("No search result with the given filter");
                    return NotFound("No search result with the given filter");
                }
                else
                {
                    logger.LogInformation("returning flight search results successfully");
                    return  Ok(result);
                }
            }
            catch(Exception exception)
            {
                logger.LogError("Error retrieving flight search results due to : ", exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,"Error retrieving flight search results");
            }
           
        }

        [HttpPost("Reservation")]
        public ActionResult<string> Reservation(ReservationsDTO reservationRequest) {
            try{
                logger.LogInformation("Request for reservation received ");
                Reservations reservationResponse = reservationService.AddReservation(reservationRequest);
                if(null!= reservationResponse && 
                    null != reservationResponse.reservationNumber && reservationResponse.error==null) {
                    logger.LogInformation("Reservation completed successfully with reservation number : "+ reservationResponse.reservationNumber);
                    return Ok(reservationResponse.reservationNumber);
                }
                else{
                    logger.LogWarning(reservationResponse.error.errorMessage);
                    return BadRequest(reservationResponse.error.errorMessage);
                }    
            }
            catch (Exception exception)
            {
                logger.LogError("Failed to make the reservation ", exception);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to make the reservation");
            }
        }

        [HttpGet("Reservation")]
        public ActionResult<ReservationsDTO> Reservation(string reservationNumber)
        {
            try
            {
                logger.LogInformation("Retrieving reservation details for reservation number : "+reservationNumber);
                ReservationsDTO reservation = reservationService.GetReservation(reservationNumber);
                if(reservation==null)
                {
                   logger.LogWarning("Invalid reservation number was sent in the request");
                   return NotFound("Invalid reservation number, please check and send the correct reference number");
                }
                else
                {
                    logger.LogInformation("reservation details retrieved successfully");
                    return Ok(reservation);
                }
            }
            catch (Exception exception)
            {
                logger.LogError("Error retrieving reservation details of reservation number "+ reservationNumber, exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
    }
}
