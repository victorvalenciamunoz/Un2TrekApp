namespace Un2TrekApp.Models;

public class Treki
{
    public int Id { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
