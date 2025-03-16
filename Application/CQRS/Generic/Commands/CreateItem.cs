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
    public class CreateItem<dbT, dtoT> where dbT : BaseModel where dtoT : BaseDto
    {
        public class Command : IRequest<Result<int>>
        {
            public dtoT Dto;
        }

        public class Handler(XGODbContext dbContext, IMapper mapper) : IRequestHandler<Command, Result<int>>
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
