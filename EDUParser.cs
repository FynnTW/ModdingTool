using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using static ModdingTool.Globals;
using static ModdingTool.parseHelpers;

namespace ModdingTool
{
    internal class EduParser
    {

        private const string UnitCardPath = "\\data\\ui\\units";
        private const string UnitInfoCardPath = "\\data\\ui\\unit_info";
        private static int _line;

        public static void ParseEdu()
        {
            ParseEu();
            Console.WriteLine(@"start parse edu");
            var lines = File.ReadAllLines(ModPath + "\\data\\export_descr_unit.txt");

            var newUnit = new Unit();
            var first = false;
            AllUnits = new Dictionary<string, Unit>();

            var index = 0;
            _line = 0;
            foreach (var line in lines)
            {
                _line++;
                if (line.StartsWith(';'))
                {
                    continue;
                }
                var newline = removeComment(line);
                if (line.Equals(""))
                {
                    continue;
                }
                newline = newline.Trim();
                var lineParts = splitLine(newline);
                if (lineParts is { Length: < 2 })
                {
                    continue;
                }
                if (lineParts != null && (bool)lineParts[0]?.Equals("type"))
                {
                    if (first)
                    {
                        newUnit.Edu_index = index;
                        if (newUnit.Ownership != null)
                        {
                            var cards = GetCards(newUnit.Dictionary, newUnit.Ownership, newUnit.Card_dict, newUnit.Info_dict, newUnit.Mercenary_unit);
                            newUnit.Card = cards[0];
                            newUnit.CardInfo = cards[1];
                        }

                        if (newUnit.Type != null)
                        {
                            AllUnits.Add(newUnit.Type, newUnit);
                            Console.WriteLine(newUnit.Type);
                        }

                        if (newUnit.Ownership != null)
                            foreach (var faction in newUnit.Ownership)
                            {
                                if (faction != null) AllFactions[faction].Unit_ownership.Add(newUnit);
                            }

                        index++;
                    }
                    first = true;
                    newUnit = new Unit();
                }
                if (first)
                {
                    AssignFields(newUnit, lineParts);
                }
                //Console.WriteLine(lineParts[0]);
            }
            newUnit.Edu_index = index;
            if (newUnit.Type != null)
            {
                AllUnits.Add(newUnit.Type, newUnit);
                Console.WriteLine(newUnit.Type);
            }

            if (newUnit.Ownership != null)
                foreach (var faction in newUnit.Ownership)
                {
                    if (faction != null) AllFactions[faction].Unit_ownership.Add(newUnit);
                }

            Console.WriteLine(@"end parse edu");
            Globals.PrintInt(AllUnits.Count);
            Globals.PrintInt(UnitNames.Count);
            Globals.PrintInt(UnitDescr.Count);
            Globals.PrintInt(UnitDescrShort.Count);
            Globals.PrintInt(ModelDb.Count);
            Globals.PrintInt(AllFactions.Count);
            Globals.PrintInt(AllCultures.Count);
            Globals.PrintInt(ExpandedEntries.Count);
            PrintFinal();

        }

        public static void ParseEu()
        {
            Console.WriteLine(@"start parse export_units");
            var lines = File.ReadAllLines(ModPath + "\\data\\text\\export_units.txt", Encoding.Unicode);
            UnitNames = new Dictionary<string, string>();
            UnitDescr = new Dictionary<string, string?>();
            UnitDescrShort = new Dictionary<string, string?>();
            foreach (var line in lines)
            {
                if (line.StartsWith('¬'))
                {
                    continue;
                }
                if (!line.Contains('{') || !line.Contains('}'))
                {
                    continue;
                }
                var newLine = line.Trim();
                var parts = StringSplitter(newLine);
                FillDicts(parts);

                //Console.WriteLine(line);
            }
            Console.WriteLine(@"end parse export_units");
        }

