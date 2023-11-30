    using Microsoft.AspNetCore.Mvc;
    using System.Net.Http;
    using System.Threading.Tasks;
    using RainfallApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace RainfallApi
{

    /// <summary>
    /// 
    /// </summary>
    /// <response code="200">A list of rainfall readings successfully retrieved</response>
    /// <response code="400">Invalid request</response>
    /// <response code="404">No readings found for the specified stationId</response>
    /// <response code="500">Internal server error</response>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class RainfallController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public RainfallController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("id/{stationId}/readings/")]
        [SwaggerOperation(summary: "Get rainfall readings by station Id")]
        [SwaggerResponse(statusCode: 200, description: "A list of rainfall readings successfully retrieved",type:typeof(RainfallReadingResponse))]
        [SwaggerResponse(statusCode: 400, description: "Invalid Request", type: typeof(ErrorResponse))]
        [SwaggerResponse(statusCode: 404, description: "No readings found for the specified stationId", type: typeof(ErrorResponse))]
        [SwaggerResponse(statusCode: 500, description: "Internal server error", type: typeof(ErrorResponse))]
        public async Task<IActionResult> GetRainfallReadings(string stationId, [FromQuery] ReadingFilter count)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://environment.data.gov.uk/flood-monitoring/id/stations/{stationId}/readings?count={count}");

                response.EnsureSuccessStatusCode();

                var rainfallResponse = await response.Content.ReadFromJsonAsync<RainfallReadingResponse>();

                return Ok(rainfallResponse);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new ErrorResponse { message = "Internal server error", detail = { new ErrorDetail { propertyName = "HttpRequest", message = ex.Message } } });
            }
        }
    }

}
