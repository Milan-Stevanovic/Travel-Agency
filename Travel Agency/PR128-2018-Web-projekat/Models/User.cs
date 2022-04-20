using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR128_2018_Web_projekat.Models
{
    public enum Role { ADMIN, MANAGER, TOURIST }

    public abstract class User
    {
        private string username;
        private string password;
        private string name;
        private string lastname;
        private string gender;
        private string email;
        private DateTime dateOfBirth;
        private Role role;
        private bool blocked;

        

        // CONSTRUCTORS
        public User() { }

        protected User(string username, string password, string name, string lastname, string gender, string email, DateTime dateOfBirth, Role role, bool blocked)
        {
            this.Username = username;
            this.Password = password;
            this.Name = name;
            this.Lastname = lastname;
            this.Gender = gender;
            this.Email = email;
            this.DateOfBirth = dateOfBirth;
            this.Role = role;
            this.Blocked = blocked;
        }

        protected User(string username, string password, string name, string lastname, string gender, string email, DateTime dateOfBirth, bool blocked)
        {
            this.Username = username;
            this.Password = password;
            this.Name = name;
            this.Lastname = lastname;
            this.Gender = gender;
            this.Email = email;
            this.DateOfBirth = dateOfBirth;
            this.Blocked = blocked;
        }

        // GETTERS AND SETTERS
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Name { get => name; set => name = value; }
        public string Lastname { get => lastname; set => lastname = value; }
        public string Gender { get => gender; set => gender = value; }
        public string Email { get => email; set => email = value; }
        public DateTime DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }
        public Role Role { get => role; set => role = value; }
        public bool Blocked { get => blocked; set => blocked = value; }

        public override string ToString()
        {
            return String.Format("{0};{1};{2};{3};{4};{5};{6};{7}", Username, Password, Name, Lastname, Gender, Email, DateOfBirth.ToString("dd/MM/yyyy"), Blocked);
        }
    }
}