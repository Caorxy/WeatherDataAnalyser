using System;
using WeatherDataAnalyser.Core;

namespace WeatherDataAnalyser.MVVM.ViewModel;

public class AnalysisViewModel : ObservableObject
{
    public RelayCommand AddCommand { get; set; }
    public RelayCommand CalculateCommand { get; set; }
    private string _addButtonVisibility;
    private string _secondLocationVisibility;
    private string _lat;
    private string _lon;    
    private string _lat2;
    private string _lon2;
    private DateTime _startDate;
    private DateTime _endDate;
    
    public string AddButtonVisibility
    {
        get => _addButtonVisibility;
        set
        {
            _addButtonVisibility = value;
            OnPropertyChanged();
        }
    }
    public string SecondLocationVisibility
    {
        get => _secondLocationVisibility;
        set
        {
            _secondLocationVisibility = value;
            OnPropertyChanged();
        }
    }    
    
    public string Lat
    {
        get => _lat;
        set
        {
            _lat = value;
            OnPropertyChanged();
        }
    }    
    
    public string Lat2
    {
        get => _lat2;
        set
        {
            _lat2 = value;
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
    
    public string Lon2
    {
        get => _lon2;
        set
        {
            _lon2 = value;
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
            if (value > DateTime.Now.AddDays(-6))
                _endDate = DateTime.Now.AddDays(-6);
            OnPropertyChanged();
        }
    }

    public AnalysisViewModel()
    {
        _addButtonVisibility = "Visible";
        _secondLocationVisibility = "Collapsed";
        _lat = "";
        _lat2 = ""; 
        _lon = "";
        _lon2 = "";
        AddCommand = new RelayCommand(_ =>
        {
            AddButtonVisibility = "Collapsed";
            SecondLocationVisibility = "Visible";
        });
        CalculateCommand = new RelayCommand(_ =>
        {
            AddButtonVisibility = "Collapsed";
        });
    }

}