using Riode.WebUI.Models.Entities.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Models.ViewModels
{
    public class UserRoleViewModule
    {
        public List<RiodeUser> Users { get; set; }
        public List<RiodeUserRole> UserRoles { get; set; }
    }
}
