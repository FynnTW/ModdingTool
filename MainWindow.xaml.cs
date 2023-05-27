using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using static ModdingTool.Globals;
using Microsoft.WindowsAPICodePack.Dialogs;
using ModdingTool.View.UserControls;

namespace ModdingTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}