        public static string?[] StringSplitter(string line)
        {
            char[] deliminators = { '{', '}' };
            string?[] splitted = line.Split(deliminators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return splitted;
        }

        public static string[] GetCards(string? unit, string?[] factions, string? cardPicDir, string? infoPicDir, bool merc)
        {
            var unitCard = "";
            var cardSearchFactions = factions;
            var infoCardSearchFactions = factions;

            if (merc)
            {
                cardSearchFactions = new[] { "mercs" };
                infoCardSearchFactions = new[] { "merc" };
            }

            if (cardPicDir != null)
            {
                cardSearchFactions = new[] { cardPicDir };
            }
            if (infoPicDir != null)
            {
                infoCardSearchFactions = new[] { infoPicDir };
            }

            foreach (var faction in cardSearchFactions)
            {
                var cardPath = ModPath + UnitCardPath + "\\";
                cardPath += faction;
                if (!(Directory.Exists(cardPath)))
                {
                    continue;
                }

                if (File.Exists(cardPath + "\\#" + unit + ".tga"))
                {
                    unitCard = cardPath + "\\#" + unit + ".tga";
                }
                else
                {
                    Console.WriteLine(@"no unit card found for unit: " + unit + @" in faction: " + faction);
                }
            }

            var unitInfoCard = "";
            foreach (var faction in infoCardSearchFactions)
            {
                var cardInfoPath = ModPath + UnitInfoCardPath + "\\";
                cardInfoPath += faction;
                if (!(Directory.Exists(cardInfoPath)))
                {
                    continue;
                }

                if (File.Exists(cardInfoPath + "\\" + unit + "_info.tga"))
                {
                    unitInfoCard = cardInfoPath + "\\" + unit + "_info.tga";
                }
                else
                {
                    Console.WriteLine(@"no unit info card found for unit: " + unit + @" in faction: " + faction);
                }
            }

            if (unitCard.Equals(""))
            {
                Console.WriteLine(@"!!no unit card at all found for unit: " + unit);
            }
            if (unitInfoCard.Equals(""))
            {
                Console.WriteLine(@"!!no unit info card at all found for unit: " + unit);
            }
            return new[] { unitCard, unitInfoCard };
        }

        public static void FillDicts(string?[] parts)
        {
            var identifier = parts[0];

            Console.WriteLine(identifier);

            var text = "";

            if (parts.Length > 1)
            {
                text = parts[1];
            }

            if (identifier != null && identifier.Contains("_descr_short"))
            {
                var split = identifier.Split("_descr_short", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                UnitDescrShort.Add(split[0], text);
            }
            else if (identifier != null && identifier.Contains("_descr"))
            {
                var split = identifier.Split("_descr", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                UnitDescr.Add(split[0], text);
            }
            else
            {
                if (text == null) return;
                if (identifier != null)
                    UnitNames.Add(identifier, text);
            }
        }

        public static void AssignFields(Unit unit, string?[]? parts)
        {
            var identifier = parts?[0];
            try
            {
                switch (identifier)
                {
                    case "type":
                        unit.Type = parts?[1]?.Trim();
                        break;

                    case "dictionary":
                        var dict = parts?[1];
                        if (dict != null)
                        {
                            unit.Dictionary = dict;
                            unit.Name = UnitNames[dict];
                            unit.Descr = UnitDescr[dict];
                            unit.DescrShort = UnitDescrShort[dict];
                        }
                        break;

                    case "category":
                        unit.Category = parts?[1]?.Trim();
                        break;

                    case "class":
                        unit.Class_type = parts?[1]?.Trim();
                        break;

                    case "voice_type":
                        unit.Voice_type = parts?[1]?.Trim();
                        break;

                    case "accent":
                        unit.Accent = parts?[1]?.Trim();
                        break;

                    case "banner faction":
                        unit.BannerFaction = parts?[1]?.Trim();
                        break;

                    case "banner holy":
                        unit.BannerHoly = parts?[1]?.Trim();
                        break;

                    case "soldier":
                        unit.Soldier = parts?[1]?.Trim();
                        unit.SoldierCount = int.Parse(parts?[2] ?? string.Empty);
                        unit.ExtrasCount = int.Parse(parts?[3] ?? string.Empty);
                        unit.Mass = float.Parse(parts?[4] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                        if (parts is { Length: > 5 })
                        {
                            unit.Radius = float.Parse(parts[5] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                            if (parts.Length > 6)
                            {
                                unit.Height = float.Parse(parts[6] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                            }
                        }
                        break;

                    case "officer":
                        if (unit.Officer1 is "")
                        {
                            unit.Officer1 = parts?[1]?.Trim();
                            break;
                        }
                        if (unit.Officer2 is "")
                        {
                            unit.Officer2 = parts?[1]?.Trim();
                            break;
                        }
                        if (unit.Officer3 is "")
                        {
                            unit.Officer3 = parts?[1]?.Trim();
                        }
                        break;

                    case "ship":
                        unit.Ship = parts?[1]?.Trim();
                        break;

                    case "engine":
                        unit.Engine = parts?[1]?.Trim();
                        break;

                    case "animal":
                        unit.Animal = parts?[1]?.Trim();
                        break;

                    case "mount":
                        unit.Mount = parts?[1]?.Trim();
                        break;

                    case "mount_effect":
                        unit.Mount_effect = parts?[1..];
                        break;

                    case "attributes":
                        unit.Attributes = parts?[1..];
                        if (unit.Attributes != null)
                            foreach (var attr in unit.Attributes)
                            {
                                switch (attr)
                                {
                                    case "mercenary_unit":
                                        unit.Mercenary_unit = true;
                                        continue;
                                    case "general_unit":
                                        unit.General_unit = true;
                                        continue;
                                }
                            }

                        break;

                    case "move_speed_mod":
                        unit.MoveSpeed = float.Parse(parts?[1] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                        break;

                    case "formation":
                        unit.Spacing_width = float.Parse(parts?[1] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                        unit.Spacing_depth = float.Parse(parts?[2] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                        unit.Spacing_width_loose = float.Parse(parts?[3] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                        unit.Spacing_depth_loose = float.Parse(parts?[4] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                        unit.Spacing_ranks = int.Parse(parts?[5] ?? string.Empty);
                        if (parts is { Length: > 6 })
                        {
                            unit.Formation_style = parts[6];
                            if (parts.Length > 7)
                            {
                                unit.Special_formation = parts[7];
                            }
                        }
                        break;

                    case "stat_health":
                        unit.Hitpoints = int.Parse(parts?[1] ?? string.Empty);
                        unit.Mount_hitpoints = int.Parse(parts?[2] ?? string.Empty);
                        break;

                    case "stat_pri":
                        unit.Pri_attack = int.Parse(parts?[1] ?? string.Empty);
                        unit.Pri_charge = int.Parse(parts?[2] ?? string.Empty);
                        unit.Pri_projectile = parts?[3];
                        unit.Pri_range = int.Parse(parts?[4] ?? string.Empty);
                        unit.Pri_ammunition = int.Parse(parts?[5] ?? string.Empty);
                        unit.Pri_weapon_type = parts?[6];
                        unit.Pri_tech_type = parts?[7];
                        unit.Pri_damage_type = parts?[8];
                        unit.Pri_sound_type = parts?[9];
                        if (parts is { Length: > 12 })
                        {
                            unit.Pri_fire_type = parts[10];
                            unit.Pri_att_delay = int.Parse(parts[11] ?? string.Empty);
                            unit.Pri_skel_factor = double.Parse(parts[12] ?? string.Empty);
                        }
                        else
                        {
                            unit.Pri_att_delay = int.Parse(parts?[10] ?? string.Empty);
                            unit.Pri_skel_factor = double.Parse(parts?[11] ?? string.Empty);
                        }
                        break;

                    case "stat_pri_attr":
                        unit.Pri_attr = parts?[1..];
                        break;

                    case "stat_sec":
                        unit.Sec_attack = int.Parse(parts?[1] ?? string.Empty);
                        unit.Sec_charge = int.Parse(parts?[2] ?? string.Empty);
                        unit.Sec_projectile = parts?[3];
                        unit.Sec_range = int.Parse(parts?[4] ?? string.Empty);
                        unit.Sec_ammunition = int.Parse(parts?[5] ?? string.Empty);
                        unit.Sec_weapon_type = parts?[6];
                        unit.Sec_tech_type = parts?[7];
                        unit.Sec_damage_type = parts?[8];
                        unit.Sec_sound_type = parts?[9];
                        if (parts is { Length: > 12 })
                        {
                            unit.Sec_fire_type = parts[10];
                            unit.Sec_att_delay = int.Parse(parts[11] ?? string.Empty);
                            unit.Sec_skel_factor = double.Parse(parts[12] ?? string.Empty);
                        }
                        else
                        {
                            unit.Sec_att_delay = int.Parse(parts?[10] ?? string.Empty);
                            unit.Sec_skel_factor = double.Parse(parts?[11] ?? string.Empty);
                        }
                        break;

                    case "stat_sec_attr":
                        unit.Sec_attr = parts?[1..];
                        break;

                    case "stat_ter":
                        unit.Ter_attack = int.Parse(parts?[1] ?? string.Empty);
                        unit.Ter_charge = int.Parse(parts?[2] ?? string.Empty);
                        unit.Ter_projectile = parts?[3];
                        unit.Ter_range = int.Parse(parts?[4] ?? string.Empty);
                        unit.Ter_ammunition = int.Parse(parts?[5] ?? string.Empty);
                        unit.Ter_weapon_type = parts?[6];
                        unit.Ter_tech_type = parts?[7];
                        unit.Ter_damage_type = parts?[8];
                        unit.Ter_sound_type = parts?[9];
                        if (parts is { Length: > 12 })
                        {
                            unit.Ter_fire_type = parts[10];
                            unit.Ter_att_delay = int.Parse(parts[11] ?? string.Empty);
                            unit.Ter_skel_factor = double.Parse(parts[12] ?? string.Empty);
                        }
                        else
                        {
                            unit.Ter_att_delay = int.Parse(parts?[10] ?? string.Empty);
                            unit.Ter_skel_factor = double.Parse(parts?[11] ?? string.Empty);
                        }
                        break;

                    case "stat_ter_attr":
                        unit.Ter_attr = parts?[1..];
                        break;

                    case "stat_pri_armour":
                        unit.Pri_armour = int.Parse(parts?[1] ?? string.Empty);
                        unit.Pri_defense = int.Parse(parts?[2] ?? string.Empty);
                        unit.Pri_shield = int.Parse(parts?[3] ?? string.Empty);
                        unit.Pri_defSound = parts?[4];
                        break;

                    case "stat_sec_armour":
                        unit.Sec_armour = int.Parse(parts?[1] ?? string.Empty);
                        unit.Sec_defense = int.Parse(parts?[2] ?? string.Empty);
                        unit.Sec_defSound = parts?[3];
                        break;

                    case "stat_heat":
                        unit.Stat_heat = int.Parse(parts?[1] ?? string.Empty);
                        break;

                    case "stat_ground":
                        unit.Stat_scrub = int.Parse(parts?[1] ?? string.Empty);
                        unit.Stat_sand = int.Parse(parts?[2] ?? string.Empty);
                        unit.Stat_forest = int.Parse(parts?[3] ?? string.Empty);
                        unit.Stat_snow = int.Parse(parts?[4] ?? string.Empty);
                        break;

                    case "stat_mental":
                        unit.Morale = int.Parse(parts?[1] ?? string.Empty);
                        unit.Discipline = parts?[2];
                        unit.Training = parts?[3];
                        if (parts is { Length: > 4 } && parts[4] is "lock_morale")
                        {
                            unit.LockMorale = true;
                        }
                        else
                        {
                            unit.LockMorale = false;
                        }
                        break;

                    case "stat_charge_dist":
                        unit.Stat_charge_dist = int.Parse(parts?[1] ?? string.Empty);
                        break;

                    case "stat_fire_delay":
                        unit.Stat_fire_delay = int.Parse(parts?[1] ?? string.Empty);
                        break;

                    case "stat_food":
                        unit.Stat_food = int.Parse(parts?[1] ?? string.Empty);
                        unit.Stat_food_sec = int.Parse(parts?[2] ?? string.Empty);
                        break;

                    case "stat_cost":
                        unit.RecruitTime = int.Parse(parts?[1] ?? string.Empty);
                        unit.RecruitCost = int.Parse(parts?[2] ?? string.Empty);
                        unit.Upkeep = int.Parse(parts?[3] ?? string.Empty);
                        unit.WpnCost = int.Parse(parts?[4] ?? string.Empty);
                        unit.ArmourCost = int.Parse(parts?[5] ?? string.Empty);
                        unit.CustomCost = int.Parse(parts?[6] ?? string.Empty);
                        unit.CustomLimit = int.Parse(parts?[7] ?? string.Empty);
                        unit.CustomIncrease = int.Parse(parts?[8] ?? string.Empty);
                        break;

                    case "armour_ug_levels":
                        if (parts != null)
                        {
                            var levels = new int[parts[1..].Length];
                            for (var i = 0; i < parts[1..].Length; i++)
                            {
                                levels[i] = int.Parse(parts[i + 1] ?? string.Empty);
                            }
                            unit.Armour_ug_levels = levels;
                        }

                        break;

                    case "armour_ug_models":
                        unit.Armour_ug_models = parts?[1..];
                        break;

                    case "ownership":
                        unit.Ownership = parts?[1..];
                        break;

                    case "recruit_priority_offset":
                        unit.Recruit_priority_offset = float.Parse(parts?[1] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                        break;

                    case "info_pic_dir":
                        unit.Info_dict = parts?[1];
                        break;

                    case "card_pic_dir":
                        unit.Card_dict = parts?[1];
                        break;

                    case "crusading_upkeep_modifier":
                        unit.CrusadeUpkeep = float.Parse(parts?[1] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(identifier);
                Console.WriteLine(@"on line: " + _line);
                Console.WriteLine(@"====================================================================================");
            }
        }
    }
}