﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FlightReservation.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class db_9c079b_airlineEntities : DbContext
    {
        public db_9c079b_airlineEntities()
            : base("name=db_9c079b_airlineEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<account> accounts { get; set; }
        public virtual DbSet<aircraft> aircraft { get; set; }
        public virtual DbSet<aircraft_seats> aircraft_seats { get; set; }
        public virtual DbSet<airport> airports { get; set; }
        public virtual DbSet<credit_card> credit_card { get; set; }
        public virtual DbSet<flight> flights { get; set; }
        public virtual DbSet<passenger> passengers { get; set; }
        public virtual DbSet<seat_class> seat_class { get; set; }
        public virtual DbSet<ticket> tickets { get; set; }
    }
}