using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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

namespace PerformanceWidget
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PerformanceCounter cpu;
        private static TextBlock _dU;
        Thread diskscanner;
        public MainWindow()
        {
            InitializeComponent();
            diskscanner = new Thread(scanDisk);
            _dU = DiskUsage;
            Console.WriteLine("Estoy aqui");
            _setWindowPosition(20, 70);
            diskscanner.Start();
        }

        private static void scanDisk()
        {
            while (_dU != null)
            {
                _DiskUsage();
                Thread.Sleep(5000);
            }
        }

        private static void _DiskUsage()
        {
            var drive = DriveInfo.GetDrives()[0];
            double ratio = drive.TotalSize / 100;
            double occupied = drive.AvailableFreeSpace / ratio;
            if (_dU != null)
            {
                _dU.Dispatcher.BeginInvoke((Action)(() =>
                {
                    _dU.Text = String.Format("{0:0.00}", occupied) + "%";
                }));
            }
            
        }

        private void _setWindowPosition(double marginR = 0, double marginB = 0)
        {
            var ScreenH = System.Windows.SystemParameters.PrimaryScreenHeight;
            var ScreenW = System.Windows.SystemParameters.PrimaryScreenWidth;
            var width = this.Width;
            var height = this.Height;
            var newX = ScreenW - marginR - width;
            var newY = ScreenH - marginB - height;
            this.Left = newX;
            this.Top = newY;
        }
    }
}
