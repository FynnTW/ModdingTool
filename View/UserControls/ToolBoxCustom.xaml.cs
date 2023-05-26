using System;
using System.Collections.Generic;
using System.Data;
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

namespace ModdingTool.View.UserControls
{
    /// <summary>
    /// Interaction logic for ToolBoxCustom.xaml
    /// </summary>
    public partial class ToolBoxCustom : UserControl
    {
        private DataTab? datatab;
        private TabControl? tabcontroller;

        public ToolBoxCustom()
        {
            InitializeComponent();
            if (Application.Current.MainWindow == null) return;
            var window = (ModdingTool.MainWindow)Application.Current.MainWindow;
            datatab = window?.FindName("DataTabLive") as DataTab;
        }

        private void Undo_Last(object sender, RoutedEventArgs e)
        {
            if (datatab == null)
            {
                if (Application.Current.MainWindow == null) return;
                var window = (ModdingTool.MainWindow)Application.Current.MainWindow;
                datatab = window?.FindName("DataTabLive") as DataTab;
            }
            if (datatab == null) { return; }
            switch (datatab.selectedTab.iLogic.Changes)
            {
                case { Count: 0 }:
                    return;
                case null:
                    return;
            }
            if (datatab.selectedTab.iLogic.Changes.Last() is not TextBox lastChange) return;
            lastChange.Undo();
            datatab.selectedTab.iLogic.Changes.Remove(datatab.selectedTab.iLogic.Changes.Last());
        }

        private void Filter(object sender, RoutedEventArgs e)
        {
            var w = new Window1
            {
                Content = new FilterWindow()
            };
            w.Show();
        }
    }
}
