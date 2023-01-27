using System;

namespace WeatherDataAnalyserTest;
using WeatherDataAnalyser.MVVM.Model;

[TestFixture]
public class StatisticsCalcTests
{
    private StatisticsCalc _statisticsCalc = null!;
    private float[] _data = null!;
    

    [SetUp]
    public void Setup()
    {
        _statisticsCalc = new StatisticsCalc();
        _data = new float[] { 1, 2, 3, 4, 5 };
    }

    [Test]
    public void CorrelationCoefficient_ValidInput_ShouldReturnExpectedData()
    {
        // Arrange
        var data1 = new float[] { 1, 2, 3, 4, 5 };
        var data2 = new float[] { 5, 4, 3, 2, 1 };

        // Act
        var result = _statisticsCalc.CorrelationCoefficient(data1, data2);

        // Assert
        Assert.That(result, Is.EqualTo(-1));
    }

    [Test]
    public void Median_ValidInput_ShouldReturnExpectedData()
    {
        // Act
        var result = _statisticsCalc.Median(_data);

        // Assert
        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public void StandardDeviation_ValidInput_ShouldReturnExpectedData()
    {
        // Act
        var result = _statisticsCalc.StandardDeviation(_data);

        // Assert
        Assert.That(Math.Round(result, 2), Is.EqualTo(1.41));
    }

    [Test]
    public void Variance_ValidInput_ShouldReturnExpectedData()
    {
        // Act
        var result = _statisticsCalc.Variance(_data);

        // Assert
        Assert.That(result, Is.EqualTo(2.0));
    }

    [Test]
    public void Mode_ValidInput_ShouldReturnExpectedData()
    {
        // Arrange
        var data = new float[] { 1, 2, 3, 3, 4, 5 };
   
        // Act
        var result = _statisticsCalc.Mode(data);

        // Assert
        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public void AsymmetryFactor_ValidInput_ShouldReturnExpectedData()
    {
        // Act
        var result = _statisticsCalc.AsymmetryFactor(_data);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void MaxIndex_ValidInput_ShouldReturnExpectedData()
    {
        // Act
        var result = _statisticsCalc.MaxIndex(_data);

        // Assert
        Assert.That(result, Is.EqualTo(4));
    }

    [Test]
    public void MinIndex_ValidInput_ShouldReturnExpectedData()
    {
        // Act
        var result = _statisticsCalc.MinIndex(_data);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void GetStatistics_ValidInput_ShouldReturnExpectedData()
    {
        // Arrange
        var data = new float[] { 1, 2, 3, 3, 4, 5 };
        var dates = new DateTime[] { 
            new DateTime(2020, 1, 1), 
            new DateTime(2020, 2, 1), 
            new DateTime(2020, 3, 1), 
            new DateTime(2020, 4, 1), 
            new DateTime(2020, 5, 1), 
            new DateTime(2020, 6, 1) 
        };

        // Act
        var result = _statisticsCalc.GetStatistics(data, dates);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Max, Is.EqualTo(5));
            Assert.That(result.MaxDate, Is.EqualTo("01.06.2020"));
            Assert.That(result.Min, Is.EqualTo(1));
            Assert.That(result.MinDate, Is.EqualTo("01.01.2020"));
            Assert.That(result.Avg, Is.EqualTo(3));
            Assert.That(result.Median, Is.EqualTo(3));
            Assert.That(result.StdDev, Is.EqualTo(1.29d));
            Assert.That(result.Variance, Is.EqualTo(1.67d));
            Assert.That(result.Dominant, Is.EqualTo(3));
            Assert.That(result.AsymmetryFactor, Is.EqualTo(0));
        });
    }
}

