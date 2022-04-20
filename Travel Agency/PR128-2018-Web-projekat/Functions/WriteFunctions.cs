using PR128_2018_Web_projekat.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace PR128_2018_Web_projekat.Functions
{
    public class WriteFunctions
    {
        // Add only one new Tourist to txt file
        public void WriteNewTourist(User user)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/tourists.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            sw.WriteLine(user);

            sw.Close();
            stream.Close();
        }

        // Add only one new Manager to txt file
        public void WriteNewManager(User user)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/managers.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            sw.WriteLine(user);

            sw.Close();
            stream.Close();
        }

        // Add new Reservaion to txt file
        public void WriteNewReservation(Reservation reservation)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/reservations.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            sw.WriteLine(reservation);

            sw.Close();
            stream.Close();
        }

        // Add new MeetingPlace to txt file
        public void WriteNewMeetingPlace(MeetingPlace meetingPlace)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/meetingPlaces.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            sw.WriteLine(meetingPlace);

            sw.Close();
            stream.Close();
        }

        // Add new Arrangement to txt file
        public void WriteNewArrangement(Arrangement arrangement)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/arrangements.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            sw.WriteLine(arrangement);

            sw.Close();
            stream.Close();
        }

        // Add new Accommodation to txt file
        public void WriteNewAccommodation(Accommodation accommodation)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/accommodations.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            sw.WriteLine(accommodation);

            sw.Close();
            stream.Close();
        }

        // Add new Comment to txt file
        public void WriteNewComment(Comment comment)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/comments.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            sw.WriteLine(comment);

            sw.Close();
            stream.Close();
        }

        // Overwrite all Comments to txt file
        public void OverwriteComments()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/comments.txt");
            FileStream stream = new FileStream(path, FileMode.Truncate);
            StreamWriter sw = new StreamWriter(stream);

            foreach (var comment in Data.comments)
            {
                sw.WriteLine(comment);
            }

            sw.Close();
            stream.Close();
        }

        // Overwrite all Tourists to txt file
        public void OverwriteTourists()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/tourists.txt");
            FileStream stream = new FileStream(path, FileMode.Truncate);
            StreamWriter sw = new StreamWriter(stream);

            foreach (var item in Data.users.Values)
            {
                if (item.Role == Role.TOURIST)
                    sw.WriteLine((Tourist)item);
            }

            sw.Close();
            stream.Close();
        }

        // Overwrite all Tourists to txt file
        public void OverwriteAdmins()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/admins.txt");
            FileStream stream = new FileStream(path, FileMode.Truncate);
            StreamWriter sw = new StreamWriter(stream);

            foreach (var item in Data.users.Values)
            {
                if (item.Role == Role.ADMIN)
                    sw.WriteLine(item);
            }

            sw.Close();
            stream.Close();
        }

        // Overwrite all Tourists to txt file
        public void OverwriteManagers()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/managers.txt");
            FileStream stream = new FileStream(path, FileMode.Truncate);
            StreamWriter sw = new StreamWriter(stream);

            foreach (var item in Data.users.Values)
            {
                if (item.Role == Role.MANAGER)
                    sw.WriteLine(item);
            }

            sw.Close();
            stream.Close();
        }

        // Overwrite all Accommodation Units to txt file
        public void OverwriteAccommodationUnits()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/aunits.txt");
            FileStream stream = new FileStream(path, FileMode.Truncate);
            StreamWriter sw = new StreamWriter(stream);

            foreach (var item in Data.accommodations)
            {
                foreach(var unit in item.AccommodationUnits)
                    sw.WriteLine(unit);
            }

            sw.Close();
            stream.Close();
        }

        public void OverwriteReservations()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/reservations.txt");
            FileStream stream = new FileStream(path, FileMode.Truncate);
            StreamWriter sw = new StreamWriter(stream);

            foreach (var item in Data.reservations.Values)
            {
                sw.WriteLine(item);
            }

            sw.Close();
            stream.Close();
        }

        public void OverwriteArrangements()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/arrangements.txt");
            FileStream stream = new FileStream(path, FileMode.Truncate);
            StreamWriter sw = new StreamWriter(stream);

            foreach (var item in Data.arrangements)
            {
                sw.WriteLine(item);
            }

            sw.Close();
            stream.Close();
        }

        public void OverwriteAccommodations()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/accommodations.txt");
            FileStream stream = new FileStream(path, FileMode.Truncate);
            StreamWriter sw = new StreamWriter(stream);

            foreach(var item in Data.accommodations)
            {
                sw.WriteLine(item);
            }

            sw.Close();
            stream.Close();
        }

        public void OverwriteMeetingPlaces()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/meetingPlaces.txt");
            FileStream stream = new FileStream(path, FileMode.Truncate);
            StreamWriter sw = new StreamWriter(stream);

            foreach (var item in Data.meetingPlaces)
            {
                sw.WriteLine(item);
            }

            sw.Close();
            stream.Close();
        }

        public void OverwriteAllocation()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/allocation.txt");
            FileStream stream = new FileStream(path, FileMode.Truncate);
            StreamWriter sw = new StreamWriter(stream);

            foreach (var item in Data.placements)
            {
                sw.WriteLine(item);
            }

            sw.Close();
            stream.Close();
        }
    }
}