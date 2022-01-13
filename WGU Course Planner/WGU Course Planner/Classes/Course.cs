using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace WGU_Course_Planner.Classes
{
    // class for course data
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Term { get; set; }
        public string CourseName { get; set; }
        public string Status { get; set; }
        public string InstructorName { get; set; }
        public string InstructorPhone { get; set; }
        public string InstructorEmail { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Notes { get; set; }
        public int Notification { get; set; }
        

    }
}
