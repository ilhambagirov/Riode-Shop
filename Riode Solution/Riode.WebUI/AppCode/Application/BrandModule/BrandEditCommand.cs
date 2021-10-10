using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.AppCode.Extensions;
using Riode.WebUI.Models.DataContext;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Application.BrandModule
{
    public class BrandEditCommand : BrandViewModel, IRequest<int>
    {

        public class BrandEditCommandHandler : IRequestHandler<BrandEditCommand, int>
        {
            readonly RiodeDBContext db;
            readonly IActionContextAccessor ctx;
            public BrandEditCommandHandler(RiodeDBContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<int> Handle(BrandEditCommand request, CancellationToken cancellationToken)
            {

                if (request.Id == null || request.Id < 0)
                {
                    return 0;
                }

                var entity = await db.Brands.FirstOrDefaultAsync(b => b.Id == request.Id && b.DeleteByUserId == null);

                if (entity == null)
                {
                    return 0;
                }

                if (ctx.IsModelStateValid())
                {
                    entity.Name = request.Name;
                    entity.Description = request.Description;
                    await db.SaveChangesAsync(cancellationToken);

                    return entity.Id;
                }
                return 0;

            }
        }
    }
}
