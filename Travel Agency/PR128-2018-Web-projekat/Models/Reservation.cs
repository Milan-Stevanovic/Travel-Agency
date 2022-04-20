using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR128_2018_Web_projekat.Models
{
    public enum Status { ACTIVE, CANCELED }

    public class Reservation
    {
        // id -> 15 characters unique
        private string id;
        private Tourist tourist;
        private Status status;
        private Arrangement arrangement;
        private AccommodationUnit accommodationUnit;

        // CONSTRUCTORS
        public Reservation(string id, Tourist tourist, Status status, Arrangement arrangement, AccommodationUnit accommodationUnit)
        {
            this.id = id;
            this.tourist = tourist;
            this.status = status;
            this.arrangement = arrangement;
            this.accommodationUnit = accommodationUnit;
        }

        // GETTERS AND SETTERS
        public string Id { get => id; set => id = value; }
        public Tourist Tourist { get => tourist; set => tourist = value; }
        public Status Status { get => status; set => status = value; }
        public Arrangement Arrangement { get => arrangement; set => arrangement = value; }
        public AccommodationUnit AccommodationUnit { get => accommodationUnit; set => accommodationUnit = value; }

        public override string ToString()
        {
            return String.Format("{0};{1};{2};{3};{4}", Id, Tourist.Username, Status, Arrangement.Id, AccommodationUnit.UnitID);
        }
    }
}