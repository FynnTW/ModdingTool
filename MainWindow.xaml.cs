using log4net.Config;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
using System.IO;
using static ModdingTool.Globals;

namespace ModdingTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        public MainWindow()
        {

            string logpath = "D:\\source\\repos\\ModdingTool\\bin\\Debug\\net7.0-windows\\proper.log";
            if (File.Exists(logpath))
            {
                File.Delete(logpath);
            }
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            factionParser.parseSMFactions();
            EDUParser.parseEU();
            EDUParser.parseEDU();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string newtext = modPathInput.Text.Trim();
            modPath = newtext;
        }
    }
}
