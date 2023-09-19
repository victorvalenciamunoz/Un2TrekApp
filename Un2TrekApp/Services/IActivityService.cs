namespace Un2TrekApp.Services
{
    public interface IActivityService
    {
        Task<List<Actividad>> GetActiveActivityList();
    }
}