using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ModdingTool.Globals;
using static ModdingTool.ParseHelpers;

namespace ModdingTool
{
    internal class FactionParser
    {
        //Parsing line number and file name used for error logging
        private static int _lineNum;
        private static string _fileName = "";

        public static void ParseSmFactions()
        {
            _fileName = "descr_sm_factions.txt";
            Console.WriteLine($@"start parse {_fileName}");

            //Try read sm_factions file
            var lines = FileReader("\\data\\descr_sm_factions.txt", _fileName, Encoding.Default);
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
                    continue;

                //Split line into parts
                var parts = LineSplitterFactions(newline);
                if (parts.Length < 2)
                {
                    //Should be something wrong with line if you hit this
                    ErrorDb.AddError("Unrecognized content", _lineNum.ToString(), _fileName);
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
            Console.WriteLine($@"end parse {_fileName}");
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
            _fileName = "descr_cultures.txt";
            Console.WriteLine($@"start parse {_fileName}");

            //Try read descr_cultures file
            var lines = FileReader("\\data\\descr_cultures.txt", _fileName, Encoding.Default);
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
                    ErrorDb.AddError("Unrecognized content", _lineNum.ToString(), _fileName);
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
            Console.WriteLine($@"end parse {_fileName}");
            _lineNum = 0;
        }

        private static void AddCulture(Culture culture)
        {
            CultureDataBase.Add(culture.Name, culture);
            Console.WriteLine(culture.Name);
        }

        public static void ParseExpanded()
        {
            _fileName = "expanded.txt";
            Console.WriteLine($@"start parse {_fileName}");

            //Try read expanded file
            var lines = FileReader("\\data\\text\\expanded.txt", _fileName, Encoding.Unicode);
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
                //Clean up line
                var parts = LocalTextLineCleaner(line, _lineNum, _fileName);
                //Add entry
                if (parts != null)
                    AddExpandedEntry(parts);
            }

            //Reset Line Counter
            Console.WriteLine($@"end parse {_fileName}");
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
                ErrorDb.AddError("Warning entry has no localization", _lineNum.ToString(), _fileName);
                ExpandedEntries[identifier.ToLower()] = "";
            }
            Console.WriteLine(@identifier.ToLower());
        }

        private static string[] LineSplitterFactions(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                ErrorDb.AddError("Warning empty line send on", _lineNum.ToString(), _fileName);
                return Array.Empty<string>();
            }

            //split by comma
            char[] delimiters = { ',' };
            var lineParts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            //Make sure there are no empty entries
            if (lineParts.Length == 0)
            {
                return lineParts;
            }

            //split first part by white space (no comma between identifier and value)
            char[] delimitersWhite = { ' ', '\t' };
            var firstParts = lineParts[0].Split(delimitersWhite, 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            //concat first part with rest of line
            lineParts = firstParts.Concat(lineParts[1..]).ToArray();
            return lineParts;
        }

        private static string[] LineSplitterCultures(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                ErrorDb.AddError("Warning empty line send on", _lineNum.ToString(), _fileName);
                return Array.Empty<string>();
            }

            //split by either comma or curly bracket (for some reason used in descr_cultures.txt)
            char[] delimiters = { ',', '}', '{' };
            var lineParts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            //Make sure there are no empty entries
            if (lineParts.Length == 0)
            {
                return lineParts;
            }

            //split first part by white space (no comma between identifier and value)
            char[] delimitersWhite = { ' ', '\t' };
            var firstParts = lineParts[0].Split(delimitersWhite, 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            //concat first part with rest of line
            lineParts = firstParts.Concat(lineParts[1..]).ToArray();
            return lineParts;
        }

