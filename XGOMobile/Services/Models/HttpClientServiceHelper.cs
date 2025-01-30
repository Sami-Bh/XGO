using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace XGOMobile.Services.Models
{
    public class HttpClientServiceHelper(HttpClientService httpClientService, MessagingService messagingService) 
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors
        #endregion

        #region Methods
        public async Task<IEnumerable<T>> GetObjectAsync<T>(string uri)
        {
            using var httpClient = await httpClientService.GetWebClient();

            var httpResponseMessage = await httpClient.GetAsync(uri);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return await httpResponseMessage.Content.ReadFromJsonAsync<T[]>();
            }
            await messagingService.ShowMessage("An error occured");
            return [];
        }

        public async Task<bool> DeleteObjectAsync(string uri)
        {
            using var httpClient = await httpClientService.GetWebClient();

            var httpResponseMessage = await httpClient.DeleteAsync(uri);

            return httpResponseMessage.IsSuccessStatusCode;
        }
        #endregion
    }
}
