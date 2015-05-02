using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlightReservation.Models;
using FlightReservation.Controllers;

namespace FlightReservation.Models
{
    public class Admin
    {
        public flight flight { get; set; }
        public ticket ticket { get; set; }


        
       
    }
}