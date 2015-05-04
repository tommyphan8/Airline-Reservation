using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlightReservation.Models
{
    public class Ticket_Flight
    {
        //public List<ticket> ticket { get; set; }
        //public List<flight> flight { get; set; }
        public flight flight { get; set; }
        public ticket ticket { get; set; }

       
    }
}