using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR128_2018_Web_projekat.Models
{
    public class Comment
    {
        private int id;
        private Tourist tourist;
        private Arrangement arrangement;
        private string commentText;
        // grade [1 - 5]
        private int grade;
        private bool approved;

        // CONSTRUCTORS
        public Comment(int id, Tourist tourist, Arrangement arrangement, string commentText, int grade)
        {
            this.Id = id;
            this.Tourist = tourist;
            this.Arrangement = arrangement;
            this.CommentText = commentText;
            this.Grade = grade;
            this.Approved = false;

        }

        // GETTERS AND SETTERS
        public int Id { get => id; set => id = value; }
        public Tourist Tourist { get => tourist; set => tourist = value; }
        public Arrangement Arrangement { get => arrangement; set => arrangement = value; }
        public string CommentText { get => commentText; set => commentText = value; }
        public int Grade { get => grade; set => grade = value; }
        public bool Approved { get => approved; set => approved = value; }

        public override string ToString()
        {
            return String.Format("{0};{1};{2};{3};{4};{5}", Id, Tourist.Username, Arrangement.Id, CommentText, Grade, Approved);
        }
    }
}