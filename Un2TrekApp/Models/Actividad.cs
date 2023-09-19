namespace Un2TrekApp.Models;

public class Actividad
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Descripcion { get; set; }

    public string ValidoDesde { get; set; }

    public string ValidoHasta { get; set; }
}
