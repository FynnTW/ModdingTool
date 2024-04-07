using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ModdingTool.View.InterfaceData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static ModdingTool.Globals;

namespace ModdingTool.ViewModel
{
    public partial class DataListViewModel : ObservableObject
    {
        [ObservableProperty]
        private static string _selected = "";
        [ObservableProperty]
        private string _selectedType = "";

        public event EventHandler<ListChangedEventArgs>? ListChanged;

        public event EventHandler<TabPickedEventArgs>? SelectionChanged;
        public static event EventHandler<SearchBoxEventArgs>? OnSearchBoxChanged;

        [ObservableProperty]
        private ObservableCollection<string> _displayedItems = new();
        [ObservableProperty]
        private string _searchBoxText = "";

        [ObservableProperty]
        private List<string> _pickList = new()
        {
            "Units",
            "Model Entries",
            "Mounts",
            "Projectiles",
            "Factions",
            "Cultures"
        };

        partial void OnSearchBoxTextChanging(string value) =>
            DataListSelectionChanged();
        partial void OnSearchBoxTextChanged(string value) =>
            DataListSelectionChanged();

        public DataListViewModel()
        {
            SelectedType = PickList[0];
            ModLoadedEvent += DataListViewModel_ModLoadedEvent;
        }

        private void DataListViewModel_ModLoadedEvent(object? sender, EventArgs e)
        {
            DataListSelectionChanged();
        }

        private void UpdateDisplayedItems(IEnumerable<string> items)
        {
            DisplayedItems.Clear();
            foreach (var item in items.Where(item => string.IsNullOrWhiteSpace(SearchBoxText) 
                                                     || item.ToLower().Contains(SearchBoxText.ToLower())))
                DisplayedItems.Add(item);
        }

        [RelayCommand]
        public void OpenNewTab()
        {
            if (Selected == "")
                return;
            OpenTabType = SelectedType;
            Tab? addedTab = SelectedType switch
            {
                "Units" => new UnitTab(Selected),
                "Model Entries" => new ModelDbTab(Selected),
                "Mounts" => new MountTab(Selected),
                "Projectiles" => new ProjectileTab(Selected),
                _ => null
            };
            if (addedTab != null)
            {
                SelectionChanged?.Invoke(this, new TabPickedEventArgs(addedTab));
            }
        }

        public void OpenNewTab(string type, string name)
        {
            if (type == "")
                return;
            Tab? addedTab = type switch
            {
                "Units" => new UnitTab(name),
                "Model Entries" => new ModelDbTab(name),
                "Mounts" => new MountTab(name),
                "Projectiles" => new ProjectileTab(name),
                _ => null
            };
            if (addedTab == null) return;
            SelectedType = type;
            DataListSelectionChanged();
            Selected = name;
            SelectionChanged?.Invoke(this, new TabPickedEventArgs(addedTab));
        }

        partial void OnSelectedTypeChanged(string value) => DataListSelectionChanged();

        [RelayCommand]
        private void DataListSelectionChanged()
        {
            switch (SelectedType)
            {
                case "Units":
                    UpdateDisplayedItems(ModData.Units.GetNames());
                    break;
                case "Model Entries":
                    UpdateDisplayedItems(ModData.BattleModelDb.GetNames());
                    break;
                case "Factions":
                    UpdateDisplayedItems(FactionDataBase.Keys.ToList());
                    break;
                case "Cultures":
                    UpdateDisplayedItems(CultureDataBase.Keys.ToList());
                    break;
                case "Mounts":
                    UpdateDisplayedItems(MountDataBase.Keys.ToList());
                    break;
                case "Projectiles":
                    UpdateDisplayedItems(ProjectileDataBase.Keys.ToList());
                    break;
            }
            OpenListType = SelectedType;
            ListChanged?.Invoke(this, new ListChangedEventArgs(SelectedType, DisplayedItems));
            Selected = "";
        }

    }

    public class TabPickedEventArgs
    {
        public Tab SelectedTab { get; }

        public TabPickedEventArgs(Tab selectedTab)
        {
            SelectedTab = selectedTab;
        }
    }

    public class SearchBoxEventArgs
    {
        public string Text { get; }

        public SearchBoxEventArgs(string text)
            => Text = text;
    }
    public class ListChangedEventArgs : EventArgs
    {
        public string SelectedListType { get; }
        public ObservableCollection<string> SelectedList { get; }

        public ListChangedEventArgs(string selectedListType, ObservableCollection<string> selectedList)
        {
            SelectedListType = selectedListType;
            SelectedList = selectedList;
        }
    }
}
