using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XGO.Storage.Api.Storage.Application.DTOs;
using XGO.Storage.Api.Storage.Domain;
using XGO.Storage.Api.Storage.Persistence;

namespace XGO.Storage.Api.Storage.Application.CQRS.StorageItems.Commands
{
    public class Create
    {
        public class Command : IRequest<StoredItemDto>
        {
            public required StoredItemDto StoredItem { get; set; }
        }

        public class Handler(XgoStorageDbContext dbContext, IMapper mapper) : IRequestHandler<Command, StoredItemDto>
        {
            public async Task<StoredItemDto> Handle(Command request, CancellationToken cancellationToken)
            {
                // Verify that the storage location exists
                var storageLocation = await dbContext.StorageLocations
                    .FirstOrDefaultAsync(x => x.Id == request.StoredItem.StorageLocationId, cancellationToken);
                
                if (storageLocation == null)
                    throw new Exception($"Storage location with ID {request.StoredItem.StorageLocationId} not found");

                var storedItem = mapper.Map<StoredItem>(request.StoredItem);
                dbContext.StoredItems.Add(storedItem);
                await dbContext.SaveChangesAsync(cancellationToken);

                return mapper.Map<StoredItemDto>(storedItem);
            }
        }
    }
} 