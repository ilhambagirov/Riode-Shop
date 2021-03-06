using MediatR;
using Riode.Domain.Models.DataContext;
using Riode.Domain.Models.Entities;
using Riode.Domain.Models.ViewModels;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.Application.ProductSizeModule
{
    public class ProductSizePagedQuery : IRequest<PagedViewModel<Size>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 3;

        public class ProductSizePagedQueryHandler : IRequestHandler<ProductSizePagedQuery, PagedViewModel<Size>>
        {
            readonly RiodeDBContext db;
            public ProductSizePagedQueryHandler(RiodeDBContext db)
            {
                this.db = db;
            }
            public async Task<PagedViewModel<Size>> Handle(ProductSizePagedQuery request, CancellationToken cancellationToken)
            {
                var query = db.Size.Where(b => b.DeleteByUserId == null && b.DeleteDate == null)
                     .AsQueryable();

                return new PagedViewModel<Size>(query, request.PageIndex, request.PageSize);
            }
        }
    }
}
