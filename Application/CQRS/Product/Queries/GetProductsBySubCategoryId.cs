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
    public class GetProductsBySubCategoryId
    {
        public class Query : IRequest< IList<ProductDto>> {
            public int SubCategoryId { get; set; }
        }
        public class Handler(XGODbContext dbContext,IMapper mapper) : IRequestHandler<Query, IList<ProductDto>>
        {
            public async Task<IList<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                 var dbProducts = await dbContext.Products.Where(x => x.SubCategoryId == request.SubCategoryId).ToListAsync(cancellationToken);
                return mapper.Map<IList<ProductDto>>(dbProducts);
            }
        }
    }
}
