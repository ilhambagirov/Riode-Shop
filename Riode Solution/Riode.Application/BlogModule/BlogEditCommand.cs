using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Riode.Application.Core.Extensions;
using Riode.Domain.Models.DataContext;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.Application.BlogModule
{
    public class BlogEditCommand : BlogViewModel, IRequest<int>
    {
        public class BlogEditCommandHandler : IRequestHandler<BlogEditCommand, int>
        {
            readonly RiodeDBContext db;
            readonly IActionContextAccessor ctx;
            readonly IHostEnvironment env;
            public BlogEditCommandHandler(RiodeDBContext db, IActionContextAccessor ctx, IHostEnvironment env)
            {
                this.db = db;
                this.ctx = ctx;
                this.env = env;
            }
            public async Task<int> Handle(BlogEditCommand request, CancellationToken cancellationToken)
            {

                if (request.Id == null || request.Id < 0)
                {
                    return 0;
                }

                if (string.IsNullOrEmpty(request.fileTemp) && request.File == null)
                {
                    ctx.ActionContext.ModelState.AddModelError("file", "Not Chosen");
                }

                var entity = await db.Blogs.FirstOrDefaultAsync(b => b.Id == request.Id && b.DeleteByUserId == null);

                if (entity == null)
                {
                    return 0;
                }

                if (ctx.IsModelStateValid())
                {
                    entity.Name = request.Name;
                    entity.Description = request.Description;
                    entity.CategoryId = request.CategoryId;
                    entity.Name = request.Name;
                    entity.Description = request.Description;
                    entity.CategoryId = request.CategoryId;

                    if (request.File != null)
                    {
                        var extension = Path.GetExtension(request.File.FileName);
                        request.fileTemp = $"{Guid.NewGuid()}{extension}";
                        var physicalAddress = Path.Combine(env.ContentRootPath,
                            "wwwroot",
                            "uploads",
                            "images",
                            "blog",
                             request.fileTemp);

                        using (var stream = new FileStream(physicalAddress, FileMode.Create, FileAccess.Write))
                        {
                            await request.File.CopyToAsync(stream);
                        }

                        if (!string.IsNullOrEmpty(entity.ImagePath))
                        {
                            System.IO.File.Delete(Path.Combine(env.ContentRootPath,
                                                       "wwwroot",
                                                       "uploads",
                                                       "images",
                                                       "blog",
                                                       entity.ImagePath));
                        }
                        entity.ImagePath = request.fileTemp;
                    }
                    await db.SaveChangesAsync(cancellationToken);
                    return entity.Id;

                }
                return 0;
            }
        }
    }
}
