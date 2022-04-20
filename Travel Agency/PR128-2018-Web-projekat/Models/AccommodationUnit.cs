using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR128_2018_Web_projekat.Models
{
    public class AccommodationUnit
    {
        private int id;
        private int unitID;
        private int numberOfGuestsAllowed;
        private bool petsAllowed;
        private double price;
        private bool occupied;
        private bool deleted;

        // CONSTRUCTORS
        public AccommodationUnit(int id, int unitID, int numberOfGuestsAllowed, bool petsAllowed, double price, bool occupied, bool deleted)
        {
            this.Id = id;
            this.UnitID = unitID;
            this.NumberOfGuestsAllowed = numberOfGuestsAllowed;
            this.PetsAllowed = petsAllowed;
            this.Price = price;
            this.Occupied = occupied;
            this.Deleted = deleted;
        }

        // GETTERS AND SETTERS
        public int Id { get => id; set => id = value; }
        public int NumberOfGuestsAllowed { get => numberOfGuestsAllowed; set => numberOfGuestsAllowed = value; }
        public bool PetsAllowed { get => petsAllowed; set => petsAllowed = value; }
        public double Price { get => price; set => price = value; }
        public bool Occupied { get => occupied; set => occupied = value; }
        public int UnitID { get => unitID; set => unitID = value; }
        public bool Deleted { get => deleted; set => deleted = value; }

        public override string ToString()
        {
            return String.Format("{0};{1};{2};{3};{4};{5};{6}", Id, UnitID, NumberOfGuestsAllowed, PetsAllowed, Price, Occupied, Deleted);
        }
    }
}