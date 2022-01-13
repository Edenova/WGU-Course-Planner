using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace WGU_Course_Planner.Classes
{
    //class for term data
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
