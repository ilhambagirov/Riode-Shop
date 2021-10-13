using System.IO;

namespace Riode.Application.Core.Extensions
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
