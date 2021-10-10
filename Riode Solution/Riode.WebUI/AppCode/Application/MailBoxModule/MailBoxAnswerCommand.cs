using MediatR;
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
   
    public class MailBoxAnswerCommand : IRequest<Contact>
    {

        public int Id { get; set; }
        public string Answer { get; set; }


        public class MailBoxAnswerCommandHandler : IRequestHandler<MailBoxAnswerCommand, Contact>
        {
            readonly RiodeDBContext db;
            readonly IConfiguration configuration;
            public MailBoxAnswerCommandHandler(RiodeDBContext db, IConfiguration configuration)
            {
                this.db = db;
                this.configuration = configuration;
            }
            public async Task<Contact> Handle(MailBoxAnswerCommand request, CancellationToken cancellationToken)
            {
                if (request.Id <= 0)
                {
                    return null;
                }

                var contact = await db.ContactPosts
                 .FirstOrDefaultAsync(m => m.Id == request.Id && m.DeleteByUserId == null && m.AnswerDate == null);
                if (contact == null)
                {
                    return null;
                }

                contact.Answer = request.Answer;
                contact.AnswerBy = 1;
                contact.AnswerDate = DateTime.Now;
                await db.SaveChangesAsync();

                var content = "ContactAnswerTemplate.html".GetStaticFileContent();

                var mailSent = configuration.SendEmail(contact.Email, "Riode Answer", content.Replace("##answer##", contact.Answer));

                return contact;
                
            }
        }

    }
}
