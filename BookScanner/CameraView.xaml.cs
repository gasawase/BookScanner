using Camera.MAUI.ZXingHelper;
using System.Diagnostics;

namespace BookScanner;

public partial class CameraView : ContentPage
{
	public CameraView()
	{
		InitializeComponent();
	}

    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (cameraView.Cameras.Count > 0)
        {
            cameraView.Camera = cameraView.Cameras.First();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
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
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Barcode Detected", result.Text, "OK");
            });
        }
    }
}