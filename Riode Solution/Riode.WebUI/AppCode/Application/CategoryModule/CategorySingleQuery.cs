﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Application.CategoryModule
{
    public class CategorySingleQuery : IRequest<Category>
    {
        public int? Id { get; set; }

        public class CategorySingleQueryHandler : IRequestHandler<CategorySingleQuery, Category>
        {
            readonly RiodeDBContext db;
            public CategorySingleQueryHandler(RiodeDBContext db)
            {
                this.db = db;
            }
            public async Task<Category> Handle(CategorySingleQuery request, CancellationToken cancellationToken)
            {
                if (request.Id == null)
                {
                    return null;
                }

                var category = await db.Category
                    .Include(c => c.Parent)
                    .FirstOrDefaultAsync(m => m.Id == request.Id);

                return category;

            }
        }
    }
}
