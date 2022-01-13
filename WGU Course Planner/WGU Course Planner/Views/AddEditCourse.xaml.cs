using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGU_Course_Planner.Classes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGU_Course_Planner
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEditCourse : ContentPage
    {
        //add/edit courses with datepicker and error handling

        private SQLiteConnection _conn;
        private bool _edit;
        private Term _term;
        private Course _course;
        private ContentPage _parent;

        // if edit is true then inObject has to be Course and it has to be Term otherwise 
        public AddEditCourse(ContentPage parent, object inObject, bool edit)
        {
            InitializeComponent();
            if (edit)
            {
                _term = null;
                _course = (Course)inObject;
                courseName.Text = _course.CourseName;
                instructorName.Text = _course.InstructorName;
                instructorEmail.Text = _course.InstructorEmail;
                instructorPhone.Text = _course.InstructorPhone;
                startDate.Date = _course.Start;
                endDate.Date = _course.End;
                courseStatus.SelectedItem = _course.Status;
                notes.Text = _course.Notes;
                enableNotification.On = _course.Notification == 1;

            }
            else
            {
                _term = (Term)inObject;
                _course = null;
                startDate.Date = DateTime.Now;
                endDate.Date = DateTime.Now;
            }
            _parent = parent;
            _edit = edit;
            _conn = DependencyService.Get<ISQLiteDb>().GetConnection();
            pageTitle.Text = edit ? "Edit course" : "Add course";

        }
        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(courseName.Text) || string.IsNullOrEmpty(instructorName.Text) || string.IsNullOrEmpty(instructorPhone.Text) || string.IsNullOrEmpty(instructorEmail.Text))
            {
                await DisplayAlert("Error.", "Please fill all fields.", "Ok");
                return;
            }
            if (startDate.Date > endDate.Date)
            {
                await DisplayAlert("Error.", "End date cannoit be before start date.", "Ok");
                return;
            }
            if (!_edit)
            {
                _course = new Course();
                _course.Term = _term.Id;
            }
            _course.CourseName = courseName.Text;
            _course.InstructorName = instructorName.Text;
            _course.InstructorEmail = instructorEmail.Text;
            _course.InstructorPhone = instructorPhone.Text;
            _course.Start = startDate.Date;
            _course.End = endDate.Date;
            _course.Status = (string)courseStatus.SelectedItem;
            _course.Notes = notes.Text;
            _course.Notification = enableNotification.On ? 1 : 0;

            if (_edit)
            {
                _conn.Update(_course);
                ((CoursePage)_parent).UpdateData(_course);
            }
            else
            {
                _conn.Insert(_course);
                ((TermPage)_parent)._courseList.Add(_course);
            }
            await Navigation.PopModalAsync();
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}