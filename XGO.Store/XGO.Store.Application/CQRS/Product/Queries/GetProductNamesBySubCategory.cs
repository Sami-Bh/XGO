using MediatR;
using Microsoft.EntityFrameworkCore;
using XGO.Store.Persistance.Models;

namespace XGO.Store.Application.CQRS.Product.Queries
{
    public class GetProductNamesBySubCategory
    {
        public class Query : IRequest<List<string>>
        {
            public required int SubCategoryId { get; set; }
        }

        public class Handler(XGODbContext dbContext) : IRequestHandler<Query, List<string>>
        {
            public async Task<List<string>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await dbContext.Products
                    .AsNoTracking()
                    .Where(x => x.SubCategoryId == request.SubCategoryId)
                    .Select(x => x.Name)
                    .OrderBy(x => x)
                    .ToListAsync(cancellationToken);
            }
        }
    }
} 