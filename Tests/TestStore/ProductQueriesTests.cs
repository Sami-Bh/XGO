using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Xunit;
using XGO.Store.Application.CQRS.Product.Queries;
using XGO.Store.Application.Filters;
using XGO.Store.Application.MappingProfiles;
using XGO.Store.Persistance;
using XGO.Store.Persistance.Models;

namespace TestStore
{
    public class ProductQueriesTests : IAsyncLifetime
    {
        private readonly XGODbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductQueriesTests()
        {
            // Setup InMemory database
            var options = new DbContextOptionsBuilder<XGODbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new XGODbContext(options);

            // Setup AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        public async Task InitializeAsync()
        {
            // Seed data from DBInitializer
            await DBInitializer.SeedDataAsync(_dbContext);
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        #region GetFilteredProducts Tests

        [Fact]
        public async Task GetFilteredProducts_WithMobilePhonesSubCategoryId_ReturnsOnlyMobilePhones()
        {
            // Arrange
            var mobilePhones = await _dbContext.SubCategories.FirstAsync(sc => sc.Name == "Mobile Phones");
            var handler = new GetFilteredProducts.Handler(_dbContext, _mapper);
            var query = new GetFilteredProducts.Query
            {
                Filter = new ProductsFilter
                {
                    SubCategoryId = mobilePhones.Id,
                    PageSize = 10,
                    PageIndex = 1
                }
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Items.Count); // DBInitializer has 5 mobile phones
            Assert.Contains(result.Items, p => p.Name == "iPhone 15");
            Assert.Contains(result.Items, p => p.Name == "Samsung Galaxy S23");
            Assert.Contains(result.Items, p => p.Name == "Google Pixel 7");
        }

        [Fact]
        public async Task GetFilteredProducts_WithElectronicsCategoryId_ReturnsAllElectronicsProducts()
        {
            // Arrange
            var electronicsCategory = await _dbContext.Categories.FirstAsync(c => c.Name == "Electronics");
            var handler = new GetFilteredProducts.Handler(_dbContext, _mapper);
            var query = new GetFilteredProducts.Query
            {
                Filter = new ProductsFilter
                {
                    CategoryId = electronicsCategory.Id,
                    PageSize = 20,
                    PageIndex = 1
                }
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Items);
            Assert.Contains(result.Items, p => p.Name == "iPhone 15");
            Assert.Contains(result.Items, p => p.Name == "Xiaomi 13 Pro");
        }

        [Fact]
        public async Task GetFilteredProducts_WithSearchTextIPhone_ReturnsMatchingProducts()
        {
            // Arrange
            var handler = new GetFilteredProducts.Handler(_dbContext, _mapper);
            var query = new GetFilteredProducts.Query
            {
                Filter = new ProductsFilter
                {
                    SearchText = "iphone",
                    PageSize = 10,
                    PageIndex = 1
                }
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Items);
            Assert.Contains(result.Items, p => p.Name == "iPhone 15");
        }

        [Fact]
        public async Task GetFilteredProducts_WithSearchTextSofa_ReturnsAllSofas()
        {
            // Arrange
            var handler = new GetFilteredProducts.Handler(_dbContext, _mapper);
            var query = new GetFilteredProducts.Query
            {
                Filter = new ProductsFilter
                {
                    SearchText = "sofa",
                    PageSize = 10,
                    PageIndex = 1
                }
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Items.Count); // DBInitializer has 5 sofas
            Assert.Contains(result.Items, p => p.Name == "Leather Sofa");
            Assert.Contains(result.Items, p => p.Name == "Fabric Sofa");
            Assert.Contains(result.Items, p => p.Name == "Futon Sofa");
        }

        [Fact]
        public async Task GetFilteredProducts_WithPagination_ReturnsCorrectPage()
        {
            // Arrange
            var handler = new GetFilteredProducts.Handler(_dbContext, _mapper);
            var query = new GetFilteredProducts.Query
            {
                Filter = new ProductsFilter
                {
                    PageSize = 5,
                    PageIndex = 1
                }
            };

            // Act
            var resultPage1 = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultPage1);
            Assert.Equal(5, resultPage1.Items.Count);
            Assert.True(resultPage1.PageCount > 1); // Should have multiple pages with all seeded data
        }

        [Fact]
        public async Task GetFilteredProducts_WithNoFilters_ReturnsAllProducts()
        {
            // Arrange
            var handler = new GetFilteredProducts.Handler(_dbContext, _mapper);
            var query = new GetFilteredProducts.Query
            {
                Filter = new ProductsFilter
                {
                    PageSize = 100,
                    PageIndex = 1
                }
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Items.Count > 0);
            // DBInitializer seeds 14 categories × 1 subcategory × 5 products = 70 products
            Assert.True(result.Items.Count >= 60);
        }

        [Fact]
        public async Task GetFilteredProducts_ResultsAreSortedByName()
        {
            // Arrange
            var sofas = await _dbContext.SubCategories.FirstAsync(sc => sc.Name == "Sofas");
            var handler = new GetFilteredProducts.Handler(_dbContext, _mapper);
            var query = new GetFilteredProducts.Query
            {
                Filter = new ProductsFilter
                {
                    SubCategoryId = sofas.Id,
                    PageSize = 10,
                    PageIndex = 1
                }
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Items.Count);
            // Verify alphabetical order
            Assert.Equal("Fabric Sofa", result.Items[0].Name);
            Assert.Equal("Futon Sofa", result.Items[1].Name);
            Assert.Equal("L-Shaped Sofa", result.Items[2].Name);
            Assert.Equal("Leather Sofa", result.Items[3].Name);
            Assert.Equal("Recliner Sofa", result.Items[4].Name);
        }

        [Fact]
        public async Task GetFilteredProducts_CombinedFilters_SearchTextAndSubCategoryId()
        {
            // Arrange
            var dairy = await _dbContext.SubCategories.FirstAsync(sc => sc.Name == "Dairy");
            var handler = new GetFilteredProducts.Handler(_dbContext, _mapper);
            var query = new GetFilteredProducts.Query
            {
                Filter = new ProductsFilter
                {
                    SubCategoryId = dairy.Id,
                    SearchText = "milk",
                    PageSize = 10,
                    PageIndex = 1
                }
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Items);
            Assert.Contains(result.Items, p => p.Name == "Milk");
        }

        [Fact]
        public async Task GetFilteredProducts_ForSofas_VerifiesHeavyAndBulkyProperties()
        {
            // Arrange
            var sofas = await _dbContext.SubCategories.FirstAsync(sc => sc.Name == "Sofas");
            var handler = new GetFilteredProducts.Handler(_dbContext, _mapper);
            var query = new GetFilteredProducts.Query
            {
                Filter = new ProductsFilter
                {
                    SubCategoryId = sofas.Id,
                    PageSize = 10,
                    PageIndex = 1
                }
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            // Most sofas are heavy and bulky (except Futon)
            var leatherSofa = result.Items.FirstOrDefault(p => p.Name == "Leather Sofa");
            Assert.NotNull(leatherSofa);
            Assert.True(leatherSofa.IsHeavy);
            Assert.True(leatherSofa.IsBulky);
        }

        #endregion

        #region GetProductNamesBySubCategory Tests

        [Fact]
        public async Task GetProductNamesBySubCategory_ForMobilePhones_ReturnsAllMobilePhoneNames()
        {
            // Arrange
            var mobilePhones = await _dbContext.SubCategories.FirstAsync(sc => sc.Name == "Mobile Phones");
            var handler = new GetProductNamesBySubCategory.Handler(_dbContext);
            var query = new GetProductNamesBySubCategory.Query { SubCategoryId = mobilePhones.Id };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Contains(result, p => p.Name == "iPhone 15");
            Assert.Contains(result, p => p.Name == "Samsung Galaxy S23");
            Assert.Contains(result, p => p.Name == "Google Pixel 7");
            Assert.Contains(result, p => p.Name == "OnePlus 11");
            Assert.Contains(result, p => p.Name == "Xiaomi 13 Pro");
        }

        [Fact]
        public async Task GetProductNamesBySubCategory_WhenNoProductsExist_ReturnsEmptyList()
        {
            // Arrange
            var handler = new GetProductNamesBySubCategory.Handler(_dbContext);
            var query = new GetProductNamesBySubCategory.Query { SubCategoryId = 99999 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetProductNamesBySubCategory_ResultsAreSortedByName()
        {
            // Arrange
            var actionFigures = await _dbContext.SubCategories.FirstAsync(sc => sc.Name == "Action Figures");
            var handler = new GetProductNamesBySubCategory.Handler(_dbContext);
            var query = new GetProductNamesBySubCategory.Query { SubCategoryId = actionFigures.Id };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            // Verify alphabetical order
            Assert.Equal("Batman Figure", result[0].Name);
            Assert.Equal("Captain America Figure", result[1].Name);
            Assert.Equal("Iron Man Figure", result[2].Name);
            Assert.Equal("Spider-Man Figure", result[3].Name);
            Assert.Equal("Superman Figure", result[4].Name);
        }

        [Fact]
        public async Task GetProductNamesBySubCategory_ReturnsOnlyIdAndName()
        {
            // Arrange
            var dairy = await _dbContext.SubCategories.FirstAsync(sc => sc.Name == "Dairy");
            var handler = new GetProductNamesBySubCategory.Handler(_dbContext);
            var query = new GetProductNamesBySubCategory.Query { SubCategoryId = dairy.Id };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.All(result, item =>
            {
                Assert.True(item.Id > 0);
                Assert.False(string.IsNullOrEmpty(item.Name));
            });
        }

        [Fact]
        public async Task GetProductNamesBySubCategory_ForDifferentSubcategories_ReturnsCorrectProducts()
        {
            // Arrange
            var books = await _dbContext.SubCategories.FirstAsync(sc => sc.Name == "Fiction");
            var handler = new GetProductNamesBySubCategory.Handler(_dbContext);
            var query = new GetProductNamesBySubCategory.Query { SubCategoryId = books.Id };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Contains(result, p => p.Name == "The Great Gatsby");
            Assert.Contains(result, p => p.Name == "1984");
            Assert.Contains(result, p => p.Name == "To Kill a Mockingbird");
        }

        [Fact]
        public async Task GetProductNamesBySubCategory_WithCancellationToken_CanBeCancelled()
        {
            // Arrange
            var subcategory = await _dbContext.SubCategories.FirstAsync();
            var handler = new GetProductNamesBySubCategory.Handler(_dbContext);
            var query = new GetProductNamesBySubCategory.Query { SubCategoryId = subcategory.Id };
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await handler.Handle(query, cancellationTokenSource.Token));
        }

        #endregion
    }
}
