using System;
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
   [Authorize(Roles = "admin")]
    public class PassengerController : Controller
    {
        private db_9c079b_airlineEntities db = new db_9c079b_airlineEntities();

        // GET: /Passenger/
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View(db.passengers.ToList());
        }

        // GET: /Passenger/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            passenger passenger = db.passengers.Find(id);
            if (passenger == null)
            {
                return HttpNotFound();
            }
            return View(passenger);
        }

        // GET: /Passenger/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Passenger/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Pid,FName,Mname,LName,DOB,Gender,Phone")] passenger passenger)
        {
            if (ModelState.IsValid)
            {
                db.passengers.Add(passenger);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(passenger);
        }

        // GET: /Passenger/Edit/5

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            passenger passenger = db.passengers.Find(id);
            if (passenger == null)
            {
                return HttpNotFound();
            }
            return View(passenger);
        }

        // POST: /Passenger/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Pid,FName,Mname,LName,DOB,Gender,Phone")] passenger passenger)
        {
            if (ModelState.IsValid)
            {
                db.Entry(passenger).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(passenger);
        }

        [Authorize(Roles = "admin")]
        // GET: /Passenger/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            passenger passenger = db.passengers.Find(id);
            if (passenger == null)
            {
                return HttpNotFound();
            }
            return View(passenger);
        }

        // POST: /Passenger/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            passenger passenger = db.passengers.Find(id);
            db.passengers.Remove(passenger);
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
