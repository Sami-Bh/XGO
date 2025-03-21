using Application.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XGOModels;
using XGORepository.Models;

namespace Application.CQRS.Generic.Queries
{
    public class GetIList<dbT, dtoT> where dbT : BaseModel where dtoT : BaseDto
    {
        public class Query : IRequest<IList<dtoT>>
        {

        }

        public class Handler(XGODbContext dbContext, IMapper mapper) : IRequestHandler<Query, IList<dtoT>>
        {
            public async Task<IList<dtoT>> Handle(Query request, CancellationToken cancellationToken)
            {

                var dbResult = await dbContext.Set<dbT>().AsNoTracking().OrderByDescending(x=>x.Id).ToListAsync(cancellationToken);
                return dbResult.Select(x => mapper.Map<dtoT>(x)).ToList();
            }
        }
    }
}
