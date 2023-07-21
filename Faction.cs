using System.Collections.Generic;

namespace ModdingTool
{
    public class Faction
    {
        private string name;
        private string culture;
        private Culture cultureLink;
        private string religion;
        private string symbol;
        private string rebel_symbol;
        private int primary_colourR;
        private int primary_colourG;
        private int primary_colourB;
        private int secondary_colourR;
        private int secondary_colourG;
        private int secondary_colourB;
        private string loading_logo;
        private string standard_index;
        private string logo_index;
        private string small_logo_index;
        private int triumph_value;
        private bool custom_battle_availability;
        private bool can_sap;
        private bool prefers_naval_invasions;
        private bool can_have_princess;
        private bool disband_to_pools = true;
        private bool can_build_siege_towers = true;
        private bool can_transmit_plague = true;
        private string has_family_tree;
        private int horde_min_units;
        private int horde_max_units;
        private int horde_max_units_reduction_every_horde;
        private int horde_unit_per_settlement_population;
        private int horde_min_named_characters;
        private int horde_max_percent_army_stack;
        private int horde_disband_percent_on_settlement_capture;
        private List<string> horde_units = new List<string>();
        private List<string> unit_ownership = new List<string>();
        private string logo;
        private string localizedName;
        private bool spawned_on_event = false;
        private string spawn_modifier;
        private string spawn_faction;
        private bool slave_faction;
        private bool papal_faction;
        private string[] periods_unavailable_in_custom_battle;

        public string Name { get => name; set => name = value; }
        public string Culture { get => culture; set => culture = value; }
        public string Religion { get => religion; set => religion = value; }
        public string Symbol { get => symbol; set => symbol = value; }
        public string Rebel_symbol { get => rebel_symbol; set => rebel_symbol = value; }
        public string Loading_logo { get => loading_logo; set => loading_logo = value; }
        public string Standard_index { get => standard_index; set => standard_index = value; }
        public string Logo_index { get => logo_index; set => logo_index = value; }
        public string Small_logo_index { get => small_logo_index; set => small_logo_index = value; }
        public int Triumph_value { get => triumph_value; set => triumph_value = value; }
        public bool Custom_battle_availability { get => custom_battle_availability; set => custom_battle_availability = value; }
        public bool Can_sap { get => can_sap; set => can_sap = value; }
        public bool Prefers_naval_invasions { get => prefers_naval_invasions; set => prefers_naval_invasions = value; }
        public bool Can_have_princess { get => can_have_princess; set => can_have_princess = value; }
        public bool Disband_to_pools { get => disband_to_pools; set => disband_to_pools = value; }
        public bool Can_build_siege_towers { get => can_build_siege_towers; set => can_build_siege_towers = value; }
        public bool Can_transmit_plague { get => can_transmit_plague; set => can_transmit_plague = value; }
        public string Has_family_tree { get => has_family_tree; set => has_family_tree = value; }
        public int Horde_min_units { get => horde_min_units; set => horde_min_units = value; }
        public int Horde_max_units { get => horde_max_units; set => horde_max_units = value; }
        public int Horde_max_units_reduction_every_horde { get => horde_max_units_reduction_every_horde; set => horde_max_units_reduction_every_horde = value; }
        public int Horde_unit_per_settlement_population { get => horde_unit_per_settlement_population; set => horde_unit_per_settlement_population = value; }
        public int Horde_min_named_characters { get => horde_min_named_characters; set => horde_min_named_characters = value; }
        public int Horde_max_percent_army_stack { get => horde_max_percent_army_stack; set => horde_max_percent_army_stack = value; }
        public int Horde_disband_percent_on_settlement_capture { get => horde_disband_percent_on_settlement_capture; set => horde_disband_percent_on_settlement_capture = value; }
        public List<string> Horde_units { get => horde_units; set => horde_units = value; }
        public string Logo { get => logo; set => logo = value; }
        public string LocalizedName { get => localizedName; set => localizedName = value; }
        public List<string> Unit_ownership { get => unit_ownership; set => unit_ownership = value; }
        public bool Spawned_on_event { get => spawned_on_event; set => spawned_on_event = value; }
        public int Primary_colourR { get => primary_colourR; set => primary_colourR = value; }
        public int Primary_colourG { get => primary_colourG; set => primary_colourG = value; }
        public int Primary_colourB { get => primary_colourB; set => primary_colourB = value; }
        public int Secondary_colourR { get => secondary_colourR; set => secondary_colourR = value; }
        public int Secondary_colourG { get => secondary_colourG; set => secondary_colourG = value; }
        public int Secondary_colourB { get => secondary_colourB; set => secondary_colourB = value; }
        public string Spawn_modifier { get => spawn_modifier; set => spawn_modifier = value; }
        public string Spawn_faction { get => spawn_faction; set => spawn_faction = value; }
        public bool Slave_faction { get => slave_faction; set => slave_faction = value; }
        public bool Papal_faction { get => papal_faction; set => papal_faction = value; }
        public Dictionary<string, FactionCharacter> FactionCharacterTypes { get; set; } = new();
        public string[] Periods_unavailable_in_custom_battle { get => periods_unavailable_in_custom_battle; set => periods_unavailable_in_custom_battle = value; }
    }
}

public class FactionCharacter
{
    public string name;
    public string[] models = new string[11];
    public int dictionary = 2;
    public string battle_model = "";
    public string battle_equip = "gladius, chainmail shirt helmet and rectangular shield";
}