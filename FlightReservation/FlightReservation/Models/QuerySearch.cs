using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlightReservation.Models
{
    public class QuerySearch
    {
        public string DepartDate { get; set; }
        public string ArrivalDate { get; set; }
        public string DAirport { get; set; }
        public string AAirport { get; set; }
    }
}