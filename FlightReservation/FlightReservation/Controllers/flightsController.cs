using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FlightReservation.Models;
using System.Data.Entity.SqlServer;
using System.Security.Principal;
using Microsoft.AspNet.Identity;


namespace FlightReservation.Controllers
{   
    
    public class flightsController : Controller
    {

        private db_9c079b_airlineEntities db = new db_9c079b_airlineEntities();

        // GET: flights
        [Authorize(Roles="admin")]
        public ActionResult Index()
        {
            
            return View(db.flights.ToList());
        }
        public ActionResult SearchResult(FormCollection collection)
        {
            var check = collection.Get("dDate");
            if (check == "")
            {
                //just an error catch, tried js client side, didnt work. this is for temporary fix only
                ModelState.AddModelError("Departure", "Please select a Date");
                return RedirectToAction("SearchResult");
            }
            else
            {
                DateTime dDate = Convert.ToDateTime(collection.Get("dDate"));
                DateTime aDate = Convert.ToDateTime(collection.Get("aDate"));
                string dAirport = collection.Get("dAirport");
                string aAirport = collection.Get("aAirport");

                var flight = from f in db.flights select f;


                flight = flight.Where(s => s.Dtime >= dDate)
                .Where(s => s.Departs.Contains(dAirport))
                .Where(s => s.Arrives.Contains(aAirport));
                return View(flight);

            }
           


            
        }
        public ActionResult SearchInput()
        {
            return View();
        }

        // GET: flights/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            flight flight = db.flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            return View(flight);
        }

        // GET: flights/Create
        [Authorize(Roles="admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: flights/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Fid,Departs,Arrives,Dtime,Atime,BasePrice,Aid")] flight flight)
        {
            if (ModelState.IsValid)
            {
                db.flights.Add(flight);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(flight);
        }

        // GET: flights/Edit/5
         [Authorize(Roles = "admin")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            flight flight = db.flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            return View(flight);
        }

        // POST: flights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Fid,Departs,Arrives,Dtime,Atime,BasePrice,Aid")] flight flight)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flight).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(flight);
        }

        [Authorize(Roles = "admin")]
        // GET: flights/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            flight flight = db.flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            return View(flight);
        }

        // POST: flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            flight flight = db.flights.Find(id);
            db.flights.Remove(flight);
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
