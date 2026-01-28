using AutoMapper;
using BuildingBlocks.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XGO.Storage.Api.Storage.Application.DTOs;
using XGO.Storage.Api.Storage.Application.Models;
using XGO.Storage.Api.Storage.Persistence;

namespace XGO.Storage.Api.Storage.Application.CQRS.StorageItems.Queries
{
    public class GetExpiringProducts
    {
        public class Query : IRequest<ListedDto<StoredItemDto>>
        {
            public required ExpiringProductsFilter Filter { get; set; }
        }
        public class Handler(XgoStorageDbContext dbContext, IMapper mapper) : IRequestHandler<Query, ListedDto<StoredItemDto>>
        {
            public async Task<ListedDto<StoredItemDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var filter = request.Filter;

                var query = dbContext.StoredItems.Include(x => x.StorageLocation).AsNoTracking().AsQueryable();
                query = filter.ExpiresInDays.HasValue ?
                    query.Where(x => x.ProductExpiryDate.HasValue && x.ProductExpiryDate.Value <= DateTime.Today.AddDays(filter.ExpiresInDays.Value)) :
                    query;
                query = !filter.IncludeAcknowledgedExpiredItems ? query.Where(x => !x.IsExpiracyAcknowledged) : query;

                // Get total count before pagination
                var totalCount = await query.CountAsync(cancellationToken);

                // Order and paginate
                query = query.OrderBy(x => x.StorageLocationId).ThenBy(x => x.ProductExpiryDate).ThenBy(x => x.ProductName);
                query = query.Skip(filter.GetSkip()).Take(filter.PageSize);

                var data = await query.ToListAsync(cancellationToken);
                var pageCount = filter.GetPageCount(totalCount);

                var mappedData = mapper.Map<IList<StoredItemDto>>(data);
                return new ListedDto<StoredItemDto> { Items = mappedData, PageCount = pageCount };
            }
        }
    }
}
