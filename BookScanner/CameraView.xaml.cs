using Camera.MAUI;
using Camera.MAUI.ZXing;
using Camera.MAUI.ZXingHelper;
using System.Diagnostics;
using BookScanner.Services;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific.AppCompat;

namespace BookScanner;

public partial class CameraView : ContentPage
{
    private readonly DatabaseService _database;
	public CameraView(DatabaseService database)
	{
		InitializeComponent();
        _database = database;
    }

    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (cameraView.Cameras.Count > 0)
        {
            cameraView.Camera = cameraView.Cameras.First();

            cameraView.BarCodeDecoder = new ZXingBarcodeDecoder();

            cameraView.BarCodeOptions = new BarcodeDecodeOptions
            {
                PossibleFormats =
                    {
                        BarcodeFormat.EAN_13,
                        BarcodeFormat.EAN_8,
                        BarcodeFormat.UPC_A,
                        BarcodeFormat.UPC_E,
                        BarcodeFormat.CODE_128,
                        BarcodeFormat.CODE_39,
                        BarcodeFormat.QR_CODE
                    },
                TryHarder = true,
                AutoRotate = true
            };

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(1000); // wait 1 second

                await cameraView.StopCameraAsync();
                await cameraView.StartCameraAsync();
            });
        }
    }

    //private void cameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    //{
    //    Debug.WriteLine("Barcode detected!");
    //    MainThread.BeginInvokeOnMainThread(() =>
    //    {
    //        barcodeResult.Text = $"{args.Result[0].BarcodeFormat}: {args.Result[0].Text}";
    //    });
    //}

    private void cameraView_BarcodeDetected(object sender, BarcodeEventArgs e)
    {
        var result = e.Result.FirstOrDefault();
        if (result != null)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Debug.WriteLine($"Detected: {result.BarcodeFormat} - {result.Text}");
                barcodeResult.Text = $"Detected: {result.BarcodeFormat} - {result.Text}";
                var isbn = result.Text;
                CheckAPIAndSendAlertCheck(isbn);
            });
        }
        else
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Debug.WriteLine("No barcode detected.");
                barcodeResult.Text = "No barcode detected.";
            });
        }
    }

    private async void OnManualSearchClicked(object sender, EventArgs e)
    {
        string isbn = isbnEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(isbn))
        {
            await DisplayAlert("Input Error", "Please enter a valid ISBN.", "OK");
            return;
        }
        CheckAPIAndSendAlertCheck(isbn);
    }

    private async void CheckAPIAndSendAlertCheck(string ISBN)
    {
        var book = await BookApi.LookupISBN(ISBN);

        if (book != null)
        {
            bool confirm = await DisplayAlert("Book Found",
                $"Title: {book.Title}\nAuthor: {book.Author}\nISBN: {book.ISBN}",
                "Yes, add it", "No");

            if (confirm)
            {
                // Access the database service
                await _database.SaveBookAsync(book);
                await DisplayAlert("Success", $"Book {book.Title} added to your library.", "OK");
                await Navigation.PopToRootAsync();
            }
        }
        else
        {
            await DisplayAlert("Not Found", "No book found with that ISBN.", "OK");
        }
    }
}