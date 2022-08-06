using AutoMapper;
using Ryanair.Reservation.DTOs;
using Ryanair.Reservation.Models.ModelClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ryanair.Reservation.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Flight, FlightDTO>();
            CreateMap<Reservations, ReservationsDTO>();
            CreateMap<ReservationsDTO, Reservations>();
        }
    }
}
