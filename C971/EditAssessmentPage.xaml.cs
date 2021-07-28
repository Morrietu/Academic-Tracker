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
    public partial class EditAssessmentPage : ContentPage
    {
        Courses currentCourse = new Courses();
        Assessments currentAssessment = new Assessments();
        public EditAssessmentPage()
        {
            InitializeComponent();
        }

        public EditAssessmentPage(Assessments selectedAssessment, Courses selectedCourses)
        {
            InitializeComponent();

            currentAssessment = selectedAssessment;
            currentCourse = selectedCourses;

            titleEntry.Text = currentAssessment.title;
            EnableNotifications.IsToggled = currentAssessment.notification;
            int pickerArray = 0;

            switch (selectedAssessment.type)
            {
                case "Objective Assessment":
                    pickerArray = 0;
                    break;
                case "Project Assessment":
                    pickerArray = 1;
                    break;
                default:
                    break;
            }

            typePicker.SelectedIndex = pickerArray;
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

                currentAssessment.courseId = currentCourse.Id;
                currentAssessment.title = titleEntry.Text;
                currentAssessment.type = tempType;
                currentAssessment.dueDate = endDatePicker.Date;

                if (EnableNotifications.IsToggled)
                {
                    currentAssessment.notification = true;
                }
                else
                {
                    currentAssessment.notification = false;
                }

                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    conn.CreateTable<Assessments>();
                    conn.Update(currentAssessment);

                }
                Navigation.PopAsync();
            }
        }
    }
}