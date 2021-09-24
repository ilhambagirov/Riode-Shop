using MediatR;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.Entities;
using Riode.WebUI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Application.BrandModule
{
    public class BrandPagedQuery : IRequest<PagedViewModel<Brands>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 3;

        public class BrandPagedQueryHandler : IRequestHandler<BrandPagedQuery, PagedViewModel<Brands>>
        {
            readonly RiodeDBContext db;
            public BrandPagedQueryHandler(RiodeDBContext db)
            {
                this.db = db;
            }
            public async Task<PagedViewModel<Brands>> Handle(BrandPagedQuery request, CancellationToken cancellationToken)
            {
                var query = db.Brands.Where(b => b.DeleteByUserId == null && b.DeleteDate == null)
                     .AsQueryable();

                /*   int recordCount = await query.CountAsync(cancellationToken);

                   var pagedData = await query.Skip((request.PageIndex - 1) * request.PageSize)
                     .Take(request.PageSize).ToListAsync(cancellationToken);*/

                return new PagedViewModel<Brands>(query, request.PageIndex, request.PageSize);
            }
        }
    }
}
