using System.Collections.Generic;

namespace ModdingTool
{
    public class Building : GameType
    {
        private bool _isHinterlandFarms = false;
        public bool isGuild { get; set; } = false;
        public bool isConvert { get; set; } = false;
        public string convert_to { get; set; } = "";
        private string _religion = "";
        public List<string> factions { get; set; } = new();
        private string _classification { get; set; } = "";
        public List<BuildingLevel> Levels { get; set; } = new();
        public List<Plugin> Plugins { get; set; } = new();
        public string Name { 
            get => _name;
            set
            {
                AddChange("Name", _name, value);
                _name = value;
                NotifyPropertyChanged();
            }
        }
        public string Religion { 
            get => _religion;
            set
            {
                AddChange("Religion", _religion, value);
                _name = value;
                NotifyPropertyChanged();
            }
        }
        
        public string Classification { 
            get => _classification;
            set
            {
                AddChange("Classification", _classification, value);
                _classification = value;
                NotifyPropertyChanged();
            }
        }
        
        public void AddBuildingLevel(BuildingLevel level) => Levels.Add(level);
        public void AddPlugin(Plugin plugin) => Plugins.Add(plugin);
        
        public void RemoveBuildingLevel(BuildingLevel level) => Levels.Remove(level);
        
        public void RemovePlugin(Plugin plugin) => Plugins.Remove(plugin);
        
        public BuildingLevel GetBuildingLevel(int index) => Levels[index];
        
        public Plugin GetPlugin(int index) => Plugins[index];
        
        public bool IsHinterland => Name.StartsWith(("hinterland_"));
        public bool IsTemple => Name.StartsWith(("temple_"));

    }

    public class BuildingLevel
    {
        public string name { get; set; } = "";
        public string settlementType { get; set; } = "";
        public string convert_to { get; set; } = "";
        public List<BuildingRequirements> requirements { get; set; } = new();
        public List<Capability> capabilities { get; set; } = new();
        public List<Capability> faction_capabilities { get; set; } = new();
        public List<RecruitPool> recruit_pools { get; set; } = new();
        public string material { get; set; } = "";
        public int construction { get; set; } = 0;
        public int cost { get; set; } = 0;
        public string settlement_min { get; set; } = "";
        public List<BuildingUpgrade> upgrades { get; set; } = new();

    }

    public class BuildingUpgrade
    {
        public string name { get; set; } = "";
        public List<BuildingRequirements> requirements { get; set; } = new();
    }

    public class Capability
    {
        public string name { get; set; } = "";
        public string value { get; set; } = "";
        public bool bonus { get; set; } = false;
        public List<BuildingRequirements> requirements { get; set; } = new();
        public string character_type { get; set; } = "";
    }

    public class RecruitPool
    {
        public string unit { get; set; } = "";
        public double initialPool { get; set; } = 0;
        public double replenishmentRate { get; set; } = 0;
        public double maximumPool { get; set; } = 0;
        public double startingExperience { get; set; } = 0;
        public List<BuildingRequirements> requirements { get; set; } = new();
    }

    public class BuildingRequirements
    {
        public string requirement { get; set; } = "";
        public string value { get; set; } = "";

    }

    public class Plugin
    {
        public List<string> factions { get; set; } = new();
        public List<string> levels { get; set; } = new();

    }
}
