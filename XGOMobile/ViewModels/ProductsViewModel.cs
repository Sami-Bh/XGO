using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XGOMobile.Services.Models;
using XGOMobile.Views;
using XGOModels;
using XGOModels.Extras;
using XGOUtilities.Constants;

namespace XGOMobile.ViewModels
{
    public class ProductsViewModel : BaseViewModel
    {
        #region Fields
        private ICommand _AddProductCommand;
        private ICommand _DeleteProductCommand;
        private List<Category> _Categories;
        private Category _SelectedCategory;
        private List<Product> _Products;
        private Product _SelectedProduct;
        private List<SubCategory> _Subcategories;
        private SubCategory _SelectedSubcategory;
        private readonly HttpUriBuilder _httpUriBuilder;
        private readonly HttpClientService _httpClientService;
        private readonly MessagingService _messagingService;
        private readonly HttpClientServiceHelper _httpClientServiceHelper;
        private bool _isWorking;
        private Product _NewProduct;


        #endregion

        #region Properties

        public Category SelectedCategory
        {
            get { return _SelectedCategory; }
            set
            {
                _SelectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                SelectedSubcategory = SelectedCategory.SubCategories.FirstOrDefault();
            }
        }
        public List<Category> Categories
        {
            get { return _Categories; }
            set
            {
                _Categories = value;
                OnPropertyChanged(nameof(Categories));
                SelectedCategory = Categories.FirstOrDefault();
            }
        }
        public Product NewProduct
        {
            get { return _NewProduct; }
            set
            {
                _NewProduct = value;
                OnPropertyChanged(nameof(NewProduct));
            }
        }


        public List<Product> Products
        {
            get { return _Products; }
            set
            {
                _Products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        public Product SelectedProduct
        {
            get { return _SelectedProduct; }
            set
            {
                _SelectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
                if (SelectedProduct != null)
                {
                    NewProduct = SelectedProduct;
                    SelectedCategory = Categories.FirstOrDefault(x => x.Id == SelectedProduct.SubCategory.CategoryId);
                    SelectedSubcategory = SelectedCategory.SubCategories.FirstOrDefault(x => x.Id == SelectedProduct.SubCategory.Id);
                }
            }
        }


        public List<SubCategory> Subcategories
        {
            get { return _Subcategories; }
            set
            {
                _Subcategories = value;
                OnPropertyChanged(nameof(Subcategories));
                SelectedSubcategory = Subcategories.FirstOrDefault();
            }
        }
        public SubCategory SelectedSubcategory
        {
            get { return _SelectedSubcategory; }
            set
            {
                _SelectedSubcategory = value;
                OnPropertyChanged(nameof(SelectedSubcategory));
            }
        }

        public bool IsWorking
        {
            get { return _isWorking; }
            set
            {
                _isWorking = value;
                OnPropertyChanged(nameof(IsWorking));
            }
        }

        public ICommand DeleteProductCommand { get { return _DeleteProductCommand ??= new Command(async () => await DeleteProductAsync(null)); } }
        public ICommand AddProductCommand { get { return _AddProductCommand ??= new Command(async () => await AddProductAsync(null)); } }


        #endregion

        #region Constructors
        public ProductsViewModel(HttpUriBuilder httpUriBuilder, HttpClientService httpClientService, MessagingService messagingService, HttpClientServiceHelper httpClientServiceHelper) : this()
        {
            _httpUriBuilder = httpUriBuilder;
            _httpClientService = httpClientService;
            _messagingService = messagingService;
            _httpClientServiceHelper = httpClientServiceHelper;
            RefreshPageAsync().WaitAsync(new CancellationTokenSource().Token);
        }
        public ProductsViewModel()
        {
            Products = [];
            NewProduct = new Product();
        }
        #endregion

        #region Methods
        public async Task RefreshPageAsync()
        {
            NewProduct = new Product();

            Products = (await GetAllProductsAsync()).ToList();

            Categories = (await GetCategoriesAsync()).ToList();
        }
        private async Task DeleteProductAsync(object obj)
        {
            try
            {
                IsWorking = true;
                var productsUri = _httpUriBuilder.GetServiceURI(ApplicationModules.Products, (ParametersConstants.Id, SelectedProduct.Id.ToString()));

                if (await _httpClientServiceHelper.DeleteObjectAsync(productsUri))
                {
                    await RefreshPageAsync();
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
        private async Task AddProductAsync(object obj)
        {
            try
            {
                IsWorking = true;
                NewProduct.SubCategoryId = SelectedSubcategory.Id;
                var productsUri = _httpUriBuilder.GetServiceURI(ApplicationModules.Products);
                using var httpClient = await _httpClientService.GetWebClient();
                var httpResponseMessage = await httpClient.PostAsJsonAsync(productsUri, NewProduct);
                if (httpResponseMessage.IsSuccessStatusCode)
                {

                    await RefreshPageAsync();
                    return;
                }

                await _messagingService.ShowMessage($"An error occured {httpResponseMessage.StatusCode}");

            }
            finally
            {
                IsWorking = false;
            }
        }

        private async Task<List<Product>> GetAllProductsAsync()
        {
            var serviceUri = _httpUriBuilder.GetServiceURI(ApplicationModules.Products);

            return (await _httpClientServiceHelper.GetObjectAsync<Product>(serviceUri)).ToList();
        }

        private async Task<List<Category>> GetCategoriesAsync()
        {
            var serviceUri = _httpUriBuilder.GetServiceURI(ApplicationModules.Categories, ApplicationActions.GetCategoriesIncludeSubCategories);
            return (await _httpClientServiceHelper.GetObjectAsync<Category>(serviceUri)).ToList();

        }

        #endregion
    }
}
