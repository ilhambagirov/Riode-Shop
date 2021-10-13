using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Riode.Application.Core.Extensions;
using Riode.Domain.Models.DataContext;
using Riode.Domain.Models.Entities;
using System.Threading;
using System.Threading.Tasks;


namespace Riode.Application.ProductSizeModule
{
    public class ProductSizeCreateCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Abbr { get; set; }

        public class ProductSizeCreateCommandHandler : IRequestHandler<ProductSizeCreateCommand, int>
        {

            readonly RiodeDBContext db;
            readonly IActionContextAccessor ctx;
            public ProductSizeCreateCommandHandler(RiodeDBContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<int> Handle(ProductSizeCreateCommand request, CancellationToken cancellationToken)
            {
                if (ctx.IsModelStateValid())
                {
                    var size = new Size();
                    size.Name = request.Name;
                    size.Description = request.Description;
                    size.Abbr = request.Abbr;
                    db.Size.Add(size);
                    await db.SaveChangesAsync(cancellationToken);
                    return size.Id;
                }
                return 0;

            }
        }
    }
}
