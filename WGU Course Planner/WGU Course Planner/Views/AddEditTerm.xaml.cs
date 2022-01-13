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
    public partial class AddEditTerm : ContentPage
    {
        //editing/adding term using datepicker and error handling

        private SQLiteConnection _conn;
        ContentPage _parent;
        private bool _edit;
        private Term _term;
        public AddEditTerm(ContentPage parent, Term term, bool edit)
        {
            InitializeComponent();
            _parent = parent;
            _term = term;
            _edit = edit;
            _conn = DependencyService.Get<ISQLiteDb>().GetConnection();
            pageTitle.Text = edit ? "Edit term" : "Add term";
            Title.Text = edit ? term.Title : "";
            Start.Date = edit ? term.Start : DateTime.Now;
            End.Date = edit ? term.End : DateTime.Now;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Title.Text))
            {
                await DisplayAlert("Error.", "Title cannot be empty.", "Ok");
                return;
            }
            if (Start.Date > End.Date)
            {
                await DisplayAlert("Error.", "End date cannoit be before start date.", "Ok");
                return;
            }
            if (!_edit)
                _term = new Term();
            _term.Title = Title.Text;
            _term.Start = Start.Date;
            _term.End = End.Date;
            if (_edit)
            {
                _conn.Update(_term);
                ((TermPage)_parent).UpdateData(_term);
            }
            else
            {
                _conn.Insert(_term);
                ((MainPage)_parent).terms.Add(_term);
            }
            await Navigation.PopModalAsync();
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}