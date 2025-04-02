namespace BookScanner;
using BookScanner.Models;

public partial class BookDetailsPage : ContentPage
{
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
}