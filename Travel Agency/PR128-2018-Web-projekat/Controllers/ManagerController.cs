using PR128_2018_Web_projekat.Functions;
using PR128_2018_Web_projekat.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static PR128_2018_Web_projekat.MvcApplication;

namespace PR128_2018_Web_projekat.Controllers
{
    public class ManagerController : Controller
    {
        WriteFunctions write = new WriteFunctions();

        // GET: Manager
        public ActionResult Index()
        {
            User currentUser = (User)Session["USER"];

            if (currentUser == null || currentUser.Role != Role.MANAGER)
            {
                return RedirectToAction("Index", "Home");
            }
            else
                return View();
        }

        public ActionResult ManageArrangements()
        {
            User currentUser = (User)Session["USER"];

            if (currentUser == null || currentUser.Role != Role.MANAGER)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.list = ((Manager)currentUser).CreatedArrangements;
                return View();
            }
        }

        public ActionResult EditArrangement()
        {
            User currentUser = (User)Session["USER"];

            if (currentUser == null || currentUser.Role != Role.MANAGER)
            {
                return RedirectToAction("Index", "Home");
            }
            else
                return View();
        }

        public ActionResult AddArrangement()
        {
            User currentUser = (User)Session["USER"];

            if (currentUser == null || currentUser.Role != Role.MANAGER)
            {
                return RedirectToAction("Index", "Home");
            }
            else
                return View();
        }

