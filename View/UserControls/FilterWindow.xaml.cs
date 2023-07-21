using ModdingTool.View.InterfaceData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static ModdingTool.Globals;

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
            AttributeList = new List<string>();
            if (Application.Current.MainWindow == null) return;
            var window = (ModdingTool.MainWindow)Application.Current.MainWindow;
            datatab = window?.FindName("DataTabLive") as DataTab;
            dataList = window?.FindName("DataListLive") as DataList;
            if (datatab?.selectedTab is not UnitTab unitTab) return;
            selectunit = unitTab;
            foreach (var prop in unitTab.SelectedUnit.GetType().GetProperties())
            {
                if (UnitTab.UnitUiText.ContainsKey(prop.Name))
                {
                    AttributeList.Add(UnitTab.UnitUiText[prop.Name]);
                }
            }
            ChooseAttrBox.ItemsSource = AttributeList;
            ChooseCondBox.ItemsSource = ConditionList;
            ChooseAttrBox.SelectedIndex = 0;
            ChooseCondBox.SelectedIndex = 0;
            RefreshFilters();
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
            dataList.UnitList = new List<string>();
            dataList.UnitList.AddRange(filterUnits);
            dataList.DataListPicker.ItemsSource = dataList.UnitList;
            dataList.DataListPicker.Items.Refresh();
        }

        private List<string> filterUnits = UnitDataBase.Keys.ToList();

        private void ApplyFilters()
        {
            filterUnits = new List<string>();
            filterUnits.AddRange(UnitDataBase.Keys);
            foreach (var unit in UnitDataBase.Keys)
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
                    var attribute = "";
                    foreach (var attr in UnitTab.UnitUiText.Where(attr => attr.Value == filter.Attribute))
                    {
                        attribute = attr.Key;
                    }
                    var unitValue = UnitDataBase[unit].GetType().GetProperty(attribute)?.GetValue(UnitDataBase[unit], null);
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
                        case List<string> list:
                            var unitValueList = list.Select(x => x.ToLower()).ToList();
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
                                    if (!unitValueList.Contains(filter.Value) && filterUnits.Contains(unit))
                                    {
                                        filterUnits.Remove(unit);
                                        filtered = true;
                                    }
                                    break;
                                case "Not Contains":
                                    if (unitValueList.Contains(filter.Value) && filterUnits.Contains(unit))
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
