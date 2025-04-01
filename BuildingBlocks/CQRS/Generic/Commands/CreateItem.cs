using AutoMapper;
using BuildingBlocks.Core;
using BuildingBlocks.DTOs;
using BuildingBlocks.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.CQRS.Generic.Commands
{
    public class CreateItem<dbT, dtoT> where dbT : BaseModel where dtoT : BaseDto
    {
        public class Command : IRequest<Result<int>>
        {
            public required dtoT Dto { get; set; }
        }

        public class Handler(DbContext dbContext, IMapper mapper) : IRequestHandler<Command, Result<int>>
        {
            public async Task<Result<int>> Handle(Command request, CancellationToken cancellationToken)
            {
                var objectToAdd = mapper.Map<dbT>(request.Dto);
                await dbContext.Set<dbT>().AddAsync(objectToAdd, cancellationToken);

                var operationResult = await dbContext.SaveChangesAsync(cancellationToken);

                return operationResult > 0 ?
                    Result<int>.Success(objectToAdd.Id) :
                    Result<int>.Failure(400, "Failed to delete data");
            }
        }
    }
}
