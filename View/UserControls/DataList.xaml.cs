using ModdingTool.View.InterfaceData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static ModdingTool.Globals;

namespace ModdingTool.View.UserControls
{
    /// <summary>
    /// Interaction logic for DataList.xaml
    /// </summary>
    public partial class DataList : UserControl
    {

        public static string Selected { get; set; }
        public string SelectedType { get; set; }

        public event EventHandler<ListChangedEventArgs>? ListChanged;
        public ObservableCollection<string> DisplayedItems { get; } = new ObservableCollection<string>();

        public DataList()
        {
            Selected = "";
            SelectedType = "";
            InitializeComponent();
            this.DataContext = this;
        }

        public void InitItems()
        {
            var pickList = new List<string> { "Units", "Model Entries", "Mounts", "Projectiles", "Factions", "Cultures" };
            DataPicker.ItemsSource = pickList;
            DataPicker.SelectedIndex = 0;
        }

        private void DataPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = DataPicker.SelectedItem.ToString();
            if (selected == null || selected.Equals(SelectedType)) return;
            switch (selected)
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
            SelectedType = selected;
            ListChanged?.Invoke(this, new ListChangedEventArgs(SelectedType, DisplayedItems));
        }

        public void UpdateDisplayedItems(List<string> items)
        {
            DisplayedItems.Clear();
            foreach (var item in items)
            {
                DisplayedItems.Add(item);
            }
        }

        private void DataListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataListPicker.SelectedItem == null) return;
            var window = Window.GetWindow(this) ?? throw new InvalidOperationException();
            var dataTabs = window.FindName("DataTabLive") as DataTab;
            var selected = DataListPicker.SelectedItem.ToString();
            if (selected != null)
                Selected = selected;

            switch (SelectedType)
            {
                case "Units":
                    dataTabs?.AddTab(new UnitTab(Selected));
                    break;
                case "Model Entries":
                    dataTabs?.AddTab(new ModelDbTab(Selected));
                    break;
                case "Mounts":
                    dataTabs?.AddTab(new MountTab(Selected));
                    break;
                case "Projectiles":
                    dataTabs?.AddTab(new ProjectileTab(Selected));
                    break;
            }
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
