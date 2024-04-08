using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ModdingTool.ViewModel.InterfaceData;
using ModdingTool.View.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static ModdingTool.Globals;

namespace ModdingTool.ViewModel
{
    public partial class ToolBoxViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<string> _sortTypes = new();

        [ObservableProperty]
        private List<string> _sortDirections = new() { "Increasing", "Decreasing" };

        [ObservableProperty]
        private string _selectedSortDirection = "Increasing";

        [ObservableProperty]
        private string _selectedSortType = "";

        [ObservableProperty]
        private bool _isPopupOpen;

        [ObservableProperty]
        private string _tabType = "";

        private ObservableCollection<string> _sortList = new();

        public ObservableCollection<string> SortList { get => _sortList; set => SetProperty(ref _sortList, value); }

        private Dictionary<string, string> _uiText = new();


        [RelayCommand]
        private void Filter()
        {
            var w = new Window1
            {
                Content = new FilterWindow(SortList, TabType)
            };
            w.Show();
        }
        
        [RelayCommand]
        private void Create()
        {
            var w = new CreationWindow(TabType);
            w.Show();
        }

        partial void OnTabTypeChanged(string value)
        {
            var filter = new FilterViewModel(SortList, TabType);
        }

        [RelayCommand]
        private void SortListPopUp()
        {
            switch (TabType)
            {
                case "Units":
                    _uiText = UnitTab.UnitUiText;
                    SortTypes = GetSortTypes<Unit>();
                    break;
                case "Model Entries":
                    _uiText = ModelDbTab.BmdbUiText;
                    SortTypes = GetSortTypes<BattleModel>();
                    break;
                case "Mounts":
                    _uiText = MountTab.MountUiText;
                    SortTypes = GetSortTypes<Mount>();
                    break;
                case "Projectiles":
                    _uiText = ProjectileTab.ProjectileUiText;
                    SortTypes = GetSortTypes<Projectile>();
                    break;
                default:
                    throw new InvalidOperationException("Unsupported type");
            }
            SortTypes.Sort();
            SelectedSortType = SortTypes[0];
            IsPopupOpen = true;
        }

        private List<string> GetSortTypes<T>()
        {
            var result = new List<string>();

            foreach (var prop in typeof(T).GetProperties())
            {
                if (typeof(IComparable).IsAssignableFrom(prop.PropertyType) && _uiText.TryGetValue(prop.Name, out var value))
                {
                    result.Add(value);
                }
            }
            return result;
        }

        [RelayCommand]
        private void SortCancel() => IsPopupOpen = false;

        [RelayCommand]
        private void SortAccept()
        {
            var attribute = _uiText.FirstOrDefault(attr => attr.Value == SelectedSortType).Key;

            object? GetAttributeValue(string x)
            {
                return TabType switch
                {
                    "Units" => ModData.Units.Get(x)?.GetType().GetProperty(attribute)?.GetValue(ModData.Units.Get(x)),
                    "Model Entries" => ModData.BattleModelDb.Get(x)
                        ?.GetType()
                        .GetProperty(attribute)
                        ?.GetValue(ModData.BattleModelDb.Get(x)),
                    "Mounts" => MountDataBase[x]?.GetType().GetProperty(attribute)?.GetValue(MountDataBase[x]),
                    "Projectiles" => ProjectileDataBase[x]
                        ?.GetType()
                        .GetProperty(attribute)
                        ?.GetValue(ProjectileDataBase[x]),
                    _ => throw new InvalidOperationException("Unsupported type")
                };
            }

            var sortedList = SelectedSortDirection switch
            {
                "Increasing" => SortList.OrderBy(GetAttributeValue).ToList(),
                "Decreasing" => SortList.OrderByDescending(GetAttributeValue).ToList(),
                _ => throw new InvalidOperationException($"Unsupported sort direction: {SelectedSortDirection}")
            };

            SortList.Clear();
            foreach (var item in sortedList)
                SortList.Add(item);

            IsPopupOpen = false;
        }
    }
}
