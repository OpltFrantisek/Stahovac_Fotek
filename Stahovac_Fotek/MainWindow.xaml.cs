using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
namespace Stahovac_Fotek
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string source;
        static string dest;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            source = source_url.Text;
            dest = dest_url.Text;
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                if (FileDownloader.DownloadFile(source, dest, "Data.xml"))
                {
                    var data = ImageFinder.FindInXml(new StreamReader(dest + @"\Data.xml"));
                    if (data.Count > 0)
                    {
                        foreach (var img in data)
                            FileDownloader.DownloadFile(img, dest);
                    }
                }             
            }).Start();
            

        }
    }

    public class FileDownloader
    {
        public static bool DownloadFile(string source_url, string dest_url, string name)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile("http://" + source_url, dest_url + @"\" + name);
            }
            return true;

        }
        public static bool DownloadFile(string source_url, string dest_url)
        {
            string name = System.IO.Path.GetFileName(source_url);
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(source_url, dest_url + @"\" + name);
            }
            return true;
        }
    }
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
