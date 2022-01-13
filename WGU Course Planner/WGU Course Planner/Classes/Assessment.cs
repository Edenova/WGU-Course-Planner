using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace WGU_Course_Planner.Classes
{
    // class for second assesment type data
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        
        public int Course { get; set; }
        public string Type { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Notification { get; set; }

    }
}
