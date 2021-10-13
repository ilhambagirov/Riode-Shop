using MediatR;
using Microsoft.EntityFrameworkCore;
using Riode.Domain.Models.DataContext;
using Riode.Domain.Models.Entities;
using Riode.Domain.Models.ViewModels;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.Application.CategoryModule
{
    public class CategoryPagedQuery : IRequest<PagedViewModel<Category>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 3;

        public class CategoryPagedQueryHandler : IRequestHandler<CategoryPagedQuery, PagedViewModel<Category>>
        {
            readonly RiodeDBContext db;
            public CategoryPagedQueryHandler(RiodeDBContext db)
            {
                this.db = db;
            }
            public async Task<PagedViewModel<Category>> Handle(CategoryPagedQuery request, CancellationToken cancellationToken)
            {
                var query = db.Category.Include(c => c.Parent)
                .Include(b => b.Children.Where(b => b.DeleteByUserId == null && b.DeleteDate == null))
                .Where(b => b.ParentId == null && b.DeleteByUserId == null && b.DeleteDate == null)
                     .AsQueryable();
                return new PagedViewModel<Category>(query, request.PageIndex, request.PageSize);
            }
        }
    }
}
