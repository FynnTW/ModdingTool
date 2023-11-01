using System.Collections.Generic;

namespace ModdingTool
{
    public class Culture
    {
        private string name;
        private string portrait_mapping;
        private int rebel_standard_index;
        private cultureSett village = new cultureSett();
        private cultureSett moot_and_bailey = new cultureSett();
        private cultureSett town = new cultureSett();
        private cultureSett wooden_castle = new cultureSett();
        private cultureSett large_town = new cultureSett();
        private cultureSett castle = new cultureSett();
        private cultureSett city = new cultureSett();
        private cultureSett fortress = new cultureSett();
        private cultureSett large_city = new cultureSett();
        private cultureSett citadel = new cultureSett();
        private cultureSett huge_city = new cultureSett();
        private string fort;
        private string fortBase;
        private int fortCost;
        private string fortWalls;
        private string fishingVillage;
        private string fishingVillageBase;
        private string fishingPort1;
        private string fishingPort1Sea;
        private string fishingPort1Base;
        private string fishingPort2;
        private string fishingPort2Sea;
        private string fishingPort2Base;
        private string fishingPort3;
        private string fishingPort3Sea;
        private string fishingPort3Base;
        private string watchtower;
        private string watchtowerBase;
        private int watchtowerCost;
        private cultureAgent spy = new cultureAgent();
        private cultureAgent assassin = new cultureAgent();
        private cultureAgent diplomat = new cultureAgent();
        private cultureAgent admiral = new cultureAgent();
        private cultureAgent merchant = new cultureAgent();
        private cultureAgent priest = new cultureAgent();
        private List<string> factions = new List<string>();
        private string localizedName;

        public string Name { get => name; set => name = value; }
        public string Portrait_mapping { get => portrait_mapping; set => portrait_mapping = value; }
        public int Rebel_standard_index { get => rebel_standard_index; set => rebel_standard_index = value; }
        public cultureSett Village { get => village; set => village = value; }
        public cultureSett Moot_and_bailey { get => moot_and_bailey; set => moot_and_bailey = value; }
        public cultureSett Town { get => town; set => town = value; }
        public cultureSett Wooden_castle { get => wooden_castle; set => wooden_castle = value; }
        public cultureSett Large_town { get => large_town; set => large_town = value; }
        public cultureSett Castle { get => castle; set => castle = value; }
        public cultureSett City { get => city; set => city = value; }
        public cultureSett Fortress { get => fortress; set => fortress = value; }
        public cultureSett Large_city { get => large_city; set => large_city = value; }
        public cultureSett Citadel { get => citadel; set => citadel = value; }
        public cultureSett Huge_city { get => huge_city; set => huge_city = value; }
        public string Fort { get => fort; set => fort = value; }
        public string FortBase { get => fortBase; set => fortBase = value; }
        public int FortCost { get => fortCost; set => fortCost = value; }
        public string FortWalls { get => fortWalls; set => fortWalls = value; }
        public string FishingVillage { get => fishingVillage; set => fishingVillage = value; }
        public string FishingVillageBase { get => fishingVillageBase; set => fishingVillageBase = value; }
        public string FishingPort1 { get => fishingPort1; set => fishingPort1 = value; }
        public string FishingPort1Base { get => fishingPort1Base; set => fishingPort1Base = value; }
        public string FishingPort2 { get => fishingPort2; set => fishingPort2 = value; }
        public string FishingPort2Base { get => fishingPort2Base; set => fishingPort2Base = value; }
        public string FishingPort3 { get => fishingPort3; set => fishingPort3 = value; }
        public string FishingPort3Base { get => fishingPort3Base; set => fishingPort3Base = value; }
        public string Watchtower { get => watchtower; set => watchtower = value; }
        public string WatchtowerBase { get => watchtowerBase; set => watchtowerBase = value; }
        public int WatchtowerCost { get => watchtowerCost; set => watchtowerCost = value; }
        public cultureAgent Spy { get => spy; set => spy = value; }
        public cultureAgent Assassin { get => assassin; set => assassin = value; }
        public cultureAgent Diplomat { get => diplomat; set => diplomat = value; }
        public cultureAgent Admiral { get => admiral; set => admiral = value; }
        public cultureAgent Merchant { get => merchant; set => merchant = value; }
        public cultureAgent Priest { get => priest; set => priest = value; }
        public string FishingPort1Sea { get => fishingPort1Sea; set => fishingPort1Sea = value; }
        public string FishingPort2Sea { get => fishingPort2Sea; set => fishingPort2Sea = value; }
        public string FishingPort3Sea { get => fishingPort3Sea; set => fishingPort3Sea = value; }
        internal List<string> Factions { get => factions; set => factions = value; }
        public string LocalizedName { get => localizedName; set => localizedName = value; }
    }

    public class cultureAgent
    {
        private string type;
        private string card;
        private string info_card;
        private int cost;
        private int population_cost;
        private int recruitment_points;

        public string Type { get => type; set => type = value; }
        public string Card { get => card; set => card = value; }
        public string Info_card { get => info_card; set => info_card = value; }
        public int Cost { get => cost; set => cost = value; }
        public int Population_cost { get => population_cost; set => population_cost = value; }
        public int Recruitment_points { get => recruitment_points; set => recruitment_points = value; }
    }

    public class cultureSett
    {
        private string model;
        private string card;
        private string aerial_base;

        public string Model { get => model; set => model = value; }
        public string Card { get => card; set => card = value; }
        public string Aerial_base { get => aerial_base; set => aerial_base = value; }
    }
}