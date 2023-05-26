using ModdingTool.View.InterfaceData;
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
using static ModdingTool.Globals;
using static ModdingTool.Unit;
using System.Windows.Shapes;

namespace ModdingTool.View.UserControls
{
    /// <summary>
    /// Interaction logic for ToolBoxCustom.xaml
    /// </summary>
    public partial class ToolBoxCustom : UserControl
    {
        private DataTab? datatab;
        private TabControl? tabcontroller;
        public List<string> sortTypes { get; set; } = new List<string>();
        public List<string> sortDirections { get; set; } = new List<string>() { "Increasing", "Decreasing" };

        public ToolBoxCustom()
        {
            InitializeComponent();
            if (Application.Current.MainWindow == null) return;
            var window = (ModdingTool.MainWindow)Application.Current.MainWindow;
            datatab = window?.FindName("DataTabLive") as DataTab;
        }

        private void Undo_Last(object sender, RoutedEventArgs e)
        {
            if (datatab == null)
            {
                if (Application.Current.MainWindow == null) return;
                var window = (ModdingTool.MainWindow)Application.Current.MainWindow;
                datatab = window?.FindName("DataTabLive") as DataTab;
            }
            if (datatab == null) { return; }
            switch (datatab.selectedTab.iLogic.Changes)
            {
                case { Count: 0 }:
                    return;
                case null:
                    return;
            }
            if (datatab.selectedTab.iLogic.Changes.Last() is not TextBox lastChange) return;
            lastChange.Undo();
            datatab.selectedTab.iLogic.Changes.Remove(datatab.selectedTab.iLogic.Changes.Last());
        }

        private void Filter(object sender, RoutedEventArgs e)
        {
            var w = new Window1
            {
                Content = new FilterWindow()
            };
            w.Show();
        }

        private void SortList(object sender, RoutedEventArgs e)
        {
            var window = (ModdingTool.MainWindow)Application.Current.MainWindow;
            datatab = window?.FindName("DataTabLive") as DataTab;
            dataList = window?.FindName("DataListLive") as DataList;
            if (datatab?.selectedTab is not UnitTab unitTab) return;
            selectunit = unitTab;
            foreach (var prop in unitTab.SelectedUnit.GetType().GetProperties())
            {
                sortTypes.Add(UnitTab.UnitUiText[prop.Name]);
            }
            popup.IsOpen = true;
            sortDirection.ItemsSource = sortDirections;
            sortType.ItemsSource = sortTypes;
            sortDirection.Items.Refresh();
            sortType.Items.Refresh();
            sortDirection.SelectedIndex = 0;
            sortType.SelectedIndex = 0;
        }

        private DataList dataList;
        private UnitTab selectunit;

        private void SortAccept_OnClick(object sender, RoutedEventArgs e)
        {
            if (dataList.UnitList.Count == 0)
            {
                dataList.UnitList = AllUnits.Keys.ToList();
            }
            var attribute = "";
            foreach (var attr in UnitTab.UnitUiText.Where(attr => attr.Value == sortType.Text))
            {
                attribute = attr.Key;
            }
            switch (sortDirection.Text)
            {
                case "Increasing":
                    dataList?.UnitList.Sort((x, y) =>
                    {
                        var propX = AllUnits[x].GetType().GetProperty(attribute)?.GetValue(AllUnits[x]);
                        var propY = AllUnits[y].GetType().GetProperty(attribute)?.GetValue(AllUnits[y]);

                        switch (propX)
                        {
                            case null when propY == null:
                                return 0;
                            case null:
                                return -1;
                            default:
                                {
                                    if (propY == null)
                                        return 1;
                                    break;
                                }
                        }

                        return Comparer<object>.Default.Compare(propX, propY);
                    });
                    break;
                case "Decreasing":
                    dataList?.UnitList.Sort((x, y) =>
                    {
                        var propX = AllUnits[x].GetType().GetProperty(attribute)?.GetValue(AllUnits[x]);
                        var propY = AllUnits[y].GetType().GetProperty(attribute)?.GetValue(AllUnits[y]);
                        switch (propX)
                        {
                            case null when propY == null:
                                return 0;
                            case null:
                                return -1;
                            default:
                                {
                                    if (propY == null)
                                        return 1;
                                    break;
                                }
                        }
                        return Comparer<object>.Default.Compare(propY, propX);
                    });
                    break;
            }

            if (dataList != null)
            {
                dataList.DataListPicker.ItemsSource = dataList?.UnitList;
                dataList?.DataListPicker.Items.Refresh();
            }

            popup.IsOpen = false;
        }

        private void SortCancel_OnClick(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = false;
        }
    }
}
