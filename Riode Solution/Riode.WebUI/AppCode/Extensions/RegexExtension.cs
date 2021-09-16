using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Extensions
{
    static public partial class Extension
    {

        static public string PlainText(this string text)
        {
            return Regex.Replace(text, @"<[^>]*>", "");
        }
    }
}
