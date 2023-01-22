using System;
using System.Threading.Tasks;

namespace WeatherDataAnalyser.MVVM.Model;

public interface IRepository
{
    public Task<HistoricalData?> GetHistoricalData(GeoCoordinates coordinates, DateTime startDate, DateTime endDate);
}