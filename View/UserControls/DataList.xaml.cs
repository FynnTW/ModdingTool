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
using static ModdingTool.Globals;

namespace ModdingTool.View.UserControls
{
    /// <summary>
    /// Interaction logic for DataList.xaml
    /// </summary>
    public partial class DataList : UserControl
    {

        public DataList()
        {
            InitializeComponent();
        }

        public void InitItems()
        {
            var pickList = new List<string> { "Units", "Model Entries", "Factions", "Cultures" };
            DataPicker.ItemsSource = pickList;
        }

        private void DataPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = DataPicker.SelectedItem.ToString();
            Print(selected);
            switch (selected)
            {
                case "Units":
                    DataListPicker.ItemsSource = AllUnits.Keys;
                    break;
                case "Model Entries":
                    DataListPicker.ItemsSource = ModelDb.Keys;
                    break;
                case "Factions":
                    DataListPicker.ItemsSource = AllFactions.Keys;
                    break;
                case "Cultures":
                    DataListPicker.ItemsSource = AllCultures.Keys;
                    break;
            }
        }
    }
}
