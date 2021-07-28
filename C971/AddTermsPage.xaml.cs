using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C971.Classes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;


namespace C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTermsPage : ContentPage
    {
        public AddTermsPage()
        {
            InitializeComponent();
        }

        private void saveButton_Clicked(object sender, EventArgs e)
        {
            Terms term = new Terms()
            {
                title = addType.Text,
                startDate = startDatePicker.Date,
                endDate = endDatePicker.Date
            };

            if (term.title == null)
            {
                DisplayAlert("Warning", "Please enter a term title.", "Retry");
            }

            else if (term.endDate < term.startDate)
            {
                DisplayAlert("Warning", "Selected end date is prior to start date.", "Retry");
            }

            else
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    conn.CreateTable<Terms>();
                    conn.Insert(term);
                }
                Navigation.PopAsync();
            }
        }
    }
}