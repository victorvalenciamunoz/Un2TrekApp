using CommunityToolkit.Maui.Alerts;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Platform;
using Un2TrekApp.Errors;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Un2TrekApp.ViewModels;

[QueryProperty(nameof(Activity), "Activity")]
public partial class TrekiMapViewModel : BaseViewModel
{
    private readonly string LabelNewTreki = "Nuevo Treki";
    private readonly ITrekiService trekiService;
    private readonly ErrorManager errorManager;
    private Location currentLocation = null;
    private CancellationTokenSource cts;
    private List<Treki> trekiList;    

    public Microsoft.Maui.Controls.Maps.Map Map;
    public ObservableCollection<Treki> TrekiList { get; private set; } = new ObservableCollection<Treki>();

    [ObservableProperty]
    Actividad activity;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAddingTreki))]
    [NotifyPropertyChangedFor(nameof(IsEditingTreki))]
    Location selectedLocation;

    [ObservableProperty]
    string selectedLocationTitle;

    [ObservableProperty]
    string selectedLocationDescription;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAddingTreki))]
    [NotifyPropertyChangedFor(nameof(IsEditingTreki))]
    [NotifyPropertyChangedFor(nameof(IsUserSelectectingTreki))]
    [NotifyPropertyChangedFor(nameof(CanUserCaptureTreki))]
    [NotifyCanExecuteChangedFor(nameof(CaptureTrekiCommand))]
    private Treki selectedTreki;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(NotIsBusy))]
    bool isBusy = false;

    [ObservableProperty]
    bool canUserCaptureTreki;

    public bool NotIsBusy => !IsBusy;

    public bool IsAddingTreki
    {
        get
        {
            return IsUserAdministrator() && SelectedLocation != null;
        }
    }

    public bool IsEditingTreki
    {
        get
        {
            return IsUserAdministrator() && SelectedTreki != null;
        }
    }

    public bool IsUserSelectectingTreki
    {
        get
        {
            return !IsUserAdministrator() && SelectedTreki != null;
        }
    }

    public bool CanCaptureTreki()
    {
        CanUserCaptureTreki = !IsUserAdministrator() && SelectedTreki != null;
        return canUserCaptureTreki;
    }

    public TrekiMapViewModel(ITrekiService trekiService, ErrorManager errorManager)
    {
        this.trekiService = trekiService;
        this.errorManager = errorManager;
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
            var ci = new CultureInfo(currentCulture)
            {
                NumberFormat = { NumberDecimalSeparator = "." }
            };
        });
    }

    [RelayCommand]
    public async Task CreateTreki()
    {
        var previous = Map.Pins.Where(p => p.Type == PinType.SearchResult).FirstOrDefault();
        if (previous != null)
        {
            var newTreki = new Treki();
            newTreki.Title = SelectedLocationTitle;
            newTreki.Description = SelectedLocationDescription;
            newTreki.Latitude = SelectedLocation.Latitude;
            newTreki.Longitude = SelectedLocation.Longitude;
            var result = await trekiService.CreateTreki(newTreki);
            if (result.HasErrors)
            {
                var creationPin = Map.Pins.Where(c => c.Label == LabelNewTreki ).FirstOrDefault();
                if (creationPin != null)
                {
                    Map.Pins.Remove(creationPin);
                }
                var error = errorManager.GetError(result.Errors[0]);
                await Snackbar.Make(error.Message, () => { }, "Aceptar", TimeSpan.FromSeconds(5)).Show();
            }
            else
            {
                Map.Pins.Remove(previous);

                CustomPin newPin = new CustomPin
                {
                    Location = new Location(newTreki.Latitude, newTreki.Longitude),
                    Type = PinType.Place,
                    Label = $"{newTreki.Title}",
                    ImageSource = ImageSource.FromFile("pinmap.png"),
                };
                newPin.MarkerClicked += NewPin_MarkerClicked;
                Map.Pins.Add(newPin);

                TrekiList.Add(newTreki);
                CleanSelectedLocation();
            }
        }
    }

    [RelayCommand]
    public void CancelCreatingTreki()
    {
        var previous = Map.Pins.Where(p => p.Type == PinType.SearchResult).FirstOrDefault();
        if (previous != null)
        {
            Map.Pins.Remove(previous);
            CleanSelectedLocation();
        }
    }

    [RelayCommand]
    public void CloseEditingTreki()
    {
        SelectedTreki = null;
    }

    [RelayCommand]
    public async Task RemoveTreki()
    {
        if (SelectedTreki != null)
        {
            var result = await trekiService.DeleteTreki(SelectedTreki);
            if (result.HasErrors)
            {
                var error = errorManager.GetError(result.Errors[0]);
                await Snackbar.Make(error.Message, () => { }, "Aceptar", TimeSpan.FromSeconds(5)).Show();
            }
            else
            {
                var pin = Map.Pins.Where(c => c.Location.Longitude == SelectedTreki.Longitude && c.Location.Latitude == SelectedTreki.Latitude).FirstOrDefault();
                if (pin != null)
                {
                    pin.MarkerClicked -= NewPin_MarkerClicked;
                    Map.Pins.Remove(pin);
                }
            }            
        }
    }

    [RelayCommand]
    public async Task ModifyTreki()
    {
        if (SelectedTreki != null)
        {
            var result = await trekiService.ModifyTreki(SelectedTreki);
            if (result.HasErrors)
            {
                var error = errorManager.GetError(result.Errors[0]);
                await Snackbar.Make(error.Message, () => { }, "Aceptar", TimeSpan.FromSeconds(5)).Show();
            }
            else
            {
                var pin = Map.Pins.Where(c => c.Location.Longitude == SelectedTreki.Longitude && c.Location.Latitude == SelectedTreki.Latitude).FirstOrDefault();
                pin.Label = SelectedTreki.Title;
                await Snackbar.Make("Treki updated", () => { }, "Aceptar", TimeSpan.FromSeconds(5)).Show();
            }
        }
    }

    [RelayCommand]
    public void CloseUserViewTreki()
    {
        SelectedTreki = null;
    }

    [RelayCommand(CanExecute = nameof(CanCaptureTreki))]
    public async Task CaptureTreki()
    {
        if (SelectedTreki != null && activity!=null)
        {
            var result = await trekiService.CaptureTreki(activity.Id, SelectedTreki, currentLocation);
            if (result.HasErrors)
            {
                var error = errorManager.GetError(result.Errors[0]);
                await Snackbar.Make(error.Message, () => { }, "Aceptar", TimeSpan.FromSeconds(5)).Show();
            }
            else
            {
                await Snackbar.Make("Congrats. You have captured a treki!", () => { }, "Aceptar", TimeSpan.FromSeconds(5)).Show();
            }   
        }
    }

    public async Task Init()
    {
        SelectedLocation = null;
        SelectedTreki = null;

        if (currentLocation == null)
            currentLocation = await GetCurrentLocation();

        await GetTrekiList(currentLocation);

        MainThread.BeginInvokeOnMainThread(async () => await CenterMapOnPosition(currentLocation));
    }

    public async Task CenterMapOnPosition(Location refLocation)
    {
        IsBusy = true;
        if (refLocation == null)
            refLocation = await GetCurrentLocation();

        if (refLocation != null)
        {
            await AddRefPositionToMap(refLocation);
            double distance = 500;
            if (TrekiList != null && TrekiList.Any())
            {
                distance = Geolocation.GeoCalculator.GetDistance(refLocation.Latitude, refLocation.Longitude
                                                                                , TrekiList[0].Latitude, TrekiList[0].Longitude, decimalPlaces: 2
                                                                                , Geolocation.DistanceUnit.Meters);
            }
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(refLocation, Distance.FromMeters(distance)));

        }
        IsBusy = false;
    }

    public async Task<Location> GetCurrentLocation()
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
            cts = new CancellationTokenSource();
            var location = await Microsoft.Maui.Devices.Sensors.Geolocation.Default.GetLocationAsync(request, cts.Token);
            return location;

        }
        catch (Exception ex)
        {
            // Unable to get location
        }
        return null;
    }

    public void NewLocationSelected(Location location)
    {
        SelectedTreki = null;

        if (!IsUserAdministrator() || IsEditingTreki) return;


        if (location != null)
        {
            SelectedLocation = location;
            var previous = Map.Pins.Where(p => p.Type == PinType.SearchResult).FirstOrDefault();
            if (previous != null)
            {
                Map.Pins.Remove(previous);
            }
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Map.Pins.Add(new CustomPin
                {
                    Location = location,
                    Type = PinType.SearchResult,
                    Label = LabelNewTreki,
                    ImageSource = ImageSource.FromFile("addlocation.png")
                });
            });
        }
    }

    private async Task AddRefPositionToMap(Location refLocation)
    {
        if (refLocation == null)
            refLocation = await GetCurrentLocation();

        if (refLocation != null)
        {
            if (Map != null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Map.Pins.Add(new CustomPin
                    {
                        Location = refLocation,
                        Type = PinType.Place,
                        Label = $"Mi posición",
                        ImageSource = ImageSource.FromFile("userlocation.png")
                    });
                });
            }
        }
    }

    private async Task GetTrekiList(Location location)
    {
        if (IsBusy) return;
        IsBusy = true;
        if (TrekiList.Any())
            TrekiList.Clear();

        trekiList = null;
        try
        {
            if (activity==null || activity.Id==0)
                trekiList = await trekiService.GetTrekiListAround(location);
            else
                trekiList = await trekiService.GetTrekiListInActivity(activity.Id);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get TREKIS: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", $"Unable to get TREKIS {ex.Message}", "Ok");
        }
        if (trekiList != null && trekiList.Any())
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Map.Pins.Clear();
                foreach (var treki in trekiList)
                {
                    CustomPin newPin = new CustomPin
                    {
                        Location = new Location(treki.Latitude, treki.Longitude),
                        Type = PinType.Place,
                        Label = $"{treki.Title}",
                        ImageSource = ImageSource.FromFile("pinmap.png"),
                    };
                    newPin.MarkerClicked += NewPin_MarkerClicked;
                    Map.Pins.Add(newPin);
                }
            });
            foreach (var treki in trekiList)
            {
                TrekiList.Add(treki);
            }
        }
        IsBusy = false;

    }

    private void NewPin_MarkerClicked(object sender, PinClickedEventArgs e)
    {
        if (sender != null && sender is CustomPin pin)
        {
            SelectedTreki = null;
            Treki point = TrekiList.ToList().Where(p => p.Latitude == pin.Location.Latitude && p.Longitude == pin.Location.Longitude).FirstOrDefault();
            if (point != null)
            {                   
                SelectedLocation = null;
                SelectedLocationTitle = string.Empty;
                SelectedLocationDescription = string.Empty;
                SelectedTreki = trekiList.Where(c => c.Equals(point)).FirstOrDefault();
                //TODO: Hacer algo
            }
        }
    }

    private bool IsUserAdministrator()
    {
        return App.UserInfo.UserRoles == App.RoleAdministrator;
    }

    private void CleanSelectedLocation()
    {
        SelectedLocationTitle = string.Empty;
        SelectedLocationDescription = string.Empty;
        SelectedLocation = null;
    }
}
