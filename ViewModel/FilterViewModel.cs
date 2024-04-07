// Ignore Spelling: Modding

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ModdingTool.View.InterfaceData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static ModdingTool.Globals;
using static ModdingTool.ViewModel.DataListViewModel;

namespace ModdingTool.ViewModel
{
    public partial class FilterViewModel : ObservableObject
    {

        #region Properties

        #region Observable Properties
        [ObservableProperty]
        private static ObservableCollection<Filter> _localFilterList = new();
        [ObservableProperty]
        private static List<string> _attributeList = new();
        [ObservableProperty]
        private string _selectedAttribute = "";
        [ObservableProperty]
        private string _selectedCondition = "Equals";
        [ObservableProperty]
        private string _value = "";
        [ObservableProperty]
        private static List<string> _conditionList = new()
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
        #endregion

        private readonly Dictionary<string, string> _uiText = new();

        private static Dictionary<string, ObservableCollection<Filter>>? s_filterLists;

        private string _tabType = "";
        private string TabType { get => _tabType; set => SetProperty(ref _tabType, value); }

        private ObservableCollection<string> _sortList = new();
        
        private string _searchBoxContent = string.Empty;

        private ObservableCollection<string> SortList { get => _sortList; set => SetProperty(ref _sortList, value); }
        #endregion

        #region Commands
        [RelayCommand]
        private void AddFilter()
        {
            LocalFilterList.Add(new Filter(SelectedAttribute, SelectedCondition, Value));
            CallApplyFilters();
        }

        [RelayCommand]
        private void RemoveButton(Filter item)
        {
            LocalFilterList.Remove(item);
            CallApplyFilters();
        }
        #endregion

        #region constructors
        public FilterViewModel() => s_filterLists ??= new();
        public FilterViewModel(ObservableCollection<string> itemList, string selectedType)
        {
            _searchBoxContent = string.Empty;
            TabType = selectedType;
            SortList = itemList;
            s_filterLists ??= new();
            LocalFilterList = s_filterLists.TryGetValue(TabType, out var list) ? list : new ObservableCollection<Filter>();
            switch (TabType)
            {
                case "Units":
                    _uiText = UnitTab.UnitUiText;
                    AttributeList = GetSortTypes<Unit>();
                    break;
                case "Model Entries":
                    _uiText = ModelDbTab.BmdbUiText;
                    AttributeList = GetSortTypes<BattleModel>();
                    break;
                case "Mounts":
                    _uiText = MountTab.MountUiText;
                    AttributeList = GetSortTypes<Mount>();
                    break;
                case "Projectiles":
                    _uiText = ProjectileTab.ProjectileUiText;
                    AttributeList = GetSortTypes<Projectile>();
                    break;
                default:
                    return;
            }
            AttributeList.Sort();
            SelectedAttribute = AttributeList[0];
            if (LocalFilterList.Count > 0)
                CallApplyFilters();
            
            
        }

        private void FilterViewModel_OnSearchBoxChanged(object? sender, SearchBoxEventArgs e)
        {
            _searchBoxContent = e.Text;
            CallApplyFilters();
        }

        #endregion

        #region Methods


        private List<string> GetSortTypes<T>()
        {
            var result = new List<string>();

            foreach (var prop in typeof(T).GetProperties())
            {
                if (_uiText.TryGetValue(prop.Name, out var value))
                {
                    result.Add(value);
                }
            }
            return result;
        }

        private void CallApplyFilters()
        {
            if (s_filterLists == null)
                return;
            switch (_tabType)
            {
                case "Units":
                    ApplyFilters(ModData.Units.GetUnits());
                    break;
                case "Model Entries":
                    ApplyFilters(ModData.BattleModelDb.GetBattleModels());
                    break;
                case "Mounts":
                    ApplyFilters(MountDataBase);
                    break;
                case "Projectiles":
                    ApplyFilters(ProjectileDataBase);
                    break;
                default:
                    throw new InvalidOperationException("Unsupported type");
            }
            s_filterLists[TabType] = LocalFilterList;
        }

