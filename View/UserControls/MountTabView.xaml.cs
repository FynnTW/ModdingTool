﻿using ModdingTool.View.InterfaceData;
using System.Windows;
using System.Windows.Controls;

namespace ModdingTool.View.UserControls
{
    /// <summary>
    /// Interaction logic for MountTabView.xaml
    /// </summary>
    public partial class MountTabView : UserControl
    {
        private DataTab? datatab;
        private TabControl? tabcontroller;
        public MountTabView()
        {
            InitializeComponent();
            if (Application.Current.MainWindow != null)
            {
                var window = (ModdingTool.MainWindow)Application.Current.MainWindow;
                datatab = window?.FindName("DataTabLive") as DataTab;
            }

            tabcontroller = datatab?.FindName("AllTabs") as TabControl;
        }

        private void Field_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox { DataContext: MountTab tab } box) return;
            if (tab.iLogic.Changes == null) return;
            if (tab.iLogic.Changes.Contains(box)) return;
            tab.iLogic.Changes?.Add(box);
        }

        private void ModelGoto_OnClick(object sender, RoutedEventArgs e)
        {
            var menu = (sender as MenuItem)?.Parent as ContextMenu;
            var textblock = menu?.PlacementTarget as TextBlock;
            if (textblock?.Text == null) return;
            var attribute = "model";
            var mount = menu?.DataContext as MountTab;
            var model = mount?.SelectedMount.GetType().GetProperty(attribute)?.GetValue(mount.SelectedMount, null);
            if (model?.ToString() == null) return;
            var newTab = new ModelDbTab(model.ToString());
            datatab?.AddTab(newTab);
        }

        private void RiderOffsetGrid_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
        }
    }
}