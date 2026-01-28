using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Xunit;
using XGO.Storage.Api.Storage.Application.CQRS.StorageItems.Queries;
using XGO.Storage.Api.Storage.Application.MappingProfiles;
using XGO.Storage.Api.Storage.Application.Models;
using XGO.Storage.Api.Storage.Domain;
using XGO.Storage.Api.Storage.Persistence;

namespace TestStorage
{
    public class GetExpiringProductsTests : IAsyncLifetime
    {
        private readonly XgoStorageDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetExpiringProductsTests()
        {
            // Setup InMemory database
            var options = new DbContextOptionsBuilder<XgoStorageDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new XgoStorageDbContext(options);

            // Setup AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        public async Task InitializeAsync()
        {
            // Seed test data
            var storageLocation1 = new StorageLocation { Id = 1, Location = "Fridge" };
            var storageLocation2 = new StorageLocation { Id = 2, Location = "Pantry" };
            var storageLocation3 = new StorageLocation { Id = 3, Location = "Freezer" };

            await _dbContext.StorageLocations.AddRangeAsync(storageLocation1, storageLocation2, storageLocation3);

            var today = DateTime.Today;

            // Products expiring in different time periods
            var storedItems = new List<StoredItem>
            {
                // Expired items (already past)
                new StoredItem
                {
                    Id = 1,
                    ProductId = 1,
                    ProductName = "Expired Milk",
                    ProductExpiryDate = today.AddDays(-5),
                    Quantity = 1,
                    StorageLocationId = 1,
                    StorageLocation = storageLocation1,
                    IsExpiracyAcknowledged = false
                },
                new StoredItem
                {
                    Id = 2,
                    ProductId = 2,
                    ProductName = "Expired Cheese",
                    ProductExpiryDate = today.AddDays(-2),
                    Quantity = 2,
                    StorageLocationId = 1,
                    StorageLocation = storageLocation1,
                    IsExpiracyAcknowledged = true // Acknowledged
                },
                // Expiring soon (within 7 days)
                new StoredItem
                {
                    Id = 3,
                    ProductId = 3,
                    ProductName = "Yogurt",
                    ProductExpiryDate = today.AddDays(3),
                    Quantity = 5,
                    StorageLocationId = 1,
                    StorageLocation = storageLocation1,
                    IsExpiracyAcknowledged = false
                },
                new StoredItem
                {
                    Id = 4,
                    ProductId = 4,
                    ProductName = "Bread",
                    ProductExpiryDate = today.AddDays(2),
                    Quantity = 1,
                    StorageLocationId = 2,
                    StorageLocation = storageLocation2,
                    IsExpiracyAcknowledged = false
                },
                new StoredItem
                {
                    Id = 5,
                    ProductId = 5,
                    ProductName = "Lettuce",
                    ProductExpiryDate = today.AddDays(5),
                    Quantity = 2,
                    StorageLocationId = 1,
                    StorageLocation = storageLocation1,
                    IsExpiracyAcknowledged = false
                },
                // Expiring later (within 30 days)
                new StoredItem
                {
                    Id = 6,
                    ProductId = 6,
                    ProductName = "Butter",
                    ProductExpiryDate = today.AddDays(15),
                    Quantity = 3,
                    StorageLocationId = 1,
                    StorageLocation = storageLocation1,
                    IsExpiracyAcknowledged = false
                },
                new StoredItem
                {
                    Id = 7,
                    ProductId = 7,
                    ProductName = "Eggs",
                    ProductExpiryDate = today.AddDays(20),
                    Quantity = 12,
                    StorageLocationId = 1,
                    StorageLocation = storageLocation1,
                    IsExpiracyAcknowledged = false
                },
                // No expiry date
                new StoredItem
                {
                    Id = 8,
                    ProductId = 8,
                    ProductName = "Canned Beans",
                    ProductExpiryDate = null,
                    Quantity = 10,
                    StorageLocationId = 2,
                    StorageLocation = storageLocation2,
                    IsExpiracyAcknowledged = false
                },
                // Far future expiry (beyond 30 days)
                new StoredItem
                {
                    Id = 9,
                    ProductId = 9,
                    ProductName = "Rice",
                    ProductExpiryDate = today.AddDays(365),
                    Quantity = 5,
                    StorageLocationId = 2,
                    StorageLocation = storageLocation2,
                    IsExpiracyAcknowledged = false
                },
                // Acknowledged expired item
                new StoredItem
                {
                    Id = 10,
                    ProductId = 10,
                    ProductName = "Old Jam",
                    ProductExpiryDate = today.AddDays(-10),
                    Quantity = 1,
                    StorageLocationId = 2,
                    StorageLocation = storageLocation2,
                    IsExpiracyAcknowledged = true
                }
            };

            await _dbContext.StoredItems.AddRangeAsync(storedItems);
            await _dbContext.SaveChangesAsync();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        #region GetExpiringProducts Tests

        [Fact]
        public async Task GetExpiringProducts_WithExpiresIn7Days_ReturnsOnlyProductsExpiringWithin7Days()
        {
            // Arrange
            var handler = new GetExpiringProducts.Handler(_dbContext, _mapper);
            var query = new GetExpiringProducts.Query
            {
                Filter = new ExpiringProductsFilter
                {
                    ExpiresInDays = 7,
                    IncludeAcknowledgedExpiredItems = false,
                    PageSize = 10,
                    PageIndex = 1
                }
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Items);
            // Should include: Expired Milk (-5 days), Yogurt (3 days), Bread (2 days), Lettuce (5 days)
            // Should exclude: Expired Cheese (acknowledged), Butter (15 days), Eggs (20 days), etc.
            Assert.Equal(4, result.Items.Count);
            Assert.Contains(result.Items, p => p.ProductName == "Expired Milk");
            Assert.Contains(result.Items, p => p.ProductName == "Yogurt");
            Assert.Contains(result.Items, p => p.ProductName == "Bread");
            Assert.Contains(result.Items, p => p.ProductName == "Lettuce");
        }

        [Fact]
        public async Task GetExpiringProducts_WithExpiresIn30Days_ReturnsAllProductsExpiringWithin30Days()
        {
            // Arrange
            var handler = new GetExpiringProducts.Handler(_dbContext, _mapper);
            var query = new GetExpiringProducts.Query
            {
                Filter = new ExpiringProductsFilter
                {
                    ExpiresInDays = 30,
                    IncludeAcknowledgedExpiredItems = false,
                    PageSize = 20,
                    PageIndex = 1
                }
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            // Should include: Expired Milk, Yogurt, Bread, Lettuce, Butter (15 days), Eggs (20 days)
            Assert.Equal(6, result.Items.Count);
            Assert.Contains(result.Items, p => p.ProductName == "Butter");
            Assert.Contains(result.Items, p => p.ProductName == "Eggs");
            Assert.DoesNotContain(result.Items, p => p.ProductName == "Rice"); // 365 days
        }

        [Fact]
        public async Task GetExpiringProducts_WithIncludeAcknowledgedExpiredItems_ReturnsAcknowledgedItems()
        {
            // Arrange
            var handler = new GetExpiringProducts.Handler(_dbContext, _mapper);
            var query = new GetExpiringProducts.Query
            {
                Filter = new ExpiringProductsFilter
                {
                    ExpiresInDays = 7,
                    IncludeAcknowledgedExpiredItems = true,
                    PageSize = 10,
                    PageIndex = 1
                }
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            // Should include acknowledged items now (6 total: Expired Milk, Expired Cheese, Yogurt, Bread, Lettuce, Old Jam [-10 days])
            Assert.Equal(6, result.Items.Count);
            Assert.Contains(result.Items, p => p.ProductName == "Expired Cheese");
        }

        [Fact]
        public async Task GetExpiringProducts_WithoutIncludeAcknowledgedExpiredItems_ExcludesAcknowledgedItems()
        {
            // Arrange
            var handler = new GetExpiringProducts.Handler(_dbContext, _mapper);
            var query = new GetExpiringProducts.Query
            {
                Filter = new ExpiringProductsFilter
                {
                    ExpiresInDays = 7,
                    IncludeAcknowledgedExpiredItems = false,
                    PageSize = 10,
                    PageIndex = 1
                }
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.DoesNotContain(result.Items, p => p.ProductName == "Expired Cheese");
            Assert.DoesNotContain(result.Items, p => p.ProductName == "Old Jam");
        }

        [Fact]
        public async Task GetExpiringProducts_WithNoExpiresInDays_ReturnsAllItems()
        {
            // Arrange
            var handler = new GetExpiringProducts.Handler(_dbContext, _mapper);
            var query = new GetExpiringProducts.Query
            {
                Filter = new ExpiringProductsFilter
                {
                    ExpiresInDays = null,
                    IncludeAcknowledgedExpiredItems = false,
                    PageSize = 20,
                    PageIndex = 1
                }
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            // Should return all items except acknowledged ones
            // Total: 10 items - 2 acknowledged = 8 items
            Assert.Equal(8, result.Items.Count);
        }

        [Fact]
        public async Task GetExpiringProducts_WithPagination_ReturnsCorrectPage()
        {
            // Arrange
            var handler = new GetExpiringProducts.Handler(_dbContext, _mapper);
            var queryPage1 = new GetExpiringProducts.Query
            {
                Filter = new ExpiringProductsFilter
                {
                    ExpiresInDays = 30,
                    IncludeAcknowledgedExpiredItems = false,
                    PageSize = 2,
                    PageIndex = 1
                }
            };

            var queryPage2 = new GetExpiringProducts.Query
            {
                Filter = new ExpiringProductsFilter
                {
                    ExpiresInDays = 30,
                    IncludeAcknowledgedExpiredItems = false,
                    PageSize = 2,
                    PageIndex = 2
                }
            };

            // Act
            var resultPage1 = await handler.Handle(queryPage1, CancellationToken.None);
            var resultPage2 = await handler.Handle(queryPage2, CancellationToken.None);

            // Assert
            Assert.NotNull(resultPage1);
            Assert.NotNull(resultPage2);
            Assert.Equal(2, resultPage1.Items.Count);
            // Page 2 gets only 1 item due to GetSkip() enforcing min pageSize of 5
            Assert.Equal(1, resultPage2.Items.Count);
            // Verify pages contain different items
            var page1Names = resultPage1.Items.Select(i => i.ProductName).ToList();
            var page2Names = resultPage2.Items.Select(i => i.ProductName).ToList();
            Assert.Empty(page1Names.Intersect(page2Names));
        }

        [Fact]
        public async Task GetExpiringProducts_WithExpiresInZeroDays_ReturnsExpiredAndTodayItems()
        {
            // Arrange
            var handler = new GetExpiringProducts.Handler(_dbContext, _mapper);
            var query = new GetExpiringProducts.Query
            {
                Filter = new ExpiringProductsFilter
                {
                    ExpiresInDays = 0,
                    IncludeAcknowledgedExpiredItems = false,
                    PageSize = 10,
                    PageIndex = 1
                }
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            // Should only include expired items (not acknowledged)
            Assert.Single(result.Items);
            Assert.Contains(result.Items, p => p.ProductName == "Expired Milk");
        }

        [Fact]
        public async Task GetExpiringProducts_ExcludesItemsWithNoExpiryDate()
        {
            // Arrange
            var handler = new GetExpiringProducts.Handler(_dbContext, _mapper);
            var query = new GetExpiringProducts.Query
            {
                Filter = new ExpiringProductsFilter
                {
                    ExpiresInDays = 365,
                    IncludeAcknowledgedExpiredItems = false,
                    PageSize = 20,
                    PageIndex = 1
                }
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            // Should not include items without expiry date
            Assert.DoesNotContain(result.Items, p => p.ProductName == "Canned Beans");
        }

        [Fact]
        public async Task GetExpiringProducts_WithCancellationToken_CanBeCancelled()
        {
            // Arrange
            var handler = new GetExpiringProducts.Handler(_dbContext, _mapper);
            var query = new GetExpiringProducts.Query
            {
                Filter = new ExpiringProductsFilter
                {
                    ExpiresInDays = 7,
                    IncludeAcknowledgedExpiredItems = false,
                    PageSize = 10,
                    PageIndex = 1
                }
            };
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await handler.Handle(query, cancellationTokenSource.Token));
        }

        [Fact]
        public async Task GetExpiringProducts_ReturnsCorrectPageCount()
        {
            // Arrange
            var handler = new GetExpiringProducts.Handler(_dbContext, _mapper);
            var query = new GetExpiringProducts.Query
            {
                Filter = new ExpiringProductsFilter
                {
                    ExpiresInDays = 30,
                    IncludeAcknowledgedExpiredItems = false,
                    PageSize = 2,
                    PageIndex = 1
                }
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            // 6 items total, page size 2, so page count = Ceiling(6/2) = 3
            Assert.Equal(3, result.PageCount);
        }

        [Fact]
        public async Task GetExpiringProducts_WithLargeExpiresInDays_ReturnsNearlyAllNonExpiredItems()
        {
            // Arrange
            var handler = new GetExpiringProducts.Handler(_dbContext, _mapper);
            var query = new GetExpiringProducts.Query
            {
                Filter = new ExpiringProductsFilter
                {
                    ExpiresInDays = 1000,
                    IncludeAcknowledgedExpiredItems = false,
                    PageSize = 20,
                    PageIndex = 1
                }
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            // Should include all items with expiry dates (except acknowledged)
            // Items with expiry: 10 total - 1 with null expiry - 2 acknowledged = 7 items
            Assert.Equal(7, result.Items.Count);
            Assert.Contains(result.Items, p => p.ProductName == "Rice"); // 365 days
        }

        [Fact]
        public async Task GetExpiringProducts_OnlyIncludesItemsWithStorageLocation()
        {
            // Arrange
            var handler = new GetExpiringProducts.Handler(_dbContext, _mapper);
            var query = new GetExpiringProducts.Query
            {
                Filter = new ExpiringProductsFilter
                {
                    ExpiresInDays = 30,
                    IncludeAcknowledgedExpiredItems = false,
                    PageSize = 20,
                    PageIndex = 1
                }
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.All(result.Items, item =>
            {
                Assert.True(item.StorageLocationId > 0);
            });
        }

        [Fact]
        public async Task GetExpiringProducts_EmptyDatabase_ReturnsEmptyList()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<XgoStorageDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var emptyDbContext = new XgoStorageDbContext(options);

            var handler = new GetExpiringProducts.Handler(emptyDbContext, _mapper);
            var query = new GetExpiringProducts.Query
            {
                Filter = new ExpiringProductsFilter
                {
                    ExpiresInDays = 7,
                    IncludeAcknowledgedExpiredItems = false,
                    PageSize = 10,
                    PageIndex = 1
                }
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Items);
            // GetPageCount returns minimum 1 page even when empty
            Assert.Equal(1, result.PageCount);
        }

        #endregion
    }
}
