using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core;
using Application.DTOs;
using MediatR;
using XGOModels;
using XGORepository.Models;

namespace Application.CQRS.Generic.Commands
{
    public class DeleteItem<dbT, dtoT> where dbT : BaseModel where dtoT : BaseDto
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
        }

        public class Handler(XGODbContext dbContext) : IRequestHandler<Command, Result<Unit>>
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