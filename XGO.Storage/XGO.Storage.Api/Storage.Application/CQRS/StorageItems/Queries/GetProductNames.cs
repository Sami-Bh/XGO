using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XGO.Storage.Api.Storage.Application.DTOs;
using XGO.Storage.Api.Storage.Persistence;

namespace XGO.Storage.Api.Storage.Application.CQRS.StorageItems.Queries
{
    public class GetProductNames
    {
        public class Query : IRequest<IList<StoredItemNameDto>>
        {
            public required string SearchText { get; set; }
        }

        public class Handler(XgoStorageDbContext dbContext, IMapper mapper) : IRequestHandler<Query, IList<StoredItemNameDto>>
        {
            public async Task<IList<StoredItemNameDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var searchText = request.SearchText.ToLower();
                var dbResult = await dbContext.StoredItems.Where(x => x.ProductName.ToLower().Contains(searchText)).ToListAsync(cancellationToken);
                var mappedResult = mapper.Map<IList<StoredItemNameDto>>(dbResult);
                return mappedResult;
            }
        }
    }
}
