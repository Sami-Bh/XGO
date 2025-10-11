using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Xunit;
using XGO.Store.Application.CQRS.Subcategories.Queries;
using XGO.Store.Application.MappingProfiles;
using XGO.Store.Persistance;
using XGO.Store.Persistance.Models;

namespace TestStore
{
    public class SubcategoryQueriesTests : IAsyncLifetime
    {
        private readonly XGODbContext _dbContext;
        private readonly IMapper _mapper;

        public SubcategoryQueriesTests()
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

        #region GetSubcategoryDetails Tests

        [Fact]
        public async Task GetSubcategoryDetails_ForMobilePhonesSubcategory_ReturnsSuccessWithProducts()
        {
            // Arrange
            var handler = new GetSubcategoryDetails.Handler(_dbContext, _mapper);
            var mobilePhones = await _dbContext.SubCategories
                .Include(sc => sc.Products)
                .FirstAsync(sc => sc.Name == "Mobile Phones");
            var query = new GetSubcategoryDetails.Query { Id = mobilePhones.Id };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsValid);
            Assert.NotNull(result.Value);
            Assert.Equal("Mobile Phones", result.Value.Name);
            Assert.True(result.Value.HasChildren);
        }

        [Fact]
        public async Task GetSubcategoryDetails_ForSofasSubcategory_ReturnsSuccessWithHeavyBulkyProducts()
        {
            // Arrange
            var handler = new GetSubcategoryDetails.Handler(_dbContext, _mapper);
            var sofas = await _dbContext.SubCategories
                .Include(sc => sc.Products)
                .FirstAsync(sc => sc.Name == "Sofas");
            var query = new GetSubcategoryDetails.Query { Id = sofas.Id };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsValid);
            Assert.NotNull(result.Value);
            Assert.Equal("Sofas", result.Value.Name);
            Assert.True(result.Value.HasChildren);
        }

        [Fact]
        public async Task GetSubcategoryDetails_WhenSubcategoryDoesNotExist_ReturnsFailure()
        {
            // Arrange
            var handler = new GetSubcategoryDetails.Handler(_dbContext, _mapper);
            var query = new GetSubcategoryDetails.Query { Id = 99999 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(404, result.ErrorCode);
            Assert.Equal("element not found", result.ErrorMessage);
        }

        [Fact]
        public async Task GetSubcategoryDetails_ForDairySubcategory_ReturnsSuccess()
        {
            // Arrange
            var handler = new GetSubcategoryDetails.Handler(_dbContext, _mapper);
            var dairy = await _dbContext.SubCategories.FirstAsync(sc => sc.Name == "Dairy");
            var query = new GetSubcategoryDetails.Query { Id = dairy.Id };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsValid);
            Assert.NotNull(result.Value);
            Assert.Equal("Dairy", result.Value.Name);
            Assert.True(result.Value.HasChildren);
        }

        #endregion

        #region GetSubcategoriesListByCategoryId Tests

        [Fact]
        public async Task GetSubcategoriesListByCategoryId_ForElectronicsCategory_ReturnsMobilePhonesSubcategory()
        {
            // Arrange
            var electronicsCategory = await _dbContext.Categories.FirstAsync(c => c.Name == "Electronics");
            var handler = new GetSubcategoriesListByCategoryId.Handler(_dbContext, _mapper);
            var query = new GetSubcategoriesListByCategoryId.Query { CategoryId = electronicsCategory.Id };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Contains(result, s => s.Name == "Mobile Phones");
            Assert.All(result, s => Assert.Equal(electronicsCategory.Id, s.CategoryId));
        }

        [Fact]
        public async Task GetSubcategoriesListByCategoryId_ForFurnitureCategory_ReturnsSofasSubcategory()
        {
            // Arrange
            var furnitureCategory = await _dbContext.Categories.FirstAsync(c => c.Name == "Furniture");
            var handler = new GetSubcategoriesListByCategoryId.Handler(_dbContext, _mapper);
            var query = new GetSubcategoriesListByCategoryId.Query { CategoryId = furnitureCategory.Id };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Contains(result, s => s.Name == "Sofas");
        }

        [Fact]
        public async Task GetSubcategoriesListByCategoryId_WhenCategoryDoesNotExist_ReturnsEmptyList()
        {
            // Arrange
            var handler = new GetSubcategoriesListByCategoryId.Handler(_dbContext, _mapper);
            var query = new GetSubcategoriesListByCategoryId.Query { CategoryId = 99999 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetSubcategoriesListByCategoryId_ForGroceryCategory_ReturnsDairySubcategory()
        {
            // Arrange
            var groceryCategory = await _dbContext.Categories.FirstAsync(c => c.Name == "Grocery");
            var handler = new GetSubcategoriesListByCategoryId.Handler(_dbContext, _mapper);
            var query = new GetSubcategoriesListByCategoryId.Query { CategoryId = groceryCategory.Id };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Contains(result, s => s.Name == "Dairy");
        }

        [Fact]
        public async Task GetSubcategoriesListByCategoryId_WithMultipleCategories_ReturnsOnlyRequestedCategorySubcategories()
        {
            // Arrange
            var toysCategory = await _dbContext.Categories.FirstAsync(c => c.Name == "Toys");
            var booksCategory = await _dbContext.Categories.FirstAsync(c => c.Name == "Books");

            var handler = new GetSubcategoriesListByCategoryId.Handler(_dbContext, _mapper);
            var toysQuery = new GetSubcategoriesListByCategoryId.Query { CategoryId = toysCategory.Id };

            // Act
            var toysResult = await handler.Handle(toysQuery, CancellationToken.None);

            // Assert
            Assert.NotNull(toysResult);
            Assert.NotEmpty(toysResult);
            Assert.All(toysResult, s => Assert.Equal(toysCategory.Id, s.CategoryId));
            Assert.DoesNotContain(toysResult, s => s.CategoryId == booksCategory.Id);
        }

        [Fact]
        public async Task GetSubcategoriesListByCategoryId_WithCancellationToken_CanBeCancelled()
        {
            // Arrange
            var handler = new GetSubcategoriesListByCategoryId.Handler(_dbContext, _mapper);
            var category = await _dbContext.Categories.FirstAsync();
            var query = new GetSubcategoriesListByCategoryId.Query { CategoryId = category.Id };
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await handler.Handle(query, cancellationTokenSource.Token));
        }

        #endregion
    }
}
