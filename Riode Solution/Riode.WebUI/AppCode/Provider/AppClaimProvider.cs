using Microsoft.AspNetCore.Authentication;
using Riode.WebUI.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Provider
{
    public class AppClaimProvider : IClaimsTransformation
    {
        readonly RiodeDBContext db;
        public AppClaimProvider(RiodeDBContext db)
        {
            this.db = db;
        }
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {

            if (principal.Identity.IsAuthenticated && principal.Identity is ClaimsIdentity cIdentity)
            {
                while (cIdentity.Claims.Any(c => !c.Type.StartsWith("http") && !c.Type.StartsWith("Asp")))
                {

                    var claim = cIdentity.Claims.First(c => !c.Type.StartsWith("http") && !c.Type.StartsWith("Asp"));

                    cIdentity.RemoveClaim(claim);
                }

                var userid = Convert.ToInt32(cIdentity.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value);

                var caimNames = new List<string>();

                var claims = db.UserClaims.Where(c => c.UserId == userid && c.ClaimValue.Equals("1")).Select(c => c.ClaimType).ToArray();
                caimNames.AddRange(claims);

                string[] roleclaims = (
                from ur in db.UserRoles
                join rc in db.RoleClaims on ur.RoleId equals rc.RoleId
                where ur.UserId == userid && rc.ClaimValue.Equals("1")
                select rc.ClaimType).ToArray();

                caimNames.AddRange(roleclaims);

                foreach (var item in caimNames)
                {
                    cIdentity.AddClaim(new Claim(item, "1"));
                }


            }
            return principal;
        }
    }
}
