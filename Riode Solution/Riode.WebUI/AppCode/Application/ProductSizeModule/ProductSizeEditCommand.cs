using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.AppCode.Extensions;
using Riode.WebUI.Models.DataContext;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Application.ProductSizeModule
{
    public class ProductSizeEditCommand : ProductSizeViewModel, IRequest<int>
    {

        public class ProductSizeEditCommandHandler : IRequestHandler<ProductSizeEditCommand, int>
        {
            readonly RiodeDBContext db;
            readonly IActionContextAccessor ctx;
            public ProductSizeEditCommandHandler(RiodeDBContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<int> Handle(ProductSizeEditCommand request, CancellationToken cancellationToken)
            {

                if (request.Id == null || request.Id < 0)
                {
                    return 0;
                }

                var entity = await db.Size.FirstOrDefaultAsync(b => b.Id == request.Id && b.DeleteByUserId == null);

                if (entity == null)
                {
                    return 0;
                }

                if (ctx.IsModelStateValid())
                {
                    entity.Name = request.Name;
                    entity.Description = request.Description;
                    entity.Abbr = request.Abbr;
                    await db.SaveChangesAsync(cancellationToken);

                    return entity.Id;
                }
                return 0;

            }
        }
    }
}
