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
    public partial class TermPage : ContentPage
    {
        //term page where you can add, edit, and delete term

        private SQLiteConnection _conn;
        public ObservableCollection<Course> _courseList;
        private Term _term;
        private MainPage _parent;
        public TermPage(MainPage parent, Term term)
        {
            InitializeComponent();
            Title = term.Title;
            _term = term;
            _parent = parent;
            _conn = DependencyService.Get<ISQLiteDb>().GetConnection();
            CourseListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(Course_Clicked);
            UpdateData(term);
        }

        public void UpdateData(Term term)
        {
            navTitle.Text = term.Title;
            DateRange.Text = $"{term.Start.ToString("MM/dd/yyyy", DateTimeFormatInfo.InvariantInfo)} - {term.End.ToString("MM/dd/yyyy", DateTimeFormatInfo.InvariantInfo)}";
            UpdateList();
            _parent.UpdateList();
        }

        public void UpdateList()
        {
            var courseList = _conn.Query<Course>($"Select * From Course Where Term = '{_term.Id}'");
            _courseList = new ObservableCollection<Course>(courseList);
            CourseListView.ItemsSource = _courseList;
            addCourseButton.IsEnabled = courseList.Count < 6;
        }

        private async void Course_Clicked(object sender, ItemTappedEventArgs e)
        {
            Course course = (Course)e.Item;
            await Navigation.PushAsync(new CoursePage(this, course));
        }

        private void AddCourseButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new AddEditCourse(this, _term, false));
        }

        private void EditTermButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new AddEditTerm(this, _term, true));
        }

        private void DeleteTermButton_Clicked(object sender, EventArgs e)
        {
            var confirmation = DisplayAlert("Alert", "Are you sure you want to drop this term?", "Yes", "No").Result;
            if (confirmation)
            {
                _conn.Delete(_term);
                _parent.UpdateList();
                Navigation.PopAsync();
            }
        }
    }
}