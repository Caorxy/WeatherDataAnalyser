using System;
using System.Globalization;
using WeatherDataAnalyser.Core;
using WeatherDataAnalyser.MVVM.Model;

namespace WeatherDataAnalyser.MVVM.ViewModel;

public class AnalysisViewModel : ObservableObject
{
    public RelayCommand AddCommand { get; set; }
    public RelayCommand CalculateCommand { get; set; }
    public RelayCommand ChangeStats { get; set; }
    private GraphViewModel? _graphVm;

    public GraphViewModel? GraphVm
    {
        get => _graphVm;
        set
        {
            _graphVm = value;
            OnPropertyChanged();
        }
    }

    private string _addButtonVisibility;
    private string _secondLocationVisibility;
    private string _lat;
    private string _lon;    
    private string _lat2;
    private string _lon2;
    private int _pos;
    private string _label;
    private DateTime _startDate;
    private DateTime _endDate;
    private StatisticsData? _statistics;
    private StatisticsData[] _statisticsArray;
    private string[] _labels = {"First Location", "Second Location"};

    
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
    public string Label
    {
        get => _label;
        set
        {
            _label = value;
            OnPropertyChanged();
        }
    }    
    public StatisticsData? Statistics
    {
        get => _statistics;
        set
        {
            _statistics = value;
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

    public AnalysisViewModel(IRepository repository, IStatisticsCalc statisticsCalc)
    {
        _addButtonVisibility = "Visible";
        _secondLocationVisibility = "Collapsed";
        _lat = "";
        _lat2 = ""; 
        _lon = "";
        _lon2 = "";
        _pos = 0;
        _label = "";
        _statisticsArray = new StatisticsData[2];
        GraphVm = new GraphViewModel();
        AddCommand = new RelayCommand(_ =>
        {
            AddButtonVisibility = "Collapsed";
            SecondLocationVisibility = "Visible";
        });
        
        CalculateCommand = new  RelayCommand(async _ =>
        {
            try
            {
                var coordinates = new GeoCoordinates
                {
                    Lat = double.Parse(Lat, CultureInfo.InvariantCulture),
                    Lon = double.Parse(Lon, CultureInfo.InvariantCulture)
                };
                if (Lat2 != "" && Lon2 != "")
                {
                    var coordinates2 = new GeoCoordinates
                    {
                        Lat = double.Parse(Lat2, CultureInfo.InvariantCulture),
                        Lon = double.Parse(Lon2, CultureInfo.InvariantCulture)
                    };
                    var data1 = await repository.GetHistoricalData(coordinates, StartDate, EndDate);
                    var data2 = await repository.GetHistoricalData(coordinates2, StartDate, EndDate);
                    GraphVm.SetBothData(data1, data2);
                }
                else
                {
                    var data1 = await repository.GetHistoricalData(coordinates, StartDate, EndDate);
                    GraphVm.CurrentGraphData = data1;
                }
                
                SetStats(statisticsCalc);
                //Data received 
            }
            catch (Exception)
            {
                // invalid format
            }
        });

        ChangeStats = new RelayCommand(_ =>
        {
            _pos = _pos == 0 ? 1 : 0;
            Statistics = _statisticsArray[_pos];
            Label = _labels[_pos];
        });
    }

    private void SetStats(IStatisticsCalc statisticsCalc)
    {
        _statisticsArray[0] = statisticsCalc.GetStatistics((float[])GraphVm?.GetCurrentData(true)!, GraphVm?.CurrentGraphData?.HourlyWeatherInfo?.Time!);
        if(GraphVm?.CurrentGraphData2 != null)
            _statisticsArray[1] = statisticsCalc.GetStatistics((float[])GraphVm?.GetCurrentData(false)!, GraphVm?.CurrentGraphData2?.HourlyWeatherInfo?.Time!);
        Statistics = _statisticsArray[_pos];
        Label = _labels[_pos];
    }
    
}