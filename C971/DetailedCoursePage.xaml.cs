using C971.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.LocalNotifications;

namespace C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailedCoursePage : ContentPage
    {
        Courses selectedCourse = new Courses();
        Assessments selectedAssessment = new Assessments();

        public DetailedCoursePage()
        {
            InitializeComponent();

            editAssessButton.IsEnabled = false;
        }

        public DetailedCoursePage(Courses currentCourse)
        {
            InitializeComponent();

            selectedCourse = currentCourse;
            detailedCourseLabel.Text = selectedCourse.title;
            activityLabel.Text = $"Activity status: {selectedCourse.activity}";
            instructnameLabel.Text = $"Name: {selectedCourse.instructorName}";
            instructemailLabel.Text = $"Email: {selectedCourse.instructorEmail}";
            instructPhoneLabel.Text = $"Phone: {selectedCourse.instructorPhone}";
            notesEditor.Text = $"Notes: {selectedCourse.notes}";
            startDatePicker.Date = selectedCourse.startDate;
            endDatePicker.Date = selectedCourse.endDate;
            editAssessButton.IsEnabled = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Assessments>();
                var assessments = conn.Query<Assessments>("SELECT * FROM Assessments WHERE courseId = '" + selectedCourse.Id + "';");

                assessListView.ItemsSource = assessments;

                foreach (var item in assessments)
                {
                    if (item.dueDate == DateTime.Today && item.notification == true)
                    {
                        CrossLocalNotifications.Current.Show("Assessment Notification", item.title + " is due today.", 100);
                    }
                }
            }
        }

        private void addAssessButton_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Assessments>();
                var assessments = conn.Query<Assessments>("SELECT * FROM Assessments WHERE courseId = '" + selectedCourse.Id + "';");

                int count = 0;
                foreach (var item in assessments)
                {
                    count++;
                }

                if (count < 2)
                {
                    Navigation.PushAsync(new AddAsessmentsPage(selectedCourse));
                }

                else
                {
                    DisplayAlert("Warning", "Cannot add more than 2 assessments.", "Okay");
                }
            }
        }

        private void editAssessButton_Clicked(object sender, EventArgs e)
        {
            selectedAssessment = assessListView.SelectedItem as Assessments;
            Navigation.PushAsync(new EditAssessmentPage(selectedAssessment, selectedCourse));

            editAssessButton.IsEnabled = false;
        }

        async private void deleteAssessButton_Clicked(object sender, EventArgs e)
        {
            selectedAssessment = assessListView.SelectedItem as Assessments;
            var ans = await DisplayAlert("Warning", "Are you sure that you want to delete the selected assessment?", "Continue", "Cancel");

            if (ans == true)
            {
                using (SQLiteConnection db = new SQLiteConnection(App.FilePath))
                {
                    db.CreateTable<Assessments>();
                    int toDelete = db.Delete(selectedAssessment);
                    
                    await Navigation.PushAsync(new DetailedCoursePage(selectedCourse));
                    CrossLocalNotifications.Current.Cancel(100);
                }
            }
        }

        private void assessListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (assessListView.SelectedItem != null)
            {
                editAssessButton.IsEnabled = true;
            }
        }
    }
}