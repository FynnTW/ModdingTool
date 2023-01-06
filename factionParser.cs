using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static ModdingTool.parseHelpers;

namespace ModdingTool
{
    internal class factionParser
    {
        public static Dictionary<string, Faction> allFactions = new Dictionary<string, Faction>();

        public static Dictionary<string, Culture> allCultures= new Dictionary<string, Culture>();

        public static Dictionary<string, string> expandedEntries = new Dictionary<string, string>();

        public static string modPath = "D:\\Fynn\\Steam\\steamapps\\common\\Medieval II Total War\\mods\\Divide_and_Conquer";


        public static void parseSMFactions()
        {
            parseExpanded();
            parseCultures();
            string[] lines = File.ReadAllLines(modPath + "\\data\\descr_sm_factions.txt");

            Faction faction = new Faction();
            bool first = false;
            allFactions = new Dictionary<string, Faction>();

            foreach (string line in lines)
            {
                if (line.StartsWith(';'))
                {
                    continue;
                }
                string newline = removeComment(line);
                if (newline.Equals(""))
                {
                    continue;
                }
                newline = newline.Trim();
                string[] parts = lineSplitter(newline);
                if (parts.Length < 2)
                {
                    continue;
                }
                if (parts[0].Equals("faction"))
                {
                    if (first)
                    {
                        allFactions.Add(faction.Name, faction);
                        allCultures[faction.Culture].Factions.Add(faction.Name, faction);
                    }
                    first = true;
                    faction = new Faction();
                }
                assignFields(faction, parts);


            }
            allFactions.Add(faction.Name, faction);
            allCultures[faction.Culture].Factions.Add(faction.Name, faction);
        }
        public static void parseCultures()
        {
            string[] lines = File.ReadAllLines(modPath + "\\data\\descr_cultures.txt");

            Culture culture = new Culture();
            bool first = false;
            allCultures = new Dictionary<string, Culture>();

            foreach (string line in lines)
            {
                if (line.StartsWith(';'))
                {
                    continue;
                }
                string newline = removeComment(line);
                if (newline.Equals(""))
                {
                    continue;
                }
                newline = newline.Trim();
                string[] parts = lineSplitterCultures(newline);
                if (parts.Length < 1)
                {
                    continue;
                }
                if (parts[0].Equals("culture"))
                {
                    if (first)
                    {
                        allCultures.Add(culture.Name, culture);
                    }
                    first = true;
                    culture = new Culture();
                }
                assignCultureFields(culture, parts);


            }
            allCultures.Add(culture.Name, culture);
        }

        static public void parseExpanded()
        {
            string[] lines = File.ReadAllLines(modPath + "\\data\\text\\expanded.txt", Encoding.Unicode);
            expandedEntries = new Dictionary<string, string>();
            foreach (string line in lines)
            {
                if (line.StartsWith('¬'))
                {
                    continue;
                }
                if (!line.Contains('{') || !line.Contains('}'))
                {
                    continue;
                }
                string newLine = line.Trim();
                string[] parts = stringSplitter(newLine);
                fillDict(parts);


                //Console.WriteLine(line);
            }
        }

        public static void fillDict(string[] parts)
        {
            String identifier = parts[0];

            if (parts.Length > 1)
            {
                expandedEntries[identifier.ToLower()] = parts[1];
            } else
            {
                expandedEntries[identifier.ToLower()] = "";
            }

        }

