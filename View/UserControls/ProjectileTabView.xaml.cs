using System.Windows;
using System.Windows.Controls;

namespace ModdingTool.View.UserControls
{
    /// <summary>
    /// Interaction logic for MountTabView.xaml
    /// </summary>
    public partial class ProjectileTabView : UserControl
    {
        private DataTab? datatab;
        private TabControl? tabcontroller;
        public ProjectileTabView()
        {
            InitializeComponent();
            if (Application.Current.MainWindow != null)
            {
                var window = (ModdingTool.MainWindow)Application.Current.MainWindow;
                datatab = window?.FindName("DataTabLive") as DataTab;
            }

            tabcontroller = datatab?.FindName("AllTabs") as TabControl;
        }

        private void Field_OnTextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Models_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
        }
    }
}
