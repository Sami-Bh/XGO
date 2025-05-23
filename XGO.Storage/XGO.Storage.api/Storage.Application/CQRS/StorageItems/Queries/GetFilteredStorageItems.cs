﻿using AutoMapper;
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
                var query = dbContext.StoredItems.Include(x => x.StorageLocation).AsNoTracking().AsQueryable();

                query = string.IsNullOrEmpty(filter.ProductNameSearchText) ? query : query.Where(x => x.ProductName.Contains(filter.ProductNameSearchText.ToLower()));
                query = filter.StorageId is null ? query : query.Where(x => x.StorageLocation != null && x.StorageLocation.Id == filter.StorageId);
                query = query.OrderBy(x => x.Id);

                var count = await query.CountAsync(cancellationToken);
                var pageCount = filter.GetPageCount(count);

                query = query.Skip(filter.GetSkip()).Take(filter.PageSize);

                var data = await query.ToListAsync(cancellationToken);

                var mappedData = mapper.Map<IList<StoredItemDto>>(data);
                return new ListedDto<StoredItemDto> { Items = mappedData, PageCount = pageCount };

            }
        }
    }
}
