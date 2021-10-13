using MediatR;
using Microsoft.EntityFrameworkCore;
using Riode.Domain.Models.DataContext;
using Riode.Domain.Models.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.Application.ProductColorModule
{
    public class ProductColorSingleQuery : IRequest<Colors>
    {

        public int Id { get; set; }

        public class ProductColorSingleQueryHandler : IRequestHandler<ProductColorSingleQuery, Colors>
        {
            readonly RiodeDBContext db;
            public ProductColorSingleQueryHandler(RiodeDBContext db)
            {
                this.db = db;
            }
            public async Task<Colors> Handle(ProductColorSingleQuery request, CancellationToken cancellationToken)
            {
                if (request.Id <= 0)
                {
                    return null;
                }

                var color = await db.Colors
               .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

                return color;
            }
        }

    }
}
