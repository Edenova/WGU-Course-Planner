using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using WGU_Course_Planner.Classes;
using Xamarin.Forms;

namespace WGU_Course_Planner
{
    public class Globals
    {
        public static List<string> GetListOfMissingAssessments(int CourseId)
        {
            List<string> res = new List<string> { "Objective", "Performance" };
            SQLiteConnection _conn = DependencyService.Get<ISQLiteDb>().GetConnection();
            var assessmentList = _conn.Query<Assessment>($"Select * From Assessment Where Course = '{CourseId}'");
            foreach (var item in assessmentList)
            {
                res.Remove(item.Type);
            }
            return res;
        }
    }
}
