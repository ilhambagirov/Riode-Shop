using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Hosting;
using Riode.Application.Core.Extensions;
using Riode.Domain.Models.DataContext;
using Riode.Domain.Models.Entities;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.Application.BlogModule
{
    public class BlogCreateCommand : IRequest<int>
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
        public int CategoryId { get; set; }
        public DateTime? PublishedDate { get; set; }
        public class BlogCreateCommandHandler : IRequestHandler<BlogCreateCommand, int>
        {

            readonly RiodeDBContext db;
            readonly IActionContextAccessor ctx;
            readonly IHostEnvironment env;
            public BlogCreateCommandHandler(RiodeDBContext db, IActionContextAccessor ctx, IHostEnvironment env)
            {
                this.db = db;
                this.ctx = ctx;
                this.env = env;
            }
            public async Task<int> Handle(BlogCreateCommand request, CancellationToken cancellationToken)
            {
                if (request.File == null)
                {
                    ctx.ActionContext.ModelState.AddModelError("file", "Not chosen");
                };

                if (ctx.IsModelStateValid())
                {
                    var blog = new Blog();
                    blog.Name = request.Name;
                    blog.Description = request.Description;
                    blog.CategoryId = request.CategoryId;
                    blog.PublishedDate = request.PublishedDate;
                    var extension = Path.GetExtension(request.File.FileName);
                    blog.ImagePath = $"{Guid.NewGuid()}{extension}";
                    var physicalAddress = Path.Combine(env.ContentRootPath,
                        "wwwroot",
                        "uploads",
                        "images",
                        "blog",
                        blog.ImagePath);

                    using (var stream = new FileStream(physicalAddress, FileMode.Create, FileAccess.Write))
                    {
                        await request.File.CopyToAsync(stream);
                    }

                    db.Add(blog);
                    await db.SaveChangesAsync(cancellationToken);
                    return blog.Id;
                }
                return 0;

            }
        }
    }
}
