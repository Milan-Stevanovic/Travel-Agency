using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR128_2018_Web_projekat.Models
{
    public enum AccommodationType { HOTEL, MOTEL, VILLA }

    public class Accommodation
    {
        private int id;
        private AccommodationType accommodationType;
        private string name;
        private int hotelStars;
        private bool pool;
        private bool spaCenter;
        private bool disabledPeopleAdapted;
        private bool wifi;
        private List<AccommodationUnit> accommodationUnits;
        private bool deleted;

        // CONSTRUCTORS
        public Accommodation() { }

        public Accommodation(int id, AccommodationType accommodationType, 
            string name, int hotelStars, bool pool, bool spaCenter, 
            bool disabledPeopleAdapted, bool wifi, bool deleted)
        {
            this.Id = id;
            this.AccommodationType = accommodationType;
            this.Name = name;
            this.HotelStars = hotelStars;
            this.Pool = pool;
            this.SpaCenter = spaCenter;
            this.DisabledPeopleAdapted = disabledPeopleAdapted;
            this.Wifi = wifi;
            this.AccommodationUnits = new List<AccommodationUnit>();
            this.Deleted = deleted;
        }


        // GETTERS AND SETTERS
        public int Id { get => id; set => id = value; }
        public AccommodationType AccommodationType { get => accommodationType; set => accommodationType = value; }
        public string Name { get => name; set => name = value; }
        public int HotelStars { get => hotelStars; set => hotelStars = value; }
        public bool Pool { get => pool; set => pool = value; }
        public bool SpaCenter { get => spaCenter; set => spaCenter = value; }
        public bool DisabledPeopleAdapted { get => disabledPeopleAdapted; set => disabledPeopleAdapted = value; }
        public bool Wifi { get => wifi; set => wifi = value; }
        public List<AccommodationUnit> AccommodationUnits { get => accommodationUnits; set => accommodationUnits = value; }
        public bool Deleted { get => deleted; set => deleted = value; }

        public override string ToString()
        {
            return String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}", Id, AccommodationType, Name, HotelStars, Pool, SpaCenter, DisabledPeopleAdapted, Wifi, Deleted);
        }
    }
}