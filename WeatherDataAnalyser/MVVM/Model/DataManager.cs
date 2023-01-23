using System;
using System.Data;
using System.Linq;

namespace WeatherDataAnalyser.MVVM.Model;

public class DataManager : IDataManager
{
    private struct DataRow
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
            dataRows[i] = new DataRow { Time = data.HourlyWeatherInfo.Time[i] };
            if (valuesIncluded[1])
                dataRows[i].Temperature = data.HourlyWeatherInfo.Temperature[i];
            if (valuesIncluded[2])
                dataRows[i].Pressure = data.HourlyWeatherInfo.Pressure[i];
            if (valuesIncluded[3])
                dataRows[i].Rain = data.HourlyWeatherInfo.Rain[i];
            if (valuesIncluded[4])
                dataRows[i].WindSpeed = data.HourlyWeatherInfo.WindSpeed[i];
            if (valuesIncluded[5])
                dataRows[i].Humidity = data.HourlyWeatherInfo.Humidity[i];
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

        return Newtonsoft.Json.JsonConvert.SerializeObject(dataRows);

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

        