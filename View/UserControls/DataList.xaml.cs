using System;
using System.Collections.Generic;
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
using ModdingTool.View.InterfaceData;
using static ModdingTool.Globals;

namespace ModdingTool.View.UserControls
{
    /// <summary>
    /// Interaction logic for DataList.xaml
    /// </summary>
    public partial class DataList : UserControl
    {

        public static string Selected { get; set; }
        public static string SelectedType { get; set; }
        public List<Filter> FilterList { get; set; } = new List<Filter>();
        public List<string> sortTypes { get; set; } = new List<string>();
        public List<string> sortDirections { get; set; } = new List<string>() { "Decreasing", "Increasing" };
        public List<string> UnitList { get; set; } = new List<string>();

        public DataList()
        {
            Selected = "";
            SelectedType = "";
            InitializeComponent();
        }

        public void InitItems()
        {
            var pickList = new List<string> { "Units", "Model Entries", "Factions", "Cultures" };
            DataPicker.ItemsSource = pickList;
            DataPicker.SelectedIndex = 0;
            UnitList = AllUnits.Keys.ToList();
        }



        private void DataPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = DataPicker.SelectedItem.ToString();
            Print(selected);
            switch (selected)
            {
                case "Units":
                    UnitList = AllUnits.Keys.ToList();
                    DataListPicker.ItemsSource = UnitList;
                    SelectedType = selected;
                    break;
                case "Model Entries":
                    DataListPicker.ItemsSource = ModelDb.Keys;
                    SelectedType = selected;
                    break;
                case "Factions":
                    DataListPicker.ItemsSource = AllFactions.Keys;
                    SelectedType = selected;
                    break;
                case "Cultures":
                    DataListPicker.ItemsSource = AllCultures.Keys;
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
        }
    }
}
