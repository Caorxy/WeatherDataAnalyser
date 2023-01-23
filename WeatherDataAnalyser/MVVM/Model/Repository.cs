using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace WeatherDataAnalyser.MVVM.Model;

public class Repository : IRepository
{
    private readonly HttpClient _client;

    public Repository()
    {
        _client = new HttpClient();
    }    
    
    public Repository(HttpClient client)
    {
        _client = client;
    }

    public async Task<HistoricalData?> GetHistoricalData(GeoCoordinates coordinates, DateTime startDate, DateTime endDate)
    {
        HistoricalData? data = null!;
        var requestUri = GetGeoRequestUri(coordinates, startDate, endDate);

        var response = await _client.GetAsync(requestUri);

        if (response.IsSuccessStatusCode)
            data = await response.Content.ReadAsAsync<HistoricalData>();

        return data;
    }


    private static string GetGeoRequestUri(GeoCoordinates coordinates, DateTime startDate, DateTime endDate)
    {
        var lat = (double)coordinates.Lat!;
        var lon = (double)coordinates.Lon!;
        var start = DateToString(startDate);
        var end = DateToString(endDate);
        
        return $"https://archive-api.open-meteo.com/v1/archive?latitude={lat.ToString(CultureInfo.InvariantCulture)}&longitude={lon.ToString(CultureInfo.InvariantCulture)}&start_date={start}&end_date={end}&hourly=temperature_2m,relativehumidity_2m,pressure_msl,rain,windspeed_10m";
    }

    private static string DateToString(DateTime date)
    {
        var infos = new string[3];
        infos[0] = date.Year.ToString();
        infos[1] = date.Month.ToString();
        infos[2] = date.Day.ToString();

        if (infos[1].Length < 2) infos[1] = "0" + infos[1];
        if (infos[2].Length < 2) infos[2] = "0" + infos[2];
        
        return infos[0] + "-" + infos[1] + "-" + infos[2];
    }
    
}