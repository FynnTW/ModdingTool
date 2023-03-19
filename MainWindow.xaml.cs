using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
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
            const string logpath = "D:\\proper.log";
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
            EduParser.ParseEu();
            EduParser.ParseEdu();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var newtext = ModPathInput.Text.Trim();
            ModPath = newtext;
        }

        private void openViewer_Click(object sender, RoutedEventArgs e)
        {
            var view = new UnitViewer();
            view.Show();
        }
    }
}