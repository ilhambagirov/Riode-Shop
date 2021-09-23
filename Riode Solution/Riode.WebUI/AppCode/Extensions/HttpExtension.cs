using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Extensions
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
