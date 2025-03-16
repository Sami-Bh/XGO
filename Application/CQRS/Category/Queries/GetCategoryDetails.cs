using Application.Core;
using Application.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XGOModels;
using XGORepository.Models;

namespace Application.CQRS.Generic.Queries
{
    public class GetCategoryDetails
    {
        public class Query : IRequest<Result<CategoryDto>>
        {
            public int Id { get; set; }
        }

        public class Handler(XGODbContext dbContext, IMapper mapper) : IRequestHandler<Query, Result<CategoryDto>>
        {
            public async Task<Result<CategoryDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await dbContext.Categories.Include(x => x.SubCategories).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                return result is null ? Result<CategoryDto>.Failure(404, "Element not found") : Result<CategoryDto>.Success(mapper.Map<CategoryDto>(result));
            }
        }
    }
}
