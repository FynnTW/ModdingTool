using ModdingTool.View.InterfaceData;
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
            if (sender is not TextBox { DataContext: ProjectileTab tab } box) return;
            if (tab.iLogic.Changes == null) return;
            if (tab.iLogic.Changes.Contains(box)) return;
            tab.iLogic.Changes?.Add(box);
        }

        private void Models_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
        }
    }
}
