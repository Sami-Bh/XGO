using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.AspNetCore.WebUtilities;
using XGOMobile.Services.Models;
using XGOModels;
using XGOModels.Extras;
using XGOUtilities.Constants;

namespace XGOMobile.ViewModels
{
    public class CategoriesViewModel : BaseViewModel
    {
        #region Fields
        private readonly AuthenticationService _authenticationService;
        private readonly HttpUriBuilder _httpUriBuilder;
        private readonly HttpClientService _httpClientService;
        private readonly MessagingService _messagingService;
        private List<Category> _Categories;
        private List<SubCategory> _SubCategories;
        private string _NewCategoryName;
        private string _NewSubCAtegoryName;

        private Category _SelectedCategory;
        private SubCategory _SelectedSubCategory;

        private bool _isWorking;
        private ICommand _AddCategoryCommand;
        private ICommand _AddSubCategoryCommand;
        private ICommand _DeleteCategoryCommand;
        private ICommand _DeteSubCategoryCommand;

        #endregion

        #region Properties
        public bool IsWorking
        {
            get { return _isWorking; }
            set
            {
                _isWorking = value;
                OnPropertyChanged(nameof(IsWorking));
            }
        }
        public string NewCategoryName
        {
            get { return _NewCategoryName; }
            set
            {
                _NewCategoryName = value;
                OnPropertyChanged(nameof(NewCategoryName));
            }
        }