        private static void AssignFields(Faction faction, string[] parts)
        {
            //name of the field
            var identifier = parts[0];

            //first value, usually all needed
            var value = parts[1];

            char[] delimitersWhite = { ' ', '\t' };

            try
            {
                switch (identifier)
                {
                    case "faction":
                        faction.Name = value;
                        if (parts.Length > 2)
                        {
                            var id = parts[2];
                            switch (id)
                            {
                                case "spawned_on_event":
                                    faction.SpawnedOnEvent = true;
                                    faction.SpawnModifier = id;
                                    break;

                                case "shadowed_by":
                                    faction.SpawnFaction = parts[3];
                                    faction.SpawnModifier = id;
                                    break;

                                case "shadowing":
                                    faction.SpawnFaction = parts[3];
                                    faction.SpawnModifier = id;
                                    break;

                                case "spawns_on_revolt":
                                    faction.SpawnFaction = parts[3];
                                    faction.SpawnModifier = id;
                                    break;

                                case "spawned_by":
                                    faction.SpawnFaction = parts[3];
                                    faction.SpawnModifier = id;
                                    break;
                            }
                        }
                        faction.LocalizedName = ExpandedEntries[value];
                        break;

                    case "culture":
                        faction.Culture = value;
                        break;

                    case "special_faction_type":
                        if (value.Equals("slave_faction"))
                        {
                            faction.SlaveFaction = true;
                            break;
                        }
                        if (value.Equals("papal_faction"))
                        {
                            faction.PapalFaction = true;
                        }
                        break;

                    case "religion":
                        faction.Religion = value;
                        break;

                    case "symbol":
                        faction.Symbol = value;
                        break;

                    case "rebel_symbol":
                        faction.RebelSymbol = value;
                        break;

                    case "primary_colour":
                        faction.PrimaryColourR = int.Parse(value.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1]);
                        faction.PrimaryColourG = int.Parse(parts[2].Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1]);
                        faction.PrimaryColourB = int.Parse(parts[3].Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1]);
                        break;

