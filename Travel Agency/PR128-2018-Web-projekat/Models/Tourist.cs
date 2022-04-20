using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR128_2018_Web_projekat.Models
{
    public class Tourist : User
    {
        private List<Arrangement> reservedArrangements;
        private int canceledReservations;


        // CONSTRUCTORS
        public Tourist(string username, string password, string name, string lastname, string gender, string email, DateTime dateOfBirth, bool blocked, int canceledReservations) 
            : base(username, password, name, lastname, gender, email, dateOfBirth, blocked)
        {
            this.Role = Role.TOURIST;
            this.ReservedArrangements = new List<Arrangement>();
            this.CanceledReservations = canceledReservations;
        }

        public Tourist() { }

        // GETTERS AND SETTERS
        public List<Arrangement> ReservedArrangements { get => reservedArrangements; set => reservedArrangements = value; }
        public int CanceledReservations { get => canceledReservations; set => canceledReservations = value; }

        public override string ToString()
        {
            return String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}", Username, Password, Name, Lastname, Gender, Email, DateOfBirth.ToString("dd/MM/yyyy"), Blocked, CanceledReservations);
        }
    }
}