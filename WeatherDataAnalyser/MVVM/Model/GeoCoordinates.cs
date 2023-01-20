using System.ComponentModel.DataAnnotations;

namespace WeatherDataAnalyser.MVVM.Model;

public class GeoCoordinates
{
    [Required] public double? Lat;
    [Required] public double? Lon;
}