using MediatR;
using Microsoft.EntityFrameworkCore;
using Riode.Domain.Models.DataContext;
using Riode.Domain.Models.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.Application.FaqModule
{
 
    public class FaqSingleQuery : IRequest<FAQ>
    {

        public int Id { get; set; }

        public class FaqSingleQueryHandler : IRequestHandler<FaqSingleQuery, FAQ>
        {
            readonly RiodeDBContext db;
            public FaqSingleQueryHandler(RiodeDBContext db)
            {
                this.db = db;
            }
            public async Task<FAQ> Handle(FaqSingleQuery request, CancellationToken cancellationToken)
            {
                if (request.Id <= 0)
                {
                    return null;
                }

                var faq = await db.FAQs
               .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

                return faq;
            }
        }

    }
}
