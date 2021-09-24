using MediatR;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Application.BrandModule
{
    public class BrandCreateCommand : IRequest<int>
    {

        public string Name { get; set; }
        public string Description { get; set; }

        public class BrandCreateCommandHandler : IRequestHandler<BrandCreateCommand, int>
        {

            readonly RiodeDBContext db;
            public BrandCreateCommandHandler(RiodeDBContext db)
            {
                this.db = db;
            }
            public async Task<int> Handle(BrandCreateCommand request, CancellationToken cancellationToken)
            {

                var brand = new Brands();
                brand.Name = request.Name;
                brand.Description = request.Description;

                db.Brands.Add(brand);
                await db.SaveChangesAsync(cancellationToken);
                return brand.Id;
            }
        }
    }
}
