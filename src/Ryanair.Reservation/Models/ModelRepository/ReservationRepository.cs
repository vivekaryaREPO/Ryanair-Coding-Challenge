//using FizzWare.NBuilder;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ryanair.Reservation.Models.IModelRepository;
using Ryanair.Reservation.Models.ModelClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryanair.Reservation.Models.ModelRepository
{
    public class ReservationRepository : IReservationRepository
    {
        public List<Reservations> reservations = new List<Reservations>();
        public List<string> keys = new List<string>();
        public Dictionary<string, int> flightBagCapacity = new Dictionary<string, int>();
        private readonly ILogger<ReservationRepository> logger;

         
        public ReservationRepository(ILogger<ReservationRepository> logger)
        {
            this.logger = logger;
            string reservationsJson = System.IO.File.ReadAllText("InitialPostReservation.json");
            Reservations reservation = JsonConvert.DeserializeObject<Reservations>(reservationsJson);
            reservation.reservationNumber="ABC123";
            this.reservations.Add(reservation);
        }

        public string GenerateKey()
        {
            logger.LogInformation("Initiating unique reservation number generation");
            bool found = true;
                Random _random = new Random();
                string s;
                var builder = new StringBuilder(3);
                while (found)
                {

                    for (int i = 0; i < 3; i++)
                    {
                        var @char = (char)_random.Next('A', 'A' + 26);
                        builder.Append(@char);
                    }
                    s = builder.ToString() + _random.Next(100, 999);
                    string temp = this.keys.FirstOrDefault(key => key.Contains(s));
                    if (temp == null)
                    {
                      this.keys.Add(s);
                      return s;
                    }
                    else
                    {
                        builder = null;
                        continue;
                    }
                }
            return null;

        }

        
        public Reservations GetReservation(string reservationNumber)
        {
            Reservations result= this.reservations.Find(reservation=>reservation.reservationNumber==reservationNumber);
            if(result!=null)
            {
                result.reservationNumber = reservationNumber;
                return result;
            }
            else
            {
                return null;
            }
        }

        public Reservations AddReservation(Reservations reservation)
        {
            logger.LogInformation("Initiating reservation addition");
            bool safeToReserve = true;
            Dictionary<string, int> passengerBagCapacityTemp = new Dictionary<string, int>();
            
            foreach (DetailsPerFlight detailsPerFlight in reservation.flights)
            {
                int currentBaggageSum = flightBagCapacity.ContainsKey(detailsPerFlight.key)? flightBagCapacity[detailsPerFlight.key]:0;
                int flightBaggageSum = detailsPerFlight.passengers.Sum(person=>person.bags);
                
                
                if ((currentBaggageSum+flightBaggageSum)>50) {
                    safeToReserve = false;
                }
                else
                {
                    passengerBagCapacityTemp.Add(detailsPerFlight.key, currentBaggageSum + flightBaggageSum);
                }
            }
            if (!safeToReserve)
            {
                string errorMessage = "Flight baggage capacity for one or more flights in your reservation is already full, please reduce the number of baggages and try again";
                Error error = new Error();
                error.errorMessage = errorMessage;
                reservation.error = error;
                logger.LogWarning(errorMessage);
                return reservation;
            }
            else
            {
                string newReservationKey = GenerateKey();
                reservation.reservationNumber = newReservationKey;
                reservations.Add(reservation);
                foreach(var temp in passengerBagCapacityTemp)
                {
                    flightBagCapacity[temp.Key]=temp.Value;
                }
                logger.LogInformation("reservation details added successfully");
                return reservation;
            }
        }
    }
}
