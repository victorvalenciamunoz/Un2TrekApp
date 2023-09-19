using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using Un2TrekApp.Models.IntegrationModels;

namespace Un2TrekApp.Services;

public class TrekiService : TokenServiceBase, ITrekiService
{
    public TrekiService(ILocalStorage localStorage) : base(localStorage)
    {
    }

    public async Task<List<Treki>> GetTrekiListInActivity(int actividad)
    {
        var trekis = new List<Treki>();
        HttpClient httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri($"{App.UrlBase}{App.ActivityEndPoint}/");
        var token = await this.GetTokenForCall();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        StringBuilder request = new StringBuilder();
        request.Append($"{actividad}");
        request.Append($"/trekilist");

        using (HttpResponseMessage response = await httpClient.GetAsync(request.ToString()))
        {
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                trekis = JsonConvert.DeserializeObject<List<Treki>>(json);
            }
        }

        return trekis;
    }

    public async Task<List<Treki>> GetTrekiListAround(Location currentLocation)
    {
        var trekis = new List<Treki>();
        HttpClient httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri($"{App.UrlBase}{App.TrekiEndPoint}/");
        var token = await this.GetTokenForCall();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        StringBuilder request = new StringBuilder();
        request.Append($"around?latitud={currentLocation.Latitude.ToString().Replace(",", ".")}&longitud={currentLocation.Longitude.ToString().Replace(",", ".")}");

        using (HttpResponseMessage response = await httpClient.GetAsync(request.ToString()))
        {
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                trekis = JsonConvert.DeserializeObject<List<Treki>>(json);
            }
        }

        return trekis;
    }

    public async Task<ServiceResultSingleElement<bool>> CreateTreki(Treki treki)
    {
        ServiceResultSingleElement<bool> result = new();
        HttpClient httpClient = new HttpClient();
        var token = await this.GetTokenForCall();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.PostAsJsonAsync<Treki>($"{App.UrlBase}{App.TrekiEndPoint}/add", treki);
        if (response.IsSuccessStatusCode)
        {
            result.Element = true;
            return result;
        }

        var errorCode = await response.Content.ReadAsStringAsync();
        result.Element = false;
        result.Errors.Add(errorCode);

        return result;
    }

    public async Task<ServiceResultSingleElement<bool>> DeleteTreki(Treki treki)
    {
        ServiceResultSingleElement<bool> result = new();

        HttpClient httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri($"{App.UrlBase}{App.TrekiEndPoint}/");
        var token = await this.GetTokenForCall();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.DeleteAsync($"{treki.Id}/delete");
        if (response.IsSuccessStatusCode)
        {
            result.Element = true;
            return result;
        }

        var errorCode = await response.Content.ReadAsStringAsync();
        result.Element = false;
        result.Errors.Add(errorCode);

        return result;
    }

    public async Task<ServiceResultSingleElement<bool>> ModifyTreki(Treki treki)
    {
        ServiceResultSingleElement<bool> result = new();

        HttpClient httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri($"{App.UrlBase}{App.TrekiEndPoint}/");
        var token = await this.GetTokenForCall();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.PutAsJsonAsync<Treki>($"{treki.Id}/update", treki);
        if (response.IsSuccessStatusCode)
        {
            result.Element = true;
            return result;
        }
        var errorCode = await response.Content.ReadAsStringAsync();
        result.Element = false;
        result.Errors.Add(errorCode);

        return result;
    }

    public async Task<ServiceResultSingleElement<bool>> CaptureTreki(int actividad, Treki treki, Location currentLocation)
    {
        ServiceResultSingleElement<bool> result = new();

        HttpClient httpClient = new HttpClient();
        var token = await this.GetTokenForCall();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CaptureTrekiRequest request = new CaptureTrekiRequest();
        request.TrekiLatitude = treki.Latitude;
        request.TrekiLongitude = treki.Longitude;
        request.CurrentLatitude = currentLocation.Latitude;
        request.CurrentLongitude = currentLocation.Longitude;
        request.ActivityId = actividad;

        var response = await httpClient.PostAsJsonAsync($"{App.UrlBase}{App.TrekiEndPoint}", request);
        if (response.IsSuccessStatusCode)
        {
            result.Element = true;
            return result;
        }

        var errorCode = await response.Content.ReadAsStringAsync();
        result.Element = false;
        result.Errors.Add(errorCode);

        return result;
    }
}
