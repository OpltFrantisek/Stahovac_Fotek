using System.Net;
namespace Stahovac_Fotek
{
    public class FileDownloader
    {
        public static bool DownloadFile(string source_url, string dest_url, string name)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFile("http://" + source_url, dest_url + @"\" + name);
                }
                catch
                {
                    return false;
                }
                
            }
            return true;
        }
        public static bool DownloadFile(string source_url, string dest_url)
        {
            string name = System.IO.Path.GetFileName(source_url);
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFile(source_url, dest_url + @"\" + name);
                }
                catch
                {
                    return false;
                }              
            }
            return true;
        }
    }
}
