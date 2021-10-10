using MediatR;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.Entities;
using Riode.WebUI.Models.ViewModels;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Application.MailBoxModule
{
    public class MailBoxPagedQuery : IRequest<PagedViewModel<Contact>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 6;
        public int TypeId { get; set; }

        public class MailBoxPagedQueryHandler : IRequestHandler<MailBoxPagedQuery, PagedViewModel<Contact>>
        {
            readonly RiodeDBContext db;
            public MailBoxPagedQueryHandler(RiodeDBContext db)
            {
                this.db = db;
            }
            public async Task<PagedViewModel<Contact>> Handle(MailBoxPagedQuery request, CancellationToken cancellationToken)
            {
                var query = db.ContactPosts.Where(cp => cp.DeleteByUserId == null).AsQueryable();
               /* ViewBag.all = query.Count();
                ViewBag.ha = query.Where(cp => cp.AnswerBy != null).Count();
                ViewBag.na = query.Where(cp => cp.AnswerBy == null).Count();
                ViewBag.Marked = query.Where(cp => cp.Marked == true).Count();*/
                switch (request.TypeId)
                {
                    case 1:
                        query = query.Where(cp => cp.AnswerBy != null);
                        break;
                    case 2:
                        query = query.Where(cp => cp.AnswerBy == null);
                        break;
                    case 3:
                        query = query.Where(cp => cp.Marked == true);
                        break;
                    default:
                        break;
                }

                return new PagedViewModel<Contact>(query, request.PageIndex, request.PageSize);
            }
        }
    }
}
