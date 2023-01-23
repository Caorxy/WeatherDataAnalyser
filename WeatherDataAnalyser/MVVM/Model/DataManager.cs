using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherDataAnalyser.MVVM.Model;

public class DataManager : IDataManager
{
    public struct DataRow
    {
        public DateTime Time { get; set; }
        public float Temperature { get; set; }
        public float Pressure { get; set; }
        public float Rain { get; set; }
        public float WindSpeed { get; set; }
        public float Humidity { get; set; }
    }


    public string PrepareToSave(HistoricalData? data, bool[] valuesIncluded, string sortBy, bool isAscending)
    {
        if (data == null) return "";
        var dataRows = new DataRow[data.HourlyWeatherInfo.Time.Length];
        for (var i = 0; i < data.HourlyWeatherInfo.Time.Length; i++)
        {
            dataRows[i] = new DataRow
            {
                Time = data.HourlyWeatherInfo.Time[i],
                Temperature = data.HourlyWeatherInfo.Temperature[i],
                Pressure = data.HourlyWeatherInfo.Pressure[i],
                Rain = data.HourlyWeatherInfo.Rain[i],
                WindSpeed = data.HourlyWeatherInfo.WindSpeed[i],
                Humidity = data.HourlyWeatherInfo.Humidity[i]
            };
        }

        dataRows = sortBy switch
        {
            "Time" => isAscending
                ? dataRows.OrderBy(x => x.Time).ToArray()
                : dataRows.OrderByDescending(x => x.Time).ToArray(),
            "Temperature" => isAscending
                ? dataRows.OrderBy(x => x.Temperature).ToArray()
                : dataRows.OrderByDescending(x => x.Temperature).ToArray(),
            "Pressure" => isAscending
                ? dataRows.OrderBy(x => x.Pressure).ToArray()
                : dataRows.OrderByDescending(x => x.Pressure).ToArray(),
            "Rain" => isAscending
                ? dataRows.OrderBy(x => x.Rain).ToArray()
                : dataRows.OrderByDescending(x => x.Rain).ToArray(),
            "WindSpeed" => isAscending
                ? dataRows.OrderBy(x => x.WindSpeed).ToArray()
                : dataRows.OrderByDescending(x => x.WindSpeed).ToArray(),
            "Humidity" => isAscending
                ? dataRows.OrderBy(x => x.Humidity).ToArray()
                : dataRows.OrderByDescending(x => x.Humidity).ToArray(),
            _ => dataRows
        };

        return ConvertToJsonCustom(dataRows, valuesIncluded);

    }

    private static string ConvertToJsonCustom(IReadOnlyList<DataRow> dataRows, IReadOnlyList<bool> valuesIncluded)
    {
        var jsonString = new StringBuilder("[");
        for (var i = 0; i < dataRows.Count; i++)
        {
            jsonString.Append('{');
            if (valuesIncluded[0])
                jsonString.Append("\"Time\":\"" + dataRows[i].Time + "\",");
            if (valuesIncluded[1])
                jsonString.Append("\"Temperature\":" + dataRows[i].Temperature + ",");
            if (valuesIncluded[2])
                jsonString.Append("\"Pressure\":" + dataRows[i].Pressure + ",");
            if (valuesIncluded[3])
                jsonString.Append("\"Rain\":" + dataRows[i].Rain + ",");
            if (valuesIncluded[4])
                jsonString.Append("\"WindSpeed\":" + dataRows[i].WindSpeed + ",");
            if (valuesIncluded[5])
                jsonString.Append("\"Humidity\":" + dataRows[i].Humidity);
            jsonString.Append('}');
            if (i < dataRows.Count - 1)
                jsonString.Append(',');
        }
        jsonString.Append(']');
        return jsonString.ToString();
    }
    

    public bool WriteToFile(string text)
    {
        try
        {
            System.IO.File.WriteAllText("../../../data.txt", text);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}

        