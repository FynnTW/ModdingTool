﻿using ModdingTool.View.InterfaceData;
using System.Collections.Generic;
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
        public List<Filter> FilterList { get; set; } = new List<Filter>();
        public List<string> sortTypes { get; set; } = new List<string>();
        public List<string> sortDirections { get; set; } = new List<string>() { "Decreasing", "Increasing" };
        public List<string> UnitList { get; set; } = new List<string>();
        public List<string> ModelList { get; set; } = new List<string>();
        public List<string> MountList { get; set; } = new List<string>();
        public List<string> ProjectileList { get; set; } = new List<string>();

        public DataList()
        {
            Selected = "";
            SelectedType = "";
            InitializeComponent();
        }

        public void InitItems()
        {
            var pickList = new List<string> { "Units", "Model Entries", "Mounts", "Projectiles", "Factions", "Cultures" };
            DataPicker.ItemsSource = pickList;
            DataPicker.SelectedIndex = 0;
            UnitList = UnitDataBase.Keys.ToList();
            ModelList = BattleModelDataBase.Keys.ToList();
            MountList = MountDataBase.Keys.ToList();
        }



        private void DataPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = DataPicker.SelectedItem.ToString();
            Print(selected);
            switch (selected)
            {
                case "Units":
                    UnitList = UnitDataBase.Keys.ToList();
                    DataListPicker.ItemsSource = UnitList;
                    SelectedType = selected;
                    break;
                case "Model Entries":
                    ModelList = BattleModelDataBase.Keys.ToList();
                    DataListPicker.ItemsSource = ModelList;
                    SelectedType = selected;
                    break;
                case "Factions":
                    DataListPicker.ItemsSource = FactionDataBase.Keys;
                    SelectedType = selected;
                    break;
                case "Cultures":
                    DataListPicker.ItemsSource = CultureDataBase.Keys;
                    SelectedType = selected;
                    break;
                case "Mounts":
                    MountList = MountDataBase.Keys.ToList();
                    DataListPicker.ItemsSource = MountList;
                    SelectedType = selected;
                    break;
                case "Projectiles":
                    ProjectileList = ProjectileDataBase.Keys.ToList();
                    DataListPicker.ItemsSource = ProjectileList;
                    SelectedType = selected;
                    break;
            }
        }

        private void DataListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataListPicker.SelectedItem == null) return;
            Window window = Window.GetWindow(this);
            var dataTabs = window.FindName("DataTabLive") as DataTab;
            var selected = DataListPicker.SelectedItem.ToString();
            if (selected != null)
            {
                Selected = selected;
            }

            if (SelectedType == "Units")
            {
                dataTabs.AddTab(new UnitTab(selected));
            }
            else if (SelectedType == "Model Entries")
            {
                dataTabs.AddTab(new ModelDbTab(selected));
            }
            else if (SelectedType == "Mounts")
            {
                dataTabs.AddTab(new MountTab(selected));
            }
            else if (SelectedType == "Projectiles")
            {
                dataTabs.AddTab(new ProjectileTab(selected));
            }
        }
    }
}
