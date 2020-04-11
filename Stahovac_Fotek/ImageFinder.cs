using System.Collections.Generic;
using System.IO;
namespace Stahovac_Fotek
{
    public class ImageFinder
    {
        public static List<string> FindInXml(StreamReader sr)
        {
            var res = new List<string>();
            var line = sr.ReadLine();
            while (line != null)
            {
                //write the lie to console window
                if (line.Contains("IMGURL"))
                {
                    line = line.Replace("<IMGURL>", "");
                    line = line.Replace("</IMGURL>", "");
                    line = line.Replace("<IMGURL_ALTERNATIVE>", "");
                    line = line.Replace("</IMGURL_ALTERNATIVE>", "");
                    line = line.Split('?')[0];
                    res.Add(line);
                }
                //Read the next line
                line = sr.ReadLine();
            }
            sr.Dispose();
            return res;
        }
    }
}