        private void ApplyFilters<T>(Dictionary<string, T> database)
        {
            var filteredItems = new List<string>();
            filteredItems.AddRange(database.Keys);
            foreach (var dataItem in database.Keys)
            {
                if (!filteredItems.Contains(dataItem))
                    continue;
                foreach (var filter in LocalFilterList)
                {
                    var attribute = "";
                    foreach (var attr in _uiText.Where(attr => attr.Value == filter.Attribute))
                    {
                        attribute = attr.Key;
                    }
                    var value = database[dataItem]?.GetType().GetProperty(attribute)?.GetValue(database[dataItem], null);
                    if (value == null)
                    {
                        continue;
                    }
                    switch (filter.Condition)
                    {
                        case "Equals":
                            if (value.ToString()?.ToLower() != filter.Value.ToLower() && filteredItems.Contains(dataItem))
                                filteredItems.Remove(dataItem);
                            break;
                        case "Not Equals":
                            if (value.ToString()?.ToLower() == filter.Value.ToLower() && filteredItems.Contains(dataItem))
                                filteredItems.Remove(dataItem);
                            break;
                        case "Contains":
                            if (value is List<string> valueList)
                            {
                                if (!valueList.Contains(filter.Value) && filteredItems.Contains(dataItem))
                                    filteredItems.Remove(dataItem);
                                break;
                            }
                            if (!value.ToString()!.ToLower().Contains(filter.Value.ToLower()) && filteredItems.Contains(dataItem))
                                filteredItems.Remove(dataItem);
                            break;
                        case "Not Contains":
                            if (value is List<string> valueList2)
                            {
                                if (!valueList2.Contains(filter.Value) && filteredItems.Contains(dataItem))
                                    filteredItems.Remove(dataItem);
                                break;
                            }
                            if (value.ToString()!.ToLower().Contains(filter.Value.ToLower()) && filteredItems.Contains(dataItem))
                                filteredItems.Remove(dataItem);
                            break;
                        case "Greater Than":
                            switch (value)
                            {
                                case int:
                                    {
                                        if (int.Parse(value.ToString() ?? throw new InvalidOperationException()) <=
                                            int.Parse(filter.Value) && filteredItems.Contains(dataItem))
                                            filteredItems.Remove(dataItem);
                                        break;
                                    }
                                case double:
                                    {
                                        if (double.Parse(value.ToString() ?? throw new InvalidOperationException()) <=
                                            double.Parse(filter.Value) && filteredItems.Contains(dataItem))
                                            filteredItems.Remove(dataItem);
                                        break;
                                    }
                            }
                            break;
                        case "Less Than":
                            switch (value)
                            {
                                case int:
                                    {
                                        if (int.Parse(value.ToString() ?? throw new InvalidOperationException()) >=
                                            int.Parse(filter.Value) && filteredItems.Contains(dataItem))
                                            filteredItems.Remove(dataItem);
                                        break;
                                    }
                                case double:
                                    {
                                        if (double.Parse(value.ToString() ?? throw new InvalidOperationException()) >=
                                            double.Parse(filter.Value) && filteredItems.Contains(dataItem))
                                            filteredItems.Remove(dataItem);
                                        break;
                                    }
                            }
                            break;
                        case "Greater Than Or Equals":
                            switch (value)
                            {
                                case int:
                                    {
                                        if (int.Parse(value.ToString() ?? throw new InvalidOperationException()) <
                                            int.Parse(filter.Value) && filteredItems.Contains(dataItem))
                                            filteredItems.Remove(dataItem);
                                        break;
                                    }
                                case double:
                                    {
                                        if (double.Parse(value.ToString() ?? throw new InvalidOperationException()) <
                                            double.Parse(filter.Value) && filteredItems.Contains(dataItem))
                                            filteredItems.Remove(dataItem);
                                        break;
                                    }
                            }
                            break;
                        case "Less Than Or Equals":
                            switch (value)
                            {
                                case int:
                                    {
                                        if (int.Parse(value.ToString() ?? throw new InvalidOperationException()) >
                                            int.Parse(filter.Value) && filteredItems.Contains(dataItem))
                                            filteredItems.Remove(dataItem);
                                        break;
                                    }
                                case double:
                                    {
                                        if (double.Parse(value.ToString() ?? throw new InvalidOperationException()) >
                                            double.Parse(filter.Value) && filteredItems.Contains(dataItem))
                                            filteredItems.Remove(dataItem);
                                        break;
                                    }
                            }
                            break;
                    }
                }
            }
            SortList.Clear();
            foreach (var item in filteredItems)
                SortList.Add(item);
        }

        #endregion
    }


    public class Filter
    {
        public Filter(string attribute, string condition, string value)
        {
            Attribute = attribute;
            Condition = condition;
            Value = value;
        }

        public string Attribute { get; set; }
        public string Condition { get; set; }
        public string Value { get; set; }
    }
}
