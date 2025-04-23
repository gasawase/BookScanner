using System.Collections.ObjectModel;
using BookScanner.Models;
using BookScanner.Services;
using BookScanner.ViewModels;

namespace BookScanner
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel _viewModel;

        public MainPage(DatabaseService database)
        {
            InitializeComponent();
            _viewModel = new MainPageViewModel(database);
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadBooksAsync();
        }

        private async void Book_Clicked(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Book selectedBook)
            {
                await Navigation.PushAsync(new BookDetailsPage(selectedBook, _viewModel._database));
            }

            BookList.SelectedItem = null;
        }

        private async void OnCameraClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CameraView(_viewModel._database));
        }
    }
}
