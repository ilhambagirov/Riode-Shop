using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Riode.WebUI.AppCode.Extensions;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Application.ProductColorModule
{
    public class ProductColorCreateCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string HexCode { get; set; }

        public class ProductColorCreateCommandHandler : IRequestHandler<ProductColorCreateCommand, int>
        {

            readonly RiodeDBContext db;
            readonly IActionContextAccessor ctx;
            public ProductColorCreateCommandHandler(RiodeDBContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<int> Handle(ProductColorCreateCommand request, CancellationToken cancellationToken)
            {
                if (ctx.IsModelStateValid())
                {
                    var color = new Colors();
                    color.Name = request.Name;
                    color.Description = request.Description;
                    color.HexCode = request.HexCode;
                    db.Colors.Add(color);
                    await db.SaveChangesAsync(cancellationToken);
                    return color.Id;
                }
                return 0;

            }
        }
    }
}
