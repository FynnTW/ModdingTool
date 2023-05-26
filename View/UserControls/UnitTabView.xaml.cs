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
        private DataTab? datatab;
        private TabControl? tabcontroller;

        public UnitTabView()
        {
            InitializeComponent();
            if (Application.Current.MainWindow != null)
            {
                var window = (ModdingTool.MainWindow)Application.Current.MainWindow;
                datatab = window?.FindName("DataTabLive") as DataTab;
            }

            tabcontroller = datatab?.FindName("AllTabs") as TabControl;
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
            if (tabcontroller == null) return;
            tabcontroller.Visibility = Visibility.Hidden;
            tabcontroller.Visibility = Visibility.Visible;
        }

        private void CancelAttr_Click(object sender, RoutedEventArgs e)
        {
            txtBox.Text = "";
            popup.IsOpen = false;
        }

        private void SoldierGoto_OnClick(object sender, RoutedEventArgs e)
        {
            var soldiertext = Soldier.Text;
            var newTab = new ModelDbTab(soldiertext);
            datatab?.AddTab(newTab);
        }

        private void Field_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox { DataContext: UnitTab tab } box) return;
            if (tab.iLogic.Changes == null) return;
            if (tab.iLogic.Changes.Contains(box)) return;
            tab.iLogic.Changes?.Add(box);
        }
    }
}
