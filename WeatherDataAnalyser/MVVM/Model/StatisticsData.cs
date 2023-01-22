using System;

namespace WeatherDataAnalyser.MVVM.Model;

public class StatisticsData
{
    public double Max { get; set; }
    public string? MaxDate { get; set; }
    public double Min { get; set; }
    public string? MinDate { get; set; }
    public double Avg { get; set; }
    public double Median { get; set; }
    public double StdDev { get; set; }
    public double Variance { get; set; }
    public double Dominant { get; set; }
    public double AsymmetryFactor { get; set; }
}