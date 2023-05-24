using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using CommunityToolkit.Mvvm.Input;
using ModdingTool.View.InterfaceData;

namespace ModdingTool.View.UserControls
{
    /// <summary>
    /// Interaction logic for DataTab.xaml
    /// </summary>
    public partial class DataTab : UserControl
    {
        public ICollection<ITab> Tabs { get; }
        public ICommand NewTabCommand;

        public DataTab()
        {
            NewTabCommand = new RelayCommand(NewTab);
            Tabs = new ObservableCollection<ITab>();
            InitializeComponent();
        }


        private void NewTab()
        {

        }

        public void AddTab(ITab tab)
        {
            Tabs.Add(tab);
            AllTabs.ItemsSource = Tabs;
        }
    }
}
