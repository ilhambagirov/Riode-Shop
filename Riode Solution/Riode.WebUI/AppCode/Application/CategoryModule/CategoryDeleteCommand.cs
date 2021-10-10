using MediatR;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.AppCode.Infrastructure;
using Riode.WebUI.Models.DataContext;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Application.CategoryModule
{
    public class CategoryDeleteCommand : IRequest<CommandJsonResponse>
    {
        public int? Id { get; set; }
        public class BrandDeleteCommandHandler : IRequestHandler<CategoryDeleteCommand, CommandJsonResponse>
        {
            readonly RiodeDBContext db;
            public BrandDeleteCommandHandler(RiodeDBContext db)
            {
                this.db = db;
            }

            public async Task<CommandJsonResponse> Handle(CategoryDeleteCommand request, CancellationToken cancellationToken)
            {
                var response = new CommandJsonResponse();
                if (request.Id == null || request.Id < 0)
                {
                    response.Error = true;
                    response.Message = "Item is not defined";
                    goto end;
                }
                var category = await db.Category
               .Include(c => c.Parent)
               .FirstOrDefaultAsync(m => m.Id == request.Id);

                if (category == null)
                {
                    response.Error = true;
                    response.Message = "Item is not found";
                    goto end;
                }

                category.DeleteByUserId = 1;
                category.DeleteDate = DateTime.Now;
                await db.SaveChangesAsync(cancellationToken);
                response.Error = false;
                response.Message = "Item is deleted";
            end:
                return response;
            }
        }
    }
}
