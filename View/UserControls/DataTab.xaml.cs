using ModdingTool.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace ModdingTool.View.UserControls
{
    /// <summary>
    /// Interaction logic for DataTab.xaml
    /// </summary>
    public partial class DataTab : UserControl
    {
        public DataTabViewModel ViewModel;
        public DataTab()
        {
            InitializeComponent();
            var window = (MainWindow)Application.Current?.MainWindow!;
            if (window?.FindName("DataListLive") is DataList dataList)
                DataContext = ViewModel = new DataTabViewModel(dataList);
            else
                throw new System.Exception("DataListLive not found");
        }
    }
}
