using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WeatherDataAnalyser.MVVM.Model;

public class StatisticsCalc : IStatisticsCalc
{
    public StatisticsData GetStatistics(float[] data, DateTime[] dates)
    {
        var result = new StatisticsData
        {
            Max = Math.Round(data.Max(), 2),
            MaxDate = dates[MaxIndex(data)].ToShortDateString(),
            Min = Math.Round(data.Min(), 2),
            MinDate = dates[MinIndex(data)].ToShortDateString(),
            Avg = Math.Round(data.Average(), 2),
            Median = Math.Round(Median(data), 2),
            StdDev = Math.Round(StandardDeviation(data), 2),
            Variance = Math.Round(Variance(data), 2),
            Dominant = Math.Round(Mode(data), 2),
            AsymmetryFactor = Math.Round(AsymmetryFactor(data), 2)
        };

        return result;
    }
    
    public double CorrelationCoefficient(float[] data1, float[] data2) {
        if (data1.Length != data2.Length)
            throw new ArgumentException("Arrays must be of the same length");

        var mean1 = data1.Average();
        var mean2 = data2.Average();
        var std1 = StandardDeviation(data1);
        var std2 = StandardDeviation(data2);
        var result = data1.Select((x, i) => (x - mean1) * (data2[i] - mean2)).Sum() / (data1.Length * std1 * std2);

        return Math.Round(result, 2);
    }
    
    private static double Median(IReadOnlyCollection<float> data)
    {
        var ordered = data.OrderBy(x => x)
            .ToArray();
        var mid = data.Count / 2;
        if (data.Count % 2 == 0) {
            return (ordered.ElementAt(mid - 1) + ordered.ElementAt(mid)) / 2;
        }
        return ordered.ElementAt(mid);
    }
    
    private static double StandardDeviation(float[] array) {
        return Math.Sqrt(Variance(array));
    }    
    
    private static double Variance(float[] array) {
        double mean = array.Average();
        return array.Select(x => (x - mean) * (x - mean))
            .Average();
    }    
    
    private static double Mode(IEnumerable<float> array) {
        // precyzja do dwoch miejsc po przecinku
        return array.Select(x => Math.Round(x, 2))
            .GroupBy(x => x)
            .OrderByDescending(g => g.Count())
            .First()
            .ToArray()
            .First();
    }
    
    private static double AsymmetryFactor(float[] array) {
        var mean = array.Average();
        var std = StandardDeviation(array);
        return array.Select(x => (x - mean) * (x - mean) * (x - mean))
            .Sum() / (std * std * std * array.Length);
    } 
    
    private static int MaxIndex(float[] array) {
        return array.AsSpan().IndexOf(array.Max());
    }    
    
    private static int MinIndex(float[] array) {
        return array.AsSpan().IndexOf(array.Min());
    }

}