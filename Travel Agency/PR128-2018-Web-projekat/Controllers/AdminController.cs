using PR128_2018_Web_projekat.Functions;
using PR128_2018_Web_projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PR128_2018_Web_projekat.Controllers
{
    public class AdminController : Controller
    {
        WriteFunctions write = new WriteFunctions();
        // GET: Admin
        public ActionResult Index()
        {
            User currentUser = (User)Session["USER"];

            if(currentUser == null || currentUser.Role != Role.ADMIN)
            {
                return RedirectToAction("Index", "Home");
            }
            else
                return View();
        }

        public ActionResult AddManager()
        {
            User currentUser = (User)Session["USER"];

            if (currentUser == null || currentUser.Role != Role.ADMIN)
            {
                return RedirectToAction("Index", "Home");
            }
            else
                return View();
        }

        public ActionResult AllUsers()
        {
            User currentUser = (User)Session["USER"];

            if (currentUser == null || currentUser.Role != Role.ADMIN)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.list = Data.users.Values.ToList();
                return View();
            }
        }

        public ActionResult SuspiciousUsers()
        {
            User currentUser = (User)Session["USER"];
            List<User> suspiciousUsers = new List<User>();
            if (currentUser == null || currentUser.Role != Role.ADMIN)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var item in Data.users.Values)
                {
                    if(item.Role == Role.TOURIST && ((Tourist)item).CanceledReservations >= 2)
                    {
                        suspiciousUsers.Add(item);
                    }
                }

                ViewBag.list = suspiciousUsers;
                return View();
            }
        }

        [HttpPost]
        public ActionResult AddManager(string username, string password, string name, string lastname, string gender, string email, string dateOfBirth)
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
                Manager manager = new Manager(username, password, name, lastname, gender, email, DateTime.ParseExact(date, "dd/MM/yyyy", null), false);

                write.WriteNewManager(manager);

                Data.users.Add(username, manager);
                return RedirectToAction("AllUsers");
            }

            return View();
        }

        public ActionResult BlockUser(string username, string action)
        {
            foreach (var item in Data.users.Values)
            {
                if (item.Username.Equals(username))
                {
                    if(action.Equals("BLOCK"))
                        item.Blocked = true;
                    if(action.Equals("UNBLOCK"))
                        item.Blocked = false;
                    break;
                }
            }

            write.OverwriteTourists();
            return RedirectToAction("SuspiciousUsers");
        }

        #region ALL USERS METHODS
        [HttpPost]
        public ActionResult SearchUsers(string name, string lastname, string role, string action)
        {
            if(action.Equals("Clear Search"))
            {
                return RedirectToAction("AllUsers");
            }

            List<User> searchList = new List<User>();
            Role tempRole = (Role)Enum.Parse(typeof(Role), role);
            foreach (var item in Data.users.Values)
            {
                if(item.Name.ToLower().Contains(name.ToLower()) && item.Lastname.ToLower().Contains(lastname.ToLower()) && 
                    item.Role == tempRole)
                {
                    searchList.Add(item);
                }
            }

            ViewBag.list = searchList;
            return View("AllUsers");
        }

        [HttpPost]
        public ActionResult SortUsers(List<string> list, string sortBy, string order)
        {
            List<User> sortedList = new List<User>();
            if (list == null)
            {
                ViewBag.list = sortedList;
                return View("Index");
            }

            foreach (var item in Data.users.Values)
            {
                foreach (string s in list)
                {
                    if (item.Username == s)
                        sortedList.Add(item);
                }
            }

            switch (sortBy)
            {
                case "Name":
                    if (order.Equals("Ascending"))
                    {
                        sortedList.Sort((x, y) => x.Name.CompareTo(y.Name));
                    }
                    else
                    {
                        sortedList.Sort((x, y) => y.Name.CompareTo(x.Name));
                    }
                    break;
                case "Lastname":
                    if (order.Equals("Ascending"))
                    {
                        sortedList.Sort((x, y) => x.Lastname.CompareTo(y.Lastname));
                    }
                    else
                    {
                        sortedList.Sort((x, y) => y.Lastname.CompareTo(x.Lastname));
                    }
                    break;
                case "Role":
                    if (order.Equals("Ascending"))
                    {
                        sortedList.Sort((x, y) => x.Role.CompareTo(y.Role));
                    }
                    else
                    {
                        sortedList.Sort((x, y) => y.Role.CompareTo(x.Role));
                    }
                    break;
            }

            ViewBag.list = sortedList;
            return View("AllUsers");
        }

        #endregion
    }
}