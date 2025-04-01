using BuildingBlocks.Core;
using BuildingBlocks.DTOs;
using BuildingBlocks.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.CQRS.Generic.Commands
{
    public class DeleteItem<dbT, dtoT> where dbT : BaseModel where dtoT : BaseDto
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
        }

        public class Handler(DbContext dbContext) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var dbObject = await dbContext.Set<dbT>().FindAsync(request.Id, cancellationToken);
                if (dbObject is null)
                    return Result<Unit>.Failure(404, string.Empty);

                dbContext.Remove(dbObject);

                var operationResult = await dbContext.SaveChangesAsync(cancellationToken);

                return operationResult > 0 ?
                    Result<Unit>.Success(Unit.Value) :
                    Result<Unit>.Failure(400, "Failed to delete data");


            }
        }

    }
}