using PR128_2018_Web_projekat.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;

namespace PR128_2018_Web_projekat
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);


            // LOAD DATA FROM .TXT FILES
            LoadAdmins();
            LoadManagers();
            LoadTourists();

            // LOAD ARRANGEMENTS
            LoadAccommodations();
            LoadAccommodaionUnits();
            LoadMeetingPlaces();
            LoadArrangements();

            FormatData();

            LoadReservations();
            LoadComments();
        }

        #region LOAD USERS
        public void LoadAdmins()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/admins.txt");
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while (!String.IsNullOrEmpty(line = sr.ReadLine()))
            {
                string[] split = line.Split(';');
                Admin user = new Admin(split[0], split[1], split[2], split[3], split[4], split[5], DateTime.ParseExact(split[6], "dd/MM/yyyy", null), bool.Parse(split[7]));
                Data.users.Add(split[0], user);
            }

            sr.Close();
            stream.Close();
        }

        public void LoadManagers()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/managers.txt");
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while (!String.IsNullOrEmpty(line = sr.ReadLine()))
            {
                string[] split = line.Split(';');
                Manager user = new Manager(split[0], split[1], split[2], split[3], split[4], split[5], DateTime.ParseExact(split[6], "dd/MM/yyyy", null), bool.Parse(split[7]));
                Data.users.Add(split[0], user);
            }

            sr.Close();
            stream.Close();
        }

        public void LoadTourists()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/tourists.txt");
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while (!String.IsNullOrEmpty(line = sr.ReadLine()))
            {
                string[] split = line.Split(';');
                Tourist user = new Tourist(split[0], split[1], split[2], split[3], split[4], split[5], DateTime.ParseExact(split[6], "dd/MM/yyyy", null), bool.Parse(split[7]), Int32.Parse(split[8]));
                Data.users.Add(split[0], user);
            }

            sr.Close();
            stream.Close();
        }
        #endregion

        #region LOAD ARRANGEMENTS
        public void LoadArrangements()
        {
            // LOAD ALL ARRANGEMENTS
            string path = HostingEnvironment.MapPath("~/App_Data/arrangements.txt");
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while (!String.IsNullOrEmpty(line = sr.ReadLine()))
            {
                string[] split = line.Split(';');
                Enum.TryParse(split[2], out ArrangementType aType);
                Enum.TryParse(split[3], out TransportationType tType);
                Arrangement arrangement = new Arrangement(Int32.Parse(split[0]), split[1], aType, tType, split[4], 
                    DateTime.ParseExact(split[5], "dd/MM/yyyy", null), DateTime.ParseExact(split[6], "dd/MM/yyyy", null), 
                    split[7], Int32.Parse(split[8]), split[9], split[10], split[11], split[12], bool.Parse(split[13]));

                Data.arrangements.Add(arrangement);
            }
            sr.Close();
            stream.Close();
        }

        public void LoadMeetingPlaces()
        {
            // FIND ALL MEETING PLACES FOR ALL ARRANGEMENTS AND INSERT THEM IN CORRECT ARRANGEMENT
            string path = HostingEnvironment.MapPath("~/App_Data/meetingPlaces.txt");
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while (!String.IsNullOrEmpty(line = sr.ReadLine()))
            {
                string[] split = line.Split(';');
                MeetingPlace meetingPlace = new MeetingPlace(Int32.Parse(split[0]), split[1], Double.Parse(split[2]), Double.Parse(split[3]));
                Data.meetingPlaces.Add(meetingPlace);
            }
            sr.Close();
            stream.Close();
        }

        public void LoadAccommodations()
        {
            // FIND ALL ACCOMMODATIONS FOR ALL ARRANGEMENTS AND INSERT THEM IN CORRECT ARRANGEMENT
            string path = HostingEnvironment.MapPath("~/App_Data/accommodations.txt");
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while (!String.IsNullOrEmpty(line = sr.ReadLine()))
            {
                string[] split = line.Split(';');
                Enum.TryParse(split[1], out AccommodationType accommodationType);
                Accommodation accommodation = new Accommodation(Int32.Parse(split[0]), accommodationType, split[2], Int32.Parse(split[3]), bool.Parse(split[4]), bool.Parse(split[5]), bool.Parse(split[6]), bool.Parse(split[7]), bool.Parse(split[8]));
                Data.accommodations.Add(accommodation);
            }
            sr.Close();
            stream.Close();
        }

        public void LoadAccommodaionUnits()
        {
            // FIND ALL ACCOMMODATION UNITS FOR ALL ARRANGEMENTS AND INSERT THEM IN CORRECT ARRANGEMENT
            string path = HostingEnvironment.MapPath("~/App_Data/aunits.txt");
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while (!String.IsNullOrEmpty(line = sr.ReadLine()))
            {
                string[] split = line.Split(';');
                AccommodationUnit accommodationUnit = new AccommodationUnit(Int32.Parse(split[0]), Int32.Parse(split[1]), Int32.Parse(split[2]), bool.Parse(split[3]), double.Parse(split[4]), bool.Parse(split[5]), bool.Parse(split[6]));
                Data.accommodationUnits.Add(accommodationUnit);
            }
            sr.Close();
            stream.Close();
        }

        public void LoadComments()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/comments.txt");
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while (!String.IsNullOrEmpty(line = sr.ReadLine()))
            {
                string[] split = line.Split(';');
                int id = Int32.Parse(split[0]);
                string username = split[1];
                int arrangementId = Int32.Parse(split[2]);
                string commentText = split[3];
                int grade = Int32.Parse(split[4]);
                bool approved = bool.Parse(split[5]);

                Tourist tourist = (Tourist)Data.users[username];
                Arrangement arrangement = Data.arrangements.Find(item => item.Id == arrangementId);

                Comment comment = new Comment(id, tourist, arrangement, commentText, grade);
                comment.Approved = approved;
                Data.comments.Add(comment);
            }
            sr.Close();
            stream.Close();
        }

        #endregion

        public void FormatData()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/allocation.txt");
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while (!String.IsNullOrEmpty(line = sr.ReadLine()))
            {
                string[] split = line.Split(';');
                Placement placement = new Placement(Int32.Parse(split[0]), Int32.Parse(split[1]), Int32.Parse(split[2]));
                Data.placements.Add(placement);
            }
            sr.Close();
            stream.Close();
            
            // Place loaded units in correct accommodation
            foreach (Accommodation accommodation in Data.accommodations)
            {
                foreach(AccommodationUnit accommodationUnit in Data.accommodationUnits)
                {
                    if(accommodation.Id == accommodationUnit.Id)
                    {
                        accommodation.AccommodationUnits.Add(accommodationUnit);
                    }
                }
            }

            foreach (var placement in Data.placements)
            {
                Data.arrangements.Find(x => x.Id == placement.ArrangementID).Accommodation = Data.accommodations.Find(x => x.Id == placement.AccommodationID);
                Data.arrangements.Find(x => x.Id == placement.ArrangementID).MeetingPlace = Data.meetingPlaces.Find(x => x.Id == placement.MeetingPlaceID);
            }

            foreach(Arrangement arrangement in Data.arrangements)
            {
                foreach(User user in Data.users.Values)
                {
                    if(arrangement.CreatedBy == user.Username)
                    {
                        ((Manager)user).CreatedArrangements.Add(arrangement);
                    }
                }
            }
        }

        // Pomocna klasa 
        public class Placement
        {
            private int arrangementID;
            private int meetingPlaceID;
            private int accommodationID;

            public Placement(int arrangementID, int meetingPlaceID, int accommodationID)
            {
                this.ArrangementID = arrangementID;
                this.MeetingPlaceID = meetingPlaceID;
                this.AccommodationID = accommodationID;
            }

            public int ArrangementID { get => arrangementID; set => arrangementID = value; }
            public int MeetingPlaceID { get => meetingPlaceID; set => meetingPlaceID = value; }
            public int AccommodationID { get => accommodationID; set => accommodationID = value; }

            public override string ToString()
            {
                return String.Format("{0};{1};{2}", ArrangementID, MeetingPlaceID, AccommodationID);
            }
        }

        public void LoadReservations()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/reservations.txt");
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while (!String.IsNullOrEmpty(line = sr.ReadLine()))
            {
                string[] split = line.Split(';');
                Enum.TryParse(split[2], out Status status);
                Arrangement arrangement = null;
                AccommodationUnit accommodationUnit = null;
                foreach (var item in Data.arrangements)
                {
                    if (item.Id == Int32.Parse(split[3]))
                        arrangement = item;
                }

                foreach (var item in Data.accommodationUnits)
                {
                    if (item.UnitID == Int32.Parse(split[4]))
                        accommodationUnit = item;
                }

                Reservation reservation = new Reservation(split[0], ((Tourist)Data.users[split[1]]), status, arrangement, accommodationUnit);
                foreach(User user in Data.users.Values)
                {
                    if(user.Username == reservation.Tourist.Username)
                    {
                        ((Tourist)user).ReservedArrangements.Add(reservation.Arrangement);
                    }
                }
                Data.reservations.Add(reservation.Id, reservation);
            }

            sr.Close();
            stream.Close();
        }
    }
}
