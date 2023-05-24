using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using static ModdingTool.Globals;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ModdingTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            const string logpath = "D:\\proper.log";
            if (File.Exists(logpath))
            {
                File.Delete(logpath);
            }
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            InitializeComponent();
        }
    }
}