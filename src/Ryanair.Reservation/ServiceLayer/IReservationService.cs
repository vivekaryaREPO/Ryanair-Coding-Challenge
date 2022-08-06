using Ryanair.Reservation.DTOs;
using Ryanair.Reservation.Models.ModelClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ryanair.Reservation.ServiceLayer
{
    public interface IReservationService
    {
        Reservations AddReservation(ReservationsDTO reservation);
        ReservationsDTO GetReservation(string reservationNumber);
    }
}
