using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using NUnit.Framework;
using RainfallApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;

namespace RainfallApi.Tests
{
    [TestFixture]
    public class RainfallControllerTests : WebApplicationFactory<RainfallApi.Startup>
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _client = CreateClient();
        }

        [Test]
        [TestCase("valid_station_id", 10)] 
        [TestCase("another_valid_station_id", 5)]
        public async Task GetRainfallReadings_ValidRequest_Returns200AndRainfallResponse()
        {
            // Arrange
            var stationId = "your_station_id";
            var count = new ReadingFilter(); 

            // Act
            var response = await _client.GetAsync($"/api/Rainfall/id/{stationId}/readings/?count={count}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var rainfallResponse = await response.Content.ReadFromJsonAsync<RainfallReadingResponse>();
            Assert.IsNotNull(rainfallResponse);
        }

        [Test]
        [TestCase("invalid_station_id", 0)] 
        [TestCase("another_invalid_station_id", 101)]
        public async Task GetRainfallReadings_InvalidRequest_Returns400AndErrorResponse()
        {
            // Arrange
            var stationId = "invalid_station_id";
            var count = new ReadingFilter();

            // Act
            var response = await _client.GetAsync($"/api/Rainfall/id/{stationId}/readings/?count={count}");

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            Assert.IsNotNull(errorResponse);
        }
    }
}
