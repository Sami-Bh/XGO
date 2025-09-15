using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XGO.Storage.Api.Storage.Application.DTOs;
using XGO.Storage.Api.Storage.Domain;
using XGO.Storage.Api.Storage.Persistence;

namespace XGO.Storage.Api.Storage.Application.CQRS.StorageItems.Commands
{
    public class UpdateBatch
    {
        public class Command : IRequest<List<StoredItemDto>>
        {
            public required List<StoredItemDto> StoredItems { get; set; }
        }

        public class Handler(XgoStorageDbContext dbContext, IMapper mapper) : IRequestHandler<Command, List<StoredItemDto>>
        {
            public async Task<List<StoredItemDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var updatedItems = new List<StoredItem>();

                foreach (var itemDto in request.StoredItems)
                {
                    var existingItem = await dbContext.StoredItems
                        .FirstOrDefaultAsync(x => x.Id == itemDto.Id, cancellationToken);

                    if (existingItem == null)
                        throw new Exception($"Stored item with ID {itemDto.Id} not found");

                    // Update the properties
                    existingItem.Quantity = itemDto.Quantity;
                    existingItem.ProductExpiryDate = itemDto.ProductExpiryDate;

                    updatedItems.Add(existingItem);
                }

                await dbContext.SaveChangesAsync(cancellationToken);

                return updatedItems.Select(item => mapper.Map<StoredItemDto>(item)).ToList();
            }
        }
    }
} 