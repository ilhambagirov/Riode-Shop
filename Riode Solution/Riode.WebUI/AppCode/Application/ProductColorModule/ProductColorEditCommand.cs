using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.AppCode.Extensions;
using Riode.WebUI.Models.DataContext;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Application.ProductColorModule
{
    public class ProductColorEditCommand : ProductColorViewModel, IRequest<int>
    {

        public class ProductColorEditCommandHandler : IRequestHandler<ProductColorEditCommand, int>
        {
            readonly RiodeDBContext db;
            readonly IActionContextAccessor ctx;
            public ProductColorEditCommandHandler(RiodeDBContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<int> Handle(ProductColorEditCommand request, CancellationToken cancellationToken)
            {

                if (/*request.Id == null ||*/ request.Id < 0)
                {
                    return 0;
                }

                var entity = await db.Colors.FirstOrDefaultAsync(b => b.Id == request.Id && b.DeleteByUserId == null);

                if (entity == null)
                {
                    return 0;
                }

                if (ctx.IsModelStateValid())
                {
                    entity.Name = request.Name;
                    entity.Description = request.Description;
                    entity.HexCode = request.HexCode;
                    await db.SaveChangesAsync(cancellationToken);

                    return entity.Id;
                }
                return 0;

            }
        }
    }
}
