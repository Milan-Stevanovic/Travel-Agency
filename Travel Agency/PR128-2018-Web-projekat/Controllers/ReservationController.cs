using PR128_2018_Web_projekat.Functions;
using PR128_2018_Web_projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PR128_2018_Web_projekat.Controllers
{
    public class ReservationController : Controller
    {
        WriteFunctions write = new WriteFunctions();

        // GET: Reservation
        public ActionResult Index()
        {
            User user = (User)Session["USER"];

            if (user == null || user.Role != Role.TOURIST)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<Reservation> list = new List<Reservation>();
                foreach (var item in Data.reservations.Values)
                {
                    if (item.Tourist.Username.Equals(user.Username))
                        list.Add(item);
                }
                ViewBag.list = list;
                return View();
            }
        }

        public ActionResult Reserve(string arrangementID, string unitID)
        {
            User user = ((User)Session["USER"]);

            foreach (var arrangement in Data.arrangements)
            {
                if (arrangement.Id == Int32.Parse(arrangementID))
                {
                    foreach (var accommodationUnit in arrangement.Accommodation.AccommodationUnits)
                    {
                        if (accommodationUnit.UnitID == Int32.Parse(unitID))
                        {
                            accommodationUnit.Occupied = true;

                            // Generate key until we get a unique one
                            string uniqueId = "";
                            do
                            {
                                uniqueId = GetAlphanumericId(15);
                            } while (Data.reservations.ContainsKey(uniqueId));

                            Reservation reservation = new Reservation(uniqueId, ((Tourist)user), Status.ACTIVE, arrangement, accommodationUnit);
                            Data.reservations.Add(uniqueId, reservation);
                            ((Tourist)user).ReservedArrangements.Add(reservation.Arrangement);

                            write.WriteNewReservation(reservation);
                            write.OverwriteAccommodationUnits();

                            break;
                        }
                    }
                    break;
                }
            }

            List<AccommodationUnit> units = new List<AccommodationUnit>();
            foreach (var arrangement in Data.arrangements)
            {
                if (arrangement.Id == Int32.Parse(arrangementID))
                {
                    ViewBag.arrangement = arrangement;
                    foreach(var unit in arrangement.Accommodation.AccommodationUnits)
                    {
                        if(!unit.Deleted)
                        {
                            units.Add(unit);
                        }
                    }
                }
            }
            ViewBag.units = units;
            return View("~/Views/Home/AccommodationDetails.cshtml");
        }

        [HttpPost]
        public ActionResult CancelReservation(string id)
        {
            User user = ((User)Session["USER"]);

            ((Tourist)Data.users[user.Username]).CanceledReservations++;

            foreach (var item in Data.arrangements)
            {
                if(item.Id == Data.reservations[id].Arrangement.Id)
                {
                    foreach (var unit in item.Accommodation.AccommodationUnits)
                    {
                        if(unit.UnitID == Data.reservations[id].AccommodationUnit.UnitID)
                        {
                            unit.Occupied = false;
                            break;
                        }
                    }
                }
            }

            Data.reservations[id].Status = Status.CANCELED;
            write.OverwriteTourists();
            write.OverwriteAccommodationUnits();
            write.OverwriteReservations();

            List<Reservation> list = new List<Reservation>();
            foreach (var item in Data.reservations.Values)
            {
                if (item.Tourist.Username.Equals(user.Username))
                    list.Add(item);
            }
            ViewBag.list = list;

            return View("Index");
        }

        public string GetAlphanumericId(int size)
        {
            string id = "";

            Random rand = new Random();

            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            for (int i = 0; i < size; i++)
            {
                id += chars[rand.Next(chars.Length)];
            }

            return id;
        }

        public ActionResult Search(string id, string startDateFrom, string startDateTo, string endDateFrom, string endDateTo, string status, string action)
        {
            if(action.Equals("Clear Search"))
            {
                return RedirectToAction("Index");
            }

            DateTime startFrom;
            DateTime startTo;
            DateTime endFrom;
            DateTime endTo;

            // CHECK IF USER ENTERED ALL DATES
            if (!string.IsNullOrEmpty(startDateFrom))
                startFrom = DateTime.Parse(startDateFrom);
            else
                startFrom = DateTime.MinValue;

            if (!string.IsNullOrEmpty(startDateTo))
                startTo = DateTime.Parse(startDateTo);
            else
                startTo = DateTime.MaxValue;

            if (!string.IsNullOrEmpty(endDateFrom))
                endFrom = DateTime.Parse(endDateFrom);
            else
                endFrom = DateTime.MinValue;

            if (!string.IsNullOrEmpty(endDateTo))
                endTo = DateTime.Parse(endDateTo);
            else
                endTo = DateTime.MaxValue;

            User currentUser = (User)Session["USER"];

            List<Reservation> searchList = new List<Reservation>();
            foreach(Reservation reservation in Data.reservations.Values)
            {
                if(reservation.Tourist.Username.Equals(currentUser.Username))
                {
                    if(reservation.Id.ToLower().Contains(id.ToLower()) && 
                        reservation.Arrangement.StartDate > startFrom && reservation.Arrangement.StartDate < startTo &&
                        reservation.Arrangement.EndDate > endFrom && reservation.Arrangement.EndDate < endTo && 
                        reservation.Status == (Status)Enum.Parse(typeof(Status), status))
                    {
                        searchList.Add(reservation);
                    }
                }
            }

            ViewBag.list = searchList;
            return View("Index");
        }

        public ActionResult Sort(List<string> list, string sortBy, string order)
        {
            List<Reservation> sortedList = new List<Reservation>();
            if (list == null)
            {
                ViewBag.list = sortedList;
                return View("Index");
            }

            foreach (var reservation in Data.reservations.Values)
            {
                foreach (string s in list)
                {
                    if (reservation.Id.Equals(s))
                        sortedList.Add(reservation);
                }
            }

            switch (sortBy)
            {
                case "Name":
                    if (order.Equals("Ascending"))
                    {
                        sortedList.Sort((x, y) => x.Arrangement.Name.CompareTo(y.Arrangement.Name));
                    }
                    else
                    {
                        sortedList.Sort((x, y) => y.Arrangement.Name.CompareTo(x.Arrangement.Name));
                    }
                    break;
                case "Date":
                    if (order.Equals("Ascending"))
                    {
                        sortedList.Sort((x, y) => x.Arrangement.StartDate.CompareTo(y.Arrangement.StartDate));
                    }
                    else
                    {
                        sortedList.Sort((x, y) => y.Arrangement.StartDate.CompareTo(x.Arrangement.StartDate));
                    }
                    break;
                case "Status":
                    if (order.Equals("Ascending"))
                    {
                        sortedList.Sort((x, y) => x.Status.CompareTo(y.Status));
                    }
                    else
                    {
                        sortedList.Sort((x, y) => y.Status.CompareTo(x.Status));
                    }
                    break;
            }

            ViewBag.list = sortedList;
            return View("Index");
        }
    }
}