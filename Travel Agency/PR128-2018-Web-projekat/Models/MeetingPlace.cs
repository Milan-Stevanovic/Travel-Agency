using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR128_2018_Web_projekat.Models
{
    public class MeetingPlace
    {
        private int id;
        private string address;
        private double longitude;
        private double latitude;

        // CONSTRUCTORS
        public MeetingPlace(int id, string address, double longitude, double latitude)
        {
            this.Id = id;
            this.Address = address;
            this.Longitude = longitude;
            this.Latitude = latitude;
        }

        // GETTERS AND SETTERS
        public int Id { get => id; set => id = value; }
        public string Address { get => address; set => address = value; }
        public double Longitude { get => longitude; set => longitude = value; }
        public double Latitude { get => latitude; set => latitude = value; }

        public override string ToString()
        {
            return String.Format("{0};{1};{2};{3}", Id, Address, Longitude, Latitude);
        }
    }
}