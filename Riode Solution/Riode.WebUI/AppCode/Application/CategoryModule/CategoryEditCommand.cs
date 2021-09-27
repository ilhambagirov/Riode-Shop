using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.AppCode.Extensions;
using Riode.WebUI.Models.DataContext;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Application.CategoryModule
{
    public class CategoryEditCommand : CategoryViewModel, IRequest<int>
    {

        public class CategoryEditCommandHandler : IRequestHandler<CategoryEditCommand, int>
        {
            readonly RiodeDBContext db;
            readonly IActionContextAccessor ctx;
            public CategoryEditCommandHandler(RiodeDBContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<int> Handle(CategoryEditCommand request, CancellationToken cancellationToken)
            {

                if (/*request.Id == null ||*/ request.Id < 0)
                {
                    return 0;
                }

                var entity = await db.Category.FirstOrDefaultAsync(b => b.Id == request.Id && b.DeleteByUserId == null);

                if (entity == null)
                {
                    return 0;
                }

                if (ctx.IsModelStateValid())
                {
                    entity.ParentId = request.ParentId;
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
