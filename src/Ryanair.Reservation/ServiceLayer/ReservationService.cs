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
    public class ReservationService: IReservationService
    {
        private readonly IReservationRepository reservationRepository;

        private readonly IMapper mapper;
        public ReservationService(IReservationRepository reservationRepository,
                                     IMapper mapper)
        {
            this.reservationRepository = reservationRepository;
            this.mapper = mapper;
        }

        public Reservations AddReservation(ReservationsDTO reservation)
        {
            Reservations reservationReq = mapper.Map<Reservations>(reservation);
            return this.reservationRepository.AddReservation(reservationReq);
        }

        public ReservationsDTO GetReservation(string reservationNumber)
        {
            Reservations reservations = this.reservationRepository.GetReservation(reservationNumber);
            ReservationsDTO res = null;
            if (null != reservations)
            {
                res = mapper.Map<ReservationsDTO>(reservations);
            }
            return res;
        }
    }
}
