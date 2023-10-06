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
using XGOMobile.Services.Models;
using XGOModels;
using XGOModels.Extras;

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
        private string _NewItemName;
        private Category _SelectedCategory;

        private bool _isWorking;
        private ICommand _AddCategoryCommand;
        private ICommand _AddSubCategoryCommand;
        private ICommand _DeleteCategoryCommand;

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
            get { return _NewItemName; }
            set
            {
                _NewItemName = value;
                OnPropertyChanged(nameof(NewCategoryName));
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

        public ICommand AddCategoryCommand { get { return _AddCategoryCommand ??= new Command(() => AddCategoryAsync(null));  } }
        public ICommand DeleteCategoryCommand { get { return _DeleteCategoryCommand ??= new Command(() => DeleteCategoryAsync(null)); } }

        public ICommand AddSubCategoryCommand { get { return _AddSubCategoryCommand ??= new Command(() => AddSubCategoryAsync(null)); } }
        private async Task AddSubCategoryAsync(object obj)
        {
        }

        private ICommand _DeteSubCategoryCommand;
        public ICommand DeteSubCategoryCommand { get { return _DeteSubCategoryCommand ??= new Command(() => DeteSubCategoryAsync(null)); } }
        private async Task DeteSubCategoryAsync(object obj)
        {
        }

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
            await GetCategoriesAsync().ContinueWith(res =>
            {
                IsWorking = false;
                Categories = new List<Category>(res.Result);
            }); ;
        }
        private async Task<List<Category>> GetCategoriesAsync()
        {
            IsWorking = true;
            var categoriesUri = _httpUriBuilder.GetServiceURI(ApplicationModules.Categories);
            using var httpClient = await _httpClientService.GetWebClient();
            var httpResponseMessage = await httpClient.GetAsync(categoriesUri);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return (await httpResponseMessage.Content.ReadFromJsonAsync<Category[]>()).ToList();
            }
            await _messagingService.ShowMessage("An error occured");
            return new List<Category>();
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
        private void AddSubCategory(object obj)
        {
        }
        private async Task DeleteCategoryAsync(object obj)
        {
            IsWorking = true;

            var categoriesUri = _httpUriBuilder.GetServiceURI(ApplicationModules.Categories);
            using var httpClient = await _httpClientService.GetWebClient();
            var httpResponseMessage = await httpClient.DeleteAsync($"{categoriesUri}/{SelectedCategory.Id}");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                await RefreshPageDataAsync();
                return;
            }
            IsWorking = false;

            await _messagingService.ShowMessage($"An error occured {httpResponseMessage.StatusCode}");
        }

        #endregion
    }
}
