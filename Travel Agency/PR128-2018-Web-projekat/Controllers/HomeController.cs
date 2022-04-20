using PR128_2018_Web_projekat.Functions;
using PR128_2018_Web_projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PR128_2018_Web_projekat.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            List<Arrangement> list = new List<Arrangement>();
            foreach(var item in Data.arrangements)
            {
                if(item.StartDate > DateTime.Now && !item.Deleted)
                {
                    list.Add(item);
                }
            }
            list.Sort((x, y) => x.StartDate.CompareTo(y.StartDate));
            ViewBag.list = list;
            return View();
        }

        public ActionResult PastArrangements()
        {
            List<Arrangement> list = new List<Arrangement>();
            foreach (var item in Data.arrangements)
            {
                if (item.EndDate < DateTime.Now && !item.Deleted)
                {
                    list.Add(item);
                }
            }
            list.Sort((x, y) => y.EndDate.CompareTo(x.EndDate));
            ViewBag.list = list;
            return View();
        }
        
        public ActionResult ArrangementDetails(string id)
        {
            int tempId = Int32.Parse(id);

            foreach (var arrangement in Data.arrangements)
            {
                if (arrangement.Id == tempId)
                {
                    ViewBag.arrangement = arrangement;
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult AccommodationDetails(string id)
        {
            int tempId =Int32.Parse(id);

            List<AccommodationUnit> units = new List<AccommodationUnit>();
            foreach (var arrangement in Data.arrangements)
            {
                if (arrangement.Id == tempId)
                {
                    ViewBag.arrangement = arrangement;
                    foreach (var item in arrangement.Accommodation.AccommodationUnits)
                    {
                        if (!item.Deleted)
                            units.Add(item);
                    }
                    ViewBag.units = units;
                }
            }

            return View();
        }

        WriteFunctions write = new WriteFunctions();

        public ActionResult Comment(string arrangementId, string commentText, string grade)
        {
            User currentUser = (User)Session["USER"];
            Arrangement arrangement = Data.arrangements.Find(item => item.Id == Int32.Parse(arrangementId));

            Comment comment = new Comment(Data.comments.Count + 1, (Tourist)currentUser, arrangement, commentText, Int32.Parse(grade));
            Data.comments.Add(comment);
            write.WriteNewComment(comment);

            return RedirectToAction("ArrangementDetails", new { id = arrangementId });
        }

        #region METHODS FOR ACCOMMODATION DETAILS PAGE
        [HttpPost]
        public ActionResult SearchAccommodationUnits(string arrangementID, string minGuests, string maxGuests, string petsAllowed, string minPrice, string maxPrice, string action)
        {
            if(action.Equals("Clear Search"))
            {
                List<AccommodationUnit> units = new List<AccommodationUnit>();
                foreach (var arrangement in Data.arrangements)
                {
                    if (arrangement.Id == Int32.Parse(arrangementID))
                    {
                        ViewBag.arrangement = arrangement;
                        foreach (var item in arrangement.Accommodation.AccommodationUnits)
                        {
                            if (!item.Deleted)
                                units.Add(item);
                        }
                        ViewBag.units = units;
                    }
                }

                return View("AccommodationDetails");
            }

            int parsedMinGuests;
            int parsedMaxGuests;
            double parsedMinPrice;
            double parsedMaxPrice;
            if (!string.IsNullOrEmpty(minGuests))
                parsedMinGuests = Int32.Parse(minGuests);
            else
                parsedMinGuests = -1;

            if (!string.IsNullOrEmpty(maxGuests))
                parsedMaxGuests = Int32.Parse(maxGuests);
            else
                parsedMaxGuests = Int32.MaxValue;

            if (!string.IsNullOrEmpty(minPrice))
                parsedMinPrice = Double.Parse(minPrice);
            else
                parsedMinPrice = -1;

            if (!string.IsNullOrEmpty(maxPrice))
                parsedMaxPrice = Double.Parse(maxPrice);
            else
                parsedMaxPrice = Double.MaxValue;

            List<AccommodationUnit> searchList = new List<AccommodationUnit>();
            foreach (var item in Data.arrangements)
            {
                if(item.Id == Int32.Parse(arrangementID))
                {
                    ViewBag.arrangement = item;
                    foreach(var unit in item.Accommodation.AccommodationUnits)
                    {
                        if(unit.NumberOfGuestsAllowed >= parsedMinGuests && unit.NumberOfGuestsAllowed <= parsedMaxGuests &&
                            unit.PetsAllowed == (petsAllowed.Equals("YES") ? true : false) && 
                            unit.Price >= parsedMinPrice && unit.Price <= parsedMaxPrice)
                        {
                            searchList.Add(unit);
                        }
                    }
                }
            }

            ViewBag.units = searchList;
            return View("AccommodationDetails");
        }

        [HttpPost]
        public ActionResult SortAccommodationUnits(string arrangementID, List<string> list, string sortBy, string order)
        {
            Arrangement tempArrangement = new Arrangement();
            List<AccommodationUnit> sortedList = new List<AccommodationUnit>();
            if (list == null)
            {
                List<AccommodationUnit> units = new List<AccommodationUnit>();
                foreach (var arrangement in Data.arrangements)
                {
                    if (arrangement.Id == Int32.Parse(arrangementID))
                    {
                        ViewBag.arrangement = arrangement;
                        foreach (var item in arrangement.Accommodation.AccommodationUnits)
                        {
                            if (!item.Deleted)
                                units.Add(item);
                        }
                        ViewBag.units = units;
                    }
                }
                return View("AccommodationDetails");
            }

            foreach (var arrangement in Data.arrangements)
            {
                if (arrangement.Id == Int32.Parse(arrangementID))
                {
                    tempArrangement = arrangement;
                    break;
                }
            }

            foreach (var item in tempArrangement.Accommodation.AccommodationUnits)
            {
                foreach(string s in list)
                {
                    if(item.UnitID == Int32.Parse(s))
                    {
                        sortedList.Add(item);
                    }
                }
            }

            switch (sortBy)
            {
                case "NumberOfGuests":
                    if (order.Equals("Ascending"))
                    {
                        sortedList.Sort((x, y) => x.NumberOfGuestsAllowed.CompareTo(y.NumberOfGuestsAllowed));
                    }
                    else
                    {
                        sortedList.Sort((x, y) => y.NumberOfGuestsAllowed.CompareTo(x.NumberOfGuestsAllowed));
                    }
                    break;
                case "Price":
                    if (order.Equals("Ascending"))
                    {
                        sortedList.Sort((x, y) => x.Price.CompareTo(y.Price));
                    }
                    else
                    {
                        sortedList.Sort((x, y) => y.Price.CompareTo(x.Price));
                    }
                    break;
            }

            ViewBag.arrangement = tempArrangement;
            ViewBag.units = sortedList;
            return View("AccommodationDetails");
        }

        #endregion

        #region METHODS FOR INDEX PAGE
        [HttpPost]
        public ActionResult Search(string startDateFrom, string startDateTo, string endDateFrom, string endDateTo, string transportationType, string arrangementType, string name, string action)
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

            // APPLY COMBINED SEARCH
            List<Arrangement> searchList = new List<Arrangement>();
            foreach(var item in Data.arrangements)
            {
                if(item.StartDate > startFrom && item.StartDate < startTo && item.EndDate > endFrom && item.EndDate < endTo && item.StartDate > DateTime.Now)
                {
                    if(item.Name.ToLower().Contains(name.ToLower().Trim()))
                    {
                        if (!string.IsNullOrEmpty(transportationType) && !string.IsNullOrEmpty(arrangementType))
                        {
                            if(item.TransportationType == (TransportationType)Enum.Parse(typeof(TransportationType), transportationType)
                                && item.ArrangementType == (ArrangementType)Enum.Parse(typeof(ArrangementType), arrangementType))
                            {
                                searchList.Add(item);
                            }
                        }
                        else if(!string.IsNullOrEmpty(transportationType))
                        {
                            if (item.TransportationType == (TransportationType)Enum.Parse(typeof(TransportationType), transportationType))
                            {
                                searchList.Add(item);
                            }
                        }
                        else if (!string.IsNullOrEmpty(arrangementType))
                        {
                            if (item.ArrangementType == (ArrangementType)Enum.Parse(typeof(ArrangementType), arrangementType))
                            {
                                searchList.Add(item);
                            }
                        }
                        else
                        {
                            searchList.Add(item);
                        }
                    }
                }
            }

            // Sort SearchedList to show from closest Arrangement
            searchList.Sort((x, y) => x.StartDate.CompareTo(y.StartDate));
            ViewBag.list = searchList;
            return View("Index");
        }

        [HttpPost]
        public ActionResult Sort(List<string> list, string sortBy, string order)
        {
            List<Arrangement> sortedList = new List<Arrangement>();
            if(list == null)
            {
                ViewBag.list = sortedList;
                return View("Index");
            }

            foreach(var item in Data.arrangements)
            {
                foreach(string s in list)
                {
                    if (item.Id == Int32.Parse(s))
                        sortedList.Add(item);
                }
            }

            switch(sortBy)
            {
                case "Name":
                    if(order.Equals("Ascending"))
                    {
                        sortedList.Sort((x, y) => x.Name.CompareTo(y.Name));
                    }
                    else
                    {
                        sortedList.Sort((x, y) => y.Name.CompareTo(x.Name));
                    }
                    break;
                case "StartDate":
                    if (order.Equals("Ascending"))
                    {
                        sortedList.Sort((x, y) => x.StartDate.CompareTo(y.StartDate));
                    }
                    else
                    {
                        sortedList.Sort((x, y) => y.StartDate.CompareTo(x.StartDate));
                    }
                    break;
                case "EndDate":
                    if (order.Equals("Ascending"))
                    {
                        sortedList.Sort((x, y) => x.EndDate.CompareTo(y.EndDate));
                    }
                    else
                    {
                        sortedList.Sort((x, y) => y.EndDate.CompareTo(x.EndDate));
                    }
                    break;
            }

            ViewBag.list = sortedList;
            return View("Index");
        }
        #endregion

        #region METHODS FOR PAST ARRANGEMENTS PAGE

        [HttpPost]
        public ActionResult SortPast(List<string> list, string sortBy, string order)
        {
            List<Arrangement> sortedList = new List<Arrangement>();
            if (list == null)
            {
                ViewBag.list = sortedList;
                return View("PastArrangements");
            }

            foreach (var item in Data.arrangements)
            {
                foreach (string s in list)
                {
                    if (item.Id == Int32.Parse(s))
                        sortedList.Add(item);
                }
            }

            switch (sortBy)
            {
                case "Name":
                    if (order.Equals("Ascending"))
                    {
                        sortedList.Sort((x, y) => x.Name.CompareTo(y.Name));
                    }
                    else
                    {
                        sortedList.Sort((x, y) => y.Name.CompareTo(x.Name));
                    }
                    break;
                case "StartDate":
                    if (order.Equals("Ascending"))
                    {
                        sortedList.Sort((x, y) => x.StartDate.CompareTo(y.StartDate));
                    }
                    else
                    {
                        sortedList.Sort((x, y) => y.StartDate.CompareTo(x.StartDate));
                    }
                    break;
                case "EndDate":
                    if (order.Equals("Ascending"))
                    {
                        sortedList.Sort((x, y) => x.EndDate.CompareTo(y.EndDate));
                    }
                    else
                    {
                        sortedList.Sort((x, y) => y.EndDate.CompareTo(x.EndDate));
                    }
                    break;
            }

            ViewBag.list = sortedList;
            return View("PastArrangements");
        }


        // Same function as "Search" but for Past Arrangements
        [HttpPost]
        public ActionResult SearchPast(string startDateFrom, string startDateTo, string endDateFrom, string endDateTo, string transportationType, string arrangementType, string name, string action)
        {
            if (action.Equals("Clear Search"))
            {
                return RedirectToAction("PastArrangements");
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

            // APPLY COMBINED SEARCH
            List<Arrangement> searchList = new List<Arrangement>();
            foreach (var item in Data.arrangements)
            {
                if (item.StartDate > startFrom && item.StartDate < startTo && item.EndDate > endFrom && item.EndDate < endTo && item.EndDate < DateTime.Now)
                {
                    if (item.Name.ToLower().Contains(name.ToLower().Trim()))
                    {
                        if (!string.IsNullOrEmpty(transportationType) && !string.IsNullOrEmpty(arrangementType))
                        {
                            if (item.TransportationType == (TransportationType)Enum.Parse(typeof(TransportationType), transportationType)
                                && item.ArrangementType == (ArrangementType)Enum.Parse(typeof(ArrangementType), arrangementType))
                            {
                                searchList.Add(item);
                            }
                        }
                        else if (!string.IsNullOrEmpty(transportationType))
                        {
                            if (item.TransportationType == (TransportationType)Enum.Parse(typeof(TransportationType), transportationType))
                            {
                                searchList.Add(item);
                            }
                        }
                        else if (!string.IsNullOrEmpty(arrangementType))
                        {
                            if (item.ArrangementType == (ArrangementType)Enum.Parse(typeof(ArrangementType), arrangementType))
                            {
                                searchList.Add(item);
                            }
                        }
                        else
                        {
                            searchList.Add(item);
                        }
                    }
                }
            }

            // Sort SearchedList to show from closest Arrangement
            searchList.Sort((x, y) => y.EndDate.CompareTo(x.EndDate));
            ViewBag.list = searchList;
            return View("PastArrangements");
        }
        #endregion
    }
}