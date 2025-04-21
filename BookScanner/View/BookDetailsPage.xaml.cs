namespace BookScanner;
using BookScanner.Models;
using BookScanner.Services;
using System.Collections.ObjectModel;

public partial class BookDetailsPage : ContentPage
{
    private readonly DatabaseService _database;
    private ObservableCollection<Book> _books = new();

    public BookDetailsPage(Book selectedBook, DatabaseService database)
	{
		InitializeComponent();
		BindingContext = selectedBook;
		_database = database;
		RatingInput.Text = selectedBook.Rating;
    }

	//Back Button
	private async void SaveAndBackButton_Clicked(object sender, EventArgs e)
	{
		Book book = (sender as Button).BindingContext as Book;

        int? rating;
        //Check the Rating
        if (!string.IsNullOrWhiteSpace(RatingInput.Text)) //if not empty
		{
			//convert string
			rating = int.Parse(RatingInput.Text);

			// check 1-5
			if (rating < 1 || rating > 5)
			{
				await DisplayAlert("Invalid Rating", "Please enter a rating 1 to 5", "OK");
				return;
			}
		}
		else
		{
			rating = null;
		}

        //Save Rating...
        book.Rating = rating.ToString();

		await _database.SaveBookAsync(book);

		await Navigation.PopAsync();
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        // check if it's a button and then if the thing that was clicked was from a book
        if (sender is Button button && button.BindingContext is Book bookToDelete)
        {
            bool confirm = await DisplayAlert("Confirm Delete", $"Remove {bookToDelete.Title} from your library?", "Yes", "No");
            if (!confirm) return;

            await _database.DeleteBookAsync(bookToDelete);

			// also removes from the observable collection HOWEVER: if it's not removed from the database first, it will show as still being present as OnAppear will overwrite the ObservableCollection change
            _books.Remove(bookToDelete);

            await Navigation.PopAsync();
        }
    }
}