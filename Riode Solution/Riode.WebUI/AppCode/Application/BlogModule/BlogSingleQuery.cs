using MediatR;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Application.BlogModule
{
    public class BlogSingleQuery : IRequest<Blog>
    {
        public int? Id { get; set; }

        public class BlogSingleQueryHandler : IRequestHandler<BlogSingleQuery, Blog>
        {
            readonly RiodeDBContext db;
            public BlogSingleQueryHandler(RiodeDBContext db)
            {
                this.db = db;
            }
            public async Task<Blog> Handle(BlogSingleQuery request, CancellationToken cancellationToken)
            {

                if (request.Id == null)
                {
                    return null;
                }

                var blog = await db.Blogs
                    .Include(b => b.Category)
                    .FirstOrDefaultAsync(m => m.Id == request.Id);

                return blog;
            }
        }
    }
}
