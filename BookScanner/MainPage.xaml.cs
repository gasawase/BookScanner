using BookScanner.Models;

namespace BookScanner
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadBooks();
        }

        private void OnCameraClicked(object sender, EventArgs e)
        {
            // Navigate to Book Details or implement camera functionality
            DisplayAlert("Camera", "Camera clicked!", "OK");
        }

        private void LoadBooks()
        {
            var books = new List<Book>
        {
            new Book { Title = "Book 1", Author = "Author A", Genre = "Fiction" },
            new Book { Title = "Book 2", Author = "Author B", Genre = "Non-Fiction" },
            new Book { Title = "Book 3", Author = "Author C", Genre = "Mystery" }
        };

            BookList.ItemsSource = books;
        }

    }
}
