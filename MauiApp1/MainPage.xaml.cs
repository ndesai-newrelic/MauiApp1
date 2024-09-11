using System.Diagnostics;
using NewRelic.MAUI.Plugin;

namespace MauiApp1;

public partial class MainPage : ContentPage
{
    int count = 0;
    private bool _isCheckingLocation;
    private CancellationTokenSource _cancelTokenSource;
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        var attr = new Dictionary<string, object>
        {
            { "application", "Test APP" },
            { "DeviceId", 1234555 },
            { "UserId", "Test USER" ?? "" }
        };
        attr.Add("level", "DEBUG");
        attr.Add("message", "This is From Maui");
        CrossNewRelic.Current.LogAttributes(attr);


        CrossNewRelic.Current.LogInfo("This is From Maui");
        CrossNewRelic.Current.LogDebug("This is From Maui");
        CrossNewRelic.Current.LogVerbose("This is From Maui");
        CrossNewRelic.Current.LogWarning("This is From Maui");
        CrossNewRelic.Current.LogError("This is From Maui");
        CrossNewRelic.Current.Log(LogLevel.VERBOSE,"This is From Maui");

        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
        keyValuePairs.Add("message", "This is From Attribute");
        keyValuePairs.Add("eat", "Pizza");
        keyValuePairs.Add("food", "tell me");

        CrossNewRelic.Current.LogAttributes(keyValuePairs);

        Console.WriteLine("This is Auto Instrumentation");

        try
        {
            throw new Exception("This is Error");
        } catch (Exception ex)
        {
            CrossNewRelic.Current.RecordException(ex, keyValuePairs);
        }


        //PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        //if (status == PermissionStatus.Granted)


        //if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
        //{
        //    // Prompt the user to turn on in settings
        //    // On iOS once a permission has been denied it may not be requested again from the application

        //}

        //if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
        //{
        //    // Prompt the user with additional information as to why the permission is needed
        //}

        //status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

        Uri uri = new("https://reactnative.dev/movies.json");

        try
        {
            HttpClientHandler httpClientHandler = CrossNewRelic.Current.GetHttpMessageHandler();
            httpClientHandler.AllowAutoRedirect = false;
            HttpClient myClient = new HttpClient(httpClientHandler);

            HttpResponseMessage response = await myClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                String content = await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception ex)
        {
            await this.DisplayAlert("Error", ex.Message, "OK");
        }


        //try
        //{
        // Uri uri = new Uri("https://www.microsoft.com");
        // await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        //}
        //catch (Exception ex)
        //{
        // // An unexpected error occurred. No browser may be installed on the device.
        //}
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
    
    private  void OnGpsClicked(object sender, EventArgs e)
    {
        Task.Delay(10000).Wait();
    }
    
    private  void OnHandleExceptionClicked(object sender, EventArgs e)
    {
        try
        {
            throw new Exception("This is a test exception");
        }
        catch (Exception ex)
        {
            CrossNewRelic.Current.RecordException(ex);
        }
    }
    
    private  void OnRecordEvents(object sender, EventArgs e)
    {
        CrossNewRelic.Current.RecordCustomEvent("TestEvent", "EventsApplication",new Dictionary<string, object> { { "key1", "value1" }, { "key2", "value2" } });
        CrossNewRelic.Current.RecordBreadcrumb("TestBreadCrumb", new Dictionary<string, object> { { "test", "value1" }, { "test1", "value2" } });
    }
    
    private void OnCrashClicked(object sender, EventArgs e)
    {
        // Deliberately throw an unhandled exception to crash the application
        throw new Exception("This is a test crash");
    }
    
    


    public async Task GetCurrentLocation()
    {
        try
        {
            _isCheckingLocation = true;

            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

            _cancelTokenSource = new CancellationTokenSource();

            Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

            if (location != null)
                Debug.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            _isCheckingLocation = false;
        }
    }

    public void CancelRequest()
    {
        if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
            _cancelTokenSource.Cancel();
    }
}