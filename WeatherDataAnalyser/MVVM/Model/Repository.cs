using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WeatherDataAnalyser.MVVM.Model;

public class Repository : IRepository
{
    private readonly HttpClient _client;

    public Repository()
    {
        _client = new HttpClient();
    }

    public async Task<HistoricalData?> GetHistoricalData(GeoCoordinates coordinates)
    {
        HistoricalData? data = null!;
        var requestUri = GetGeoRequestUri(coordinates);

        var response = await _client.GetAsync(requestUri);

        if (response.IsSuccessStatusCode)
            data = await response.Content.ReadFromJsonAsync<HistoricalData>();

        return data;
    }


    private static string GetGeoRequestUri(GeoCoordinates coordinates)
    {
        var lat = (double)coordinates.Lat!;
        var lon = (double)coordinates.Lon!;
        var infos = new string[3];
        infos[0] = DateTime.Now.AddDays(-20).Year.ToString();
        infos[1] = DateTime.Now.AddDays(-20).Month.ToString();
        infos[2] = DateTime.Now.AddDays(-20).Day.ToString();

        if (infos[1].Length < 2) infos[1] = "0" + infos[1];
        if (infos[2].Length < 2) infos[2] = "0" + infos[2];
        
        var dateNow = infos[0] + "-" + infos[1] + "-" + infos[2];
        
        return $"https://archive-api.open-meteo.com/v1/archive?latitude={lat.ToString(CultureInfo.InvariantCulture)}&longitude={lon.ToString(CultureInfo.InvariantCulture)}&start_date=1993-01-01&end_date={dateNow}&hourly=temperature_2m,relativehumidity_2m,pressure_msl,rain,windspeed_10m";
    }
    
}