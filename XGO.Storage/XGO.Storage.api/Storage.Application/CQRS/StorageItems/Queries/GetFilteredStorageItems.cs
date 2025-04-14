using AutoMapper;
using BuildingBlocks.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XGO.Storage.Api.Storage.Application.DTOs;
using XGO.Storage.Api.Storage.Application.Models;
using XGO.Storage.Api.Storage.Persistence;

namespace XGO.Storage.Api.Storage.Application.CQRS.StorageItems.Queries
{
    public class GetFilteredStorageItems
    {
        public class Query : IRequest<ListedDto<StoredItemDto>>
        {
            public required ProductsFilter ProductsFilter { get; set; }
        }
        public class Handler(XgoStorageDbContext dbContext, IMapper mapper) : IRequestHandler<Query, ListedDto<StoredItemDto>>
        {
            public async Task<ListedDto<StoredItemDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var filter = request.ProductsFilter;
                var query = dbContext.StoredItems.AsNoTracking().AsQueryable();

                query = string.IsNullOrEmpty(filter.ProductNameSearchText) ? query : query.Where(x => x.ProductName.Contains(filter.ProductNameSearchText.ToLower()));
                query = query.OrderBy(x => x.ProductName);

                var pageCount = await query.CountAsync(cancellationToken);
                query = query.Skip(filter.GetSkip()).Take(filter.PageSize);

                var data = await query.ToListAsync(cancellationToken);

                var mappedData = mapper.Map<IList<StoredItemDto>>(data);
                return new ListedDto<StoredItemDto> { Items = mappedData, PageCount = pageCount };

            }
        }
    }
}
