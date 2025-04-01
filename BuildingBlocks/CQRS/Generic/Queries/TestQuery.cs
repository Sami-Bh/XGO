using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace BuildingBlocks.CQRS.Generic.Queries
{
    public class TestQuery
    {
        public class Query:IRequest<string> { }

        public class Handler : IRequestHandler<Query, string>
        {
            public async Task<string> Handle(Query request, CancellationToken cancellationToken)
            {
                await Task.Delay(500, cancellationToken);
                return "hello";
            }
        }
    }
}
