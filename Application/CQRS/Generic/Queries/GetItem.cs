using Application.Core;
using Application.DTOs;
using AutoMapper;
using MediatR;
using XGOModels;
using XGORepository.Models;

namespace Application.CQRS.Generic.Queries
{
    public class GetItem<dbT, dtoT> where dbT : BaseModel where dtoT : BaseDto
    {
        public class Query : IRequest<Result<dtoT>>
        {
            public int Id { get; set; }
        }

        public class Handler(XGODbContext dbContext,IMapper mapper) : IRequestHandler<Query, Result<dtoT>>
        {
            public async Task<Result<dtoT>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await dbContext.Set<dbT>().FindAsync(request.Id, cancellationToken);
                return result is null ? Result<dtoT>.Failure(404, "Element not found") : Result<dtoT>.Success(mapper.Map<dtoT>( result));
            }
        }
    }
}
