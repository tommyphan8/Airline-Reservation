using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FlightReservation.Models;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Data.SqlClient;



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

        public string takenSeat(int fid)
        {
            var temp = from a in db.tickets select a;
            temp = temp.Where(a => a.Fid.Equals(fid));
            //var n = temp.Count();
            var list = temp.ToList();
            int i, j,k;
            string taken = "";
            if (list.Count != 0)
            {
                i = list[0].SeatNum / 10 + 1;
                j = list[0].SeatNum % 10;
                taken += "'" + i + "_" + j + "'";
                for (k = 1; k < list.Count; k++)
                {
                    i = list[k].SeatNum / 10 + 1;
                    j = list[k].SeatNum % 10;
                    taken += ",'" + i + "_" + j + "'";
                }
            }
           
            //foreach (var item in list)
            //{
            //    //i = item.SeatNum / 10 + 1;
            //    //j = item.SeatNum % 10;
            //    //taken += "'"+i + '_' + j + "',";
            //    taken += item.SeatNum;
            //}
            Console.Write(taken);
            return taken;
        }


        public int firstClass(int aid)
        {
            var fseat = from a in db.aircraft_seats select a;
            fseat = fseat.Where(a => a.Aid.Equals(aid));
            fseat = fseat.Where(a => a.Sid.Equals(2));
            var list = fseat.ToList();
            int fnum = list[0].Capacity;
            return fnum;
        }

        public int ecoClass(int aid)
        {
            var fseat = from a in db.aircraft_seats select a;
            fseat = fseat.Where(a => a.Aid.Equals(aid));
            fseat = fseat.Where(a => a.Sid.Equals(1));
            var list = fseat.ToList();
            int fnum = list[0].Capacity;
            return fnum;
        }

        public int Aid(int Fid)
        {
            flight flight = db.flights.Find(Fid);
            return unchecked((int)flight.Aid);
        }


        public ActionResult listTicket()
        {
            long pidLong = Pid();
            int pid = unchecked((int)pidLong);
            var ticket = from t in db.tickets select t;
            var flight = from f in db.flights select f;
            ticket = ticket.Where(s => s.Pid.Equals(pid));
            var ticketList = ticket.ToList();
            var flightList = flight.ToList();

            List<Ticket_Flight> obj = new List<Ticket_Flight>();

            ticket ticketTemp = new ticket();
            flight flightTemp = new flight();
            //List<ticket> list = new List<ticket>();
            //List<flight> list1 = new List<flight>();

            foreach (var temp in ticketList)
            {
                Ticket_Flight tempp = new Ticket_Flight();
                tempp.ticket = temp;
                tempp.flight = flightList.Find(x => x.Fid.Equals(temp.Fid));
                System.Diagnostics.Debug.WriteLine(tempp.flight.Departs);
                // System.Diagnostics.Debug.WriteLine(temp.ticket.FinalPrice);
                obj.Add(tempp);
            }

            obj.Sort(delegate(Ticket_Flight ps1, Ticket_Flight ps2) { return DateTime.Compare(ps1.flight.Dtime, ps2.flight.Dtime); });

            foreach (var temp in obj)
            {
                System.Diagnostics.Debug.WriteLine(temp.flight.Departs);
            }


            //System.Diagnostics.Debug.WriteLine(ticket.Count());

            return View(obj);
        }


        // send confirmation email
        public void sendEmail()
        {
            var mailMessage = new MailMessage();
            mailMessage.To.Add("linhcao1611@yahoo.com");
            mailMessage.From = new MailAddress("mrlinh1611@gmail.com");
            mailMessage.Subject = "testing 2 ";
            mailMessage.Body = "Hello Mr. Aderson";
            mailMessage.IsBodyHtml = false;

            var smptClient = new SmtpClient { EnableSsl = true };
            smptClient.Credentials = CredentialCache.DefaultNetworkCredentials;

            try
            {
                smptClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateMessageWithAttachment(): {0}",
                      ex.ToString());
            }

        }

        //error handling code for user who tired to access to ticket
        // GET: /Ticket/
        [Authorize]
        public ActionResult Index()
        {
            if (User.IsInRole("admin"))
            {
                System.Diagnostics.Debug.WriteLine(Pid());
                return View(db.tickets.ToList());


            }
            else
            {
                return RedirectToAction("listTicket");
            }


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
            long temp = Pid();
            int aid = Aid(int.Parse(fid));
            int fnum = firstClass(aid);
            int econum = ecoClass(aid);
            string taken = takenSeat(int.Parse(fid));
            Console.WriteLine(taken);
            if (temp != null)
            {
                //return RedirectToAction("Create", "Ticket", Json(new { Fid = fid, price = price, fnum = fnum, econum = econum, arr = taken }));
                return RedirectToAction("Create", "Ticket", new { Fid = fid, price = price, fnum = fnum, econum = econum, arr=taken });
               // return RedirectToAction("Create", "Ticket", new { Fid = fid, price = price, fnum = fnum, econum = econum },Json(new{arr=taken}));
            }
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

                return RedirectToAction("Create", "Ticket", new { Fid = fid });

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
        public ActionResult Create(string Fid, string taken)
        {
            ViewBag.list = taken;

            return View();
        }

        // POST: /Ticket/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FinalPrice,Status,SeatNum,SeatClass,Fid,Pid,Num_of_bags")] ticket ticket, int Fid)
        {
            Random random = new Random();
            if (ModelState.IsValid)
            {
                long tid = db.tickets.Count();
                flight flight = db.flights.Find(Fid);
                //ticket.FinalPrice = flight.BasePrice ;
                ticket.Pid = Pid();
                ticket.Tid = random.Next();
                db.tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("listTicket", "Ticket");
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
        public ActionResult Edit([Bind(Include = "Tid,FinalPrice,Status,SeatNum,SeatClass,Fid,Pid,Num_of_bags")] ticket ticket)
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


        //Delete ticket for only User's tickets
        //set up to redirect to listTicket action instead of Index
        public ActionResult DeleteUser(long? id)
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
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedUser(long id)
        {
            ticket ticket = db.tickets.Find(id);
            db.tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("listTicket");
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
