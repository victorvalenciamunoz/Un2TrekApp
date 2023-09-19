using Newtonsoft.Json;
using System.Text;

namespace Un2TrekApp.Services;

public class ActivityService : TokenServiceBase, IActivityService
{
    public ActivityService(ILocalStorage localStorage) : base(localStorage)
    {

    }

    public async Task<List<Actividad>> GetActiveActivityList()
    {
        var activityList = new List<Actividad>();
        HttpClient httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri($"{App.UrlBase}{App.ActivityEndPoint}/");
        var token = await this.GetTokenForCall();

        StringBuilder request = new StringBuilder();
        request.Append("active");
        request.Append($"?currentDate={DateTime.Now.ToString("MM/dd/yy H:mm:ss")}");

        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        using (HttpResponseMessage response = await httpClient.GetAsync(request.ToString()))
        {
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                activityList = JsonConvert.DeserializeObject<List<Actividad>>(json);
            }
        }

        return activityList;
    }
}
