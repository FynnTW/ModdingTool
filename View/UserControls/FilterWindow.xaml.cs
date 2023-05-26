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
using static ModdingTool.Globals;
using ModdingTool.View.InterfaceData;

namespace ModdingTool.View.UserControls
{
    /// <summary>
    /// Interaction logic for FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : UserControl
    {
        public static List<Filter> localFilterList { get; set; } = new List<Filter>();
        public static List<string> AttributeList { get; set; } = new List<string>();

        public static List<string> ConditionList { get; set; } = new List<string>
        {
            "Equals",
            "Not Equals",
            "Greater Than",
            "Less Than",
            "Greater Than Or Equals",
            "Less Than Or Equals",
            "Contains",
            "Not Contains"
        };

        private DataList dataList;
        private DataTab datatab;
        private UnitTab selectunit;

        public FilterWindow()
        {
            InitializeComponent();
            if (Application.Current.MainWindow == null) return;
            var window = (ModdingTool.MainWindow)Application.Current.MainWindow;
            datatab = window?.FindName("DataTabLive") as DataTab;
            dataList = window?.FindName("DataListLive") as DataList;
            if (datatab?.selectedTab is not UnitTab unitTab) return;
            selectunit = unitTab;
            foreach (var prop in unitTab.SelectedUnit.GetType().GetProperties())
            {
                AttributeList.Add(prop.Name);
            }
            ChooseAttrBox.ItemsSource = AttributeList;
            ChooseCondBox.ItemsSource = ConditionList;
            ChooseCondBox.SelectedIndex = 0;
        }

        private void AddFilter_OnClick(object sender, RoutedEventArgs e)
        {
            var newFilter = new Filter
            {
                Attribute = ChooseAttrBox.Text,
                Condition = ChooseCondBox.Text,
                Value = ChooseValueBox.Text
            };
            dataList.FilterList.Add(newFilter);
            RefreshFilters();
        }

        private void RefreshFilters()
        {
            Filtergrid.ItemsSource = dataList.FilterList;
            Filtergrid.Items.Refresh();
            ApplyFilters();
            dataList.DataListPicker.ItemsSource = filterUnits;
            dataList.DataListPicker.Items.Refresh();
        }

        private List<string> filterUnits = AllUnits.Keys.ToList();

        private void ApplyFilters()
        {
            filterUnits = AllUnits.Keys.ToList();
            foreach (var unit in AllUnits.Keys)
            {
                if (!filterUnits.Contains(unit))
                {
                    continue;
                }
                var filtered = false;
                foreach (var filter in dataList.FilterList)
                {
                    if (filtered)
                    {
                        continue;
                    }
                    var unitValue = AllUnits[unit].GetType().GetProperty(filter.Attribute)?.GetValue(AllUnits[unit], null);
                    switch (unitValue)
                    {
                        case null:
                            continue;
                        case string:
                            unitValue = unitValue.ToString().ToLower();
                            switch (filter.Condition)
                            {
                                case "Equals":
                                    if (unitValue.ToString() != filter.Value && filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }

                                    break;
                                case "Not Equals":
                                    if (unitValue.ToString() == filter.Value && filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }

                                    break;
                                case "Contains":
                                    if (!unitValue.ToString().Contains(filter.Value) && filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }
                                    break;
                                case "Not Contains":
                                    if (unitValue.ToString().Contains(filter.Value) && filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }
                                    break;
                            }
                            break;
                        case int:
                            switch (filter.Condition)
                            {
                                case "Equals":
                                    if (unitValue.ToString() != filter.Value && filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }

                                    break;
                                case "Not Equals":
                                    if (unitValue.ToString() == filter.Value && filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }

                                    break;
                                case "Greater Than":
                                    if (int.Parse(unitValue.ToString() ?? throw new InvalidOperationException()) <=
                                        int.Parse(filter.Value) && filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }

                                    break;
                                case "Less Than":
                                    if (int.Parse(unitValue.ToString() ?? throw new InvalidOperationException()) >=
                                        int.Parse(filter.Value) && filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }

                                    break;
                                case "Greater Than Or Equals":
                                    if (int.Parse(unitValue.ToString() ?? throw new InvalidOperationException()) <
                                        int.Parse(filter.Value) && filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }

                                    break;
                                case "Less Than Or Equals":
                                    if (int.Parse(unitValue.ToString() ?? throw new InvalidOperationException()) >
                                        int.Parse(filter.Value) && filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }

                                    break;
                                case "Contains":
                                    if (!unitValue.ToString().Contains(filter.Value) && filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }
                                    break;
                                case "Not Contains":
                                    if (unitValue.ToString().Contains(filter.Value) && filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }
                                    break;
                            }

                            break;
                        case double:
                            switch (filter.Condition)
                            {
                                case "Equals":
                                    if (unitValue.ToString() != filter.Value && filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }

                                    break;
                                case "Not Equals":
                                    if (unitValue.ToString() == filter.Value && filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }

                                    break;
                                case "Greater Than":
                                    if (double.Parse(unitValue.ToString()) <= double.Parse(filter.Value) &&
                                        filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }

                                    break;
                                case "Less Than":
                                    if (double.Parse(unitValue.ToString()) >= double.Parse(filter.Value) &&
                                        filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }

                                    break;
                                case "Greater Than Or Equals":
                                    if (double.Parse(unitValue.ToString()) < double.Parse(filter.Value) &&
                                        filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }

                                    break;
                                case "Less Than Or Equals":
                                    if (double.Parse(unitValue.ToString()) > double.Parse(filter.Value) &&
                                        filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }

                                    break;
                                case "Contains":
                                    if (!unitValue.ToString().Contains(filter.Value) && filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }
                                    break;
                                case "Not Contains":
                                    if (unitValue.ToString().Contains(filter.Value) && filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }
                                    break;
                            }
                            break;
                        case List:
                            switch (filter.Condition)
                            {
                                case "Equals":
                                    if (unitValue.ToString() != filter.Value && filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }
                                    break;
                                case "Not Equals":
                                    if (unitValue.ToString() == filter.Value && filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }
                                    break;
                                case "Contains":
                                    if (!unitValue.ToString().Contains(filter.Value) && filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }
                                    break;
                                case "Not Contains":
                                    if (unitValue.ToString().Contains(filter.Value) && filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }
                                    break;
                            }

                            break;
                    }
                }
            }

        }

        private void RemoveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button) return;
            var row = ModelDbTabView.FindVisualParent<DataGridRow>(button);
            if (row.Item is Filter dataItem) dataList.FilterList.Remove(dataItem);
            RefreshFilters();
        }
    }

    public class Filter
    {
        public string Attribute { get; set; }
        public string Condition { get; set; }
        public string Value { get; set; }
    }
}
