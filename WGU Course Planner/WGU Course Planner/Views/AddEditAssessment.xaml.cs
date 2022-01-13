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
    public partial class AddEditAssessment : ContentPage
    {
        //add/edit assesment with error handling

        private SQLiteConnection _conn;
        private bool _edit;
        private Course _course;
        private Assessment _assessment;
        private ContentPage _parent;
        
        // if edit is true then inObject has to be Assessment and it has to be Course otherwise 
        public AddEditAssessment(ContentPage parent, object inObject, bool edit, List<string> missingTypes)
        {
            InitializeComponent();

            assessmentType.Items.Clear();
            foreach (var item in missingTypes)
            {
                assessmentType.Items.Add(item);
            }

            if (edit)
            {
                _course = null;
                _assessment = (Assessment)inObject;
                assessmentTitle.Text = _assessment.Title;
                startDate.Date = _assessment.Start;
                endDate.Date = _assessment.End;
                assessmentType.Items.Add(_assessment.Type);
                assessmentType.SelectedItem = _assessment.Type;
                enableNotification.On = _assessment.Notification == 1;

            }
            else
            {
                _course = (Course)inObject;
                _assessment = null;
                startDate.Date = DateTime.Now;
                endDate.Date = DateTime.Now;
                if (missingTypes.Count == 1)
                    assessmentType.SelectedItem = missingTypes[0];
            }
            _parent = parent;
            _edit = edit;
            _conn = DependencyService.Get<ISQLiteDb>().GetConnection();
            pageTitle.Text = edit ? "Edit assessment" : "Add assessment";

        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(assessmentTitle.Text))
            {
                await DisplayAlert("Error.", "Please enter assessment title.", "Ok");
                return;
            }
            if (string.IsNullOrEmpty(assessmentTitle.Text))
            {
                await DisplayAlert("Error.", "Please fill all fields.", "Ok");
                return;
            }
            if (assessmentType.SelectedItem == null)
            {
                await DisplayAlert("Error.", "Please select assessment type.", "Ok");
                return;
            }
            if (!_edit)
            {
                _assessment = new Assessment();
                _assessment.Course = _course.Id;
            }
            _assessment.Title = assessmentTitle.Text;
            _assessment.Start = startDate.Date;
            _assessment.End = endDate.Date;
            _assessment.Type = (string)assessmentType.SelectedItem;
            _assessment.Notification = enableNotification.On ? 1 : 0;

            if (_edit)
            {
                _conn.Update(_assessment);
                ((AssessmentPage)_parent).UpdateData(_assessment);
            }
            else
            {
                _conn.Insert(_assessment);
                ((AssessmentListPage)_parent).UpdateList();
            }
            await Navigation.PopModalAsync();
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}