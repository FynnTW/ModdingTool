using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using static ModdingTool.Globals;
using static ModdingTool.ParseHelpers;

namespace ModdingTool
{
    internal class FactionParser
    {
        //Parsing line number used for error logging
        private static int _lineNum = 0;

        public static void ParseSmFactions()
        {
            Console.WriteLine(@"start parse descr_sm_factions");

            //Try read sm_factions file
            var lines = FileReader("\\data\\descr_sm_factions.txt", "descr_sm_factions.txt", Encoding.Default);
            if (lines == null) { return; } //something very wrong if you hit this

            //Reset line counter
            _lineNum = 0;

            //Initialize Global Factions Database
            FactionDataBase = new Dictionary<string, Faction>();

            //Create new faction for first entry
            var faction = new Faction();
            var first = true;

            //Loop through lines
            foreach (var line in lines)
            {
                //Increase line counter
                _lineNum++;

                //Remove Comments and Faulty lines
                var newline = CleanLine(line);
                if (string.IsNullOrWhiteSpace(newline))
                {
                    continue;
                }

                //Split line into parts
                var parts = LineSplitterFactions(newline);
                if (parts.Length < 2)
                {
                    //Should be something wrong with line if you hit this
                    ErrorDB.AddError("Unrecognized content", _lineNum.ToString(), "descr_sm_factions.txt");
                    continue;
                }

                //Entry is completed, next entry is starting
                if (parts[0].Equals("faction"))
                {
                    //Add new faction if it is not the first time we hit this
                    if (!first)
                    {
                        AddFaction(faction);
                    }
                    first = false;
                    faction = new Faction();
                }

                //Fill out the faction field the line is about
                AssignFields(faction, parts);
            }

            //Add last faction
            AddFaction(faction);

            //Reset Line Counter
            Console.WriteLine(@"end parse descr_sm_factions");
            _lineNum = 0;
        }

        private static void AddFaction(Faction fac)
        {
            FactionDataBase.Add(fac.Name, fac);
            Console.WriteLine(fac.Name);
            CultureDataBase[fac.Culture].Factions.Add(fac.Name);
        }

        public static void ParseCultures()
        {
            Console.WriteLine(@"start parse descr_cultures");

            //Try read descr_cultures file
            var lines = FileReader("\\data\\descr_cultures.txt", "descr_cultures.txt", Encoding.Default);
            if (lines == null) { return; } //something very wrong if you hit this

            //Reset line counter
            _lineNum = 0;

            //Initialize Global Cultures Database
            CultureDataBase = new Dictionary<string, Culture>();

            //Create new culture for first entry
            var culture = new Culture();
            var first = true;

            //Loop through lines
            foreach (var line in lines)
            {
                //Increase line counter
                _lineNum++;

                //Remove Comments and Faulty lines
                //This file has valid line with only brackets so remove them too
                var newline = line.Replace("}", "");
                newline = newline.Replace("{", "").Trim();
                newline = CleanLine(newline);
                if (string.IsNullOrWhiteSpace(newline))
                {
                    continue;
                }

                //Split line into parts
                var parts = LineSplitterCultures(newline);
                if (parts.Length < 1)
                {
                    //Should be something wrong with line if you hit this
                    ErrorDB.AddError("Unrecognized content", _lineNum.ToString(), "descr_cultures.txt");
                    continue;
                }

                //Entry is completed, next entry is starting
                if (parts[0].Equals("culture"))
                {
                    //Add new culture if it is not the first time we hit this
                    if (!first)
                    {
                        AddCulture(culture);
                    }
                    first = false;
                    culture = new Culture();
                }

                //Fill out the culture field the line is about
                AssignCultureFields(culture, parts);
            }

            //Add last culture
            AddCulture(culture);

            //Reset Line Counter
            Console.WriteLine(@"end parse descr_cultures");
            _lineNum = 0;
        }

        private static void AddCulture(Culture culture)
        {
            CultureDataBase.Add(culture.Name, culture);
            Console.WriteLine(culture.Name);
        }

        public static void ParseExpanded()
        {
            Console.WriteLine(@"start parse expanded");

            //Try read expanded file
            var lines = FileReader("\\data\\text\\expanded.txt", "expanded.txt", Encoding.Unicode);
            if (lines == null) { return; }

            //Reset line counter
            _lineNum = 0;

            //Initialize Global Expanded Text Entries Database
            ExpandedEntries = new Dictionary<string, string>();

            //Loop through lines
            foreach (var line in lines)
            {
                //Increase line counter
                _lineNum++;
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                if (line.StartsWith('¬'))
                {
                    continue;
                }
                if (!line.Contains('{') || !line.Contains('}'))
                {
                    ErrorDB.AddError("Unrecognized content", _lineNum.ToString(), "expanded.txt");
                    continue;
                }
                var newLine = line.Trim();
                var parts = CurlySplitter(newLine);
                AddExpandedEntry(parts);
            }
            Console.WriteLine(@"end parse expanded");
            _lineNum = 0;
        }

        private static void AddExpandedEntry(IReadOnlyList<string> parts)
        {
            var identifier = parts[0];

            if (parts.Count > 1)
            {
                ExpandedEntries[identifier.ToLower()] = parts[1];
            }
            else
            {
                ErrorDB.AddError("Warning entry has no localization", _lineNum.ToString(), "expanded.txt");
                ExpandedEntries[identifier.ToLower()] = "";
            }
            Console.WriteLine(@identifier.ToLower());
        }

