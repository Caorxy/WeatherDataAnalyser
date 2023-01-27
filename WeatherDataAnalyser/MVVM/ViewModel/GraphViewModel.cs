using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WeatherDataAnalyser.Core;
using LiveChartsCore;
using LiveChartsCore.Easing;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using WeatherDataAnalyser.MVVM.Model;

namespace WeatherDataAnalyser.MVVM.ViewModel;

public class GraphViewModel : ObservableObject
{
    public readonly string[] Titles = { "Temperature", "Pressure", "Wind", "Humidity", "Rain" };
    private List<ISeries> _series;
    public int Pos;
    private string _title;
    private string _isNoteVisible;
    private bool _areLinesVisible;
    private bool _areColumnsVisible;
            
    public IEnumerable<float>? GetCurrentData(bool first)
    {
        var pos = first ? Pos : Pos + 5;
        return pos switch
        {
            0 => CurrentGraphData?.HourlyWeatherInfo.Temperature!,
            1 => CurrentGraphData?.HourlyWeatherInfo.Pressure!,
            2 => CurrentGraphData?.HourlyWeatherInfo.WindSpeed!,
            3 => CurrentGraphData?.HourlyWeatherInfo.Humidity!,
            4 => CurrentGraphData?.HourlyWeatherInfo.Rain!,
            5 => CurrentGraphData2?.HourlyWeatherInfo.Temperature!,
            6 => CurrentGraphData2?.HourlyWeatherInfo.Pressure!,
            7 => CurrentGraphData2?.HourlyWeatherInfo.WindSpeed!,
            8 => CurrentGraphData2?.HourlyWeatherInfo.Humidity!,
            9 => CurrentGraphData2?.HourlyWeatherInfo.Rain!,
            _ => null
        };
    }

    public RelayCommand LinesVisibilityChange { get; set; }
    public RelayCommand ColumnsVisibilityChange { get; set; }

    private HistoricalData? _currentGraphData;
    private HistoricalData? _currentGraphData2;

    private Axis[] _xaxes;

    public Axis[] XAxes
    {
        get => _xaxes;
        set
        {
            _xaxes = value;
            OnPropertyChanged();
        }
    }
    public HistoricalData? CurrentGraphData
    {
        get => _currentGraphData;
        set
        {
            _currentGraphData = value;
            OnGraphPropertyChanged();
        }
    }    
    
    public HistoricalData? CurrentGraphData2
    {
        get => _currentGraphData2;
        set
        {
            _currentGraphData2 = value;
            OnGraphPropertyChanged();
        }
    }

