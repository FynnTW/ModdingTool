﻿using CommunityToolkit.Mvvm.ComponentModel;
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

        [ObservableProperty]
        private ObservableCollection<string> _displayedItems = new();

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

        public DataListViewModel()
        {
            SelectedType = PickList[0];
            ModLoadedEvent += DataListViewModel_ModLoadedEvent;
        }

        private void DataListViewModel_ModLoadedEvent(object? sender, EventArgs e)
        {
            DataListSelectionChanged();
        }

        public void UpdateDisplayedItems(List<string> items)
        {
            DisplayedItems.Clear();
            foreach (var item in items)
            {
                DisplayedItems.Add(item);
            }
        }

        [RelayCommand]
        public void OpenNewTab()
        {
            if (Selected == "")
                return;
            ITab? addedTab = SelectedType switch
            {
                "Units" => new UnitTab(Selected),
                "Model Entries" => new ModelDbTab(Selected),
                "Mounts" => new MountTab(Selected),
                "Projectiles" => new ProjectileTab(Selected),
                _ => null
            };
            if (addedTab != null)
                SelectionChanged?.Invoke(this, new TabPickedEventArgs(addedTab));
        }

        partial void OnSelectedTypeChanged(string value) => DataListSelectionChanged();

        [RelayCommand]
        private void DataListSelectionChanged()
        {
            switch (SelectedType)
            {
                case "Units":
                    UpdateDisplayedItems(UnitDataBase.Keys.ToList());
                    break;
                case "Model Entries":
                    UpdateDisplayedItems(BattleModelDataBase.Keys.ToList());
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
            ListChanged?.Invoke(this, new ListChangedEventArgs(SelectedType, DisplayedItems));
            Selected = "";
        }

    }

    public class TabPickedEventArgs
    {
        public ITab SelectedTab { get; }

        public TabPickedEventArgs(ITab selectedTab)
        {
            SelectedTab = selectedTab;
        }
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
