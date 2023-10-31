using ModdingTool.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ModdingTool.View.UserControls
{
    /// <summary>
    /// Interaction logic for FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : UserControl
    {
        public FilterWindow(ObservableCollection<string> itemList, string selectedType)
        {
            InitializeComponent();
            DataContext = new FilterViewModel(itemList, selectedType);
        }
    }
}
