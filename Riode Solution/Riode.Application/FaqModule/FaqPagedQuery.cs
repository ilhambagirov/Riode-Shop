using MediatR;
using Riode.Domain.Models.DataContext;
using Riode.Domain.Models.Entities;
using Riode.Domain.Models.ViewModels;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Riode.Application.FaqModule
{
    public class FaqPagedQuery : IRequest<PagedViewModel<FAQ>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 3;

        public class FaqPagedQueryQueryHandler : IRequestHandler<FaqPagedQuery, PagedViewModel<FAQ>>
        {
            readonly RiodeDBContext db;
            public FaqPagedQueryQueryHandler(RiodeDBContext db)
            {
                this.db = db;
            }
            public async Task<PagedViewModel<FAQ>> Handle(FaqPagedQuery request, CancellationToken cancellationToken)
            {
                var query = db.FAQs.Where(b => b.DeleteByUserId == null && b.DeleteDate == null)
                     .AsQueryable();

                return new PagedViewModel<FAQ>(query, request.PageIndex, request.PageSize);
            }
        }
    }
}
