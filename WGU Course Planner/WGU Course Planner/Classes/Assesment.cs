using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace WGU_Course_Planner.Classes
{
    //Class for Assesment data 
    class Assesment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        
        public string Course { get; set; }
        public string Type { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Notification { get; set; }

    }
}