        public ActionResult ManageAccommodations()
        {
            User currentUser = (User)Session["USER"];

            if (currentUser == null || currentUser.Role != Role.MANAGER)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.list = Data.accommodations;
                return View();
            }
        }

        public ActionResult ManageUnits(string id)
        {
            User currentUser = (User)Session["USER"];

            if (currentUser == null || currentUser.Role != Role.MANAGER)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (Accommodation accommodation in Data.accommodations)
                {
                    if (accommodation.Id == Int32.Parse(id))
                    {
                        ViewBag.accommodation = accommodation;
                        ViewBag.units = accommodation.AccommodationUnits;
                        break;
                    }
                }
                return View();
            }
        }

        public ActionResult EditAccommodation(string id)
        {
            User currentUser = (User)Session["USER"];

            if (currentUser == null || currentUser.Role != Role.MANAGER)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (Accommodation accommodation in Data.accommodations)
                {
                    if (accommodation.Id == Int32.Parse(id))
                    {
                        ViewBag.accommodation = accommodation;
                        break;
                    }
                }
                return View();
            }
        }

        public ActionResult AddAccommodation()
        {
            User currentUser = (User)Session["USER"];

            if (currentUser == null || currentUser.Role != Role.MANAGER)
            {
                return RedirectToAction("Index", "Home");
            }
            else
                return View();
        }

        public ActionResult EditUnit(string accommodationId, string unitID)
        {
            User currentUser = (User)Session["USER"];

            if (currentUser == null || currentUser.Role != Role.MANAGER)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (Accommodation accommodation in Data.accommodations)
                {
                    if (accommodation.Id == Int32.Parse(accommodationId))
                    {
                        foreach (AccommodationUnit unit in accommodation.AccommodationUnits)
                        {
                            if (unit.UnitID == Int32.Parse(unitID))
                            {
                                ViewBag.unit = unit;
                                break;
                            }
                        }
                        break;
                    }
                }

                return View();
            }
        }

        public ActionResult AddUnit(string accommodationId)
        {
            User currentUser = (User)Session["USER"];

            if (currentUser == null || currentUser.Role != Role.MANAGER)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.accommodationId = accommodationId;
                return View();
            }
        }

        public ActionResult EditArrangementPass(string id)
        {
            foreach (Arrangement arrangement in Data.arrangements)
            {
                if (arrangement.Id == Int32.Parse(id))
                {
                    ViewBag.arrangement = arrangement;
                    break;
                }
            }

            return View("EditArrangement");
        }

        public ActionResult Reservations()
        {
            User currentUser = (User)Session["USER"];

            if (currentUser == null || currentUser.Role != Role.MANAGER)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<Reservation> reservations = new List<Reservation>();
                foreach (Reservation reservation in Data.reservations.Values)
                {
                    foreach(Arrangement arrangement in ((Manager)currentUser).CreatedArrangements)
                    {
                        if(arrangement.Id == reservation.Arrangement.Id)
                        {
                            reservations.Add(reservation);
                        }
                    }
                }

                ViewBag.list = reservations;
                return View();
            }
        }

        public ActionResult ReservationDetails(string id)
        {
            User currentUser = (User)Session["USER"];

            if (currentUser == null || currentUser.Role != Role.MANAGER)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.reservation = Data.reservations[id];

                return View();
            }
        }

        public ActionResult Comments()
        {
            User currentUser = (User)Session["USER"];

            if (currentUser == null || currentUser.Role != Role.MANAGER)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<Comment> comments = new List<Comment>();
                foreach(Arrangement createdArrangement in ((Manager)currentUser).CreatedArrangements)
                {
                    foreach(Comment comment in Data.comments)
                    {
                        if(createdArrangement.Id == comment.Arrangement.Id)
                        {
                            comments.Add(comment);
                        }
                    }
                }

                ViewBag.comments = comments;
                return View();
            }
        }

        public ActionResult CommentApproval(string commentId, string action)
        {
            User currentUser = (User)Session["USER"];

            if (currentUser == null || currentUser.Role != Role.MANAGER)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach(Comment comment in Data.comments)
                {
                    if(comment.Id == Int32.Parse(commentId))
                    {
                        switch(action)
                        {
                            case "APPROVE":
                                comment.Approved = true;
                                break;
                            case "DISAPPROVE":
                                comment.Approved = false;
                                break;
                        }
                        break;
                    }
                }
                write.OverwriteComments();

                return RedirectToAction("Comments");
            }
        }

        // =========================================================  ADD ACCOMMODATION  =========================================================
        [HttpPost]
        public ActionResult AddAccommodation(string id, string accommodationType, string name,
            string hotelStars, string pool, string spaCenter, string disabledPeopleAdapted, string wifi)
        {
            AccommodationType type = (AccommodationType)Enum.Parse(typeof(AccommodationType), accommodationType);
            if (String.IsNullOrEmpty(name) || (type == AccommodationType.HOTEL && string.IsNullOrEmpty(hotelStars)))
            {
                ViewBag.error = "You must fill all fields!";
                return View();
            }

            int stars = 0;
            if (hotelStars != null)
                stars = Int32.Parse(hotelStars);

            Accommodation newAccommodation = new Accommodation(Int32.Parse(id), type, name, stars, bool.Parse(pool), bool.Parse(spaCenter), bool.Parse(disabledPeopleAdapted), bool.Parse(wifi), false);
            Data.accommodations.Add(newAccommodation);

            write.WriteNewAccommodation(newAccommodation);

            return RedirectToAction("ManageAccommodations");
        }

        // =========================================================  Save Edited Accommodation  =========================================================
        [HttpPost]
        public ActionResult SaveAccommodationEdit(string id, string accommodationType, string name,
            string hotelStars, string pool, string spaCenter, string disabledPeopleAdapted, string wifi)
        {
            AccommodationType type = (AccommodationType)Enum.Parse(typeof(AccommodationType), accommodationType);
            int stars = 0;
            if (hotelStars != null)
            {
                stars = Int32.Parse(hotelStars);
            }

            // MODIFY LIST OF ACCOMMODATIONS
            foreach (Accommodation accommodation in Data.accommodations)
            {
                if (accommodation.Id == Int32.Parse(id))
                {
                    accommodation.Name = name;
                    accommodation.AccommodationType = type;
                    accommodation.HotelStars = stars;
                    accommodation.Pool = bool.Parse(pool);
                    accommodation.SpaCenter = bool.Parse(spaCenter);
                    accommodation.DisabledPeopleAdapted = bool.Parse(disabledPeopleAdapted);
                    accommodation.Wifi = bool.Parse(wifi);
                }
            }

            write.OverwriteAccommodations();

            return RedirectToAction("ManageAccommodations");
        }

        // =========================================================  Delete Accommodation Unit  =========================================================
        public ActionResult DeleteUnit(string accommodationId, string unitID)
        {
            foreach (Accommodation accommodation in Data.accommodations)
            {
                if (accommodation.Id == Int32.Parse(accommodationId))
                {
                    foreach (AccommodationUnit unit in accommodation.AccommodationUnits)
                    {
                        if (unit.UnitID == Int32.Parse(unitID))
                        {
                            unit.Deleted = true;
                            break;
                        }
                    }
                    break;
                }
            }
            write.OverwriteAccommodationUnits();

            return RedirectToAction("ManageUnits", new { id = accommodationId });
        }

        // =========================================================  Save Edited Unit  =========================================================
        public ActionResult SaveUnitEdit(string accommodationId, string unitID, string numberOfGuests, string petsAllowed, string price)
        {
            if (numberOfGuests == null)
            {
                if (string.IsNullOrEmpty(price))
                {
                    return RedirectToAction("EditUnit", new { accommodationId = accommodationId, unitID = unitID });
                }
                else
                {
                    foreach (Accommodation accommodation in Data.accommodations)
                    {
                        if (accommodation.Id == Int32.Parse(accommodationId))
                        {
                            foreach (AccommodationUnit unit in accommodation.AccommodationUnits)
                            {
                                if (unit.UnitID == Int32.Parse(unitID))
                                {
                                    unit.PetsAllowed = bool.Parse(petsAllowed);
                                    unit.Price = double.Parse(price);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(price) || string.IsNullOrEmpty(numberOfGuests))
                {
                    return RedirectToAction("EditUnit", new { accommodationId = accommodationId, unitID = unitID });
                }
                else
                {
                    foreach (Accommodation accommodation in Data.accommodations)
                    {
                        if (accommodation.Id == Int32.Parse(accommodationId))
                        {
                            foreach (AccommodationUnit unit in accommodation.AccommodationUnits)
                            {
                                if (unit.UnitID == Int32.Parse(unitID))
                                {
                                    unit.NumberOfGuestsAllowed = Int32.Parse(numberOfGuests);
                                    unit.PetsAllowed = bool.Parse(petsAllowed);
                                    unit.Price = double.Parse(price);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            write.OverwriteAccommodationUnits();
            return RedirectToAction("ManageUnits", new { id = accommodationId });
        }

        // =========================================================  Add New Accommodation Unit  =========================================================
        public ActionResult AddNewUnit(string accommodationId, string numberOfGuests, string petsAllowed, string price)
        {
            if(String.IsNullOrEmpty(numberOfGuests) || String.IsNullOrEmpty(price))
            {
                return RedirectToAction("AddUnit", new { accommodationId = accommodationId });
            }

            foreach(Accommodation accommodation in Data.accommodations)
            {
                if(accommodation.Id == Int32.Parse(accommodationId))
                {
                    int unitID = accommodation.AccommodationUnits.Count + 1;
                    AccommodationUnit newUnit = new AccommodationUnit(accommodation.Id, unitID, Int32.Parse(numberOfGuests), bool.Parse(petsAllowed), double.Parse(price), false, false);
                    accommodation.AccommodationUnits.Add(newUnit);
                    break;
                }
            }

            write.OverwriteAccommodationUnits();
            return RedirectToAction("ManageUnits", new { id = accommodationId });
        }

        // =========================================================  Delete Arrangement  =========================================================
        public ActionResult DeleteArrangement(string id)
        {
            foreach (Arrangement arrangement in Data.arrangements)
            {
                if (arrangement.Id == Int32.Parse(id))
                {
                    arrangement.Deleted = true;
                    break;
                }
            }
            write.OverwriteArrangements();
            return RedirectToAction("ManageArrangements");
        }

        // =========================================================  Delete Accommodation  =========================================================
        public ActionResult DeleteAccommodation(string id)
        {
            foreach (Accommodation accommodation in Data.accommodations)
            {
                if (accommodation.Id == Int32.Parse(id))
                {
                    accommodation.Deleted = true;
                    break;
                }
            }

            write.OverwriteAccommodations();
            return RedirectToAction("ManageAccommodations");
        }

        // =========================================================  Save Edited Arrangement  =========================================================
        public ActionResult SaveArrangementEdit(string id, string name, string arrangementType, string transportationType,
            string location, string startDate, string endDate, string address, string longitude, string latitude,
            string time, string numberOfPassengers, string description, string travelProgram, HttpPostedFileBase image, string accommodationId)
        {
            int oldMeetingPlaceId = Data.arrangements.Find(x => x.Id == Int32.Parse(id)).MeetingPlace.Id;

            string tempImage = "";
            if (image == null)
            {
                foreach (var item in Data.arrangements)
                {
                    if (item.Id == Int32.Parse(id))
                    {
                        tempImage = item.Image;
                    }
                }
            }
            else
            {
                try
                {
                    string filename = Path.GetFileName(image.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images/Arrangements"), filename);
                    image.SaveAs(path);
                    tempImage = filename;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            bool oldDeleted = false;
            foreach (var item in Data.arrangements)
            {
                if (item.Id == Int32.Parse(id))
                {
                    oldDeleted = item.Deleted;
                }
            }


            ArrangementType aType = (ArrangementType)Enum.Parse(typeof(ArrangementType), arrangementType);
            TransportationType tType = (TransportationType)Enum.Parse(typeof(TransportationType), transportationType);

            string[] dateSplit = startDate.Split('-');
            string date = dateSplit[2] + "/" + dateSplit[1] + "/" + dateSplit[0];
            DateTime start = DateTime.ParseExact(date, "dd/MM/yyyy", null);

            dateSplit = endDate.Split('-');
            date = dateSplit[2] + "/" + dateSplit[1] + "/" + dateSplit[0];
            DateTime end = DateTime.ParseExact(date, "dd/MM/yyyy", null);

            MeetingPlace meetingPlace = new MeetingPlace(oldMeetingPlaceId, address, Double.Parse(longitude), Double.Parse(latitude));
            Accommodation accommodation = Data.accommodations.Find(x => x.Id == Int32.Parse(accommodationId));

            User currentUser = (User)Session["USER"];
            Arrangement arrangement = new Arrangement(Int32.Parse(id), name, aType, tType, location, start, end, time, Int32.Parse(numberOfPassengers), description, travelProgram, tempImage, currentUser.Username, oldDeleted);
            arrangement.MeetingPlace = meetingPlace;
            arrangement.Accommodation = accommodation;

            Data.meetingPlaces.Find(x => x.Id == oldMeetingPlaceId).Address = meetingPlace.Address;
            Data.meetingPlaces.Find(x => x.Id == oldMeetingPlaceId).Longitude = meetingPlace.Longitude;
            Data.meetingPlaces.Find(x => x.Id == oldMeetingPlaceId).Latitude = meetingPlace.Latitude;

            for (int i = 0; i < Data.arrangements.Count; i++)
            {
                if (Data.arrangements[i].Id == arrangement.Id)
                    Data.arrangements[i] = arrangement;
            }

            Data.placements.Find(x => x.ArrangementID == arrangement.Id).AccommodationID = arrangement.Accommodation.Id;
            Data.placements.Find(x => x.ArrangementID == arrangement.Id).MeetingPlaceID = arrangement.MeetingPlace.Id;

            write.OverwriteArrangements();
            write.OverwriteMeetingPlaces();
            write.OverwriteAllocation();

            return RedirectToAction("ManageArrangements");
        }

        // =========================================================  Add New Arrangement  =========================================================
        [HttpPost]
        public ActionResult AddArrangement(string id, string name, string arrangementType, string transportationType,
            string location, string startDate, string endDate, string address, string longitude, string latitude,
            string time, string numberOfPassengers, string description, string travelProgram, HttpPostedFileBase image, string accommodationId)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(arrangementType) ||
                string.IsNullOrEmpty(transportationType) || string.IsNullOrEmpty(location) || string.IsNullOrEmpty(startDate) ||
                string.IsNullOrEmpty(endDate) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(longitude) ||
                string.IsNullOrEmpty(latitude) || string.IsNullOrEmpty(time) || string.IsNullOrEmpty(numberOfPassengers) ||
                string.IsNullOrEmpty(description) || string.IsNullOrEmpty(travelProgram) || image == null ||
                string.IsNullOrEmpty(accommodationId))
            {
                ViewBag.error = "You must fill all fields!";
                return View();
            }

            try
            {
                string filename = Path.GetFileName(image.FileName);
                string path = Path.Combine(Server.MapPath("~/Images/Arrangements"), filename);
                image.SaveAs(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            ArrangementType aType = (ArrangementType)Enum.Parse(typeof(ArrangementType), arrangementType);
            TransportationType tType = (TransportationType)Enum.Parse(typeof(TransportationType), transportationType);

            string[] dateSplit = startDate.Split('-');
            string date = dateSplit[2] + "/" + dateSplit[1] + "/" + dateSplit[0];
            DateTime start = DateTime.ParseExact(date, "dd/MM/yyyy", null);

            dateSplit = endDate.Split('-');
            date = dateSplit[2] + "/" + dateSplit[1] + "/" + dateSplit[0];
            DateTime end = DateTime.ParseExact(date, "dd/MM/yyyy", null);

            User currentUser = (User)Session["USER"];
            Arrangement newArrangement = new Arrangement(Int32.Parse(id), name, aType, tType, location, start, end, time, Int32.Parse(numberOfPassengers), description, travelProgram, image.FileName, currentUser.Username, false);

            int numberOfMeetingPlaces = Data.meetingPlaces.Count + 1;
            MeetingPlace meetingPlace = new MeetingPlace(numberOfMeetingPlaces, address, Double.Parse(longitude), Double.Parse(latitude));
            foreach (var accommodation in Data.accommodations)
            {
                if (accommodation.Id == Int32.Parse(accommodationId))
                {
                    newArrangement.Accommodation = accommodation;
                    break;
                }
            }
            newArrangement.MeetingPlace = meetingPlace;

            Data.meetingPlaces.Add(meetingPlace);
            Data.arrangements.Add(newArrangement);

            Data.placements.Add(new Placement(newArrangement.Id, meetingPlace.Id, newArrangement.Accommodation.Id));
            ((Manager)currentUser).CreatedArrangements.Add(newArrangement);

            write.WriteNewMeetingPlace(meetingPlace);
            write.WriteNewArrangement(newArrangement);
            write.OverwriteAllocation();

            return RedirectToAction("ManageArrangements");
        }

        #region METHODS (SEARCH AND SORT) FOR ACCOMMODATIONS
        public ActionResult SearchAccommodations(string accommodationType, string name, string pool, string spaCenter, string disabledPeopleAdapted, string wifi, string action)
        {
            if(action.Equals("Clear Search"))
            {
                return RedirectToAction("ManageAccommodations");
            }
            List<Accommodation> list = new List<Accommodation>();
            foreach(Accommodation accommodation in Data.accommodations)
            {
                if(accommodation.AccommodationType == (AccommodationType)Enum.Parse(typeof(AccommodationType), accommodationType) &&
                    accommodation.Name.ToLower().Contains(name.ToLower()) &&
                    accommodation.Pool == bool.Parse(pool) &&
                    accommodation.SpaCenter == bool.Parse(spaCenter) &&
                    accommodation.DisabledPeopleAdapted == bool.Parse(disabledPeopleAdapted) &&
                    accommodation.Wifi == bool.Parse(wifi))
                {
                    list.Add(accommodation);
                }
            }

            ViewBag.list = list;
            return View("ManageAccommodations");
        }

        public ActionResult SortAccommodations(List<string> list, string sortBy, string order)
        {
            List<Accommodation> sortedList = new List<Accommodation>();
            if (list == null)
            {
                ViewBag.list = sortedList;
                return View("ManageAccommodations");
            }

            foreach(Accommodation accommodation in Data.accommodations)
            {
                foreach (string s in list)
                {
                    if(accommodation.Id == Int32.Parse(s))
                    {
                        sortedList.Add(accommodation);
                    }
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
                case "UnitsNumber":
                    if (order.Equals("Ascending"))
                    {
                        sortedList.Sort((x, y) => x.AccommodationUnits.Count.CompareTo(y.AccommodationUnits.Count));
                    }
                    else
                    {
                        sortedList.Sort((x, y) => y.AccommodationUnits.Count.CompareTo(x.AccommodationUnits.Count));
                    }
                    break;
                case "AvailableUnits":
                    if (order.Equals("Ascending"))
                    {
                        sortedList.Sort((x, y) => x.AccommodationUnits.Where(availabe => availabe.Occupied == false).Count().CompareTo(y.AccommodationUnits.Where(availabe => availabe.Occupied == false).Count()));
                    }
                    else
                    {
                        sortedList.Sort((x, y) => y.AccommodationUnits.Where(availabe => availabe.Occupied == false).Count().CompareTo(x.AccommodationUnits.Where(availabe => availabe.Occupied == false).Count()));
                    }
                    break;
            }

            ViewBag.list = sortedList;
            return View("ManageAccommodations");
        }
        #endregion

        #region METHODS (SEARCH AND SORT) FOR ACCOMMODATION UNITS
        public ActionResult SearchAccommodationUnits(string accommodationId, string minGuests, string maxGuests, string petsAllowed, string minPrice, string maxPrice, string action)
        {
            if (action.Equals("Clear Search"))
            {
                List<AccommodationUnit> units = new List<AccommodationUnit>();
                foreach (var accommodation in Data.accommodations)
                {
                    if (accommodation.Id == Int32.Parse(accommodationId))
                    {
                        ViewBag.accommodation = accommodation;
                        foreach (var item in accommodation.AccommodationUnits)
                        {
                            units.Add(item);
                        }
                        ViewBag.units = units;
                    }
                }

                return View("ManageUnits");
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
            foreach (var item in Data.accommodations)
            {
                if (item.Id == Int32.Parse(accommodationId))
                {
                    ViewBag.accommodation = item;
                    foreach (var unit in item.AccommodationUnits)
                    {
                        if (unit.NumberOfGuestsAllowed >= parsedMinGuests && unit.NumberOfGuestsAllowed <= parsedMaxGuests &&
                            unit.PetsAllowed == (petsAllowed.Equals("YES") ? true : false) &&
                            unit.Price >= parsedMinPrice && unit.Price <= parsedMaxPrice)
                        {
                            searchList.Add(unit);
                        }
                    }
                }
            }

            ViewBag.units = searchList;
            return View("ManageUnits");
        }

        public ActionResult SortAccommodationUnits(string accommodationId, List<string> list, string sortBy, string order)
        {
            Accommodation tempAccommodation = new Accommodation();
            List<AccommodationUnit> sortedList = new List<AccommodationUnit>();
            if (list == null)
            {
                List<AccommodationUnit> units = new List<AccommodationUnit>();
                foreach (var accommodation in Data.accommodations)
                {
                    if (accommodation.Id == Int32.Parse(accommodationId))
                    {
                        ViewBag.accommodation = accommodation;
                        foreach (var item in accommodation.AccommodationUnits)
                        {
                            units.Add(item);
                        }
                        ViewBag.units = units;
                    }
                }
                return View("ManageUnits");
            }

            foreach (var accommodation in Data.accommodations)
            {
                if (accommodation.Id == Int32.Parse(accommodationId))
                {
                    tempAccommodation = accommodation;
                    break;
                }
            }

            foreach (var item in tempAccommodation.AccommodationUnits)
            {
                foreach (string s in list)
                {
                    if (item.UnitID == Int32.Parse(s))
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

            ViewBag.accommodation = tempAccommodation;
            ViewBag.units = sortedList;
            return View("ManageUnits");
        }
        #endregion

        #region METHODS (SEARCH AND SORT) FOR ARRANGEMENTS
        [HttpPost]
        public ActionResult Search(string startDateFrom, string startDateTo, string endDateFrom, string endDateTo, string transportationType, string arrangementType, string name, string action)
        {
            User currentUser = (User)Session["USER"];
            if (action.Equals("Clear Search"))
            {
                return RedirectToAction("ManageArrangements");
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
                if (item.CreatedBy.Equals(currentUser.Username))
                {
                    if (item.StartDate > startFrom && item.StartDate < startTo && item.EndDate > endFrom && item.EndDate < endTo)
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
            }

            // Sort SearchedList to show from closest Arrangement
            searchList.Sort((x, y) => x.StartDate.CompareTo(y.StartDate));
            ViewBag.list = searchList;
            return View("ManageArrangements");
        }

        [HttpPost]
        public ActionResult Sort(List<string> list, string sortBy, string order)
        {
            List<Arrangement> sortedList = new List<Arrangement>();
            if (list == null)
            {
                ViewBag.list = sortedList;
                return View("ManageArrangements");
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
            return View("ManageArrangements");
        }
        #endregion
    }
}