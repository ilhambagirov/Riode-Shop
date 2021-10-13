using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Riode.Domain.Models.Entities.Membership
{
    public class RiodeUser : IdentityUser<int>
    {
        public bool Banned { get; set; }
    }
}
