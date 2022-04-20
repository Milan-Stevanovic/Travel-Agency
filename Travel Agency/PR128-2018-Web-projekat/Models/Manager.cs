using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR128_2018_Web_projekat.Models
{
    public class Manager : User
    {
        private List<Arrangement> createdArrangements;

        // CONSTRUCTORS
        public Manager(string username, string password, string name, string lastname, string gender, string email, DateTime dateOfBirth, bool blocked) 
            : base(username, password, name, lastname, gender, email, dateOfBirth, blocked)
        {
            this.Role = Role.MANAGER;
            this.CreatedArrangements = new List<Arrangement>();
        }

        public Manager() { }

        // GETTERS AND SETTERS
        public List<Arrangement> CreatedArrangements { get => createdArrangements; set => createdArrangements = value; }
    }
}