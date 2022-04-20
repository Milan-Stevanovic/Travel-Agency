using PR128_2018_Web_projekat.Functions;
using PR128_2018_Web_projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PR128_2018_Web_projekat.Controllers
{
    public class AccountController : Controller
    {
        private WriteFunctions write = new WriteFunctions();

        // GET: Login Page
        public ActionResult Login()
        {
            return View();
        }

        // GET: Register Page
        public ActionResult Register()
        {
            return View();
        }

        // GET: Edit Profile Page
        public ActionResult ProfilePage()
        {
            if (Session["USER"] == null)
                return RedirectToAction("Index", "Home");
            else
                return View();

        }

        // POST : Login
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.emptyError = "[ERROR] You must fill all fields!";
                return View();
            }

            if (Data.users.ContainsKey(username) && Data.users[username].Password.Equals(password))
            {
                if (!Data.users[username].Blocked)
                {
                    Session["USER"] = Data.users[username];
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "[ERROR] Account blocked!";
                    return View();
                }
            }
            else
            {
                ViewBag.error = "[ERROR] Wrong login info!";
                return View();
            }
        }

        // POST : Register
        [HttpPost]
        public ActionResult Register(string username, string password, string name, string lastname, string gender, string email, string dateOfBirth)
        {
            bool valid = true;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(lastname) ||
                string.IsNullOrEmpty(gender) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(dateOfBirth))
            {
                ViewBag.emptyError = "[ERROR] You must fill all fields!";
                return View();
            }

            if (Data.users.ContainsKey(username))
            {
                ViewBag.usernameExistsError = "Username already in use!";
                valid = false;
            }
            foreach (User u in Data.users.Values)
            {
                if (u.Email.Equals(email))
                {
                    ViewBag.emailError = "Email already in use!";
                    valid = false;
                }
            }

            if (valid)
            {
                string[] dateSplit = dateOfBirth.Split('-');
                string date = dateSplit[2] + "/" + dateSplit[1] + "/" + dateSplit[0];
                Tourist tourist = new Tourist(username, password, name, lastname, gender, email, DateTime.ParseExact(date, "dd/MM/yyyy", null), false, 0);

                write.WriteNewTourist(tourist);

                Data.users.Add(username, tourist);
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }



        // EDIT PROFILE SAVE
        public ActionResult EditProfie(string password, string name, string lastname, string gender, string email, string dateOfBirth)
        {
            User thisUser = (User)Session["User"];

            Data.users.Remove(thisUser.Username);

            bool valid = true;
            foreach (var item in Data.users.Values)
            {
                if(item.Email.ToLower().Equals(email.ToLower()))
                {
                    ViewBag.usernameError = "Email already in use!";
                    valid = false;
                }
            }

            if(valid)
            {
                User user = null;
                string[] dateSplit = dateOfBirth.Split('-');
                string date = dateSplit[2] + "/" + dateSplit[1] + "/" + dateSplit[0];
                switch(thisUser.Role)
                {
                    case Role.ADMIN:
                        user = new Admin(thisUser.Username, password, name, lastname, gender, email, DateTime.ParseExact(date, "dd/MM/yyyy", null), false);
                        break;
                    case Role.MANAGER:
                        user = new Manager(thisUser.Username, password, name, lastname, gender, email, DateTime.ParseExact(date, "dd/MM/yyyy", null), false);
                        break;
                    case Role.TOURIST:
                        user = new Tourist(thisUser.Username, password, name, lastname, gender, email, DateTime.ParseExact(date, "dd/MM/yyyy", null), false, ((Tourist)thisUser).CanceledReservations);
                        break;
                }

                Data.users.Add(user.Username, user);

                switch (thisUser.Role)
                {
                    case Role.ADMIN:
                        write.OverwriteAdmins();
                        break;
                    case Role.MANAGER:
                        write.OverwriteManagers();
                        break;
                    case Role.TOURIST:
                        write.OverwriteTourists();
                        break;
                }
                Session["USER"] = Data.users[thisUser.Username];
            }

            return View("ProfilePage");
        }
    }
}