using MediatR;
using Microsoft.EntityFrameworkCore;
using Riode.Domain.Models.DataContext;
using Riode.Domain.Models.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.Application.MailBoxModule
{
    public class MailBoxSingleQuery : IRequest<Contact>
    {

        public int Id { get; set; }

        public class MailBoxSingleQueryHandler : IRequestHandler<MailBoxSingleQuery, Contact>
        {
            readonly RiodeDBContext db;
            public MailBoxSingleQueryHandler(RiodeDBContext db)
            {
                this.db = db;
            }
            public async Task<Contact> Handle(MailBoxSingleQuery request, CancellationToken cancellationToken)
            {
                if (request.Id <= 0)
                {
                    return null;
                }

                var  mail = await db.ContactPosts
               .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

                return mail;
            }
        }

    }
}

