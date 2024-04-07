using CommunityToolkit.Mvvm.ComponentModel;
using ModdingTool.View.InterfaceData;
using ModdingTool.View.UserControls;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ModdingTool.ViewModel
{
    public partial class DataTabViewModel : ObservableObject
    {
        [ObservableProperty]
        private ICollection<Tab> _tabs;
        [ObservableProperty]
        private Tab _selectedTab = null;

        public DataTabViewModel() =>
            Tabs = new ObservableCollection<Tab>();
        public DataTabViewModel(DataList dataList)
        {
            if (dataList.DataContext is DataListViewModel dataListViewmodel)
                dataListViewmodel.SelectionChanged += OnTabPicked;
            Tabs = new ObservableCollection<Tab>();
        }

        private void OnTabPicked(object? sender, TabPickedEventArgs e) =>
            AddTab(e.SelectedTab);
        
        


        public void AddTab(Tab tab)
        {
            Tabs.Add(tab);
            SelectedTab = tab;
        }

        public void RemoveTab(Tab tab)
        {
            Tabs.Remove(tab);
        }
    }
}
