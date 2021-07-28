using C971.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.LocalNotifications;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAsessmentsPage : ContentPage
    {
        Courses currentCourse = new Courses();
        public AddAsessmentsPage()
        {
            InitializeComponent();
        }

        public AddAsessmentsPage(Courses selectedCourse)
        {
            InitializeComponent();

            currentCourse = selectedCourse;
        }

        private void saveButton_Clicked(object sender, EventArgs e)
        {
            if (titleEntry.Text == null || typePicker == null)
            {
                DisplayAlert("Warning", "All fields must be filled.", "Retry");
            }

            else
            {
                string tempType = null;
                switch (typePicker.SelectedIndex)
                {
                    case 0:
                        tempType = "Objective Assessment";
                        break;
                    case 1:
                        tempType = "Project Assessment";
                        break;
                    default:
                        break;
                }

                Assessments assessment = new Assessments()
                {
                    courseId = currentCourse.Id,
                    title = titleEntry.Text,
                    type = tempType,
                    dueDate = endDatePicker.Date
                };

                if (EnableNotifications.IsToggled)
                {
                    assessment.notification = true;
                }
                else
                {
                    assessment.notification = false;
                }

                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    conn.CreateTable<Assessments>();
                    conn.Insert(assessment);
                }
                Navigation.PopAsync();
            }
        }
    }
}