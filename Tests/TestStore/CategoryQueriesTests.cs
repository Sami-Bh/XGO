using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Xunit;
using XGO.Store.Application.CQRS.Category.Queries;
using XGO.Store.Application.MappingProfiles;
using XGO.Store.Persistance;
using XGO.Store.Persistance.Models;

namespace TestStore
{
    public class CategoryQueriesTests : IAsyncLifetime
    {
        private readonly XGODbContext _dbContext;
        private readonly IMapper _mapper;

        public CategoryQueriesTests()
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

        [Fact]
        public async Task GetCategoryDetails_ForElectronicsCategory_ReturnsSuccessWithSubcategories()
        {
            // Arrange
            var handler = new GetCategoryDetails.Handler(_dbContext, _mapper);
            var electronicsCategory = await _dbContext.Categories.FirstAsync(c => c.Name == "Electronics");
            var query = new GetCategoryDetails.Query { Id = electronicsCategory.Id };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsValid);
            Assert.NotNull(result.Value);
            Assert.Equal("Electronics", result.Value.Name);
            Assert.True(result.Value.HasChildren);
        }

        [Fact]
        public async Task GetCategoryDetails_ForFurnitureCategory_ReturnsSuccessWithSofasSubcategory()
        {
            // Arrange
            var handler = new GetCategoryDetails.Handler(_dbContext, _mapper);
            var furnitureCategory = await _dbContext.Categories.FirstAsync(c => c.Name == "Furniture");
            var query = new GetCategoryDetails.Query { Id = furnitureCategory.Id };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsValid);
            Assert.NotNull(result.Value);
            Assert.Equal("Furniture", result.Value.Name);
            Assert.True(result.Value.HasChildren);
        }

        [Fact]
        public async Task GetCategoryDetails_WhenCategoryDoesNotExist_ReturnsFailure()
        {
            // Arrange
            var handler = new GetCategoryDetails.Handler(_dbContext, _mapper);
            var query = new GetCategoryDetails.Query { Id = 99999 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(404, result.ErrorCode);
            Assert.Equal("Element not found", result.ErrorMessage);
        }

        [Fact]
        public async Task GetCategoryDetails_ForGroceryCategory_ReturnsSuccess()
        {
            // Arrange
            var handler = new GetCategoryDetails.Handler(_dbContext, _mapper);
            var groceryCategory = await _dbContext.Categories.FirstAsync(c => c.Name == "Grocery");
            var query = new GetCategoryDetails.Query { Id = groceryCategory.Id };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsValid);
            Assert.NotNull(result.Value);
            Assert.Equal("Grocery", result.Value.Name);
            Assert.True(result.Value.HasChildren);
        }

        [Fact]
        public async Task GetCategoryDetails_VerifyAllSeededCategoriesExist()
        {
            // Arrange
            var handler = new GetCategoryDetails.Handler(_dbContext, _mapper);
            var expectedCategories = new[] { "Electronics", "Furniture", "Toys", "Books", "Appliances",
                "Automotive", "Clothing", "Shoes", "Beauty & Health", "Sports", "Home Decor",
                "Grocery", "Pet Supplies", "Music Instruments" };

            // Act & Assert
            foreach (var categoryName in expectedCategories)
            {
                var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
                Assert.NotNull(category);

                var query = new GetCategoryDetails.Query { Id = category.Id };
                var result = await handler.Handle(query, CancellationToken.None);

                Assert.True(result.IsValid);
                Assert.Equal(categoryName, result.Value.Name);
            }
        }

        [Fact]
        public async Task GetCategoryDetails_WithCancellationToken_CanBeCancelled()
        {
            // Arrange
            var handler = new GetCategoryDetails.Handler(_dbContext, _mapper);
            var category = await _dbContext.Categories.FirstAsync();
            var query = new GetCategoryDetails.Query { Id = category.Id };
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await handler.Handle(query, cancellationTokenSource.Token));
        }
    }
}
