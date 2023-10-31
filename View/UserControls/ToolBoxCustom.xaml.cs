using ModdingTool.ViewModel;
using System.Windows.Controls;

namespace ModdingTool.View.UserControls
{
    /// <summary>
    /// Interaction logic for ToolBoxCustom.xaml
    /// </summary>
    public partial class ToolBoxCustom : UserControl
    {
        public ToolBoxCustom()
        {
            InitializeComponent();
            DataContext = new ToolBoxViewModel();
        }
    }
}
