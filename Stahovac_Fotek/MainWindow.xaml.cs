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
        static bool deleteXml = false;
        static event EventHandler Progress;
        public delegate void UpdateProgress(double progress);
        public MainWindow()
        {
            InitializeComponent();
            p_bar.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            source = source_url.Text;
            dest = dest_url.Text;
            p_bar.Visibility = Visibility.Visible;
            p_bar.Foreground = Brushes.Green;
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                if (FileDownloader.DownloadFile(source, dest, "Data.xml"))
                {
                    var data = ImageFinder.FindInXml(new StreamReader(dest + @"\Data.xml"));
                    if (data.Count > 0)
                    {
                        for (int i = 0; i < data.Count; i++)
                        {
                            FileDownloader.DownloadFile(data[i], dest);
                            p_bar.Dispatcher.BeginInvoke((Action)(() => p_bar.Value = i / (data.Count / 100.0)));

                        }
                        p_bar.Dispatcher.BeginInvoke((Action)(() => p_bar.Foreground = Brushes.GreenYellow ));
                    }
                }
            }).Start();


        }
        public void UpdateInfo(double progress)
        {
            p_bar.Value = progress;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
           // deleteXml = (bool)check_box.IsChecked;
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
