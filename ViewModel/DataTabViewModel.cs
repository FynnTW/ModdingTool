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
        private ICollection<ITab> _tabs;
        [ObservableProperty]
        private ITab _selectedTab = null;

        public DataTabViewModel() =>
            Tabs = new ObservableCollection<ITab>();
        public DataTabViewModel(DataList dataList)
        {
            if (dataList.DataContext is DataListViewModel dataListViewmodel)
                dataListViewmodel.SelectionChanged += OnTabPicked;
            Tabs = new ObservableCollection<ITab>();
        }

        private void OnTabPicked(object? sender, TabPickedEventArgs e) =>
            AddTab(e.SelectedTab);


        public void AddTab(ITab tab)
        {
            Tabs.Add(tab);
            SelectedTab = tab;
        }

        public void RemoveTab(ITab tab)
        {
            Tabs.Remove(tab);
        }
    }
}
