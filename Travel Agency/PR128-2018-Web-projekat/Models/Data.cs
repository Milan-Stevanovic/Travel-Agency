using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static PR128_2018_Web_projekat.MvcApplication;

namespace PR128_2018_Web_projekat.Models
{
    public class Data
    {
        public static List<Placement> placements = new List<Placement>();

        public static Dictionary<string, User> users = new Dictionary<string, User>();
        public static List<Arrangement> arrangements = new List<Arrangement>();
        public static Dictionary<string, Reservation> reservations = new Dictionary<string, Reservation>();
        public static List<Comment> comments = new List<Comment>();

        public static List<Accommodation> accommodations = new List<Accommodation>();
        public static List<AccommodationUnit> accommodationUnits = new List<AccommodationUnit>();
        public static List<MeetingPlace> meetingPlaces = new List<MeetingPlace>();
    }
}