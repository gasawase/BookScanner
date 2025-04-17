using System.Collections.ObjectModel;
using BookScanner.Models;
using BookScanner.Services;

namespace BookScanner
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseService _database;
        private ObservableCollection<Book> _books = [];
        public MainPage(DatabaseService database)
        {
            InitializeComponent();
            _database = database;
            BookList.ItemsSource = _books;
        }

        private async void OnCameraClicked(object sender, EventArgs e)
        {
            // open new camera view
            await Navigation.PushAsync(new CameraView(_database));
        }

		private async void Book_Clicked(object sender, SelectionChangedEventArgs e)
		{
            if (e.CurrentSelection.FirstOrDefault() is Book selectedBook)
            {

                await Navigation.PushAsync(new BookDetailsPage(selectedBook, _database));
            }

            // Always clear selection so clicking again works
            BookList.SelectedItem = null;
        }

        private async Task LoadBooksAsync()
        {
            var books = await _database.GetBooksAsync();
            _books.Clear();
            foreach (var book in books)
            {
                _books.Add(book);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadBooksAsync();
        }
    }
}
