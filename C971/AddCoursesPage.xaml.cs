using C971.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCoursesPage : ContentPage
    {
        Terms selectedTerm = new Terms();

        public AddCoursesPage()
        {
            InitializeComponent();
        }

        public AddCoursesPage(Terms currentTerm)
        {
            InitializeComponent();

            selectedTerm = currentTerm;
        }

        private void saveButton_Clicked(object sender, EventArgs e)
        {
            string tempActivity = null;
            switch (activityPicker.SelectedIndex)
            {
                case 0:
                    tempActivity = "Not Started";
                    break;
                case 1:
                    tempActivity = "Plane to take";
                    break;
                case 2:
                    tempActivity = "In Progress";
                    break;
                case 3:
                    tempActivity = "Completed";
                    break;
                case 4:
                    tempActivity = "Dropped";
                    break;
                default:
                    break;
            }

            Courses course = new Courses()
            {
                termId = selectedTerm.Id,
                title = courseTitleEntry.Text,
                activity = tempActivity,
                instructorName = instructnameEntry.Text,
                instructorEmail = instructemailEntry.Text,
                instructorPhone = instructPhoneEntry.Text,
                notes = notesEditor.Text,
                startDate = startDatePicker.Date,
                endDate = endDatePicker.Date
            };

            if (course.title == null
                || course.activity == null 
                || course.instructorName == null
                || course.instructorEmail == null
                || course.instructorPhone == null)
            {
                DisplayAlert("Warning", "All fields must be filled.", "Retry");
            }

            else if (course.endDate < course.startDate)
            {
                DisplayAlert("Warning", "Selected end date is prior to start date.", "Retry");
            }

            else if (CheckEmailFormat(course.instructorEmail) == false && CheckPhoneFormat(course.instructorPhone) == false)
            {
                DisplayAlert("Warning", "Email and/or phone number does not match required format.", "Retry");
            }

            else
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    conn.CreateTable<Courses>();
                    conn.Insert(course);
                    
                }
                Navigation.PopAsync();
            }
        }
        #region Helper Functions
        public bool CheckPhoneFormat(string phone)
        {
            Regex validFormat = new Regex(@"[0-9]{3}-[0-9]{3}-[0-9]{4}");
            if (validFormat.IsMatch(phone))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckEmailFormat(string email)
        {
            Regex validFormat = new Regex(@"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-z]{2,}\b");
            if (validFormat.IsMatch(email))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}