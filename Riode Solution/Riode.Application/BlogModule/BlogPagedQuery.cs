using MediatR;
using Microsoft.EntityFrameworkCore;
using Riode.Domain.Models.DataContext;
using Riode.Domain.Models.Entities;
using Riode.Domain.Models.ViewModels;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.Application.BlogModule
{
    public class BlogPagedQuery : IRequest<PagedViewModel<Blog>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 3;

        public class BlogPagedQueryHandler : IRequestHandler<BlogPagedQuery, PagedViewModel<Blog>>
        {
            readonly RiodeDBContext db;
            public BlogPagedQueryHandler(RiodeDBContext db)
            {
                this.db = db;
            }
            public async Task<PagedViewModel<Blog>> Handle(BlogPagedQuery request, CancellationToken cancellationToken)
            {
                var query = db.Blogs
                    .Include(b => b.Category)
                .Where(b => b.DeleteByUserId == null && b.PublishedDate != null)
                     .AsQueryable();

                return new PagedViewModel<Blog>(query, request.PageIndex, request.PageSize);
            }
        }
    }
}
