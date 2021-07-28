using C971.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailedTermPage : ContentPage
    {
        Terms currentTerm;
        Courses selectedCourse = new Courses();

        public DetailedTermPage()
        {
            InitializeComponent();
            editCoursesButton.IsEnabled = false;
            viewDetailedCoursesButton.IsEnabled = false;
        }

        public DetailedTermPage(Terms selectedTerm)
        {
            InitializeComponent();

            currentTerm = selectedTerm;
            detailedTermLabel.Text = currentTerm.title;
            startDatePicker.Date = currentTerm.startDate;
            endDatePicker.Date = currentTerm.endDate;

            editCoursesButton.IsEnabled = false;
            viewDetailedCoursesButton.IsEnabled = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Courses>();
                var courses = conn.Query<Courses>("SELECT * FROM Courses WHERE termId = '" + currentTerm.Id + "';");

                coursesListView.ItemsSource = courses;
            }
        }

        private void addCoursesButton_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Courses>();
                var courses = conn.Query<Courses>("SELECT * FROM Courses WHERE termId = '" + currentTerm.Id + "';");

                int count = 0;
                foreach (var item in courses)
                {
                    count++;
                }

                if (count < 6)
                {
                    Navigation.PushAsync(new AddCoursesPage(currentTerm));
                }

                else
                {
                    DisplayAlert("Warning", "Cannot add more than 6 courses.", "Okay");
                }
            }
        }

        async private void deleteCoursesButton_Clicked(object sender, EventArgs e)
        {
            selectedCourse = coursesListView.SelectedItem as Courses;
            var ans = await DisplayAlert("Warning", "Are you sure that you want to delete the selected course and corresponding assessments?", "Continue", "Cancel");

            if (ans == true)
            {
                using (SQLiteConnection db = new SQLiteConnection(App.FilePath))
                {
                    db.CreateTable<Courses>();
                    int toDelete = db.Delete(selectedCourse);
                    await Navigation.PushAsync(new DetailedTermPage(currentTerm));
                }
            }
        }

        private void coursesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (coursesListView.SelectedItem != null)
            {
                editCoursesButton.IsEnabled = true;
                viewDetailedCoursesButton.IsEnabled = true;
            }
        }

        private void editCoursesButton_Clicked(object sender, EventArgs e)
        {
            selectedCourse = coursesListView.SelectedItem as Courses;
            Navigation.PushAsync(new EditCoursePage(selectedCourse, currentTerm));

            editCoursesButton.IsEnabled = false;
            viewDetailedCoursesButton.IsEnabled = false;
        }

        private void viewDetailedCoursesButton_Clicked(object sender, EventArgs e)
        {
            selectedCourse = coursesListView.SelectedItem as Courses;
            Navigation.PushAsync(new DetailedCoursePage(selectedCourse));

            editCoursesButton.IsEnabled = false;
            viewDetailedCoursesButton.IsEnabled = false;
        }
    }
}