        private static string[] LineSplitterFactions(string line)
        {
            char[] delimiters = { ',' };
            var lineParts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            char[] delimitersWhite = { ' ', '\t' };
            var firstParts = lineParts[0].Split(delimitersWhite, 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            lineParts = firstParts.Concat(lineParts[1..]).ToArray();
            return lineParts;
        }

        private static string[] LineSplitterCultures(string line)
        {
            char[] delimiters = { ',', '}', '{' };
            var lineParts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            char[] delimitersWhite = { ' ', '\t' };
            if (lineParts.Length == 0)
            {
                return lineParts;
            }
            var firstParts = lineParts[0].Split(delimitersWhite, 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            lineParts = firstParts.Concat(lineParts[1..]).ToArray();
            return lineParts;
        }

        private static void AssignFields(Faction faction, string[] parts)
        {
            var identifier = parts[0];

            var x = parts[1];
            char[] delimitersWhite = { ' ', '\t' };

            switch (identifier)
            {
                case "faction":
                    faction.Name = x;
                    if (parts.Length > 2)
                    {
                        var id = parts[2];
                        switch (id)
                        {
                            case "spawned_on_event":
                                faction.Spawned_on_event = true;
                                faction.Spawn_modifier = id;
                                break;

                            case "shadowed_by":
                                faction.Spawn_faction = parts[3];
                                faction.Spawn_modifier = id;
                                break;

                            case "shadowing":
                                faction.Spawn_faction = parts[3];
                                faction.Spawn_modifier = id;
                                break;

                            case "spawns_on_revolt":
                                faction.Spawn_faction = parts[3];
                                faction.Spawn_modifier = id;
                                break;

                            case "spawned_by":
                                faction.Spawn_faction = parts[3];
                                faction.Spawn_modifier = id;
                                break;
                        }
                    }
                    faction.LocalizedName = ExpandedEntries[x];
                    break;

                case "culture":
                    faction.Culture = x;
                    break;

                case "special_faction_type":
                    if (x.Equals("slave_faction"))
                    {
                        faction.Slave_faction = true;
                        break;
                    }
                    if (x.Equals("papal_faction"))
                    {
                        faction.Papal_faction = true;
                        break;
                    }
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
                    faction.Primary_colourR = int.Parse(x.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1]);
                    faction.Primary_colourG = int.Parse(parts[2].Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1]);
                    faction.Primary_colourB = int.Parse(parts[3].Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1]);
                    break;

                case "secondary_colour":
                    faction.Secondary_colourR = int.Parse(x.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1]);
                    faction.Secondary_colourG = int.Parse(parts[2].Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1]);
                    faction.Secondary_colourB = int.Parse(parts[3].Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1]);
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

                case "periods_unavailable_in_custom_battle":
                    faction.Periods_unavailable_in_custom_battle = parts[1..];
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

        private static string? _settlementLevel;

        private static void AssignCultureFields(Culture culture, string[] parts)
        {
            var identifier = parts[0];

            var x = "";

            if (parts.Length > 1)
            {
                x = parts[1];
            }

            char[] delimitersWhite = { ' ', '\t' };

            switch (identifier)
            {
                case "culture":
                    culture.Name = x;
                    culture.LocalizedName = ExpandedEntries[x.ToLower()];
                    break;

                case "portrait_mapping":
                    culture.Portrait_mapping = x;
                    break;

                case "rebel_standard_index":
                    culture.Rebel_standard_index = int.Parse(x);
                    break;

                case "village":
                    _settlementLevel = "village";
                    break;

                case "moot_and_bailey":
                    _settlementLevel = "moot_and_bailey";
                    break;

                case "town":
                    _settlementLevel = "town";
                    break;

                case "wooden_castle":
                    _settlementLevel = "wooden_castle";
                    break;

                case "large_town":
                    _settlementLevel = "large_town";
                    break;

                case "castle":
                    _settlementLevel = "castle";
                    break;

                case "city":
                    _settlementLevel = "city";
                    break;

                case "fortress":
                    _settlementLevel = "fortress";
                    break;

                case "large_city":
                    _settlementLevel = "large_city";
                    break;

                case "citadel":
                    _settlementLevel = "citadel";
                    break;

                case "huge_city":
                    _settlementLevel = "huge_city";
                    break;

                case "normal":
                    if (parts.Length < 3)
                    {
                        var newparts = parts[1].Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        parts = parts.Concat(newparts).ToArray();
                    }
                    switch (_settlementLevel)
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
                    switch (_settlementLevel)
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

                case "fort_wall":
                    culture.FortWalls = x;
                    break;

                case "fishing_village":
                    culture.FishingVillage = x;
                    culture.FishingVillageBase = parts[2];
                    break;

                case "port_land":
                    if (string.IsNullOrWhiteSpace(culture.FishingPort1))
                    {
                        culture.FishingPort1 = x;
                        culture.FishingPort1Base = parts[2];
                    }
                    else if (string.IsNullOrWhiteSpace(culture.FishingPort2))
                    {
                        culture.FishingPort2 = x;
                        culture.FishingPort2Base = parts[2];
                    }
                    else
                    {
                        culture.FishingPort3 = x;
                        culture.FishingPort3Base = parts[2];
                    }
                    break;

                case "port_sea":
                    if (string.IsNullOrWhiteSpace(culture.FishingPort1Sea))
                    {
                        culture.FishingPort1Sea = x;
                    }
                    else if (string.IsNullOrWhiteSpace(culture.FishingPort2Sea))
                    {
                        culture.FishingPort2Sea = x;
                    }
                    else
                    {
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