using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XGORepository.Models;

namespace Application.CQRS.Product.Queries
{
    public class GetFilteredProducts
    {
        public class Query : IRequest<IList<ProductDto>>
        {
            public int SubCategoryId { get; set; }
            public int CategoryId { get; set; }
            public string SearchText { get; set; } = "";
        }
        public class Handler(XGODbContext dbContext, IMapper mapper) : IRequestHandler<Query, IList<ProductDto>>
        {
            public async Task<IList<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = dbContext.Products.AsNoTracking().AsQueryable();
                if (request.SubCategoryId > 0)
                {
                    query= query.Where(x => x.SubCategoryId == request.SubCategoryId);
                }
                else if (request.CategoryId > 0)
                {

                    var subIds = await dbContext.SubCategories.Where(x => x.CategoryId == request.CategoryId).Select(x => x.Id).ToListAsync(cancellationToken);
                    query = query.Where(x => subIds.Contains(x.SubCategoryId));
                }
                query=string.IsNullOrEmpty(request.SearchText)?
                    query: 
                    query.Where(x=>x.Name.ToLower().Contains(request.SearchText.ToLower()));

                var dbProducts = await query.ToListAsync(cancellationToken);
                return mapper.Map<IList<ProductDto>>(dbProducts);
            }
        }
    }
}
