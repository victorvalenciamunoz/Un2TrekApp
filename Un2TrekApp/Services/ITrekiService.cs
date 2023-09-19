using Un2TrekApp.Models.IntegrationModels;

namespace Un2TrekApp.Services;

public interface ITrekiService
{
    Task<List<Treki>> GetTrekiListInActivity(int actividad);

    Task<List<Treki>> GetTrekiListAround(Location currentLocation);

    Task<ServiceResultSingleElement<bool>> CreateTreki(Treki treki);

    Task<ServiceResultSingleElement<bool>> DeleteTreki(Treki treki);

    Task<ServiceResultSingleElement<bool>> ModifyTreki(Treki treki);

    Task<ServiceResultSingleElement<bool>> CaptureTreki(int actividad, Treki treki, Location currentLocation);
}