        public string NewSubCategoryName
        {
            get { return _NewSubCAtegoryName; }
            set
            {
                _NewSubCAtegoryName = value;
                OnPropertyChanged(nameof(NewSubCategoryName));
            }
        }
        public List<Category> Categories
        {
            get { return _Categories; }
            set
            {
                _Categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public Category SelectedCategory
        {
            get { return _SelectedCategory; }
            set
            {
                _SelectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                if (value != null) { GetSubCategoriesByCategory(); }

            }
        }
        public List<SubCategory> SubCategories
        {
            get { return _SubCategories; }
            set
            {
                _SubCategories = value;
                OnPropertyChanged(nameof(SubCategories));
            }
        }

        public SubCategory SelectedSubCategory
        {
            get { return _SelectedSubCategory; }
            set
            {
                _SelectedSubCategory = value;
                OnPropertyChanged(nameof(SelectedSubCategory));
            }
        }

        public ICommand AddCategoryCommand { get { return _AddCategoryCommand ??= new Command(async () => await AddCategoryAsync(null)); } }
        public ICommand DeleteCategoryCommand { get { return _DeleteCategoryCommand ??= new Command(async () => await DeleteCategoryAsync(null)); } }

        public ICommand AddSubCategoryCommand { get { return _AddSubCategoryCommand ??= new Command(async () => await AddSubCategoryAsync(null)); } }

        public ICommand DeteSubCategoryCommand { get { return _DeteSubCategoryCommand ??= new Command(async () => await DeteSubCategoryAsync(null)); } }

        #endregion

        #region Constructors
        public CategoriesViewModel(AuthenticationService authenticationService, HttpUriBuilder httpUriBuilder, HttpClientService httpClientService, MessagingService messagingService)
        {
            _authenticationService = authenticationService;
            _httpUriBuilder = httpUriBuilder;
            _httpClientService = httpClientService;
            _messagingService = messagingService;
            RefreshPageDataAsync();
        }
        //public CategoriesViewModel() { }
        #endregion

        #region Methods
        public async Task RefreshPageDataAsync()
        {
            NewCategoryName = string.Empty;
            NewSubCategoryName = string.Empty;
            Categories = [];
            SubCategories = [];

            await GetCategoriesAsync().ContinueWith(res =>
            {
                IsWorking = false;
                Categories = new List<Category>(res.Result);
            });
        }
        private async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            IsWorking = true;
            var categoriesUri = _httpUriBuilder.GetServiceURI(ApplicationModules.Categories);
            return await GetObjectAsync<Category>(categoriesUri);
        }

        private async Task AddCategoryAsync(object obj)
        {
            if (string.IsNullOrWhiteSpace(NewCategoryName))
            {
                await _messagingService.ShowMessage("Category name cannot be empty");
                return;
            }
            IsWorking = true;

            var newCategory = new Category { Name = NewCategoryName };
            var categoriesUri = _httpUriBuilder.GetServiceURI(ApplicationModules.Categories);
            using var httpClient = await _httpClientService.GetWebClient();
            var httpResponseMessage = await httpClient.PostAsJsonAsync(categoriesUri, newCategory);
            if (httpResponseMessage.IsSuccessStatusCode)
            {

                await RefreshPageDataAsync();
                return;
            }
            IsWorking = false;

            await _messagingService.ShowMessage($"An error occured {httpResponseMessage.StatusCode}");
        }

        private async Task DeleteCategoryAsync(object obj)
        {
            await Delete(ApplicationModules.Categories, SelectedCategory.Id);

        }

        private async Task DeteSubCategoryAsync(object obj)
        {
            await Delete(ApplicationModules.SubCategories, SelectedSubCategory.Id);
        }

        private async Task Delete(ApplicationModules applicationModules, int objectId)
        {
            try
            {
                IsWorking = true;
                var categoriesUri = _httpUriBuilder.GetServiceURI(applicationModules, (ParametersConstants.Id, objectId.ToString()));

                if (await DeleteObjectAsync(categoriesUri))
                {
                    await RefreshPageDataAsync();
                }
                else
                {
                    await _messagingService.ShowMessage($"An error occured");
                }
            }
            finally
            {
                IsWorking = false;
            }
        }

        private async Task AddSubCategoryAsync(object obj)
        {

            if (string.IsNullOrWhiteSpace(NewSubCategoryName))
            {
                await _messagingService.ShowMessage("Sub Category name cannot be empty");
                return;
            }
            if (SelectedCategory is null)
            {
                await _messagingService.ShowMessage("Category name cannot be empty");
                return;
            }

            try
            {
                IsWorking = true;

                var newSubCategory = new SubCategory { Name = NewSubCategoryName };
                var subCategoriesUri = _httpUriBuilder.GetServiceURI(ApplicationModules.SubCategories, ApplicationActions.Create, (ParametersConstants.CategoryId, SelectedCategory.Id.ToString()));


                using var httpClient = await _httpClientService.GetWebClient();
                var httpResponseMessage = await httpClient.PostAsJsonAsync(subCategoriesUri, newSubCategory);

                if (httpResponseMessage.IsSuccessStatusCode)
                {

                    await RefreshPageDataAsync();
                    return;
                }

                await _messagingService.ShowMessage($"An error occured {httpResponseMessage.StatusCode}");
            }
            finally
            {
                IsWorking = false;

            }
        }

        private async Task GetSubCategoriesByCategory()
        {
            IsWorking = true;
            var categoriesUri = _httpUriBuilder.GetServiceURI(ApplicationModules.SubCategories, ApplicationActions.GetByCategoryId, (ParametersConstants.Id, SelectedCategory.Id.ToString()));

            var subCategories = await GetObjectAsync<SubCategory>(categoriesUri);
            SubCategories = subCategories.ToList();
            IsWorking = false;
        }


        private async Task<IEnumerable<T>> GetObjectAsync<T>(string uri)
        {
            using var httpClient = await _httpClientService.GetWebClient();

            var httpResponseMessage = await httpClient.GetAsync(uri);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return await httpResponseMessage.Content.ReadFromJsonAsync<T[]>();
            }
            await _messagingService.ShowMessage("An error occured");
            return [];
        }

        private async Task<bool> DeleteObjectAsync(string uri)
        {
            using var httpClient = await _httpClientService.GetWebClient();

            var httpResponseMessage = await httpClient.DeleteAsync(uri);

            return httpResponseMessage.IsSuccessStatusCode;
        }
        #endregion
    }
}
