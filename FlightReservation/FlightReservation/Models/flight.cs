//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.ComponentModel.DataAnnotations;

namespace FlightReservation.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class flight
    {
        public long Fid { get; set; }
        public string Departs { get; set; }
        public string Arrives { get; set; }
        [Required(ErrorMessage="Pick a Date")]
        public System.DateTime Dtime { get; set; }
        public System.DateTime Atime { get; set; }
        public float BasePrice { get; set; }
        public long Aid { get; set; }
    }
}
