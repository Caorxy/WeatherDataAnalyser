using System;
using System.Globalization;
using WeatherDataAnalyser.Core;
using WeatherDataAnalyser.MVVM.Model;

namespace WeatherDataAnalyser.MVVM.ViewModel;

public class SaveToFileViewModel : AbstractViewModel
{

    private string _sortBy;
    private string _opacity1;
    private string _opacity2;
    private bool _dateIncluded;
    private bool _temperatureIncluded;
    private bool _pressureIncluded;
    private bool _rainIncluded;
    private bool _windIncluded;
    private bool _humidityIncluded;
    private bool _isAscending;
    private string _successNoteVisibility;

    public RelayCommand DateIncludedChanged { get; set; }
    public RelayCommand TemperatureIncludedChanged { get; set; }
    public RelayCommand PressureIncludedChanged { get; set; }
    public RelayCommand RainIncludedChanged { get; set; }
    public RelayCommand WindIncludedChanged { get; set; }
    public RelayCommand HumidityIncludedChanged { get; set; }
    public RelayCommand IsAscendingChanged { get; set; }
    public RelayCommand SaveDataToFile { get; set; }
    
    public string Opacity1
    {
        get => _opacity1;
        set
        {
            _opacity1 = value;
            OnPropertyChanged();
        }
    }       
    public string Opacity2
    {
        get => _opacity2;
        set
        {
            _opacity2 = value;
            OnPropertyChanged();
        }
    }      
    public string SuccessNoteVisibility
    {
        get => _successNoteVisibility;
        set
        {
            _successNoteVisibility = value;
            OnPropertyChanged();
        }
    }
    
    public string SortBy 
    {
        get => _sortBy;
        set
        {
            _sortBy = value;
            OnPropertyChanged();
        }
    }

    public SaveToFileViewModel(IRepository repository, IDataManager dataManager)
    {
        Lat = "";
        Lon = "";
        _opacity1 = "0.5";
        _opacity2 = "1";
        _sortBy = "Time";
        _dateIncluded = true;
        _temperatureIncluded = true;
        _pressureIncluded = true;
        _rainIncluded = true;
        _windIncluded = true;
        _humidityIncluded = true;
        _isAscending = false;
        _successNoteVisibility = "Collapsed";

        DateIncludedChanged = new RelayCommand(_ =>
        {
            _dateIncluded = _dateIncluded == false;
        });
        TemperatureIncludedChanged = new RelayCommand(_ =>
        {
            _temperatureIncluded = _temperatureIncluded == false;
        });
        PressureIncludedChanged = new RelayCommand(_ =>
        {
            _pressureIncluded = _pressureIncluded == false;
        });
        RainIncludedChanged = new RelayCommand(_ =>
        {
            _rainIncluded = _rainIncluded == false;
        });
        WindIncludedChanged = new RelayCommand(_ =>
        {
            _windIncluded = _windIncluded == false;
        });
        HumidityIncludedChanged = new RelayCommand(_ =>
        {
            _humidityIncluded = _humidityIncluded == false;
        });
        IsAscendingChanged = new RelayCommand(_ =>
        {
            _isAscending = _isAscending == false;
            (Opacity1, Opacity2) = (Opacity2, Opacity1);
        });
        
        SaveDataToFile = new RelayCommand(async _ =>
        {
            try
            {
                var coordinates = new GeoCoordinates
                {
                    Lat = double.Parse(Lat, CultureInfo.InvariantCulture),
                    Lon = double.Parse(Lon, CultureInfo.InvariantCulture)
                };
                var data = await repository.GetHistoricalData(coordinates, StartDate, EndDate);
                var valuesIncluded = new[]
                {
                    _dateIncluded, _temperatureIncluded, _pressureIncluded, _rainIncluded, _windIncluded,
                    _humidityIncluded
                };
                if (data == null) return;
                var result = dataManager.PrepareToSave(data, valuesIncluded, _sortBy, _isAscending);
                if(dataManager.WriteToFile(result))
                    SuccessNoteVisibility = "Visible";
            } catch (FormatException)
            {
                // invalid format
            }
        });
    }   
}
