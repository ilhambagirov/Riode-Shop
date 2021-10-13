using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace Riode.Application.Core.Extensions
{
    static public partial class Extension
    {
        static public string GetCurrentCulture(this HttpContext context)
        {
            var match = Regex.Match(context.Request.Path, @"\/(?<lang>az|en|ru)\/?.*");
            if (match.Success)
            {
                return match.Groups["lang"].Value;
            }
            if (context.Request.Cookies.TryGetValue("lang", out string lang))
            {
                return lang;
            }
            return "en";
        }


    }
}