                    case "secondary_colour":
                        faction.SecondaryColourR = int.Parse(value.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1]);
                        faction.SecondaryColourG = int.Parse(parts[2].Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1]);
                        faction.SecondaryColourB = int.Parse(parts[3].Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1]);
                        break;

                    case "loading_logo":
                        faction.LoadingLogo = value;
                        break;

                    case "standard_index":
                        faction.StandardIndex = value;
                        break;

                    case "logo_index":
                        faction.LogoIndex = value;
                        break;

                    case "small_logo_index":
                        faction.SmallLogoIndex = value;
                        break;

                    case "triumph_value":
                        faction.TriumphValue = int.Parse(value);
                        break;

                    case "custom_battle_availability":
                        faction.CustomBattleAvailability = ToBool(value);
                        break;

                    case "periods_unavailable_in_custom_battle":
                        faction.PeriodsUnavailableInCustomBattle = parts[1..].ToList();
                        break;

                    case "can_sap":
                        faction.CanSap = ToBool(value);
                        break;

                    case "prefers_naval_invasions":
                        faction.PrefersNavalInvasions = ToBool(value);
                        break;

                    case "can_have_princess":
                        faction.CanHavePrincess = ToBool(value);
                        break;

                    case "disband_to_pools":
                        faction.DisbandToPools = ToBool(value);
                        break;

                    case "can_build_siege_towers":
                        faction.CanBuildSiegeTowers = ToBool(value);
                        break;

                    case "can_transmit_plague":
                        faction.CanTransmitPlague = ToBool(value);
                        break;

                    case "has_family_tree":
                        faction.HasFamilyTree = value;
                        break;

                    case "horde_min_units":
                        faction.HordeMinUnits = int.Parse(value);
                        break;

                    case "horde_max_units":
                        faction.HordeMaxUnits = int.Parse(value);
                        break;

                    case "horde_max_units_reduction_every_horde":
                        faction.HordeMaxUnitsReductionEveryHorde = int.Parse(value);
                        break;

                    case "horde_unit_per_settlement_population":
                        faction.HordeUnitPerSettlementPopulation = int.Parse(value);
                        break;

                    case "horde_min_named_characters":
                        faction.HordeMinNamedCharacters = int.Parse(value);
                        break;

                    case "horde_max_percent_army_stack":
                        faction.HordeMaxPercentArmyStack = int.Parse(value);
                        break;

                    case "horde_disband_percent_on_settlement_capture":
                        faction.HordeDisbandPercentOnSettlementCapture = int.Parse(value);
                        break;

                    case "horde_unit":
                        faction.HordeUnits.Add(value);
                        break;
                }
            }
            catch (Exception e)
            {
                ErrorDb.AddError(e.Message, _lineNum.ToString(), _fileName);
                Console.WriteLine(e);
                Console.WriteLine(identifier);
                Console.WriteLine(@"on line: " + _lineNum);
                Console.WriteLine(@"====================================================================================");
            }
        }

        private static string? _settlementLevel;

        private static void AssignCultureFields(Culture culture, string[] parts)
        {
            var identifier = parts[0];

            var value = "";

            if (parts.Length > 1)
            {
                value = parts[1];
            }

            char[] delimitersWhite = { ' ', '\t' };

            try
            {
                switch (identifier)
                {
                    case "culture":
                        culture.Name = value;
                        culture.LocalizedName = ExpandedEntries[value.ToLower()];
                        break;

                    case "portrait_mapping":
                        culture.Portrait_mapping = value;
                        break;

                    case "rebel_standard_index":
                        culture.Rebel_standard_index = int.Parse(value);
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
                                culture.Village.Model = value;
                                culture.Village.Aerial_base = parts[2];
                                break;

                            case "moot_and_bailey":
                                culture.Moot_and_bailey.Model = value;
                                culture.Moot_and_bailey.Aerial_base = parts[2];
                                break;

                            case "town":
                                culture.Town.Model = value;
                                culture.Town.Aerial_base = parts[2];
                                break;

                            case "wooden_castle":
                                culture.Wooden_castle.Model = value;
                                culture.Wooden_castle.Aerial_base = parts[2];
                                break;

                            case "large_town":
                                culture.Large_town.Model = value;
                                culture.Large_town.Aerial_base = parts[2];
                                break;

                            case "castle":
                                culture.Castle.Model = value;
                                culture.Castle.Aerial_base = parts[2];
                                break;

                            case "city":
                                culture.City.Model = value;
                                culture.City.Aerial_base = parts[2];
                                break;

                            case "fortress":
                                culture.Fortress.Model = value;
                                culture.Fortress.Aerial_base = parts[2];
                                break;

                            case "large_city":
                                culture.Large_city.Model = value;
                                culture.Large_city.Aerial_base = parts[2];
                                break;

                            case "citadel":
                                culture.Citadel.Model = value;
                                culture.Citadel.Aerial_base = parts[2];
                                break;

                            case "huge_city":
                                culture.Huge_city.Model = value;
                                culture.Huge_city.Aerial_base = parts[2];
                                break;
                        }
                        break;

                    case "card":
                        switch (_settlementLevel)
                        {
                            case "village":
                                culture.Village.Card = value;
                                break;

                            case "moot_and_bailey":
                                culture.Moot_and_bailey.Card = value;
                                break;

                            case "town":
                                culture.Town.Card = value;
                                break;

                            case "wooden_castle":
                                culture.Wooden_castle.Card = value;
                                break;

                            case "large_town":
                                culture.Large_town.Card = value;
                                break;

                            case "castle":
                                culture.Castle.Card = value;
                                break;

                            case "city":
                                culture.City.Card = value;
                                break;

                            case "fortress":
                                culture.Fortress.Card = value;
                                break;

                            case "large_city":
                                culture.Large_city.Card = value;
                                break;

                            case "citadel":
                                culture.Citadel.Card = value;
                                break;

                            case "huge_city":
                                culture.Huge_city.Card = value;
                                break;
                        }
                        break;

                    case "fort":
                        culture.Fort = value;
                        culture.FortBase = parts[2];
                        break;

                    case "fort_cost":
                        culture.FortCost = int.Parse(value);
                        break;

                    case "fort_wall":
                        culture.FortWalls = value;
                        break;

                    case "fishing_village":
                        culture.FishingVillage = value;
                        culture.FishingVillageBase = parts[2];
                        break;

                    case "port_land":
                        if (string.IsNullOrWhiteSpace(culture.FishingPort1))
                        {
                            culture.FishingPort1 = value;
                            culture.FishingPort1Base = parts[2];
                        }
                        else if (string.IsNullOrWhiteSpace(culture.FishingPort2))
                        {
                            culture.FishingPort2 = value;
                            culture.FishingPort2Base = parts[2];
                        }
                        else
                        {
                            culture.FishingPort3 = value;
                            culture.FishingPort3Base = parts[2];
                        }
                        break;

                    case "port_sea":
                        if (string.IsNullOrWhiteSpace(culture.FishingPort1Sea))
                        {
                            culture.FishingPort1Sea = value;
                        }
                        else if (string.IsNullOrWhiteSpace(culture.FishingPort2Sea))
                        {
                            culture.FishingPort2Sea = value;
                        }
                        else
                        {
                            culture.FishingPort3Sea = value;
                        }
                        break;

                    case "watchtower":
                        culture.Watchtower = value;
                        culture.WatchtowerBase = parts[2];
                        break;

                    case "watchtower_cost":
                        culture.WatchtowerCost = int.Parse(value);
                        break;

                    case "spy":
                        parts = value.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        value = parts[0];
                        culture.Spy.Card = value;
                        culture.Spy.Info_card = parts[1];
                        culture.Spy.Cost = int.Parse(parts[3]);
                        culture.Spy.Population_cost = int.Parse(parts[4]);
                        culture.Spy.Recruitment_points = int.Parse(parts[5]);
                        break;

                    case "assassin":
                        parts = value.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        value = parts[0];
                        culture.Assassin.Card = value;
                        culture.Assassin.Info_card = parts[1];
                        culture.Assassin.Cost = int.Parse(parts[3]);
                        culture.Assassin.Population_cost = int.Parse(parts[4]);
                        culture.Assassin.Recruitment_points = int.Parse(parts[5]);
                        break;

                    case "diplomat":
                        parts = value.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        value = parts[0];
                        culture.Diplomat.Card = value;
                        culture.Diplomat.Info_card = parts[1];
                        culture.Diplomat.Cost = int.Parse(parts[3]);
                        culture.Diplomat.Population_cost = int.Parse(parts[4]);
                        culture.Diplomat.Recruitment_points = int.Parse(parts[5]);
                        break;

                    case "admiral":
                        parts = value.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        value = parts[0];
                        culture.Admiral.Card = value;
                        culture.Admiral.Info_card = parts[1];
                        culture.Admiral.Cost = int.Parse(parts[3]);
                        culture.Admiral.Population_cost = int.Parse(parts[4]);
                        culture.Admiral.Recruitment_points = int.Parse(parts[5]);
                        break;

                    case "merchant":
                        parts = value.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        value = parts[0];
                        culture.Merchant.Card = value;
                        culture.Merchant.Info_card = parts[1];
                        culture.Merchant.Cost = int.Parse(parts[3]);
                        culture.Merchant.Population_cost = int.Parse(parts[4]);
                        culture.Merchant.Recruitment_points = int.Parse(parts[5]);
                        break;

                    case "priest":
                        parts = value.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        value = parts[0];
                        culture.Priest.Card = value;
                        culture.Priest.Info_card = parts[1];
                        culture.Priest.Cost = int.Parse(parts[3]);
                        culture.Priest.Population_cost = int.Parse(parts[4]);
                        culture.Priest.Recruitment_points = int.Parse(parts[5]);
                        break;
                }
            }
            catch (Exception e)
            {
                ErrorDb.AddError(e.Message, _lineNum.ToString(), _fileName);
                Console.WriteLine(e);
                Console.WriteLine(identifier);
                Console.WriteLine(@"on line: " + _lineNum);
                Console.WriteLine(@"====================================================================================");
            }
        }
    }
}