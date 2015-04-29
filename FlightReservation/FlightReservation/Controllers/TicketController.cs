﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FlightReservation.Models;
using Microsoft.AspNet.Identity;



namespace FlightReservation.Controllers
{

    [Authorize] // This prevents user from accessing TicketController without logging in.
    public class TicketController : Controller
    {
        private db_9c079b_airlineEntities db = new db_9c079b_airlineEntities();

        public long Pid()
        {
                string temp = User.Identity.GetUserName();
                var accountSess = from a in db.accounts select a;
                accountSess = accountSess.Where(s => s.Email.Contains(temp));
                var list = accountSess.ToList();
                long pid = list[0].Pid;
                System.Diagnostics.Debug.WriteLine(pid);
                return pid;
           
         }

        //public float Price(long Fid)
        //{
        //    var flight = from f in db.flights select f;
        //    flight = flight.Where(s => s.Fid.Equals(Fid));
        //    retu
        //}



        // GET: /Ticket/
        public ActionResult Index()
        {
           
            System.Diagnostics.Debug.WriteLine(Pid());
            return View(db.tickets.ToList());
        }

        public ActionResult SearchFlight()
        {
            return View();
        }

        public ActionResult SearchResult(string depart_input, string arrives, DateTime? dDate)
        {
            var flight = from f in db.flights select f;

            if (!String.IsNullOrEmpty(depart_input))
            {
                flight = flight.Where(s => s.Departs.Contains(depart_input));

            }

            if (!String.IsNullOrEmpty(arrives))
            {
                flight = flight.Where(s => s.Arrives.Contains(arrives));
            }

            if (dDate != null)
            {
                flight = flight.Where(s => s.Dtime >= dDate);// this one means u select all the flight from the departure date
            }


            return View(flight);
        }


        public ActionResult Passenger(string fid, float price)
        {
            //long temp = Pid();
            //if (temp != null)
            //{
            //    return RedirectToAction("Create", "Ticket", new { Fid = fid, price=price });
            //}
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Passenger([Bind(Include = "FName,Mname,LName,DOB,Gender,Phone")] passenger passenger, string fid)
        {

            if (ModelState.IsValid)
            {
                passenger.Pid = Pid();
                db.passengers.Add(passenger);
                db.SaveChanges();
                
                return RedirectToAction("Create", "Ticket", new{ Fid=fid});
               
            }

            return View(passenger);
        }

        // GET: /Ticket/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ticket ticket = db.tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: /Ticket/Create
        public ActionResult Create(string Fid)
        {
            
            
            return View();
        }

        // POST: /Ticket/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="FinalPrice,Status,SeatNum,SeatClass,Fid,Pid,Num_of_bags")] ticket ticket, int Fid)
        {
            if (ModelState.IsValid)
            {
                long tid = db.tickets.Count();
                flight flight = db.flights.Find(Fid);
                ticket.FinalPrice = flight.BasePrice ;
                ticket.Pid = Pid();
                ticket.Tid = tid + 1;
                db.tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Details", "Ticket", new { id = tid+1 });
            }

            return View(ticket);
        }

        // GET: /Ticket/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ticket ticket = db.tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: /Ticket/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Tid,FinalPrice,Status,SeatNum,SeatClass,Fid,Pid,Num_of_bags")] ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ticket);
        }

        // GET: /Ticket/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ticket ticket = db.tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: /Ticket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ticket ticket = db.tickets.Find(id);
            db.tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
