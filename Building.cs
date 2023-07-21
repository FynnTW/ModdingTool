using System.Collections.Generic;

namespace ModdingTool
{
    public class Building
    {
        public string name { get; set; } = "";
        public bool isTemple { get; set; } = false;
        public bool isHinterland { get; set; } = false;
        public bool isHinterlandFarms { get; set; } = false;
        public bool isGuild { get; set; } = false;
        public bool isConvert { get; set; } = false;
        public string convert_to { get; set; } = "";
        public string religion { get; set; } = "";
        public List<string> factions { get; set; } = new();
        public string classification { get; set; } = "";
        public List<BuildingLevel> levels { get; set; } = new();
        public List<Plugin> plugins { get; set; } = new();

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
