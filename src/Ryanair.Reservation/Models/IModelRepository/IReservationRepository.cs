using Ryanair.Reservation.Models.ModelClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ryanair.Reservation.Models.IModelRepository
{
   public interface IReservationRepository
    {
        Reservations AddReservation(Reservations reservations);

        Reservations GetReservation(string reservationNumber);


    }
}
