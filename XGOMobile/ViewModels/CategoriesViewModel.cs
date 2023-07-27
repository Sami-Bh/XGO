using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using XGOMobile.Services.Models;
using XGOModels;
using XGOModels.Extras;

namespace XGOMobile.ViewModels
{
    public class CategoriesViewModel : BaseViewModel
    {
        private readonly AuthenticationService _authenticationService;
        private readonly HttpUriBuilder _httpUriBuilder;
        private readonly HttpClientService _httpClientService;
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors
        public CategoriesViewModel(AuthenticationService authenticationService, HttpUriBuilder httpUriBuilder, HttpClientService httpClientService)
        {
            _authenticationService = authenticationService;
            _httpUriBuilder = httpUriBuilder;
            _httpClientService = httpClientService;

            GetCategoriesAsyn().ContinueWith(res =>
            {

            });
        }

        #endregion

        #region Methods
        private async Task<List<Category>> GetCategoriesAsyn()
        {
            var categoriesUri = _httpUriBuilder.GetServiceURI(ApplicationModules.Categories);
            using var httpClient = await _httpClientService.GetWebClient();
            var httpResponseMessage = await httpClient.GetAsync(categoriesUri);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return (await httpResponseMessage.Content.ReadFromJsonAsync<Category[]>()).ToList();
            }
            return new List<Category>();
        }
        #endregion
    }
}
