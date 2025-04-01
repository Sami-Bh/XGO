using AutoMapper;
using BuildingBlocks.DTOs;
using BuildingBlocks.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.CQRS.Generic.Queries
{
    public class GetIList<dbT, dtoT> where dbT : BaseModel where dtoT : BaseDto
    {
        public class Query : IRequest<IList<dtoT>>
        {

        }

        public class Handler(DbContext dbContext, IMapper mapper) : IRequestHandler<Query, IList<dtoT>>
        {
            public async Task<IList<dtoT>> Handle(Query request, CancellationToken cancellationToken)
            {

                var dbResult = await dbContext.Set<dbT>().AsNoTracking().OrderByDescending(x => x.Id).ToListAsync(cancellationToken);
                return dbResult.Select(x => mapper.Map<dtoT>(x)).ToList();
            }
        }
    }
}
