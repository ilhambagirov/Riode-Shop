using MediatR;
using Microsoft.EntityFrameworkCore;
using Riode.Domain.Models.DataContext;
using Riode.Domain.Models.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.Application.BrandModule
{
    public class BrandSingleQuery : IRequest<Brands>
    {

        public int Id { get; set; }

        public class BrandSingleQueryHandler : IRequestHandler<BrandSingleQuery, Brands>
        {
            readonly RiodeDBContext db;
            public BrandSingleQueryHandler(RiodeDBContext db)
            {
                this.db = db;
            }
            public async Task<Brands> Handle(BrandSingleQuery request, CancellationToken cancellationToken)
            {
                if (request.Id <= 0)
                {
                    return null;
                }

                var brand = await db.Brands
               .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

                return brand;
            }
        }

    }
}
