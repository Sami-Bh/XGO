using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XGO.ApiGateway.Models;

namespace XGO.ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpenAIFunctionsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OpenAIFunctionsController> _logger;

        public OpenAIFunctionsController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<OpenAIFunctionsController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Returns the OpenAPI specification for GetExpiringItems function
        /// </summary>
        [HttpGet("schema")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult GetOpenApiSchema()
        {
            // Redirect to the Swaggergenerated OpenAPI endpoint
            // The auto-generated schema only includes get-expiring-items (other endpoints are hidden with ApiExplorerSettings)
            return Redirect("/swagger/v1/swagger.json");
        }

        /// <summary>
        /// Invokes the GetExpiringItems function with authentication
        /// </summary>
        [HttpGet("get-expiring-items")]
#if !DEBUG
        [Authorize]
#endif
        public async Task<IActionResult> GetExpiringItems([FromQuery] GetExpiringItemsRequest request)
        {
            try
            {
                var storageApiUrl = _configuration["ReverseProxy:Clusters:Storage_Cluster:Destinations:destination1:Address"];
                if (string.IsNullOrEmpty(storageApiUrl))
                {
                    _logger.LogError("Storage API URL not configured");
                    return StatusCode(500, new { error = "Storage API URL not configured" });
                }

                var httpClient = _httpClientFactory.CreateClient();

                // Forward authentication token
                if (Request.Headers.ContainsKey("Authorization"))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", Request.Headers["Authorization"].ToString());
                }

                // Build query string
                var queryParams = new List<string>();
                if (request.ExpiresInDays.HasValue)
                    queryParams.Add($"expiresInDays={request.ExpiresInDays.Value}");
                queryParams.Add($"includeAcknowledgedExpiredItems={request.IncludeAcknowledgedExpiredItems}");
                queryParams.Add($"pageSize={request.PageSize}");
                queryParams.Add($"pageIndex={request.PageIndex}");

                var queryString = string.Join("&", queryParams);
                var url = $"{storageApiUrl}/api/StoredItems/GetExpiringItems?{queryString}";

                _logger.LogInformation("Calling Storage API: {Url}", url);

                var response = await httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Storage API returned error: {StatusCode} - {Content}",
                        response.StatusCode, content);
                    return StatusCode((int)response.StatusCode, content);
                }

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling GetExpiringItems");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }
    }
}
