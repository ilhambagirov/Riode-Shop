using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Riode.WebUI.AppCode.Extensions;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Application.CategoryModule
{
    public class CategoryCreateCommand : IRequest<int>
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }

        public class CategoryCreateCommandHandler : IRequestHandler<CategoryCreateCommand, int>
        {

            readonly RiodeDBContext db;
            readonly IActionContextAccessor ctx;
            public CategoryCreateCommandHandler(RiodeDBContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<int> Handle(CategoryCreateCommand request, CancellationToken cancellationToken)
            {
                if (ctx.IsModelStateValid())
                {
                    var category = new Category();
                    category.Name = request.Name;
                    category.Description = request.Description;
                    category.ParentId = request.ParentId;

                    db.Category.Add(category);
                    await db.SaveChangesAsync(cancellationToken);
                    return category.Id;
                }
                return 0;

            }
        }
    }
}
