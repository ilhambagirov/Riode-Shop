using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Riode.WebUI.AppCode.Extensions;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Riode.WebUI.AppCode.Application.MailBoxModule
{
    public class MailBoxMarkCommand : IRequest<Contact>
    {

        public int? Id { get; set; }
        public string Answer { get; set; }


        public class MailBoxMarkCommandHandler : IRequestHandler<MailBoxMarkCommand, Contact>
        {
            readonly RiodeDBContext db;
            readonly IActionContextAccessor ctx;
            public MailBoxMarkCommandHandler(RiodeDBContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<Contact> Handle(MailBoxMarkCommand request, CancellationToken cancellationToken)
            {
                if (request.Id == null && request.Id <= 0)
                {
                    return null;
                }
                if (ctx.IsModelStateValid())
                {
                    var contact = await db.ContactPosts
                                  .FirstOrDefaultAsync(m => m.Id == request.Id && m.DeleteByUserId == null);
                    if (contact == null)
                    {
                        return null;
                    }
                    if (contact.Marked == true)
                    {
                        contact.Marked = false;
                    }
                    else
                    {
                        contact.Marked = true;
                    }
                    await db.SaveChangesAsync(cancellationToken);
                    return contact;
                }
                return null;
            }
        }

    }
}
