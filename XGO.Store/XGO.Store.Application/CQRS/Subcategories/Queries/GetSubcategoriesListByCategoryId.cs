using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XGO.Store.Application.DTOs;
using XGO.Store.Persistance.Models;

namespace XGO.Store.Application.CQRS.Subcategories.Queries
{
    public class GetSubcategoriesListByCategoryId
    {
        public class Query : IRequest<IList<SubCategoryDto>>
        {
            public int CategoryId { get; set; }
        }

        public class Handler(XGODbContext dbContext, IMapper mapper) : IRequestHandler<Query, IList<SubCategoryDto>>
        {
            public async Task<IList<SubCategoryDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await dbContext.SubCategories.Where(x => x.CategoryId == request.CategoryId).Select(x => mapper.Map<SubCategoryDto>(x)).ToListAsync(cancellationToken);
            }
        }
    }
}
