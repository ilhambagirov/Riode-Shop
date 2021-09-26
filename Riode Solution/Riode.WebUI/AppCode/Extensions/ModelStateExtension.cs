using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Extensions
{
    static public partial class Extension
    {

        static public bool IsModelStateValid(this IActionContextAccessor ctx)
        {
            return ctx.ActionContext.ModelState.IsValid;
        }
    }
}
