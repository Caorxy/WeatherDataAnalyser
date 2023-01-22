using System;
using System.Globalization;
using WeatherDataAnalyser.Core;
using WeatherDataAnalyser.MVVM.Model;

namespace WeatherDataAnalyser.MVVM.ViewModel;

public class PredictionsViewModel : ObservableObject
{
    private string _lat;
    private string _lon;  
    private StatisticsData? _statistics;
    public RelayCommand Next { get; set; }
    public RelayCommand Back { get; set; }

    public RelayCommand CalculateCommand { get; set; }
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
    
    public StatisticsData? Statistics
    {
        get => _statistics;
        set
        {
            _statistics = value;
            OnPropertyChanged();
        }
    }
    public PredictionsViewModel(IRepository repository, IStatisticsCalc statisticsCalc)
    {
        _lat = "";
        _lon = "";
        GraphVm = new GraphViewModel();

        CalculateCommand = new  RelayCommand(async _ =>
        {
            try
            {
                var coordinates = new GeoCoordinates
                {
                    Lat = double.Parse(Lat, CultureInfo.InvariantCulture),
                    Lon = double.Parse(Lon, CultureInfo.InvariantCulture)
                };
                var data1 = await repository.GetHistoricalData(coordinates, new DateTime(2003, 1, 1), DateTime.Now.AddDays(-7));
                GraphVm.CurrentGraphData = data1;
                
                
                SetStats(statisticsCalc);
                //Data received 
            }
            catch (Exception)
            {
                // invalid format
            }
        });
        
        Next = new RelayCommand(_ =>
        {
            GraphVm.Pos = (GraphVm.Pos + 1) % 5;
            GraphVm.Title = GraphVm.Titles[GraphVm.Pos];
            GraphVm.OnGraphPropertyChanged();
            SetStats(statisticsCalc);
        });

        Back = new RelayCommand(_ =>
        {
            GraphVm.Pos = GraphVm.Pos == 0 ? 4 : GraphVm.Pos - 1;
            GraphVm.Title = GraphVm.Titles[GraphVm.Pos];
            GraphVm.OnGraphPropertyChanged();
            SetStats(statisticsCalc);
        });
    }

    private void SetStats(IStatisticsCalc statisticsCalc)
    {
        if (GraphVm?.CurrentGraphData != null)
            Statistics = statisticsCalc.GetStatistics((float[])GraphVm?.GetCurrentData(true)!, GraphVm?.CurrentGraphData?.HourlyWeatherInfo?.Time!);
    }
}