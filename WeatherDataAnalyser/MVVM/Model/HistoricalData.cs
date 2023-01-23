using System;
using Newtonsoft.Json;

namespace WeatherDataAnalyser.MVVM.Model;


public class HistoricalData
{
    [JsonProperty(PropertyName = "hourly")]
    public HourlyWeatherData HourlyWeatherInfo = null!;
}
public class HourlyWeatherData
{
    [JsonProperty(PropertyName = "time")]
    public DateTime[] Time { get; set; } = null!;

    [JsonProperty(PropertyName = "temperature_2m")]
    public float[] Temperature { get; set; } = null!;

    [JsonProperty(PropertyName = "relativehumidity_2m")]
    public float[] Humidity { get; set; } = null!;

    [JsonProperty(PropertyName = "pressure_msl")]
    public float[] Pressure { get; set; } = null!;

    [JsonProperty(PropertyName = "rain")]
    public float[] Rain { get; set; } = null!;

    [JsonProperty(PropertyName = "windspeed_10m")]
    public float[] WindSpeed { get; set; } = null!;
}