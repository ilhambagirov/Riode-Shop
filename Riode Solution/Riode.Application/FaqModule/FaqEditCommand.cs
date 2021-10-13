using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Riode.Application.Core.Extensions;
using Riode.Domain.Models.DataContext;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.Application.FaqModule
{
    
    public class FaqEditCommand : FaqViewModel, IRequest<int>
    {

        public class FaqEditCommandHandler : IRequestHandler<FaqEditCommand, int>
        {
            readonly RiodeDBContext db;
            readonly IActionContextAccessor ctx;
            public FaqEditCommandHandler(RiodeDBContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<int> Handle(FaqEditCommand request, CancellationToken cancellationToken)
            {

                if (request.Id == null || request.Id < 0)
                {
                    return 0;
                }

                var entity = await db.FAQs.FirstOrDefaultAsync(b => b.Id == request.Id && b.DeleteByUserId == null);

                if (entity == null)
                {
                    return 0;
                }

                if (ctx.IsModelStateValid())
                {
                    entity.Question = request.Question;
                    entity.Answer = request.Answer;
                    await db.SaveChangesAsync(cancellationToken);
                    return entity.Id;
                }
                return 0;

            }
        }
    }
}
