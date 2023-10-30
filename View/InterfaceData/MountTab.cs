using System.Collections.Generic;
using System.Linq;
using static ModdingTool.Globals;

namespace ModdingTool.View.InterfaceData
{
    public class MountTab : ITab.Tab
    {
        public Mount SelectedMount { get; set; }
        public static string[] ModelEntries { get; set; }

        public static Dictionary<string, string> MountUiText { get; set; } = new Dictionary<string, string>()
        {
            {"type", "Type"},
            {"mount_class", "Mount Class"},
            {"model", "Model"},
            {"radius", "Radius"},
            {"x_radius", "X Radius"},
            {"y_offset", "Y Offset"},
            {"height", "Height"},
            {"mass", "Mass"},
            {"banner_height", "Banner Height"},
            {"bouyancy_offset", "Bouyancy Offset"},
            {"water_trail_effect", "Water Trail Effect"},
            {"root_node_height", "Root Node Height"},
            {"attack_delay", "Attack Delay"},
            {"dead_radius", "Dead Radius"},
            {"tusk_z", "Tusk Z"},
            {"tusk_radius", "Tusk Radius"},
            {"riders", "Riders"},
        };
        public static List<string> MountTypes { get; set; } = new List<string>()
        {
            "horse",
            "elephant",
            "camel"
        };
        public MountTab(string name)
        {
            Title = name;
            SelectedMount = MountDataBase[Title];
            ModelEntries = BattleModelDataBase.Keys.ToArray();
        }
    }
}
