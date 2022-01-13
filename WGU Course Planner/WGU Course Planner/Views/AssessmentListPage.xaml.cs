using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGU_Course_Planner.Classes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGU_Course_Planner
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssessmentListPage : ContentPage
    {
        private SQLiteConnection _conn;
        public ObservableCollection<Assessment> _assessmentList;
        private Course _course;

        public AssessmentListPage(Course course)
        {
            InitializeComponent();
            _course = course;
            _conn = DependencyService.Get<ISQLiteDb>().GetConnection();
            AssessmentListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(Assesment_Clicked);
            UpdateData(course);
        }

        public void UpdateData(Course course)
        {
            courseName.Text = course.CourseName;
            UpdateList();
        }

        public void UpdateList()
        {
            var assessmentList = _conn.Query<Assessment>($"Select * From Assessment Where Course = '{_course.Id}'");
            _assessmentList = new ObservableCollection<Assessment>(assessmentList);
            AssessmentListView.ItemsSource = _assessmentList;
            addAssessmentButton.IsEnabled = assessmentList.Count < 2;
        }

        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddEditAssessment(this, _course, false, Globals.GetListOfMissingAssessments(_course.Id)));
        }

        private async void Assesment_Clicked(object sender, ItemTappedEventArgs e)
        {
            Assessment assessment = (Assessment)e.Item;
            await Navigation.PushAsync(new AssessmentPage(this, assessment));
        }
    }
}