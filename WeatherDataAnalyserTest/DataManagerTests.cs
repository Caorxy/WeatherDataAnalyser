using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeatherDataAnalyserTest;
using WeatherDataAnalyser.MVVM.Model;

[TestFixture]
public class DataManagerTests
{
    private IDataManager _dataManager = null!;
    private HistoricalData? _data;

    [SetUp]
    public void SetUp()
    {
        _dataManager = new DataManager();
        _data = new HistoricalData
        {
            HourlyWeatherInfo = new HourlyWeatherData
            {
                Time = new[] { new DateTime(2022, 1, 1, 1, 0, 0), new DateTime(2022, 1, 1, 2, 0, 0), new DateTime(2022, 1, 1, 3, 0, 0) },
                Temperature = new float[] { 20, 25, 30 },
                Pressure = new float[] { 1000, 900, 1050 },
                Rain = new float[] { 0, 0, 2 },
                WindSpeed = new float[] { 10, 15, 11 },
                Humidity = new float[] { 50, 60, 100 }
            }
        };
    }

    [Test]
    public void PrepareToSave_WhenDataIsNull_ShouldReturnEmptyString()
    {
        var result = _dataManager.PrepareToSave(null, new[] { true, true, true, true, true, true }, "Time", true);
        Assert.That(result, Is.EqualTo(string.Empty));
    }
    
    [Test]
    public void PrepareToSave_WhenValuesIncludedIsFalse_ShouldReturnJsonWithoutIncludedValues()
    {
        var result = _dataManager.PrepareToSave(_data, new[] { false, false, false, false, false, false }, "Time", true);
        Assert.That(result, Is.EqualTo("[{},{},{}]"));
    }

    [Test]
    public void PrepareToSave_WhenSortByTemperatureDescending_ShouldReturnSortedData() 
    {
        var result = _dataManager.PrepareToSave(_data, new[] { true, true, true, true, true, true }, "Temperature", false);

        var jsonData = JsonConvert.DeserializeObject<List<DataManager.DataRow>>(result);
        Assert.Multiple(() =>
        {
            Assert.That(jsonData, Is.Not.Null);
            Assert.That(jsonData![0].Temperature, Is.EqualTo(30));
            Assert.That(jsonData[1].Temperature, Is.EqualTo(25));
            Assert.That(jsonData[2].Temperature, Is.EqualTo(20));
        });
    }

    [Test]
    public void PrepareToSave_WhenSortByTimeAscending_ShouldReturnSortedData()
    {
        var result = _dataManager.PrepareToSave(_data, new[] { true, true, true, true, true, true }, "Time", true);

        var jsonData = JsonConvert.DeserializeObject<List<DataManager.DataRow>>(result);
        Assert.Multiple(() =>
        {
            Assert.That(jsonData, Is.Not.Null);
            Assert.That(jsonData![0].Time, Is.EqualTo(new DateTime(2022, 1, 1, 1, 0, 0)));
            Assert.That(jsonData[1].Time, Is.EqualTo(new DateTime(2022, 1, 1, 2, 0, 0)));
            Assert.That(jsonData[2].Time, Is.EqualTo(new DateTime(2022, 1, 1, 3, 0, 0)));
        });
    }
    
    [Test]
    public void PrepareToSave_WhenSortByPressureAscending_ShouldReturnSortedData()
    {
        var result = _dataManager.PrepareToSave(_data, new[] { true, true, true, true, true, true }, "Pressure", true);

        var jsonData = JsonConvert.DeserializeObject<List<DataManager.DataRow>>(result);
        Assert.Multiple(() =>
        {
            Assert.That(jsonData, Is.Not.Null);
            Assert.That(jsonData![0].Pressure, Is.EqualTo(900));
            Assert.That(jsonData[1].Pressure, Is.EqualTo(1000));
            Assert.That(jsonData[2].Pressure, Is.EqualTo(1050));
        });
    }

    [Test]
    public void PrepareToSave_WhenSortByRainDescending_ShouldReturnSortedData()
    {
        var result = _dataManager.PrepareToSave(_data, new[] { true, true, true, true, true, true }, "Rain", false);

        var jsonData = JsonConvert.DeserializeObject<List<DataManager.DataRow>>(result);
        Assert.Multiple(() =>
        {
            Assert.That(jsonData, Is.Not.Null);
            Assert.That(jsonData![0].Rain, Is.EqualTo(2));
            Assert.That(jsonData[1].Rain, Is.EqualTo(0));
            Assert.That(jsonData[2].Rain,Is.EqualTo(0));
        });
    }
    
    [Test]
    public void PrepareToSave_WhenSortByWindSpeedAscending_ShouldReturnSortedData()
    {
        var result = _dataManager.PrepareToSave(_data, new[] { true, true, true, true, true, true }, "WindSpeed", true);

        var jsonData = JsonConvert.DeserializeObject<List<DataManager.DataRow>>(result);
        Assert.Multiple(() =>
        {
            Assert.That(jsonData, Is.Not.Null);
            Assert.That(jsonData![0].WindSpeed, Is.EqualTo(10));
            Assert.That(jsonData[1].WindSpeed, Is.EqualTo(11));
            Assert.That(jsonData[2].WindSpeed, Is.EqualTo(15));
        });
    }

    [Test]
    public void PrepareToSave_WhenSortByHumidityDescending_ShouldReturnSortedData()
    {
        var result = _dataManager.PrepareToSave(_data, new[] { true, true, true, true, true, true }, "Humidity", false);

        var jsonData = JsonConvert.DeserializeObject<List<DataManager.DataRow>>(result);
        Assert.Multiple(() =>
        {
            Assert.That(jsonData, Is.Not.Null);
            Assert.That(jsonData![0].Humidity, Is.EqualTo(100));
            Assert.That(jsonData[1].Humidity, Is.EqualTo(60));
            Assert.That(jsonData[2].Humidity, Is.EqualTo(50));
        });
    }


}
