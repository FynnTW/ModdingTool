﻿using ModdingTool.View.InterfaceData;
using ModdingTool.ViewModel;
using Sdl.MultiSelectComboBox.EventArgs;
using Sdl.MultiSelectComboBox.Themes.Generic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

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
            attribute = textblock.Name switch
            {
                "Officer1a" => "Officer1",
                "Officer2a" => "Officer2",
                "Officer3a" => "Officer3",
                "Soldiera" => "Soldier",
                "ArmourModelOnea" => "ArmourModelOne",
                "ArmourModelBasea" => "ArmourModelBase",
                "ArmourModelTwoa" => "ArmourModelTwo",
                "ArmourModelThreea" => "ArmourModelThree",
                _ => attribute
            };
            if (attribute == null) return;
            var unit = menu?.DataContext as UnitTab;
            var model = (string) unit?.SelectedUnit.GetType().GetProperty(attribute)?.GetValue(unit.SelectedUnit, null)!;
            if (string.IsNullOrWhiteSpace(model)) return;
            var newTab = new ModelDbTab(model);
            var dataViewModel = datatab?.DataContext as DataTabViewModel;
            dataViewModel?.AddTab(newTab);
        }

        private void SoldierGotoHyper_OnClick(object sender, RoutedEventArgs e)
        {
            var textblock = (sender as Hyperlink)?.Parent as TextBlock;
            if (textblock?.Text == null) return;
            var attribute = "";
            attribute = textblock.Name switch
            {
                "Officer1a" => "Officer1",
                "Officer2a" => "Officer2",
                "Officer3a" => "Officer3",
                "Soldiera" => "Soldier",
                "ArmourModelOnea" => "ArmourModelOne",
                "ArmourModelBasea" => "ArmourModelBase",
                "ArmourModelTwoa" => "ArmourModelTwo",
                "ArmourModelThreea" => "ArmourModelThree",
                _ => attribute
            };
            if (attribute == null) return;
            var unit = textblock?.DataContext as UnitTab;
            var model = (string) unit?.SelectedUnit.GetType().GetProperty(attribute)?.GetValue(unit.SelectedUnit, null)!;
            if (string.IsNullOrWhiteSpace(model)) return;
            var newTab = new ModelDbTab(model);
            var dataViewModel = datatab?.DataContext as DataTabViewModel;
            dataViewModel?.AddTab(newTab);
        }

        private void Field_OnTextChanged(object sender, TextChangedEventArgs e)
        {
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

        private void Special_formation_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Special_formation.SelectedItem == null) return;
            if (Formation_style.SelectedItem == null) return;
            if ((sender as ComboBox)?.DataContext is not UnitTab utab) return;

            utab.SpecialFormationStylesX = UnitTab.SpecialFormationStyles;
            utab.FormationStylesX = UnitTab.FormationStyles;

            if (UnitTab.SpecialFormationStyles.Contains(Formation_style.SelectedItem))
            {
                utab.SpecialFormationStylesX = UnitTab.FormationStyles;
                if (Formation_style.SelectedItem != null && utab.SpecialFormationStylesX.Contains(Formation_style.SelectedItem))
                    utab.SpecialFormationStylesX.Remove((string)Formation_style.SelectedItem);
            }
            else
            {
                utab.SpecialFormationStylesX = UnitTab.SpecialFormationStyles;
            }

            if (UnitTab.FormationStyles.Contains(Special_formation.SelectedItem))
            {
                if (Special_formation.SelectedItem != null)
                    utab.FormationStylesX.Remove((string)Special_formation.SelectedItem);
            }
            else
            {
                utab.FormationStylesX = UnitTab.FormationStyles;
            }

            Formation_style.ItemsSource = utab.FormationStylesX;
            Special_formation.ItemsSource = utab.SpecialFormationStylesX;
            Formation_style.Items.Refresh();
            Special_formation.Items.Refresh();
        }

        private void MountGoto_OnClick(object sender, RoutedEventArgs e)
        {
            var unit = (sender as Hyperlink)?.DataContext as UnitTab;
            var model = unit?.SelectedUnit.Mount;
            if (string.IsNullOrWhiteSpace(model)) return;
            var newTab = new MountTab(model);
            var dataViewModel = datatab?.DataContext as DataTabViewModel;
            dataViewModel?.AddTab(newTab);
        }

        private void ProjectileGoto_OnClick(object sender, RoutedEventArgs e)
        {
            var unit = (sender as Hyperlink)?.DataContext as UnitTab;
            var model = unit?.SelectedUnit.PriProjectile;
            if (string.IsNullOrWhiteSpace(model)) return;
            if (model.Trim().Equals("no")) return;
            var newTab = new ProjectileTab(model);
            var dataViewModel = datatab?.DataContext as DataTabViewModel;
            dataViewModel?.AddTab(newTab);
        }

        private void Projectile2Goto_OnClick(object sender, RoutedEventArgs e)
        {
            var unit = (sender as Hyperlink)?.DataContext as UnitTab;
            var model = unit?.SelectedUnit.SecProjectile;
            if (string.IsNullOrWhiteSpace(model)) return;
            if (model.Trim().Equals("no")) return;
            var newTab = new ProjectileTab(model);
            var dataViewModel = datatab?.DataContext as DataTabViewModel;
            dataViewModel?.AddTab(newTab);
        }

        private void Projectile3Goto_OnClick(object sender, RoutedEventArgs e)
        {
            var unit = (sender as Hyperlink)?.DataContext as UnitTab;
            var model = unit?.SelectedUnit.TerProjectile;
            if (string.IsNullOrWhiteSpace(model)) return;
            if (model.Trim().Equals("no")) return;
            var newTab = new ProjectileTab(model);
            var dataViewModel = datatab?.DataContext as DataTabViewModel;
            dataViewModel?.AddTab(newTab);
        }
    }
}