    public void SetBothData(HistoricalData? data1, HistoricalData? data2)
    {
        _currentGraphData = data1;
        CurrentGraphData2 = data2;
    }

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }
    public string IsNoteVisible
    {
        get => _isNoteVisible;
        set
        {
            _isNoteVisible = value;
            OnPropertyChanged();
        }
    }

    public List<ISeries> Series
    {
        get => _series;
        set
        {
            _series = value;
            OnPropertyChanged();
        }
    }

    public GraphViewModel()
    {
        var values1 = new List<double>();
        var values2 = new List<double>();

        var columnSeries1 = new ColumnSeries<double>
        {
            Values = values1,
            Stroke = null,
            Padding = 2
        };
        var columnSeries2 = new ColumnSeries<double>
        {
            Values = values2,
            Stroke = null,
            Padding = 2
        };

        columnSeries1.PointMeasured += OnPointMeasured;
        columnSeries2.PointMeasured += OnPointMeasured;

        Pos = 0;
        _title = Titles[Pos];
        _isNoteVisible = "Collapsed";
        _series = new List<ISeries> { columnSeries1 };
        _xaxes = Array.Empty<Axis>();
        _areLinesVisible = false;
        _areColumnsVisible = true;

        
        LinesVisibilityChange = new RelayCommand(_ =>
        {
            _areLinesVisible = _areLinesVisible != true;
            OnGraphPropertyChanged();
        });    
        
        ColumnsVisibilityChange = new RelayCommand(_ =>
        {
            _areColumnsVisible = _areColumnsVisible != true;
            OnGraphPropertyChanged();
        });
    }

    private void OnPointMeasured(ChartPoint<double, RoundedRectangleGeometry, LabelGeometry> point)
    {
        var visual = point.Visual;
        if (visual is null) return;

        var delayedFunction = new DelayedFunction(EasingFunctions.BuildCustomElasticOut(1.5f, 0.60f), point, 0.01f);

        _ = visual
            .TransitionateProperties(
                nameof(visual.Y),
                nameof(visual.Height))
            .WithAnimation(animation =>
                animation
                    .WithDuration(delayedFunction.Speed)
                    .WithEasingFunction(delayedFunction.Function));
    }

    public void OnGraphPropertyChanged()
    {
        var values1 = new List<double>();
        var values2 = new List<double>();
        var dateValues = new List<string>();
        var count = CurrentGraphData?.HourlyWeatherInfo.Time.Length;
        var data = GetCurrentData(true) as float[] ?? (GetCurrentData(true) ?? Array.Empty<float>()).ToArray();
        var data2 = GetCurrentData(false) as float[] ?? (GetCurrentData(false) ?? Array.Empty<float>()).ToArray();
        
        switch (count)
        {
            case > 8000:
                IsNoteVisible = "Visible";
                CalculateAvg(data, data2, values1, values2, dateValues, 24*7);
                break;
            case > 800:
                IsNoteVisible = "Visible";
                CalculateAvg(data, data2, values1, values2, dateValues, 24);
                break;
            default:
            {
                IsNoteVisible = "Collapsed";
                for (var i=0; i < data.Length; i++)
                {
                    values1.Add(Math.Round(data[i], 2));
                    if(data2.Length != 0)
                        values2.Add(Math.Round(data2[i],2));
                    var date = CurrentGraphData?.HourlyWeatherInfo?.Time[i].ToString(CultureInfo.InvariantCulture);
                    date = date?[..^3];
                    dateValues.Add(date!);
                }

                break;
            }
        }

        
        var columnSeries = new ColumnSeries<double>
        {
            Values = values1,
            Stroke = null,
            Name = "First Location",
            IsHoverable = !_areLinesVisible,
            Padding = 2,
            Fill = new SolidColorPaint(SKColors.DarkBlue),
            IsVisible = _areColumnsVisible
        };       
        
        var lineSeries = new LineSeries<double>
        {
            Values = values1,
            Stroke = new SolidColorPaint(SKColors.DarkBlue) {StrokeThickness = 2},
            Name = "First Location",
            GeometryStroke = null,
            GeometryFill = null,
            Fill = null,
            IsVisible = _areLinesVisible
        };    
        
        var lineSeries2 = new LineSeries<double>
        {
            Values = values2,
            Stroke = new SolidColorPaint(SKColors.DarkRed) {StrokeThickness = 2},
            Name = "Second Location",
            GeometryStroke = null,
            GeometryFill = null,
            Fill = null,
            IsVisible = _areLinesVisible && CurrentGraphData2 != null
        };
        
        var columnSeries2 = new ColumnSeries<double>
        {
            Values = values2,
            Name = "Second Location",
            Stroke = null,
            IsHoverable = !_areLinesVisible,
            Fill = new SolidColorPaint(SKColors.DarkRed),
            Padding = 2,
            IsVisible = _areColumnsVisible && CurrentGraphData2 != null
        };
        
        XAxes = new []
        {
            new Axis
            {
                Name = "Date",
                Labels = dateValues,
                TextSize = 8,
                LabelsPaint = new SolidColorPaint(SKColors.White)
            }
        };


        columnSeries.PointMeasured += OnPointMeasured;
        columnSeries2.PointMeasured += OnPointMeasured;
        Series = new List<ISeries> { lineSeries, lineSeries2, columnSeries, columnSeries2 };
    }
    
    private void CalculateAvg(IReadOnlyList<float> data, IReadOnlyList<float> data2, ICollection<double> values1, ICollection<double> values2, ICollection<string> dateValues, int divider) 
    {
        float dailyAvg = 0;
        float dailyAvg2 = 0;
        var date = "";
        var n = divider;
        for (var i=0; i < data.Count; i++)
        {
            dailyAvg += data[i];
            if(data2.Count != 0)
                dailyAvg2 += data2[i >= data2.Count? 0 : i];
            n--;
            if (n == divider/2){
                date = CurrentGraphData?.HourlyWeatherInfo?.Time[i].ToString(CultureInfo.InvariantCulture);
                date = date![..^9];
            }

            if (n != 0) continue;
            values1.Add(Math.Round(dailyAvg / divider, 2));
            values2.Add(Math.Round(dailyAvg2 / divider, 2));
            dateValues.Add(date);
            dailyAvg = 0;
            dailyAvg2 = 0;
            date = "";
            n = divider;
        }
    }
    
}

