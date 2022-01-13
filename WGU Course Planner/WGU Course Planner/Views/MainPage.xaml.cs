using Plugin.LocalNotifications;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGU_Course_Planner.Classes;
using Xamarin.Forms;


namespace WGU_Course_Planner
{
    public partial class MainPage : ContentPage

    {
        private SQLiteConnection _conn;
        public ObservableCollection<Term> terms;
        private bool pushNotification = true;

        public MainPage()
        {
            InitializeComponent();
            _conn = DependencyService.Get<ISQLiteDb>().GetConnection();
            termListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(Term_Click);
        }
        //Generate term, courses, and assessments. Generate sample if none are found
        protected override async void OnAppearing()
        {
            _conn.CreateTable<Term>();
            _conn.CreateTable<Course>();
            _conn.CreateTable<Assessment>();

            var termLst = _conn.Table<Term>().ToList();
            var courseLst = _conn.Table<Course>().ToList();
            var assessmentLst = _conn.Table<Assessment>().ToList();

            if (!termLst.Any())
            {
                var thisMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var sampleTerm = new Term
                {
                    Title = "Term 1",
                    Start = thisMonth,
                    End = thisMonth.AddMonths(4).AddDays(-1)
                };
                _conn.Insert(sampleTerm);
                termLst.Add(sampleTerm);

                var sampleCourse = new Course
                {
                    CourseName = "Mobile Application Using C#",
                    Start = thisMonth,
                    End = thisMonth.AddMonths(2).AddDays(-1),
                    Status = "In-Progress",
                    InstructorName = "Eden Piatnichko",
                    InstructorPhone = "720-266-3160",
                    InstructorEmail = "epiatni@wgu.edu",
                    Notification = 1,
                    Notes = "This is a test.",
                    Term = sampleTerm.Id
                };
                _conn.Insert(sampleCourse);

                var sampleAssessment = new Assessment
                {
                    Title = "Test Objective asses.",
                    Start = thisMonth.AddMonths(1),
                    End = thisMonth.AddMonths(1).AddDays(5),
                    Course = sampleCourse.Id,
                    Type = "Objective",
                    Notification = 1
                };
                _conn.Insert(sampleAssessment);
                sampleAssessment = new Assessment
                {
                    Title = "Test Performance asses.",
                    Start = thisMonth.AddMonths(2),
                    End = thisMonth.AddMonths(2).AddDays(5),
                    Course = sampleCourse.Id,
                    Type = "Performance",
                    Notification = 1
                };
                _conn.Insert(sampleAssessment);
            }
            //notification handling
            if (pushNotification == true)
            {
                pushNotification = false;
                int courseId = 0;
                foreach (Course course in courseLst)
                {
                    courseId++;
                    if (course.Notification == 1)
                    {
                        if (course.Start == DateTime.Today)
                            CrossLocalNotifications.Current.Show("Reminder", $"{course.CourseName} begins today!", courseId);
                        if (course.End == DateTime.Today)
                            CrossLocalNotifications.Current.Show("Reminder", $"{course.CourseName} ends today!", courseId);
                    }
                }

                int assessmentId = courseId;
                foreach (Assessment assessment in assessmentLst)
                {
                    assessmentId++;
                    if (assessment.Notification == 1)
                    {
                        if (assessment.Start == DateTime.Today)
                            CrossLocalNotifications.Current.Show("Reminder", $"{assessment.Title} begins today!", assessmentId);
                        if (assessment.End == DateTime.Today)
                            CrossLocalNotifications.Current.Show("Reminder", $"{assessment.Title} ends today!", assessmentId);
                    }
                }
            }
            terms = new ObservableCollection<Term>(termLst);
            termListView.ItemsSource = terms;
            base.OnAppearing();

        }

        public void UpdateList()
        {
            var termList = _conn.Query<Term>($"Select * From Term");
            terms = new ObservableCollection<Term>(termList);
            termListView.ItemsSource = terms;
        }

        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddEditTerm(this, null, false));
        }

        private async void Term_Click(object sender, ItemTappedEventArgs e)
        {
            Term term = (Term)e.Item;
            await Navigation.PushAsync(new TermPage(this, term));
        }
    }
}
