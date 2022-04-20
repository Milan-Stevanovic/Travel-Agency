using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR128_2018_Web_projekat.Models
{
    public enum ArrangementType { BED_AND_BREAKFAST, HALF_BOARD, FULL_BOARD, ALL_INCLUSIVE, APARTMENT_RENTAL }
    public enum TransportationType { BUS, PLANE, BUS_AND_PLANE, INDIVIDUAL, OTHER }
    
    public class Arrangement
    {
        private int id;
        private string name;
        private ArrangementType arrangementType;
        private TransportationType transportationType;
        private string location;
        private DateTime startDate;
        private DateTime endDate;
        private MeetingPlace meetingPlace;
        private string meetingTime;
        private int maxNumberOfPassengers;
        private string description;
        private string travelProgram;
        private string image;
        private Accommodation accommodation;
        private string createdBy;
        private bool deleted;
        
        // CONSTRUCTORS
        public Arrangement() { }

        public Arrangement(int id, string name, ArrangementType arrangementType, 
            TransportationType transportationType, string location,
            DateTime startDate, DateTime endDate, MeetingPlace meetingPlace, 
            string meetingTime, int maxNumberOfPassengers, string description, 
            string travelProgram, string image, Accommodation accommodation, bool deleted)
        {
            this.Id = id;
            this.Name = name;
            this.ArrangementType = arrangementType;
            this.TransportationType = transportationType;
            this.Location = location;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.MeetingPlace = meetingPlace;
            this.MeetingTime = meetingTime;
            this.MaxNumberOfPassengers = maxNumberOfPassengers;
            this.Description = description;
            this.TravelProgram = travelProgram;
            this.Image = image;
            this.Accommodation = accommodation;
            this.Deleted = deleted;
        }

        public Arrangement(int id, string name, ArrangementType arrangementType,
            TransportationType transportationType, string location,
            DateTime startDate, DateTime endDate,
            string meetingTime, int maxNumberOfPassengers, string description,
            string travelProgram, string image, string createdBy, bool deleted)
        {
            this.Id = id;
            this.Name = name;
            this.ArrangementType = arrangementType;
            this.TransportationType = transportationType;
            this.Location = location;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.MeetingTime = meetingTime;
            this.MaxNumberOfPassengers = maxNumberOfPassengers;
            this.Description = description;
            this.TravelProgram = travelProgram;
            this.Image = image;
            this.CreatedBy = createdBy;
            this.Deleted = deleted;
        }


        // GETTERS AND SETTERS
        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public ArrangementType ArrangementType { get => arrangementType; set => arrangementType = value; }
        public TransportationType TransportationType { get => transportationType; set => transportationType = value; }
        public string Location { get => location; set => location = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
        public MeetingPlace MeetingPlace { get => meetingPlace; set => meetingPlace = value; }
        public string MeetingTime { get => meetingTime; set => meetingTime = value; }
        public int MaxNumberOfPassengers { get => maxNumberOfPassengers; set => maxNumberOfPassengers = value; }
        public string Description { get => description; set => description = value; }
        public string TravelProgram { get => travelProgram; set => travelProgram = value; }
        public string Image { get => image; set => image = value; }
        public Accommodation Accommodation { get => accommodation; set => accommodation = value; }
        public string CreatedBy { get => createdBy; set => createdBy = value; }
        public bool Deleted { get => deleted; set => deleted = value; }

        public override string ToString()
        {
            return String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13}",
                Id, Name, ArrangementType, TransportationType, Location, StartDate.ToString("dd/MM/yyyy"), EndDate.ToString("dd/MM/yyyy"),
                MeetingTime, MaxNumberOfPassengers, Description, TravelProgram, Image, CreatedBy, Deleted);
        }
    }
}