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
using ModdingTool.View.InterfaceData;
using Sdl.MultiSelectComboBox.Themes.Generic;

namespace ModdingTool.View.UserControls
{
    /// <summary>
    /// Interaction logic for UnitTabView.xaml
    /// </summary>
    public partial class UnitTabView : UserControl
    {
        public UnitTabView()
        {
            InitializeComponent();
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = true;
        }

        private void acceptAttr_Click(object sender, RoutedEventArgs e)
        {
            UnitTab.AttributeTypes.Add(txtBox.Text);
            popup.IsOpen = false;
            Attributes.ItemsSource = UnitTab.AttributeTypes;
            Attributes.SelectedItems.Add(txtBox.Text);
            FocusManager.SetFocusedElement(this, Pri_armour);
            txtBox.Text = "";
            MainWindow window = (ModdingTool.MainWindow)App.Current.MainWindow;
            var datatab = window?.FindName("DataTabLive") as DataTab;
            var tabcontroller = datatab?.FindName("AllTabs") as TabControl;
            tabcontroller.Visibility = Visibility.Hidden;
            tabcontroller.Visibility = Visibility.Visible;
        }

        private void CancelAttr_Click(object sender, RoutedEventArgs e)
        {
            txtBox.Text = "";
            popup.IsOpen = false;
        }
    }
}
