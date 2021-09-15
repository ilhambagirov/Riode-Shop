using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Extensions
{
    static public partial class Extension
    {

        static public string GetStaticFileContent(this string filePath)
        {
            using (var stream = new StreamReader(Path.Combine(
                "wwwroot",
                "static",
                filePath)))
            {
                return stream.ReadToEnd();
            }
        }
    }
}
