﻿using System;
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
using Sdl.MultiSelectComboBox.EventArgs;
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
            var menu = (sender as MenuItem)?.Parent as ContextMenu;
            var textblock = menu?.PlacementTarget as TextBlock;
            if (textblock?.Text == null) return;
            var attribute = "";
            if (textblock.Text.Contains("Officer"))
            {
                attribute = textblock.Name switch
                {
                    "Officer1a" => "Officer1",
                    "Officer2a" => "Officer2",
                    "Officer3a" => "Officer3",
                    _ => attribute
                };
            }
            else
            {
                foreach (var attr in UnitTab.UnitUiText.Where(attr => attr.Value == textblock?.Text))
                {
                    attribute = attr.Key;
                }
            }
            if (attribute == null) return;
            var unit = menu?.DataContext as UnitTab;
            var model = unit?.SelectedUnit.GetType().GetProperty(attribute)?.GetValue(unit.SelectedUnit, null);
            if (model?.ToString() == null) return;
            var newTab = new ModelDbTab(model.ToString());
            datatab?.AddTab(newTab);
        }

        private void Field_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox { DataContext: UnitTab tab } box) return;
            if (tab.iLogic.Changes == null) return;
            if (tab.iLogic.Changes.Contains(box)) return;
            tab.iLogic.Changes?.Add(box);
        }

        private void Ownership_OnSelectedItemsChanged(object? sender, SelectedItemsChangedEventArgs e)
        {
            if (Ownership.SelectedItems.Cast<object?>().Where(item => item != null).All(item => item?.ToString() != "all")) return;
            if ((sender as MultiSelectComboBox)?.DataContext is not UnitTab utab) return;
            utab.SelectedUnit.Ownership = new List<string> { "all" };
            Ownership.SelectedItems = utab.SelectedUnit.Ownership;
            Ownership.Visibility = Visibility.Hidden;
            Ownership.Visibility = Visibility.Visible;
        }

        private void EraZero_OnSelectedItemsChanged(object? sender, SelectedItemsChangedEventArgs e)
        {
            if (EraZero.SelectedItems.Cast<object?>().Where(item => item != null).All(item => item?.ToString() != "all")) return;
            if ((sender as MultiSelectComboBox)?.DataContext is not UnitTab utab) return;
            utab.SelectedUnit.EraZero = new List<string> { "all" };
            EraZero.SelectedItems = utab.SelectedUnit.EraZero;
            EraZero.Visibility = Visibility.Hidden;
            EraZero.Visibility = Visibility.Visible;
        }

        private void EraOne_OnSelectedItemsChanged(object? sender, SelectedItemsChangedEventArgs e)
        {
            if (EraOne.SelectedItems.Cast<object?>().Where(item => item != null).All(item => item?.ToString() != "all")) return;
            if ((sender as MultiSelectComboBox)?.DataContext is not UnitTab utab) return;
            utab.SelectedUnit.EraOne = new List<string> { "all" };
            EraOne.SelectedItems = utab.SelectedUnit.EraOne;
            EraOne.Visibility = Visibility.Hidden;
            EraOne.Visibility = Visibility.Visible;
        }

        private void EraTwo_OnSelectedItemsChanged(object? sender, SelectedItemsChangedEventArgs e)
        {
            if (EraTwo.SelectedItems.Cast<object?>().Where(item => item != null).All(item => item?.ToString() != "all")) return;
            if ((sender as MultiSelectComboBox)?.DataContext is not UnitTab utab) return;
            utab.SelectedUnit.EraTwo = new List<string> { "all" };
            EraTwo.SelectedItems = utab.SelectedUnit.EraTwo;
            EraTwo.Visibility = Visibility.Hidden;
            EraTwo.Visibility = Visibility.Visible;
        }
    }
}
