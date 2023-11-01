using ModdingTool.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace ModdingTool.View.UserControls
{
    /// <summary>
    /// Interaction logic for DataList.xaml
    /// </summary>
    public partial class DataList : UserControl
    {

        public DataListViewModel ViewModel;

        public DataList()
        {
            InitializeComponent();
            ViewModel = new DataListViewModel();
            DataContext = ViewModel;
        }

        private void DataListPicker_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel.OpenNewTab();
        }
    }

}
