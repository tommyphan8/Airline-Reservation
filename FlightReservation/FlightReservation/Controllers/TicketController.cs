using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Airline.Models;

namespace Airline.Controllers
{
    public class TicketController : Controller
    {
        private db_9c079b_airlineEntities db = new db_9c079b_airlineEntities();


        public ActionResult Result(long? id)
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

        // GET: /Ticket/
        public ActionResult Index()
        {
            return View(db.tickets.ToList());
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

        public ActionResult SearchFlight(string depart_input, string arrives, DateTime? dDate)
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

        // GET: /Ticket/Create
        public ActionResult Create(string Fid, string Pid)
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
                return RedirectToAction("Result",ticket.Tid.ToString());
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
