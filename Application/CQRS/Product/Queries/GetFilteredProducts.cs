using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApplicationXGO.Models.Filters;
using XGORepository.Models;

namespace Application.CQRS.Product.Queries
{
    public class GetFilteredProducts
    {
        public class Query : IRequest<ListedDto<ProductDto>>
        {
            public required ProductsFilter Filter { get; set; }
        }
        public class Handler(XGODbContext dbContext, IMapper mapper) : IRequestHandler<Query, ListedDto<ProductDto>>
        {
            public async Task<ListedDto<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = dbContext.Products.AsNoTracking().AsQueryable();
                if (request.Filter.SubCategoryId > 0)
                {
                    query= query.Where(x => x.SubCategoryId == request.Filter.SubCategoryId);
                }
                else if (request.Filter.CategoryId > 0)
                {

                    var subIds = await dbContext.SubCategories.Where(x => x.CategoryId == request.Filter.CategoryId).Select(x => x.Id).ToListAsync(cancellationToken);
                    query = query.Where(x => subIds.Contains(x.SubCategoryId));
                }
                query=string.IsNullOrEmpty(request.Filter.SearchText)?
                    query: 
                    query.Where(x=>x.Name.ToLower().Contains(request.Filter.SearchText.ToLower()));

                var count = await query.CountAsync(cancellationToken);
                var pageCount = request.Filter.GetPageCount(count);

                query = query.Skip(request.Filter.GetSkip()).Take(request.Filter.PageSize);
                query = query.OrderBy(x => x.Name);

                var dbProducts = await query.ToListAsync(cancellationToken);
                var mappedDtos= mapper.Map<IList<ProductDto>>(dbProducts);

                return new ListedDto<ProductDto> {Items= mappedDtos,PageCount= pageCount };
            }
        }
    }
}
