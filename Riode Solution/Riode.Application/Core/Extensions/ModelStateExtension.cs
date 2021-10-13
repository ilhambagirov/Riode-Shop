using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Riode.Application.Core.Extensions
{
    static public partial class Extension
    {

        static public bool IsModelStateValid(this IActionContextAccessor ctx)
        {
            return ctx.ActionContext.ModelState.IsValid;
        }
    }
}
