using C971.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace C971
{
    public partial class MainPage : ContentPage
    {
        Terms selectedTerm = new Terms();

        public MainPage()
        {
            InitializeComponent();

            viewTermButton.IsEnabled = false;
        }

        private void addTermButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddTermsPage());

            viewTermButton.IsEnabled = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using(SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Terms>();
                var terms = conn.Table<Terms>().ToList();
                termsListView.ItemsSource = terms;
                viewTermButton.IsEnabled = false;
            }
        }

        async private void deleteTermButton_Clicked(object sender, EventArgs e)
        {
            selectedTerm = termsListView.SelectedItem as Terms;
            bool ans = false;

            if (selectedTerm != null)
            {
                ans = await DisplayAlert("Warning", "Are you sure that you want to delete the selected term and corresponding classes?", "Continue", "Cancel");

                if (ans == true)
                {
                    using (SQLiteConnection db = new SQLiteConnection(App.FilePath))
                    {
                        db.CreateTable<Terms>();

                        int toDelete = db.Delete(selectedTerm);
                        await Navigation.PushAsync(new MainPage());
                    }
                }
            }
        }

        private void viewTermButton_Clicked(object sender, EventArgs e)
        {
            selectedTerm = termsListView.SelectedItem as Terms;
            Navigation.PushAsync(new DetailedTermPage(selectedTerm));

            viewTermButton.IsEnabled = false;
        }

        private void termsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (termsListView.SelectedItem != null)
            {
                viewTermButton.IsEnabled = true;
            }
        }
    }
}
