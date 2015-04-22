﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FlightReservation.Models;

namespace FlightReservation.Controllers
{
    public class TicketController : Controller
    {
        private db_9c079b_airlineEntities db = new db_9c079b_airlineEntities();

        // GET: /Ticket/
        public ActionResult Index()
        {
            return View(db.tickets.ToList());
        }

        public ActionResult SearchInput()
        {
            return View();
        }

        public ActionResult SearchResult(FormCollection collection)
        {
            //string date = collection.Get("dDate");
            DateTime dDate = Convert.ToDateTime(collection.Get("dDate"));
            DateTime aDate = Convert.ToDateTime(collection.Get("aDate"));
            string dAirport = collection.Get("dAirport");
            string aAirport = collection.Get("aAirport");

            //DateTime dDate = Convert.ToDateTime(collection.Get("dDate"));
            //string dAirport = collection.Get("dAirport");
            var flight = from f in db.flights select f;

            //flight = flight.Where(s => s.Departs.Contains(dAirport));
            //flight = flight.Where(s => s.Dtime => collection.Get("dDate"));
            flight = flight.Where(s => s.Dtime >= dDate)
            .Where(s => s.Departs.Contains(dAirport))
            .Where(s => s.Departs.Contains(dAirport));
            //flight = flight.Where(s => s.Atime >= aDate);



            return View(flight);
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
        public ActionResult Create([Bind(Include="Tid,FinalPrice,Status,SeatNum,SeatClass,Fid,Pid,Num_of_bags")] ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
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
