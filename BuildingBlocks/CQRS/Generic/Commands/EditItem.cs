using AutoMapper;
using BuildingBlocks.Core;
using BuildingBlocks.DTOs;
using BuildingBlocks.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.CQRS.Generic.Commands
{
    public class EditItem<dbT, dtoT> where dbT : BaseModel where dtoT : BaseDto
    {
        public class Command : IRequest<Result<Unit>>
        {
            public dtoT Dto;
        }

        public class Handler(DbContext dbContext, IMapper mapper) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var mappedObject = mapper.Map<dbT>(request.Dto);

                dbContext.Update(mappedObject);
                var operationResult = await dbContext.SaveChangesAsync(cancellationToken);

                return operationResult > 0 ?
                    Result<Unit>.Success(Unit.Value) :
                    Result<Unit>.Failure(400, "Failed to delete data");
            }
        }

    }
}
