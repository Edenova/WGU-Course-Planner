using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGU_Course_Planner.Classes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGU_Course_Planner
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoursePage : ContentPage
    {
        //course page where you can add, edit, and delete courses

        private SQLiteConnection _conn;
        private Course _course;
        private TermPage _parent;
        public CoursePage(TermPage parent, Course course)
        {
            InitializeComponent();
            navTitle.Text = course.CourseName;
            _conn = DependencyService.Get<ISQLiteDb>().GetConnection();
            _course = course;
            _parent = parent;
            UpdateData(course);
        }

        public void UpdateData(Course course)
        {
            courseStatus.Text = course.Status;
            startDate.Text = course.Start.ToString("MM/dd/yyyy", DateTimeFormatInfo.InvariantInfo);
            endDate.Text = course.End.ToString("MM/dd/yyyy", DateTimeFormatInfo.InvariantInfo);
            instructorName.Text = course.InstructorName;
            instructorPhone.Text = course.InstructorPhone;
            instructorEmail.Text = course.InstructorEmail;
            notification.Text = course.Notification == 1 ? "Yes" : "No";
            notes.Text = course.Notes;
            _parent.UpdateList();
        }

        private async void AssessmentsButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AssessmentListPage(_course));
        }

        private async void EditCourseButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddEditCourse(this, _course, true));
        }

        private void ShareNotesButton_Clicked(object sender, EventArgs e)
        {
        }

        private void DeleteCourseButton_Clicked(object sender, EventArgs e)
        {
            var confirmation = DisplayAlert("Alert", "Are you sure you want to drop this course?", "Yes", "No").Result;
            if (confirmation)
            {
                _conn.Delete(_course);
                Navigation.PopAsync();
            }
        }
    }
}