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

        public static Dictionary<string, string> expandedEntries = new Dictionary<string, string>();

        public static string modPath = "D:\\Fynn\\Steam\\steamapps\\common\\Medieval II Total War\\mods\\Divide_and_Conquer";


        public static void parseSMFactions()
        {
            parseExpanded();
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
                    }
                    first = true;
                    faction = new Faction();
                }
                assignFields(faction, parts);


            }
            allFactions.Add(faction.Name, faction);
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
    }
}
