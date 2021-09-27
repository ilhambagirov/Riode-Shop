using MediatR;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.Entities;
using Riode.WebUI.Models.ViewModels;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Application.ProductColorModule
{

    public class ProductColorPagedQuery : IRequest<PagedViewModel<Colors>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 3;

        public class ProductColorPagedQueryHandler : IRequestHandler<ProductColorPagedQuery, PagedViewModel<Colors>>
        {
            readonly RiodeDBContext db;
            public ProductColorPagedQueryHandler(RiodeDBContext db)
            {
                this.db = db;
            }
            public async Task<PagedViewModel<Colors>> Handle(ProductColorPagedQuery request, CancellationToken cancellationToken)
            {
                var query = db.Colors.Where(b => b.DeleteByUserId == null && b.DeleteDate == null)
                     .AsQueryable();

                return new PagedViewModel<Colors>(query, request.PageIndex, request.PageSize);
            }
        }
    }
}
