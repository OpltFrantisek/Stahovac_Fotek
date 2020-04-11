using System;
using System.IO;
using System.Linq;
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
            p_bar.Visibility = Visibility.Hidden;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            source = source_url.Text;
            dest = dest_url.Text;
            p_bar.Visibility = Visibility.Visible;
            p_bar.Foreground = Brushes.Green;
            if (Directory.Exists(dest))
            {
                new Thread(() =>
                {
                    int ok = 0;
                    Thread.CurrentThread.IsBackground = true;
                    if (FileDownloader.DownloadFile(source, dest, "Data.xml"))
                    {
                        var data = ImageFinder.FindInXml(new StreamReader(dest + @"\Data.xml"));
                        if (data.Count > 0)
                        {
                            for (int i = 0; i < data.Count; i++)
                            {
                                if (FileDownloader.DownloadFile(data[i], dest))
                                {
                                    ok++;
                                }                                                     
                                p_bar.Dispatcher.BeginInvoke((Action)(() => p_bar.Value = i / (data.Count / 100.0)));                               
                            }
                            p_bar.Dispatcher.BeginInvoke((Action)(() => p_bar.Foreground = Brushes.GreenYellow));
                            MessageBoxResult result = MessageBox.Show(string.Format("Hotovo úspěšné staženo {0} z {1} nalezených obrázků.",ok,data.Count),
                                        "Jupíííí",
                                        MessageBoxButton.OK);
                        }
                    }
                    else
                    {
                        MessageBoxResult result = MessageBox.Show("Nedaří se stáhnout XML soubor. Zkontroluj cestu",
                                         "Ups",
                                         MessageBoxButton.OK);

                    }
                }).Start();
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Špatně zadana cesta pro ukládání fotek. Pro naformátovnátí C:\\ stistkněte OK",
                                          "Ups",
                                          MessageBoxButton.OK);
            }                
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog(this).GetValueOrDefault())
            {
                 dest_url.Text = dialog.SelectedPath;
            }
        }
    }
}
