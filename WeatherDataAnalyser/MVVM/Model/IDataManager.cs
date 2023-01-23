namespace WeatherDataAnalyser.MVVM.Model;

public interface IDataManager
{
    public string PrepareToSave(HistoricalData? data, bool[] valuesIncluded, string sortBy, bool isAscending);
    public bool WriteToFile(string text);
}