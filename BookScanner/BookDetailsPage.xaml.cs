namespace BookScanner;
using BookScanner.Models;
using BookScanner.Services;
using System.Collections.ObjectModel;

public partial class BookDetailsPage : ContentPage
{
    private readonly DatabaseService _database;
    private ObservableCollection<Book> _books = new();


    public BookDetailsPage(Book selectedBook)
	{
		InitializeComponent();
		BindingContext = selectedBook;
	}

	//Back Button
	private async void SaveAndBackButton_Clicked(object sender, EventArgs e)
	{
		//Check the Rating
		if (!string.IsNullOrWhiteSpace(RatingInput.Text)) //if not empty
		{
			//convert string
			int RatingInt = int.Parse(RatingInput.Text);

			//check 0-5
			if (RatingInt < 0 || RatingInt > 5)
			{
				await DisplayAlert("Invalid Input", "Please enter a rating 0 to 5", "OK");
				return;
			}
			else
			{
				//return to contact page
				await Navigation.PopAsync();
			}
		}

		//Save Rating...

		//Save Notes...

		//Save Tags... (comma seperated)
		
	}

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        // check if it's a button and then if the thing that was clicked was from a book
        if (sender is Button button && button.BindingContext is Book bookToDelete)
        {
            bool confirm = await DisplayAlert("Confirm Delete", $"Delete {bookToDelete.Title}?", "Yes", "No");
            if (!confirm) return;
			await Navigation.PopAsync();

            //await _database.DeleteBookAsync(bookToDelete);

			// also removes from the observable collection HOWEVER: if it's not removed from the database first, it will show as still being present as OnAppear will overwrite the ObservableCollection change
            //_books.Remove(bookToDelete);
        }
    }
}