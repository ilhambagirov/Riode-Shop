using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Riode.Application.Core.Extensions;
using Riode.Domain.Models.DataContext;
using Riode.Domain.Models.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.Application.FaqModule
{
   
    public class FaqCreateCommand : IRequest<int>
    {
        public string Question { get; set; }
        public string Answer { get; set; }

        public class FaqCreateCommandHandler : IRequestHandler<FaqCreateCommand, int>
        {

            readonly RiodeDBContext db;
            readonly IActionContextAccessor ctx;
            public FaqCreateCommandHandler(RiodeDBContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<int> Handle(FaqCreateCommand request, CancellationToken cancellationToken)
            {
                if (ctx.IsModelStateValid())
                {
                    var faq = new FAQ();
                    faq.Question = request.Question;
                    faq.Answer = request.Answer;
                    
                    db.FAQs.Add(faq);
                    await db.SaveChangesAsync(cancellationToken);
                    return faq.Id;
                }
                return 0;

            }
        }
    }
}
