using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core;
using Application.DTOs;
using AutoMapper;
using MediatR;
using XGOModels;
using XGORepository.Models;

namespace Application.CQRS.Generic.Commands
{
    public class EditItem<dbT, dtoT> where dbT : BaseModel where dtoT : BaseDto
    {
        public class Command : IRequest<Result<Unit>>
        {
            public dtoT Dto;
        }

        public class Handler(XGODbContext dbContext, IMapper mapper) : IRequestHandler<Command, Result<Unit>>
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
