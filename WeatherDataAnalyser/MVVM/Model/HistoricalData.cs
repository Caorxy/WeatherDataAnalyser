using System;
using Newtonsoft.Json;

namespace WeatherDataAnalyser.MVVM.Model;


public class HistoricalData
{
    [JsonProperty(PropertyName = "hourly")]
    public HourlyWeatherData? HourlyWeatherInfo;
}
public class HourlyWeatherData
{
    [JsonProperty(PropertyName = "time")]
    public DateTime[]? Time { get; set; }
    [JsonProperty(PropertyName = "temperature_2m")]
    public float[]? Temperature { get; set; }
    [JsonProperty(PropertyName = "relativehumidity_2m")]
    public float[]? Humidity { get; set; }
    [JsonProperty(PropertyName = "pressure_msl")]
    public float[]? Pressure { get; set; }
    [JsonProperty(PropertyName = "rain")]
    public float[]? Rain { get; set; }
    [JsonProperty(PropertyName = "windspeed_10m")]
    public float[]? WindSpeed { get; set; }
}