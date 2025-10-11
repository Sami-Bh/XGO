using AutoMapper;
using BuildingBlocks.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XGO.Storage.Api.Storage.Application.DTOs;
using XGO.Storage.Api.Storage.Persistence;

namespace XGO.Storage.Api.Storage.Application.CQRS.StorageLocation.Queries
{
    public class GetList
    {
        public class Query : IRequest<Result<IList<StorageLocationDto>>>
        {

        }

        public class Handler(XgoStorageDbContext dbContext, IMapper mapper) : IRequestHandler<Query, Result<IList<StorageLocationDto>>>
        {
            public async Task<Result<IList<StorageLocationDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var storageLocations = await dbContext.StorageLocations.Include(x => x.StoredItems).ToListAsync(cancellationToken);
                return Result<IList<StorageLocationDto>>.Success(storageLocations.Select(x => mapper.Map<StorageLocationDto>(x)).ToList());
            }
        }
    }
}
