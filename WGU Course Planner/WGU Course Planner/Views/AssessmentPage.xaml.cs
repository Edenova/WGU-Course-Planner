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
    public partial class AssessmentPage : ContentPage
    {
        private SQLiteConnection _conn;
        private Assessment _assessment;
        private AssessmentListPage _parent;

        public AssessmentPage(AssessmentListPage parent, Assessment assessment)
        {
            InitializeComponent();
            _assessment = assessment;
            _parent = parent;
            _conn = DependencyService.Get<ISQLiteDb>().GetConnection();
            UpdateData(assessment);
        }

        public void UpdateData(Assessment assessment)
        {
            navTitle.Text = assessment.Title;
            startDate.Text = assessment.Start.ToString("MM/dd/yyyy", DateTimeFormatInfo.InvariantInfo);
            endDate.Text = assessment.End.ToString("MM/dd/yyyy", DateTimeFormatInfo.InvariantInfo);
            assessmentType.Text = assessment.Type;
            notification.Text = assessment.Notification == 1 ? "Yes" : "No";
            _parent.UpdateList();
        }

        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddEditAssessment(this, _assessment, true, Globals.GetListOfMissingAssessments(_assessment.Course)));
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            var confirmation = await DisplayAlert("Alert", "Are you sure you want to drop this assessment?", "Yes", "No");
            if (confirmation)
            {
                _conn.Delete(_assessment);
                _parent.UpdateList();
                await Navigation.PopAsync();
            }
        }
    }
}