        public static string[] lineSplitter(string line)
        {

            char[] delimiters = { ',' };
            string[] lineParts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            char[] delimitersWhite = { ' ', '\t' };
            string[] firstParts = lineParts[0].Split(delimitersWhite, 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            lineParts = firstParts.Concat(lineParts[1..]).ToArray();
            return lineParts;
        }

        public static string[] lineSplitterCultures(string line)
        {

            char[] delimiters = { ',', '}', '{' };
            string[] lineParts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            char[] delimitersWhite = { ' ', '\t' };
            if (lineParts.Length == 0)
            {
                return lineParts;
            }
            string[] firstParts = lineParts[0].Split(delimitersWhite, 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            lineParts = firstParts.Concat(lineParts[1..]).ToArray();
            return lineParts;
        }

        public static void assignFields(Faction faction, string[] parts)
        {
            String identifier = parts[0];

            String x = parts[1];

            switch (identifier)
            {
                case "faction":
                    faction.Name = x;
                    if (parts.Length > 2 && parts[2].Equals("spawned_on_event"))
                    {
                        faction.Spawned_on_event = true;
                    }
                    faction.LocalizedName = expandedEntries[x];
                    break;
                case "culture":
                    faction.Culture = x;
                    break;
                case "religion":
                    faction.Religion = x;
                    break;
                case "symbol":
                    faction.Symbol = x;
                    break;
                case "rebel_symbol":
                    faction.Rebel_symbol = x;
                    break;
                case "primary_colour":
                    faction.Primary_colour = x;
                    break;
                case "secondary_colour":
                    faction.Secondary_colour = x;
                    break;
                case "loading_logo":
                    faction.Loading_logo = x;
                    break;
                case "standard_index":
                    faction.Standard_index = x;
                    break;
                case "logo_index":
                    faction.Logo_index = x;
                    break;
                case "small_logo_index":
                    faction.Small_logo_index = x;
                    break;
                case "triumph_value":
                    faction.Triumph_value = int.Parse(x);
                    break;
                case "custom_battle_availability":
                    faction.Custom_battle_availability = ToBool(x);
                    break;
                case "can_sap":
                    faction.Can_sap = ToBool(x);
                    break;
                case "prefers_naval_invasions":
                    faction.Prefers_naval_invasions = ToBool(x);
                    break;
                case "can_have_princess":
                    faction.Can_have_princess = ToBool(x);
                    break;
                case "disband_to_pools":
                    faction.Disband_to_pools = ToBool(x);
                    break;
                case "can_build_siege_towers":
                    faction.Can_build_siege_towers = ToBool(x);
                    break;
                case "can_transmit_plague":
                    faction.Can_transmit_plague = ToBool(x);
                    break;
                case "has_family_tree":
                    faction.Has_family_tree = x;
                    break;
                case "horde_min_units":
                    faction.Horde_min_units = int.Parse(x);
                    break;
                case "horde_max_units":
                    faction.Horde_max_units = int.Parse(x);
                    break;
                case "horde_max_units_reduction_every_horde":
                    faction.Horde_max_units_reduction_every_horde = int.Parse(x);
                    break;
                case "horde_unit_per_settlement_population":
                    faction.Horde_unit_per_settlement_population = int.Parse(x);
                    break;
                case "horde_min_named_characters":
                    faction.Horde_min_named_characters = int.Parse(x);
                    break;
                case "horde_max_percent_army_stack":
                    faction.Horde_max_percent_army_stack = int.Parse(x);
                    break;
                case "horde_disband_percent_on_settlement_capture":
                    faction.Horde_disband_percent_on_settlement_capture = int.Parse(x);
                    break;
                case "horde_unit":
                    faction.Horde_units.Add(x);
                    break;
            }

        }

        private static string settlement_level;

        public static void assignCultureFields(Culture culture, string[] parts)
        {
            String identifier = parts[0];

            String x = "";

            if (parts.Length > 1)
            {
                x = parts[1];
            }

            char[] delimitersWhite = { ' ', '\t' };

            switch (identifier)
            {
                case "culture":
                    culture.Name = x;
                    culture.LocalizedName = expandedEntries[x.ToLower()];
                    break;
                case "portrait_mapping":
                    culture.Portrait_mapping = x;
                    break;
                case "rebel_standard_index":
                    culture.Rebel_standard_index = int.Parse(x);
                    break;
                case "village":
                    settlement_level = "village";
                    break;
                case "moot_and_bailey":
                    settlement_level = "moot_and_bailey";
                    break;
                case "town":
                    settlement_level = "town";
                    break;
                case "wooden_castle":
                    settlement_level = "wooden_castle";
                    break;
                case "large_town":
                    settlement_level = "large_town";
                    break;
                case "castle":
                    settlement_level = "castle";
                    break;
                case "city":
                    settlement_level = "city";
                    break;
                case "fortress":
                    settlement_level = "fortress";
                    break;
                case "large_city":
                    settlement_level = "large_city";
                    break;
                case "citadel":
                    settlement_level = "citadel";
                    break;
                case "huge_city":
                    settlement_level = "huge_city";
                    break;
                case "normal":
                    switch(settlement_level)
                    {
                        case "village":
                            culture.Village.Model = x;
                            culture.Village.Aerial_base = parts[2];
                            break;
                        case "moot_and_bailey":
                            culture.Moot_and_bailey.Model = x;
                            culture.Moot_and_bailey.Aerial_base = parts[2];
                            break;
                        case "town":
                            culture.Town.Model = x;
                            culture.Town.Aerial_base = parts[2];
                            break;
                        case "wooden_castle":
                            culture.Wooden_castle.Model = x;
                            culture.Wooden_castle.Aerial_base = parts[2];
                            break;
                        case "large_town":
                            culture.Large_town.Model = x;
                            culture.Large_town.Aerial_base = parts[2];
                            break;
                        case "castle":
                            culture.Castle.Model = x;
                            culture.Castle.Aerial_base = parts[2];
                            break;
                        case "city":
                            culture.City.Model = x;
                            culture.City.Aerial_base = parts[2];
                            break;
                        case "fortress":
                            culture.Fortress.Model = x;
                            culture.Fortress.Aerial_base = parts[2];
                            break;
                        case "large_city":
                            culture.Large_city.Model = x;
                            culture.Large_city.Aerial_base = parts[2];
                            break;
                        case "citadel":
                            culture.Citadel.Model = x;
                            culture.Citadel.Aerial_base = parts[2];
                            break;
                        case "huge_city":
                            culture.Huge_city.Model = x;
                            culture.Huge_city.Aerial_base = parts[2];
                            break;
                    }
                    break;
                case "card":
                    switch(settlement_level)
                    {
                        case "village":
                            culture.Village.Card = x;
                            break;
                        case "moot_and_bailey":
                            culture.Moot_and_bailey.Card = x;
                            break;
                        case "town":
                            culture.Town.Card = x;
                            break;
                        case "wooden_castle":
                            culture.Wooden_castle.Card = x;
                            break;
                        case "large_town":
                            culture.Large_town.Card = x;
                            break;
                        case "castle":
                            culture.Castle.Card = x;
                            break;
                        case "city":
                            culture.City.Card = x;
                            break;
                        case "fortress":
                            culture.Fortress.Card = x;
                            break;
                        case "large_city":
                            culture.Large_city.Card = x;
                            break;
                        case "citadel":
                            culture.Citadel.Card = x;
                            break;
                        case "huge_city":
                            culture.Huge_city.Card = x;
                            break;
                    }
                    break;
                case "fort":
                    culture.Fort = x;
                    culture.FortBase = parts[2];
                    break;
                case "fort_cost":
                    culture.FortCost = int.Parse(x);
                    break;
                case "fishing_village":
                    culture.FishingVillage = x;
                    culture.FishingVillageBase = parts[2];
                    break;
                case "port_land":
                    if (culture.FishingPort1 == null)
                    {
                        culture.FishingPort1 = x;
                        culture.FishingPort1Base = parts[2];
                    } else if (culture.FishingPort2 == null)
                    {
                        culture.FishingPort2 = x;
                        culture.FishingPort2Base = parts[2];
                    }
                    else {
                        culture.FishingPort3 = x;
                        culture.FishingPort3Base = parts[2];
                    }
                    break;
                case "port_sea":
                    if (culture.FishingPort1Sea == null)
                    {
                        culture.FishingPort1Sea = x;
                    } else if (culture.FishingPort2Sea == null)
                    {
                        culture.FishingPort2Sea = x;
                    }
                    else {
                        culture.FishingPort3Sea = x;
                    }
                    break;
                case "watchtower":
                    culture.Watchtower = x;
                    culture.WatchtowerBase = parts[2];
                    break;
                case "watchtower_cost":
                    culture.WatchtowerCost = int.Parse(x);
                    break;
                case "spy":
                    parts = x.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    x = parts[0];
                    culture.Spy.Card = x;
                    culture.Spy.Info_card = parts[1];
                    culture.Spy.Cost = int.Parse(parts[3]);
                    culture.Spy.Population_cost = int.Parse(parts[4]);
                    culture.Spy.Recruitment_points = int.Parse(parts[5]);
                    break;
                case "assassin":
                    parts = x.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    x = parts[0];
                    culture.Assassin.Card = x;
                    culture.Assassin.Info_card = parts[1];
                    culture.Assassin.Cost = int.Parse(parts[3]);
                    culture.Assassin.Population_cost = int.Parse(parts[4]);
                    culture.Assassin.Recruitment_points = int.Parse(parts[5]);
                    break;
                case "diplomat":
                    parts = x.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    x = parts[0];
                    culture.Diplomat.Card = x;
                    culture.Diplomat.Info_card = parts[1];
                    culture.Diplomat.Cost = int.Parse(parts[3]);
                    culture.Diplomat.Population_cost = int.Parse(parts[4]);
                    culture.Diplomat.Recruitment_points = int.Parse(parts[5]);
                    break;
                case "admiral":
                    parts = x.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    x = parts[0];
                    culture.Admiral.Card = x;
                    culture.Admiral.Info_card = parts[1];
                    culture.Admiral.Cost = int.Parse(parts[3]);
                    culture.Admiral.Population_cost = int.Parse(parts[4]);
                    culture.Admiral.Recruitment_points = int.Parse(parts[5]);
                    break;
                case "merchant":
                    parts = x.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    x = parts[0];
                    culture.Merchant.Card = x;
                    culture.Merchant.Info_card = parts[1];
                    culture.Merchant.Cost = int.Parse(parts[3]);
                    culture.Merchant.Population_cost = int.Parse(parts[4]);
                    culture.Merchant.Recruitment_points = int.Parse(parts[5]);
                    break;
                case "priest":
                    parts = x.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    x = parts[0];
                    culture.Priest.Card = x;
                    culture.Priest.Info_card = parts[1];
                    culture.Priest.Cost = int.Parse(parts[3]);
                    culture.Priest.Population_cost = int.Parse(parts[4]);
                    culture.Priest.Recruitment_points = int.Parse(parts[5]);
                    break;
            }

        }
    }
}
