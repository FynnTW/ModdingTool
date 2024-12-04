using System.Collections.Generic;

namespace ModdingTool
{
    public class Building : GameType
    {
        private static readonly List<string> Classifications = new()
        {
            "fortification",
            "infrastructure",
            "military",
            "religious",
            "convertion",
            "guild",
            "other"
        };
        public bool IsGuild  => Name.StartsWith("guild_");
        public bool IsConvert => Name.StartsWith("convert_");
        private string _convertTo = "";
        private string _religion = "";
        private List<string> _factions = new();
        private List<string> _cultures = new();
        public List<string> LevelNames { get; set; } = new();
        private string _classification { get; set; } = "";
        public List<BuildingLevel> Levels { get; set; } = new();

        public int LevelCount => Levels.Count;
        public List<Plugin> Plugins { get; set; } = new();
        public List<string> Factions
        {
            get => _factions;
            set
            {
                _factions = value;
                NotifyPropertyChanged();
            }
        }
        public List<string> Cultures
        {
            get => _cultures;
            set
            {
                _cultures = value;
                NotifyPropertyChanged();
            }
        }
        
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
                _religion = value;
                NotifyPropertyChanged();
            }
        }
        
        public string Classification { 
            get => _classification;
            set
            {
                if (!Classifications.Contains(value))
                {
                    Globals.ErrorDb.AddError($"Building {Name} has an invalid classification: {value}");
                }
                AddChange("Classification", _classification, value);
                _classification = value;
                NotifyPropertyChanged();
            }
        }
        
        public string ConvertTo { 
            get => _convertTo;
            set
            {
                AddChange("ConvertTo", _convertTo, value);
                _convertTo = value;
                NotifyPropertyChanged();
            }
        }
        
        public void AddFaction(string faction) => Factions.Add(faction);
        public void AddCulture(string culture) => Cultures.Add(culture);
        
        public void RemoveFaction(string faction) => Factions.Remove(faction);
        
        public void RemoveCulture(string culture) => Cultures.Remove(culture);
        
        public void AddBuildingLevel(BuildingLevel level) => Levels.Add(level);
        public void AddPlugin(Plugin plugin) => Plugins.Add(plugin);
        
        public void RemoveBuildingLevel(BuildingLevel level) => Levels.Remove(level);
        
        public void RemovePlugin(Plugin plugin) => Plugins.Remove(plugin);
        
        public BuildingLevel GetBuildingLevel(int index) => Levels[index];
        
        public Plugin GetPlugin(int index) => Plugins[index];
        
        public bool IsHinterland => Name.StartsWith(("hinterland_"));
        public bool IsTemple => Name.StartsWith(("temple_"));
        public bool IsFarms => Name.StartsWith(("hinterland_farms"));

    }

    public class BuildingLevel : GameType
    {

        private static readonly List<string> Materials = new()
        {
            "wooden",
            "stone",
        };
        private static readonly List<string> SettlementLevels = new()
        {
            "village",
            "town",
            "large_town",
            "city",
            "large_city",
            "huge_city",
            "moot_and_bailey",
            "wooden_castle",
            "castle",
            "fortress",
            "citadel",
        };
        public string Name
        {
            get => _name;
            set
            {
                AddChange("Name", _name, value);
                _name = value;
                NotifyPropertyChanged();
            }
        }

        public bool AvailableCity { get; set; } = true;
        public bool AvailableCastle { get; set; } = true;

        private string _condition = "";
        private string _convertTo = "";
        private string _material = "";
        private int _constructionTime = 0;
        private int _constructionCost = 0;
        private string _settlementMinLevel = "village";

        public string Condition
        {
            get => _condition;
            set
            {
                AddChange("Condition", _condition, value);
                _condition = value;
                NotifyPropertyChanged();
            }
        }
        
        public string ConvertTo
        {
            get => _convertTo;
            set
            {
                AddChange("ConvertTo", _convertTo, value);
                _convertTo = value;
                NotifyPropertyChanged();
            }
        }
        
        public string Material
        {
            get => _material;
            set
            {
                if (!Materials.Contains(value))
                {
                    Globals.ErrorDb.AddError($"Building {Name} has an invalid material: {value}");
                    return;
                }
                AddChange("Material", _material, value);
                _material = value;
                NotifyPropertyChanged();
            }
        }
        
        public int ConstructionTime
        {
            get => _constructionTime;
            set
            {
                AddChange("ConstructionTime", _constructionTime, value);
                _constructionTime = value;
                NotifyPropertyChanged();
            }
        }
        
        public int ConstructionCost
        {
            get => _constructionCost;
            set
            {
                AddChange("ConstructionCost", _constructionCost, value);
                _constructionCost = value;
                NotifyPropertyChanged();
            }
        }

        public string SettlementMinLevel
        {
            get => _settlementMinLevel;
            set
            {
                if (!SettlementLevels.Contains(value))
                {
                    Globals.ErrorDb.AddError($"Building {Name} has an invalid settlement level: {value}");
                    return;
                }
                AddChange("SettlementMinLevel", _settlementMinLevel, value);
                _settlementMinLevel = value;
                NotifyPropertyChanged();
            }
        }
        
        private List<Capability> _capabilities = new();
        private List<Capability> _factionCapabilities = new();
        public List<Capability> Capabilities
        {
            get => _capabilities;
            set
            {
                _capabilities = value;
                NotifyPropertyChanged();
            }
        }
        public int CapabilityCount => Capabilities.Count;
        public List<Capability> FactionCapabilities
        {
            get => _factionCapabilities;
            set
            {
                _factionCapabilities = value;
                NotifyPropertyChanged();
            }
        }
        public int FactionCapabilityCount => FactionCapabilities.Count;
        public void AddCapability(Capability capability) => Capabilities.Add(capability);
        public void AddFactionCapability(Capability capability) => FactionCapabilities.Add(capability);
        public void RemoveCapability(Capability capability) => Capabilities.Remove(capability);
        public void RemoveFactionCapability(Capability capability) => FactionCapabilities.Remove(capability);
        private List<BuildingUpgrade> _upgrades  = new();
        public List<BuildingUpgrade> Upgrades
        {
            get => _upgrades;
            set
            {
                _upgrades = value;
                NotifyPropertyChanged();
            }
        }
        public int UpgradeCount => Upgrades.Count;
        public void AddUpgrade(BuildingUpgrade upgrade) => Upgrades.Add(upgrade);
        public void RemoveUpgrade(BuildingUpgrade upgrade) => Upgrades.Remove(upgrade);
        

    }

    public class BuildingUpgrade : GameType
    {
        public string Name
        {
            get => _name;
            set
            {
                AddChange("Name", _name, value);
                _name = value;
                NotifyPropertyChanged();
            }
        }

        private string _condition = "";

        public string Condition
        {
            get => _condition;
            set
            {
                AddChange("Condition", _condition, value);
                _condition = value;
                NotifyPropertyChanged();
            }
        }
    }

    public class Capability : GameType
    {
        private int _value = 0;
        private bool _bonus = false;
        private string _agentType  = "";
        private string _condition = "";
        private string _unit = "";
        private double _initialPool= 0;
        private double _replenishmentRate = 0;
        private double _maximumPool  = 0;
        private int _startingExperience = 0;
        
        public string Type
        {
            get => _name;
            set
            {
                AddChange("Type", _name, value);
                _name = value;
                NotifyPropertyChanged();
            }
        }

        public int Value
        {
            get => _value;
            set 
            {
                AddChange("Value", _value, value);
                _value = value;
                NotifyPropertyChanged();
            }
        }
        
        public bool Bonus
        {
            get => _bonus;
            set
            {
                AddChange("Bonus", _bonus, value);
                _bonus = value;
                NotifyPropertyChanged();
            }
        }
        
        public string AgentType
        {
            get => _agentType;
            set
            {
                AddChange("AgentType", _agentType, value);
                _agentType = value;
                NotifyPropertyChanged();
            }
        }
        
        public string Unit
        {
            get => _unit;
            set
            {
                AddChange("Unit", _unit, value);
                _unit = value;
                NotifyPropertyChanged();
            }
        }
        
        public double InitialPool
        {
            get => _initialPool;
            set
            {
                AddChange("InitialPool", _initialPool, value);
                _initialPool = value;
                NotifyPropertyChanged();
            }
        }
        
        public double ReplenishmentRate
        {
            get => _replenishmentRate;
            set
            {
                AddChange("ReplenishmentRate", _replenishmentRate, value);
                _replenishmentRate = value;
                NotifyPropertyChanged();
            }
        }
        
        public double MaximumPool
        {
            get => _maximumPool;
            set
            {
                AddChange("MaximumPool", _maximumPool, value);
                _maximumPool = value;
                NotifyPropertyChanged();
            }
        }
        
        public int StartingExperience
        {
            get => _startingExperience;
            set
            {
                AddChange("StartingExperience", _startingExperience, value);
                _startingExperience = value;
                NotifyPropertyChanged();
            }
        }
        
        public string Condition
        {
            get => _condition;
            set
            {
                AddChange("Condition", _condition, value);
                _condition = value;
                NotifyPropertyChanged();
            }
        }
        
    }
    
    public class Plugin
    {
        public List<string> factions { get; set; } = new();
        public List<string> levels { get; set; } = new();

    }
}
