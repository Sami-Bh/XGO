using Application.DTOs;
using AutoMapper;
using BuildingBlocks.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XGORepository.Models;

namespace Application.CQRS.Subcategories.Queries
{
    public class GetSubcategoryDetails
    {
        public class Query : IRequest<Result<SubCategoryDto>>
        {
            public int Id { get; set; }
        }

        public class Handler(XGODbContext dbContext, IMapper mapper) : IRequestHandler<Query, Result<SubCategoryDto>>
        {
            public async Task<Result<SubCategoryDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var subcategory = await dbContext.SubCategories.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                return subcategory is null ? Result<SubCategoryDto>.Failure(404, "element not found") :
                   Result<SubCategoryDto>.Success(mapper.Map<SubCategoryDto>(subcategory));

            }
        }
    }
}
