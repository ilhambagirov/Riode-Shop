using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Riode.Application.Core.Infrastructure;
using Riode.Domain.Models.DataContext;

namespace Riode.Application.BrandModule
{
    public class BrandDeleteCommand : IRequest<CommandJsonResponse>
    {
        public int? Id { get; set; }
        public class BrandDeleteCommandHandler : IRequestHandler<BrandDeleteCommand, CommandJsonResponse>
        {
            readonly RiodeDBContext db;
            public BrandDeleteCommandHandler(RiodeDBContext db)
            {
                this.db = db;
            }

            public async Task<CommandJsonResponse> Handle(BrandDeleteCommand request, CancellationToken cancellationToken)
            {

                var response = new CommandJsonResponse();
                if (request.Id == null || request.Id < 0)
                {
                    response.Error = true;
                    response.Message = "Item is not defined";
                    goto end;
                }

                var entity = await db.Brands.FirstOrDefaultAsync(b => b.Id == request.Id && b.DeleteByUserId == null);

                if (entity == null)
                {
                    response.Error = true;
                    response.Message = "Item is not found";
                    goto end;
                }

                entity.DeleteByUserId = 1;
                entity.DeleteDate = DateTime.Now;
                await db.SaveChangesAsync(cancellationToken);
                response.Error = false;
                response.Message = "Item is deleted";
            end:
                return response;
            }
        }
    }
}
