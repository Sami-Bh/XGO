using MediatR;
using Microsoft.EntityFrameworkCore;
using XGO.Store.Persistance.Models;

namespace XGO.Store.Application.CQRS.Product.Queries
{
    public class GetProductNamesBySubCategory
    {
        public record ProductInfo(int Id, string Name);

        public class Query : IRequest<List<ProductInfo>>
        {
            public required int SubCategoryId { get; set; }
        }

        public class Handler(XGODbContext dbContext) : IRequestHandler<Query, List<ProductInfo>>
        {
            public async Task<List<ProductInfo>> Handle(Query request, CancellationToken cancellationToken)
            {
                var dbitems = await dbContext.Products
                   .AsNoTracking()
                   .Where(x => x.SubCategoryId == request.SubCategoryId)
                   .OrderBy(x => x.Name)
                   .ToListAsync(cancellationToken);
                return dbitems.Select(x => new ProductInfo(x.Id, x.Name)).ToList();

            }
        }
    }
}