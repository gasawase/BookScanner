using System.Collections.ObjectModel;
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

		private async void Book_Clicked(object sender, SelectionChangedEventArgs e)
		{
			var selectedBook = e.CurrentSelection.FirstOrDefault() as Book;
			if (selectedBook != null)
			{
				//await DisplayAlert("Book", $"Book clicked! Title: {selectedBook.Title}", "OK");
				await Navigation.PushAsync(new BookDetailsPage(selectedBook));
			}
		}

		private void LoadBooks()
        {
            var books = new List<Book>
            {
                new Book { Title = "Book 1", Author = "Author A", Genre = "Fiction", Description = "test 111 fjanieownaglndslnaig ", ISBN ="1234567890987" },
                new Book { Title = "Book 2", Author = "Author B", Genre = "Non-Fiction", Description = "test 222 njfonoiangionaj nionna nfjdna nfiona  fneioanfe", ISBN ="6758493021234" },
                new Book {Title = "Book 3", Author = "Author C", Genre = "Mystery", Description = "test 333 nf ejwangiornvdnzok ;jiok lnaklgnjkldn ovkdl,snvkodlsn mkvold;nvmkolfeadnv kofmldksnbj fsdkl", ISBN = "0987654321123"}
            };

            BookList.ItemsSource = books;
        }

    }
}
