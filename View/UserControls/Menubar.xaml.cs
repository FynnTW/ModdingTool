using System.Windows;
using ModdingTool.ViewModel;
using UserControl = System.Windows.Controls.UserControl;

namespace ModdingTool.View.UserControls
{
    /// <summary>
    /// Interaction logic for Menubar.xaml
    /// </summary>
    public partial class Menubar : UserControl
    {
        public Menubar()
        {
            InitializeComponent();
            DataContext = new MenuBarViewModel();
        }
    }
}
