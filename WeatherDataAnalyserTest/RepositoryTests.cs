using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
namespace WeatherDataAnalyserTest;
using WeatherDataAnalyser.MVVM.Model;

public class RepositoryTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task TestGetHistoricalData_Success()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"hourly\": {\"temperature_2m\": [25.5]}}", Encoding.UTF8, "application/json")
            });

        var client = new HttpClient(mockHttpMessageHandler.Object);
        var repository = new Repository(client);

        var coordinates = new GeoCoordinates() { Lat = 12.34, Lon = 56.78 };
        var startDate = new DateTime(2022, 01, 01);
        var endDate = new DateTime(2022, 01, 05);

        // Act
        var data = await repository.GetHistoricalData(coordinates, startDate, endDate);

        // Assert
        Assert.That(data, Is.Not.Null);
        Assert.That(data?.HourlyWeatherInfo.Temperature[0], Is.EqualTo(25.5));
    }
    
    [Test]
    public async Task TestGetHistoricalData_BadRequest()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("Invalid coordinates")
            });

        var client = new HttpClient(mockHttpMessageHandler.Object);
        var repository = new Repository(client);

        var coordinates = new GeoCoordinates() { Lat = -91, Lon = 56.78 };
        var startDate = new DateTime(2022, 01, 01);
        var endDate = new DateTime(2022, 01, 05);

        // Act
        var data = await repository.GetHistoricalData(coordinates, startDate, endDate);

        // Assert
        Assert.That(data, Is.Null);
    }
    
    [Test]
    public async Task TestGetHistoricalData_ServerError()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("Server Error")
            });

        var client = new HttpClient(mockHttpMessageHandler.Object);
        var repository = new Repository(client);

        var coordinates = new GeoCoordinates() { Lat = 12.34, Lon = 56.78 };
        var startDate = new DateTime(2022, 01, 01);
        var endDate = new DateTime(2022, 01, 05);

        // Act
        var data = await repository.GetHistoricalData(coordinates, startDate, endDate);

        // Assert
        Assert.That(data, Is.Null);
    }
    
    [Test]
    public async Task TestGetHistoricalData_Success_AllPropertiesReturned()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"hourly\": {\"time\": [\"2022-01-01T00:00:00\", \"2022-01-01T01:00:00\", \"2022-01-01T02:00:00\"], \"temperature_2m\": [25.5, 26.5, 27.5], \"relativehumidity_2m\": [60, 65, 70], \"pressure_msl\": [1013, 1015, 1017], \"rain\": [0, 0.1, 0.2], \"windspeed_10m\": [5, 6, 7]}}", Encoding.UTF8, "application/json")
            });

        var client = new HttpClient(mockHttpMessageHandler.Object);
        var repository = new Repository(client);

        var coordinates = new GeoCoordinates() { Lat = 12.34, Lon = 56.78 };
        var startDate = new DateTime(2022, 01, 01);
        var endDate = new DateTime(2022, 01, 05);

        // Act
        var data = await repository.GetHistoricalData(coordinates, startDate, endDate);

        // Assert
        Assert.That(data, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(data?.HourlyWeatherInfo.Temperature, Is.EqualTo(new[] { 25.5f, 26.5f, 27.5f }));
            Assert.That(data?.HourlyWeatherInfo.Humidity, Is.EqualTo(new[] { 60f, 65f, 70f }));
            Assert.That(data?.HourlyWeatherInfo.Pressure, Is.EqualTo(new[] { 1013f, 1015f, 1017f }));
            Assert.That(data?.HourlyWeatherInfo.Rain, Is.EqualTo(new[] { 0f, 0.1f, 0.2f }));
            Assert.That(data?.HourlyWeatherInfo.WindSpeed, Is.EqualTo(new[] { 5f, 6f, 7f }));
        });
    }
}