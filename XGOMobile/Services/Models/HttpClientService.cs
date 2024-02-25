using System.Net.Http.Headers;

namespace XGOMobile.Services.Models
{
    public class HttpClientService
    {
        private readonly AuthenticationService _authenticationService;
        private readonly HttpUriBuilder _httpUriBuilder;
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors
        public HttpClientService() { }

        public HttpClientService(AuthenticationService authenticationService, HttpUriBuilder httpUriBuilder)
        {
            _authenticationService = authenticationService;
            _httpUriBuilder = httpUriBuilder;
        }
        #endregion

        #region Methods
        public async Task<HttpClient> GetWebClient()
        {
#if AUTHENTICATIONREQUIRED
            var authentifactionToken = await _authenticationService.GetToken();

#endif
            var httpClient = new HttpClient();
#if AUTHENTICATIONREQUIRED
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authentifactionToken.Token);
            
#endif
            return httpClient;
        }
        #endregion
    }
}
