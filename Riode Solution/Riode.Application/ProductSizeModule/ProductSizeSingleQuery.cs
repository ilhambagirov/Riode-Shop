using MediatR;
using Microsoft.EntityFrameworkCore;
using Riode.Domain.Models.DataContext;
using Riode.Domain.Models.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.Application.ProductSizeModule
{
    public class ProductSizeSingleQuery : IRequest<Size>
    {
        public int Id { get; set; }

        public class ProductSizeSingleQueryHandler : IRequestHandler<ProductSizeSingleQuery, Size>
        {
            readonly RiodeDBContext db;
            public ProductSizeSingleQueryHandler(RiodeDBContext db)
            {
                this.db = db;
            }
            public async Task<Size> Handle(ProductSizeSingleQuery request, CancellationToken cancellationToken)
            {
                if (request.Id <= 0)
                {
                    return null;
                }

                var size = await db.Size
               .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);
                return size;
            }
        }

    }
}
