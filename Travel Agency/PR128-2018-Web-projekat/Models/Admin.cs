using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR128_2018_Web_projekat.Models
{
    public class Admin : User
    {
        // CONSTRUCTORS 
        public Admin(string username, string password, string name, string lastname, string gender, string email, DateTime dateOfBirth, bool blocked) 
            : base(username, password, name, lastname, gender, email, dateOfBirth, blocked)
        {
            this.Role = Role.ADMIN;
        }

        public Admin() { }
    }
}