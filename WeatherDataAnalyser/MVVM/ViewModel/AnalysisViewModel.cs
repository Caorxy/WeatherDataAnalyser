﻿using System;
using System.Globalization;
using WeatherDataAnalyser.Core;
using WeatherDataAnalyser.MVVM.Model;

namespace WeatherDataAnalyser.MVVM.ViewModel;

public class AnalysisViewModel : AbstractViewModel
{
    public RelayCommand AddCommand { get; set; }
    public RelayCommand CalculateCommand { get; set; }
    public RelayCommand ChangeStats { get; set; }
    public RelayCommand Next { get; set; }
    public RelayCommand Back { get; set; }
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
    private string _statsVisibility;
    private string _lat2;
    private string _lon2;
    private int _pos;
    private string _label;
    private double _correlation;
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
    public string StatsVisibility
    {
        get => _statsVisibility;
        set
        {
            _statsVisibility = value;
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
    
    
    public string Lat2
    {
        get => _lat2;
        set
        {
            _lat2 = value;
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
    
    public double Correlation
    {
        get => _correlation;
        set
        {
            _correlation = value;
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

    public AnalysisViewModel(IRepository repository, IStatisticsCalc statisticsCalc)
    {
        _addButtonVisibility = "Visible";
        _secondLocationVisibility = "Collapsed";
        _statsVisibility = "Collapsed";
        _lat2 = ""; 
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
                StatsVisibility = "Visible";
                //Data received 
            }
            catch (Exception)
            {
                // invalid format
            }
        });
        
        Next = new RelayCommand(_ =>
        {
            StatsVisibility = "Collapsed";
            GraphVm.Pos = (GraphVm.Pos + 1) % 5;
            GraphVm.Title = GraphVm.Titles[GraphVm.Pos];
            GraphVm.OnGraphPropertyChanged();
            SetStats(statisticsCalc);
            StatsVisibility = "Visible";
        });

        Back = new RelayCommand(_ =>
        {
            StatsVisibility = "Collapsed";
            GraphVm.Pos = GraphVm.Pos == 0 ? 4 : GraphVm.Pos - 1;
            GraphVm.Title = GraphVm.Titles[GraphVm.Pos];
            GraphVm.OnGraphPropertyChanged();
            SetStats(statisticsCalc);
            StatsVisibility = "Visible";
        });

        ChangeStats = new RelayCommand(_ =>
        {
            StatsVisibility = "Collapsed";
            _pos = _pos == 0 ? 1 : 0;
            Statistics = _statisticsArray[_pos];
            Label = _labels[_pos];
            StatsVisibility = "Visible";
        });
    }

    private void SetStats(IStatisticsCalc statisticsCalc)
    {
        if (GraphVm?.CurrentGraphData != null)
            _statisticsArray[0] = statisticsCalc.GetStatistics((float[])GraphVm?.GetCurrentData(true)!, GraphVm?.CurrentGraphData?.HourlyWeatherInfo.Time!);
        if (GraphVm?.CurrentGraphData2 != null)
        {
            _statisticsArray[1] = statisticsCalc.GetStatistics((float[])GraphVm?.GetCurrentData(false)!,
                GraphVm?.CurrentGraphData2?.HourlyWeatherInfo.Time!);

            Correlation = statisticsCalc.CorrelationCoefficient((float[])GraphVm?.GetCurrentData(true)!,
                (float[])GraphVm?.GetCurrentData(false)!);
        }

        Statistics = _statisticsArray[_pos];
        Label = _labels[_pos];
    }
    
}