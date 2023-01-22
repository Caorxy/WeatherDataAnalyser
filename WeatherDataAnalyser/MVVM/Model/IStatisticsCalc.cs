using System;
using System.Collections.Generic;

namespace WeatherDataAnalyser.MVVM.Model;

public interface IStatisticsCalc
{
    public StatisticsData GetStatistics(float[] data, DateTime[] dates);
}