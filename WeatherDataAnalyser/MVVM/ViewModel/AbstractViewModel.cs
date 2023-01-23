using System;
using WeatherDataAnalyser.Core;

namespace WeatherDataAnalyser.MVVM.ViewModel;

public abstract class AbstractViewModel : ObservableObject
{
    private string _lat = null!;
    private string _lon = null!;
    private DateTime _startDate;
    private DateTime _endDate;
    public string Lat
    {
        get => _lat;
        set
        {
            _lat = value;
            OnPropertyChanged();
        }
    }  
    public string Lon
    {
        get => _lon;
        set
        {
            _lon = value;
            OnPropertyChanged();
        }
    }
    
    public DateTime StartDate 
    {
        get => _startDate;
        set
        {
            if (value < new DateTime(1993, 01, 01)) return;
            _startDate = value;
            OnPropertyChanged();
        }
    }
    public DateTime EndDate 
    {
        get => _endDate;
        set
        {
            if (value > DateTime.Now) return;
            _endDate = value;
            if (value > DateTime.Now.AddDays(-7))
                _endDate = DateTime.Now.AddDays(-7);
            OnPropertyChanged();
        }
    }
}