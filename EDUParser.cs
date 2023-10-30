// Ignore Spelling: Edu

using ModdingTool.View.InterfaceData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using static ModdingTool.Globals;
using static ModdingTool.ParseHelpers;

namespace ModdingTool
{
    internal class EduParser
    {

        private const string UNIT_CARD_PATH = "\\data\\ui\\units";
        private const string UNIT_INFO_CARD_PATH = "\\data\\ui\\unit_info";
        private const string FACTION_SYMBOL_PATH = "\\data\\ui\\faction_symbols";
        private static int _lineNum;
        private static string _fileName = "";
        public static List<string> EduEndComments = new();
        public static List<string> EduIdentifiers = new()
        {
            "type",
            "dictionary",
            "category",
            "class",
            "voice_type",
            "accent",
            "banner faction",
            "banner holy",
            "soldier",
            "officer",
            "ship",
            "engine",
            "animal",
            "mount",
            "mount_effect",
            "attributes",
            "move_speed_mod",
            "formation",
            "stat_health",
            "stat_pri",
            "stat_pri_attr",
            "stat_sec",
            "stat_sec_attr",
            "stat_ter",
            "stat_ter_attr",
            "stat_pri_armour",
            "stat_sec_armour",
            "stat_heat",
            "stat_ground",
            "stat_mental",
            "stat_charge_dist",
            "stat_fire_delay",
            "stat_food",
            "stat_cost",
            "armour_ug_levels",
            "armour_ug_models",
            "ownership",
            "era 0",
            "era 1",
            "era 2",
            "info_pic_dir",
            "card_pic_dir",
            "crusading_upkeep_modifier",
            "recruit_priority_offset"
        };

        public static void WriteEdu()
        {
            var newEdu = UnitDataBase.Values.Aggregate("", (current, unit) => current + WriteEduEntry(unit));
            newEdu += EduEndComments.Aggregate("", (current, comment) => current + (comment + "\n"));
            File.WriteAllText(@"export_descr_unit.txt", newEdu);
        }

        private static string AddComment(string identifier, Unit unit)
        {
            if (!unit.Comments.ContainsKey(identifier)) return "";
            if (unit.Comments[identifier].Count == 0) return "";
            var comment = unit.Comments[identifier].Aggregate("", (current, c) => current + (c + "\n"));
            return comment;
        }

        private static string WriteEduEntry(Unit unit)
        {
            var entry = "";
            var identifier = "type";
            foreach (var id in EduIdentifiers)
            {
                unit.CommentsInLine.TryAdd(id, "");
            }
            entry += AddComment(identifier, unit);
            entry += identifier + "\t\t\t\t\t" + unit.Type + " " + unit.CommentsInLine[identifier] + "\n";
            identifier = "dictionary";
            entry += AddComment(identifier, unit);
            entry += identifier + "\t\t\t\t" + unit.Dictionary + " " + unit.CommentsInLine[identifier] + "\n";
            identifier = "category";
            entry += AddComment(identifier, unit);
            entry += "category\t\t\t\t" + unit.Category + " " + unit.CommentsInLine[identifier] + "\n";
            identifier = "class";
            entry += AddComment(identifier, unit);
            entry += "class\t\t\t\t\t" + unit.Class_type + " " + unit.CommentsInLine[identifier] + "\n";
            identifier = "voice_type";
            entry += AddComment(identifier, unit);
            entry += "voice_type\t\t\t\t" + unit.Voice_type + " " + unit.CommentsInLine[identifier] + "\n";
            if (!string.IsNullOrWhiteSpace(unit.Accent))
            {
                identifier = "accent";
                entry += AddComment(identifier, unit);
                entry += "accent\t\t\t\t\t" + unit.Accent + " " + unit.CommentsInLine[identifier] + "\n";
            }
            identifier = "banner faction";
            entry += AddComment(identifier, unit);
            entry += "banner faction\t\t\t" + unit.BannerFaction + " " + unit.CommentsInLine[identifier] + "\n";
            identifier = "banner holy";
            entry += AddComment(identifier, unit);
            entry += "banner holy\t\t\t\t" + unit.BannerHoly + " " + unit.CommentsInLine[identifier] + "\n";

            identifier = "soldier";
            entry += AddComment(identifier, unit);
            entry += "soldier\t\t\t\t\t" +
                     unit.Soldier + ", " +
                     unit.SoldierCount + ", " +
                     unit.ExtrasCount + ", " +
                     FormatFloat(unit.Mass);
            if (unit.Radius != null)
            {
                var radius = (double)unit.Radius;
                entry += ", " + FormatFloat(radius);
            }
            if (unit.Height != null)
            {
                var height = (double)unit.Height;
                entry += ", " + FormatFloat(height);
            }
            entry += unit.CommentsInLine[identifier] + "\n";

            if (!string.IsNullOrWhiteSpace(unit.Officer1))
            {
                identifier = "officer";
                entry += AddComment(identifier, unit);
                entry += "officer\t\t\t\t\t" + unit.Officer1 + " " + unit.CommentsInLine[identifier] + "\n";
            }
            if (!string.IsNullOrWhiteSpace(unit.Officer2))
            {
                entry += "officer\t\t\t\t\t" + unit.Officer2 + "\n";
            }
            if (!string.IsNullOrWhiteSpace(unit.Officer3))
            {
                entry += "officer\t\t\t\t\t" + unit.Officer3 + "\n";
            }
            if (!string.IsNullOrWhiteSpace(unit.Ship))
            {
                identifier = "ship";
                entry += AddComment(identifier, unit);
                entry += "ship\t\t\t\t\t" + unit.Ship + " " + unit.CommentsInLine[identifier] + "\n";
            }
            if (!string.IsNullOrWhiteSpace(unit.Engine))
            {
                identifier = "engine";
                entry += AddComment(identifier, unit);
                entry += "engine\t\t\t\t\t" + unit.Engine + " " + unit.CommentsInLine[identifier] + "\n";
            }
            if (!string.IsNullOrWhiteSpace(unit.Animal))
            {
                identifier = "animal";
                entry += AddComment(identifier, unit);
                entry += "animal\t\t\t\t\t" + unit.Animal + " " + unit.CommentsInLine[identifier] + "\n";
            }
            if (!string.IsNullOrWhiteSpace(unit.Mount))
            {
                identifier = "mount";
                entry += AddComment(identifier, unit);
                entry += "mount\t\t\t\t\t" + unit.Mount + " " + unit.CommentsInLine[identifier] + "\n";
            }
            var mountEffectString = MakeCommaString(unit.Mount_effect);
            if (!string.IsNullOrWhiteSpace(mountEffectString))
            {
                identifier = "mount_effect";
                entry += AddComment(identifier, unit);
                entry += "mount_effect\t\t\t" + mountEffectString + " " + unit.CommentsInLine[identifier] + "\n";
            }
            var attributesString = MakeCommaString(unit.Attributes);
            if (!string.IsNullOrWhiteSpace(attributesString))
            {
                identifier = "attributes";
                entry += AddComment(identifier, unit);
                entry += "attributes\t\t\t\t" + attributesString + " " + unit.CommentsInLine[identifier] + "\n";
            }

            identifier = "move_speed_mod";
            entry += AddComment(identifier, unit);
            entry += "move_speed_mod\t\t\t" + FormatFloat(unit.MoveSpeed) + " " + unit.CommentsInLine[identifier] + "\n";

            identifier = "formation";
            entry += AddComment(identifier, unit);
            entry += "formation\t\t\t\t" +
                     FormatFloatSingle(unit.Spacing_width) + ", " +
                     FormatFloatSingle(unit.Spacing_depth) + ", " +
                     FormatFloatSingle(unit.Spacing_width_loose) + ", " +
                     FormatFloatSingle(unit.Spacing_depth_loose) + ", " +
                                       unit.Spacing_ranks + ", " +
                                       unit.Formation_style;
            if (!string.IsNullOrWhiteSpace(unit.Special_formation))
            {
                entry += ", " + unit.Special_formation;
            }
            entry += unit.CommentsInLine[identifier] + "\n";

            identifier = "stat_health";
            entry += AddComment(identifier, unit);
            entry += "stat_health\t\t\t\t" + unit.Hitpoints + ", " + unit.Mount_hitpoints + " " + unit.CommentsInLine[identifier] + "\n";

            identifier = "stat_pri";
            entry += AddComment(identifier, unit);
            entry += "stat_pri\t\t\t\t" +
                     unit.Pri_attack + ", " +
                     unit.Pri_charge + ", " +
                     unit.Pri_projectile + ", " +
                     unit.Pri_range + ", " +
                     unit.Pri_ammunition + ", " +
                     unit.Pri_weapon_type + ", " +
                     unit.Pri_tech_type + ", " +
                     unit.Pri_damage_type + ", " +
                     unit.Pri_sound_type;
            if (!string.IsNullOrWhiteSpace(unit.Pri_fire_type))
            {
                entry += ", " + unit.Pri_fire_type;
            }
            entry += ", " + unit.Pri_att_delay + ", " + FormatFloat(unit.Pri_skel_factor) + " " + unit.CommentsInLine[identifier] + "\n";

            var priAttributesString = "no";
            if (unit.Pri_attr is { Count: > 0 })
            {
                priAttributesString = MakeCommaString(unit.Pri_attr);
            }
            identifier = "stat_pri_attr";
            entry += AddComment(identifier, unit);
            entry += "stat_pri_attr\t\t\t" + priAttributesString + " " + unit.CommentsInLine[identifier] + "\n";
            if (unit.Sec_weapon_type == "no" || string.IsNullOrWhiteSpace(unit.Sec_weapon_type))
            {
                identifier = "stat_sec";
                entry += AddComment(identifier, unit);
                entry += "stat_sec\t\t\t\t0, 0, no, 0, 0, no, melee_simple, blunt, none, 0, 1" + " " + unit.CommentsInLine[identifier] + "\n";
                entry += "stat_sec_attr\t\t\t" + "no" + "\n";
            }
            else
            {
                identifier = "stat_sec";
                entry += AddComment(identifier, unit);
                entry += "stat_sec\t\t\t\t" +
                         unit.Sec_attack + ", " +
                         unit.Sec_charge + ", " +
                         unit.Sec_projectile + ", " +
                         unit.Sec_range + ", " +
                         unit.Sec_ammunition + ", " +
                         unit.Sec_weapon_type + ", " +
                         unit.Sec_tech_type + ", " +
                         unit.Sec_damage_type + ", " +
                         unit.Sec_sound_type;
                if (!string.IsNullOrWhiteSpace(unit.Sec_fire_type))
                {
                    entry += ", " + unit.Sec_fire_type;
                }
                entry += ", " + unit.Sec_att_delay + ", " + FormatFloat(unit.Sec_skel_factor) + " " + unit.CommentsInLine[identifier] + "\n";
                var secAttributesString = "no";
                if (unit is { Sec_attr: { Count: > 0 }, Sec_attr: { } }) secAttributesString = MakeCommaString(unit.Sec_attr);
                identifier = "stat_sec_attr";
                entry += AddComment(identifier, unit);
                entry += "stat_sec_attr\t\t\t" + secAttributesString + " " + unit.CommentsInLine[identifier] + "\n";
            }
            if (!string.IsNullOrWhiteSpace(unit.Ter_weapon_type) && unit.Ter_weapon_type != "no")
            {
                identifier = "stat_ter";
                entry += AddComment(identifier, unit);
                entry += "stat_ter\t\t\t\t" +
                         unit.Ter_attack + ", " +
                         unit.Ter_charge + ", " +
                         unit.Ter_projectile + ", " +
                         unit.Ter_range + ", " +
                         unit.Ter_ammunition + ", " +
                         unit.Ter_weapon_type + ", " +
                         unit.Ter_tech_type + ", " +
                         unit.Ter_damage_type + ", " +
                         unit.Ter_sound_type;
                if (!string.IsNullOrWhiteSpace(unit.Ter_fire_type))
                {
                    entry += ", " + unit.Ter_fire_type;
                }
                entry += ", " + unit.Ter_att_delay + ", " + FormatFloat(unit.Ter_skel_factor) + " " + unit.CommentsInLine[identifier] + "\n";
                var terAttributesString = "no";
                if (unit.Ter_attr is { Count: > 0 })
                {
                    terAttributesString = MakeCommaString(unit.Ter_attr);
                }
                identifier = "stat_ter_attr";
                entry += AddComment(identifier, unit);
                entry += "stat_ter_attr\t\t\t" + terAttributesString + " " + unit.CommentsInLine[identifier] + "\n";
            }

            identifier = "stat_pri_armour";
            entry += AddComment(identifier, unit);
            entry += "stat_pri_armour\t\t\t" +
                     unit.Pri_armour + ", " +
                     unit.Pri_defense + ", " +
                     unit.Pri_shield + ", " +
                     unit.Pri_defSound + " " + unit.CommentsInLine[identifier] + "\n";

            identifier = "stat_sec_armour";
            entry += AddComment(identifier, unit);
            entry += "stat_sec_armour\t\t\t" +
                     unit.Sec_armour + ", " +
                     unit.Sec_defense + ", " +
                     unit.Sec_defSound + " " + unit.CommentsInLine[identifier] + "\n";

            identifier = "stat_heat";
            entry += AddComment(identifier, unit);
            entry += "stat_heat\t\t\t\t" + unit.Stat_heat + " " + unit.CommentsInLine[identifier] + "\n";

            identifier = "stat_ground";
            entry += AddComment(identifier, unit);
            entry += "stat_ground\t\t\t\t" +
                     unit.Stat_scrub + ", " +
                     unit.Stat_forest + ", " +
                     unit.Stat_snow + ", " +
                     unit.Stat_sand + " " + unit.CommentsInLine[identifier] + "\n";

            identifier = "stat_mental";
            entry += AddComment(identifier, unit);
            entry += "stat_mental\t\t\t\t" +
                     unit.Morale + ", " +
                     unit.Discipline + ", " +
                     unit.Training;
            if (unit.LockMorale)
            {
                entry += ", lock_morale";
            }
            entry += " " + unit.CommentsInLine[identifier] + "\n";

            identifier = "stat_charge_dist";
            entry += AddComment(identifier, unit);
            entry += "stat_charge_dist\t\t" + unit.Stat_charge_dist + " " + unit.CommentsInLine[identifier] + "\n";

            identifier = "stat_fire_delay";
            entry += AddComment(identifier, unit);
            entry += "stat_fire_delay\t\t\t" + unit.Stat_fire_delay + " " + unit.CommentsInLine[identifier] + "\n";

            identifier = "stat_food";
            entry += AddComment(identifier, unit);
            entry += "stat_food\t\t\t\t" +
                     unit.Stat_food + ", " +
                     unit.Stat_food_sec + " " + unit.CommentsInLine[identifier] + "\n";

            identifier = "stat_cost";
            entry += AddComment(identifier, unit);
            entry += "stat_cost\t\t\t\t" +
                     unit.RecruitTime + ", " +
                     unit.RecruitCost + ", " +
                     unit.Upkeep + ", " +
                     unit.WpnCost + ", " +
                     unit.ArmourCost + ", " +
                     unit.CustomCost + ", " +
                     unit.CustomLimit + ", " +
                     unit.CustomIncrease + " " + unit.CommentsInLine[identifier] + "\n";

            identifier = "armour_ug_levels";
            entry += AddComment(identifier, unit);
            entry += "armour_ug_levels\t\t" + unit.ArmourlvlBase;
            if (!string.IsNullOrWhiteSpace(unit.ArmourlvlOne))
            {
                entry += ", " + unit.ArmourlvlOne;
            }
            if (!string.IsNullOrWhiteSpace(unit.ArmourlvlTwo))
            {
                entry += ", " + unit.ArmourlvlTwo;
            }
            if (!string.IsNullOrWhiteSpace(unit.ArmourlvlThree))
            {
                entry += ", " + unit.ArmourlvlThree;
            }
            entry += " " + unit.CommentsInLine[identifier] + "\n";

            identifier = "armour_ug_models";
            entry += AddComment(identifier, unit);
            entry += "armour_ug_models\t\t" + unit.ArmourModelBase;
            if (!string.IsNullOrWhiteSpace(unit.ArmourModelOne))
            {
                entry += ", " + unit.ArmourModelOne;
            }
            if (!string.IsNullOrWhiteSpace(unit.ArmourModelTwo))
            {
                entry += ", " + unit.ArmourModelTwo;
            }
            if (!string.IsNullOrWhiteSpace(unit.ArmourModelThree))
            {
                entry += ", " + unit.ArmourModelThree;
            }
            entry += " " + unit.CommentsInLine[identifier] + "\n";

            identifier = "ownership";
            entry += AddComment(identifier, unit);
            entry += "ownership\t\t\t\t";
            var ownershipString = MakeCommaString(unit.Ownership);
            if (!string.IsNullOrWhiteSpace(ownershipString))
            {
                entry += ownershipString;
            }
            entry += " " + unit.CommentsInLine[identifier] + "\n";


            var eraString = MakeCommaString(unit.EraZero);
            if (!string.IsNullOrWhiteSpace(eraString))
            {
                identifier = "era 0";
                entry += AddComment(identifier, unit);
                entry += "era 0\t\t\t\t\t" + eraString + " " + unit.CommentsInLine[identifier] + "\n";
            }
            eraString = MakeCommaString(unit.EraOne);
            if (!string.IsNullOrWhiteSpace(eraString))
            {
                identifier = "era 1";
                entry += AddComment(identifier, unit);
                entry += "era 1\t\t\t\t\t" + eraString + " " + unit.CommentsInLine[identifier] + "\n";
            }
            eraString = MakeCommaString(unit.EraTwo);
            if (!string.IsNullOrWhiteSpace(eraString))
            {
                identifier = "era 2";
                entry += AddComment(identifier, unit);
                entry += "era 2\t\t\t\t\t" + eraString + " " + unit.CommentsInLine[identifier] + "\n";
            }
            if (!string.IsNullOrWhiteSpace(unit.Info_dict))
            {
                identifier = "info_pic_dir";
                entry += AddComment(identifier, unit);
                entry += "info_pic_dir\t\t\t" + unit.Info_dict + " " + unit.CommentsInLine[identifier] + "\n";
            }
            if (!string.IsNullOrWhiteSpace(unit.Card_dict))
            {
                identifier = "card_pic_dir";
                entry += AddComment(identifier, unit);
                entry += "card_pic_dir\t\t\t" + unit.Card_dict + " " + unit.CommentsInLine[identifier] + "\n";
            }
            if (Math.Abs(unit.CrusadeUpkeep - 1.0) > 0.001)
            {
                identifier = "crusading_upkeep_modifier";
                entry += AddComment(identifier, unit);
                entry += "crusading_upkeep_modifier\t" + FormatFloat(unit.CrusadeUpkeep) + " " + unit.CommentsInLine[identifier] + "\n";
            }
            identifier = "recruit_priority_offset";
            entry += AddComment(identifier, unit);
            entry += "recruit_priority_offset\t" + unit.Recruit_priority_offset + " " + unit.CommentsInLine[identifier] + "\n";
            entry += "\n";
            entry += "\n";
            entry += "\n";
            return entry;
        }

        private static string MakeCommaString(List<string> list)
        {
            var commaString = "";
            if (list.Count == 0) return commaString;
            foreach (var element in list)
            {
                commaString += element;
                if (element != list.Last())
                {
                    commaString += ", ";
                }
            }
            return commaString;
        }

        private static string FormatFloat(double input)
        {
            if (Math.Abs(input - 1) < 0.01)
            {
                return "1";
            }
            return input == 0 ? "0" : input.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private static string FormatFloatSingle(double input)
        {
            if (Math.Abs(input - 1) < 0.01)
            {
                return "1";
            }
            return input == 0 ? "0" : input.ToString("0.0", CultureInfo.InvariantCulture);
        }

        public static void ParseEdu()
        {
            _fileName = "export_descr_unit.txt";
            Console.WriteLine($@"start parse {_fileName}");

            //Try read file
            var lines = FileReader("\\data\\export_descr_unit.txt", _fileName, Encoding.UTF8);
            if (lines == null) { return; }

            //Initialize Global Unit Database
            UnitDataBase = new Dictionary<string, Unit>();

            //Make first entry
            var newUnit = new Unit();
            var first = true;

            //Reset line counter and EDU index tracker
            var index = 0;
            _lineNum = 0;

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
                var lineParts = SplitEduLine(newline);
                if (lineParts is { Length: < 2 })
                {
                    //Should be something wrong with line if you hit this
                    ErrorDb.AddError("Unrecognized content", _lineNum.ToString(), _fileName);
                    continue;
                }

                //Entry is completed, next entry is starting
                if (lineParts != null && (bool)lineParts[0]?.Equals("type"))
                {
                    if (!first)
                    {
                        newUnit.Edu_index = index;
                        var cards = GetCards(newUnit.Dictionary, newUnit.Ownership, newUnit.Card_dict, newUnit.Info_dict, newUnit.Mercenary_unit);
                        newUnit.Card = cards[0];
                        newUnit.CardInfo = cards[1];
                        newUnit.FactionSymbol = cards[2];

                        if (newUnit.Type != null)
                        {
                            AddUnit(newUnit);
                        }

                        index++;
                    }
                    first = false;
                    newUnit = new Unit();
                }

                //assign field line is about
                AssignFields(newUnit, lineParts);
            }

            //Add last unit
            newUnit.Edu_index = index;
            AddUnit(newUnit);

            EduEndComments.AddRange(CommentCache);
            CommentCache.Clear();


            Console.WriteLine($@"end parse {_fileName}");

        }

        private static void AddUnit(Unit unit)
        {
            if (unit.Type == null) return;
            UnitDataBase.Add(unit.Type, unit);
            Console.WriteLine(unit.Type);

            foreach (var faction in unit.Ownership.Where(_ => true))
            {
                if (faction == "all")
                {
                    foreach (var fac in FactionDataBase)
                    {
                        fac.Value.Unit_ownership.Add(unit.Type);
                    }
                }
                else if (FactionDataBase.Keys.Contains(faction))
                {
                    FactionDataBase[faction].Unit_ownership.Add(unit.Type);
                }
                else if (CultureDataBase.Keys.Contains(faction))
                {
                    foreach (var fac in CultureDataBase.SelectMany(cult => cult.Value.Factions))
                    {
                        FactionDataBase[fac].Unit_ownership.Add(unit.Type);
                    }
                }
                else
                {
                    ErrorDb.AddError($@"Faction not found {faction}", _lineNum.ToString(), _fileName);
                    Console.WriteLine(@"faction not found: " + faction);
                }
            }
        }

        public static void ParseEu()
        {
            _fileName = "export_units.txt";
            Console.WriteLine($@"start parse {_fileName}");

            //Try read file
            var lines = FileReader("\\data\\text\\export_units.txt", _fileName, Encoding.Unicode);
            if (lines == null) { return; }

            //Reset line counter
            _lineNum = 0;

            //Initialize Global names and descriptions database
            UnitNames = new Dictionary<string, string>();
            UnitDescr = new Dictionary<string, string?>();
            UnitDescrShort = new Dictionary<string, string?>();

            //Loop through lines
            foreach (var line in lines)
            {
                //Increase line counter
                _lineNum++;
                //Clean up line
                var parts = LocalTextLineCleaner(line, _lineNum, _fileName);
                if (parts != null)
                    AddUnitStringEntry(parts);

            }
            Console.WriteLine($@"end parse {_fileName}");
        }

        private static string[] GetCards(string? unit, List<string> factions, string? cardPicDir, string? infoPicDir, bool merc)
        {
            var unitCard = "";
            var cardSearchFactions = factions;
            var infoCardSearchFactions = factions;

            if (merc)
            {
                cardSearchFactions = new List<string> { "mercs" };
                infoCardSearchFactions = new List<string> { "merc" };
            }

            if (cardPicDir != null)
            {
                cardSearchFactions = new List<string> { cardPicDir };
            }
            if (infoPicDir != null)
            {
                infoCardSearchFactions = new List<string> { infoPicDir };
            }

            foreach (var faction in cardSearchFactions)
            {
                var cardPath = ModPath + UNIT_CARD_PATH + "\\";
                cardPath += faction;
                if (!(Directory.Exists(cardPath)))
                {
                    ErrorDb.AddError($@"Card path not found {cardPath}", _lineNum.ToString(), _fileName);
                    continue;
                }

                if (File.Exists(cardPath + "\\#" + unit + ".tga"))
                {
                    unitCard = cardPath + "\\#" + unit + ".tga";
                }
                else
                {
                    ErrorDb.AddError($@"no unit card found for unit: {unit} in faction: {faction}");
                }
            }

            var unitInfoCard = "";
            foreach (var faction in infoCardSearchFactions)
            {
                var cardInfoPath = ModPath + UNIT_INFO_CARD_PATH + "\\";
                cardInfoPath += faction;
                if (!(Directory.Exists(cardInfoPath)))
                {
                    ErrorDb.AddError($@"Info Card path not found {cardInfoPath}", _lineNum.ToString(), _fileName);
                    continue;
                }

                if (File.Exists(cardInfoPath + "\\" + unit + "_info.tga"))
                {
                    unitInfoCard = cardInfoPath + "\\" + unit + "_info.tga";
                }
                else
                {
                    ErrorDb.AddError($@"no unit info card found for unit: {unit} in faction: {faction}");
                }
            }

            // Faction Symbols
            var factionSymbol = "";
            var factionSymbolPath = ModPath + FACTION_SYMBOL_PATH + "\\";

            // If the unit is slave only, just set to slave and move on
            if (factions.Count == 1 && factions[0] == "slave")
            {
                factionSymbolPath += "slave";
            }
            // Otherwise, add the first non-slave faction
            else if (factions.Count == 1 && factions[0] != "slave")
            {
                factionSymbolPath += factions[0];
            }
            else if (factions.Count > 1)
            {
                foreach (var faction in factions)
                {
                    if (faction != "slave")
                    {
                        factionSymbolPath += faction;
                        break;
                    }
                }
            }

            if (File.Exists(factionSymbolPath + ".tga"))
            {
                factionSymbol = factionSymbolPath + ".tga";
            }

            if (unitCard.Equals(""))
            {
                ErrorDb.AddError($@"!!no unit card at all found for unit: {unit}");
            }
            if (unitInfoCard.Equals(""))
            {
                ErrorDb.AddError($@"!!no unit info card at all found for unit: {unit}");
            }
            return new[] { unitCard, unitInfoCard, factionSymbol };
        }

        private static void AddUnitStringEntry(string?[] parts)
        {
            var identifier = parts[0];

            Console.WriteLine(identifier);

            var text = "";

            if (parts.Length > 1)
            {
                text = parts[1];
            }
            else
            {
                ErrorDb.AddError($@"Unit localization is empty {identifier}", _lineNum.ToString(), _fileName);
            }

            if (identifier != null && identifier.Contains("_descr_short"))
            {
                var split = identifier.Split("_descr_short", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var result = UnitDescrShort.TryAdd(split[0], text);
                // if (!result)
                // {
                //     ErrorDb.AddError($@"Duplicate _descr_short entries found for {split[0]}", split[0], _fileName);
                // }
            }
            else if (identifier != null && identifier.Contains("_descr"))
            {
                var split = identifier.Split("_descr", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var result = UnitDescr.TryAdd(split[0], text);
                // if (!result)
                // {
                //     ErrorDb.AddError($@"Duplicate _descr entries found for {split[0]}", split[0], _fileName);
                // }
            }
            else
            {
                if (text == null || identifier == null) return;
                if (!UnitNames.ContainsKey(identifier))
                {
                    UnitNames.Add(identifier, text);
                }
            }
        }

        private static void AssignComments(string identifier, Unit unit)
        {
            unit.Comments[identifier] = new List<string>();
            unit.Comments[identifier].AddRange(CommentCache);
            CommentCache.Clear();
            unit.CommentsInLine[identifier] = "";
            unit.CommentsInLine[identifier] = CommentCacheInLine;
            CommentCacheInLine = "";
        }

        private static void AssignFields(Unit unit, string?[]? parts)
        {
            var identifier = parts?[0];
            try
            {
                switch (identifier)
                {
                    case "type":
                        AssignComments(identifier, unit);
                        unit.Type = parts?[1]?.Trim();
                        break;

                    case "dictionary":
                        AssignComments(identifier, unit);
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
                        AssignComments(identifier, unit);
                        unit.Category = parts?[1]?.Trim();
                        break;

                    case "class":
                        AssignComments(identifier, unit);
                        unit.Class_type = parts?[1]?.Trim();
                        break;

                    case "voice_type":
                        AssignComments(identifier, unit);
                        unit.Voice_type = parts?[1]?.Trim();
                        break;

                    case "accent":
                        AssignComments(identifier, unit);
                        unit.Accent = parts?[1]?.Trim();
                        break;

                    case "banner faction":
                        AssignComments(identifier, unit);
                        unit.BannerFaction = parts?[1]?.Trim();
                        break;

                    case "banner holy":
                        AssignComments(identifier, unit);
                        unit.BannerHoly = parts?[1]?.Trim();
                        break;

                    case "soldier":
                        AssignComments(identifier, unit);
                        unit.Soldier = parts?[1]?.Trim().ToLower();
                        UsedModels.Add(unit.Soldier);
                        unit.SoldierCount = int.Parse(parts?[2] ?? string.Empty);
                        unit.ExtrasCount = int.Parse(parts?[3] ?? string.Empty);
                        unit.Mass = double.Parse(parts?[4] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                        if (parts is { Length: > 5 })
                        {
                            unit.Radius = double.Parse(parts[5] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                            if (parts.Length > 6)
                            {
                                unit.Height = double.Parse(parts[6] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                            }
                        }
                        break;

                    case "officer":
                        AssignComments(identifier, unit);
                        if (unit.Officer1 is "")
                        {
                            unit.Officer1 = parts?[1]?.Trim().ToLower();
                            UsedModels.Add(unit.Officer1);
                            break;
                        }
                        if (unit.Officer2 is "")
                        {
                            unit.Officer2 = parts?[1]?.Trim().ToLower();
                            UsedModels.Add(unit.Officer2);
                            break;
                        }
                        if (unit.Officer3 is "")
                        {
                            unit.Officer3 = parts?[1]?.Trim().ToLower();
                            UsedModels.Add(unit.Officer3);
                        }
                        break;

                    case "ship":
                        AssignComments(identifier, unit);
                        unit.Ship = parts?[1]?.Trim().ToLower();
                        break;

                    case "mounted_engine":
                        AssignComments(identifier, unit);
                        unit.Mounted_engine = parts?[1]?.Trim().ToLower();
                        break;

                    case "engine":
                        AssignComments(identifier, unit);
                        unit.Engine = parts?[1]?.Trim().ToLower();
                        break;

                    case "animal":
                        AssignComments(identifier, unit);
                        unit.Animal = parts?[1]?.Trim().ToLower();
                        break;

                    case "mount":
                        AssignComments(identifier, unit);
                        unit.Mount = parts?[1]?.Trim().ToLower();
                        UsedModels.Add(MountDataBase[unit.Mount].model.Trim().ToLower());
                        UsedMounts.Add(unit.Mount);
                        break;

                    case "mount_effect":
                        AssignComments(identifier, unit);
                        if (parts == null || parts.Length < 2)
                        {
                            break;
                        }
                        foreach (var attr in parts[1..])
                        {
                            if (attr == null) break;
                            unit.Mount_effect.Add(attr);
                        }
                        break;

                    case "attributes":
                        AssignComments(identifier, unit);
                        if (parts == null || parts.Length < 2)
                        {
                            break;
                        }
                        foreach (var attr in parts?[1..]!)
                        {
                            if (attr == null) break;
                            unit.Attributes.Add(attr);
                            if (!UnitTab.AttributeTypes.Contains(attr))
                            {
                                UnitTab.AttributeTypes.Add(attr);
                            }
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
                        AssignComments(identifier, unit);
                        unit.MoveSpeed = double.Parse(parts?[1] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                        break;

                    case "formation":
                        AssignComments(identifier, unit);
                        unit.Spacing_width = double.Parse(parts?[1] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                        unit.Spacing_depth = double.Parse(parts?[2] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                        unit.Spacing_width_loose = double.Parse(parts?[3] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                        unit.Spacing_depth_loose = double.Parse(parts?[4] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
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
                        AssignComments(identifier, unit);
                        unit.Hitpoints = int.Parse(parts?[1] ?? string.Empty);
                        unit.Mount_hitpoints = int.Parse(parts?[2] ?? string.Empty);
                        break;

                    case "stat_pri":
                        AssignComments(identifier, unit);
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
                        AssignComments(identifier, unit);
                        if (parts?.Length < 2)
                        {
                            break;
                        }
                        foreach (var part in parts?[1..]!)
                        {
                            if (part != null) unit.Pri_attr?.Add(part);
                        }
                        break;

                    case "stat_sec":
                        AssignComments(identifier, unit);
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
                        AssignComments(identifier, unit);
                        if (parts?.Length < 2)
                        {
                            break;
                        }
                        foreach (var part in parts?[1..]!)
                        {
                            if (part != null) unit.Sec_attr?.Add(part);
                        }
                        break;

                    case "stat_ter":
                        AssignComments(identifier, unit);
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
                        AssignComments(identifier, unit);
                        if (parts?.Length < 2)
                        {
                            break;
                        }
                        foreach (var part in parts?[1..]!)
                        {
                            if (part != null) unit.Ter_attr?.Add(part);
                        }
                        break;

                    case "stat_pri_armour":
                        AssignComments(identifier, unit);
                        unit.Pri_armour = int.Parse(parts?[1] ?? string.Empty);
                        unit.Pri_defense = int.Parse(parts?[2] ?? string.Empty);
                        unit.Pri_shield = int.Parse(parts?[3] ?? string.Empty);
                        unit.Pri_defSound = parts?[4];
                        break;

                    case "stat_sec_armour":
                        AssignComments(identifier, unit);
                        unit.Sec_armour = int.Parse(parts?[1] ?? string.Empty);
                        unit.Sec_defense = int.Parse(parts?[2] ?? string.Empty);
                        unit.Sec_defSound = parts?[3];
                        break;

                    case "stat_heat":
                        AssignComments(identifier, unit);
                        unit.Stat_heat = int.Parse(parts?[1] ?? string.Empty);
                        break;

                    case "stat_ground":
                        AssignComments(identifier, unit);
                        unit.Stat_scrub = int.Parse(parts?[1] ?? string.Empty);
                        unit.Stat_sand = int.Parse(parts?[2] ?? string.Empty);
                        unit.Stat_forest = int.Parse(parts?[3] ?? string.Empty);
                        unit.Stat_snow = int.Parse(parts?[4] ?? string.Empty);
                        break;

                    case "stat_mental":
                        AssignComments(identifier, unit);
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
                        AssignComments(identifier, unit);
                        unit.Stat_charge_dist = int.Parse(parts?[1] ?? string.Empty);
                        break;

                    case "stat_fire_delay":
                        AssignComments(identifier, unit);
                        unit.Stat_fire_delay = int.Parse(parts?[1] ?? string.Empty);
                        break;

                    case "stat_food":
                        AssignComments(identifier, unit);
                        unit.Stat_food = int.Parse(parts?[1] ?? string.Empty);
                        unit.Stat_food_sec = int.Parse(parts?[2] ?? string.Empty);
                        break;

                    case "stat_cost":
                        AssignComments(identifier, unit);
                        unit.RecruitTime = int.Parse(parts?[1] ?? string.Empty);
                        unit.RecruitCost = int.Parse(parts?[2] ?? string.Empty);
                        unit.Upkeep = int.Parse(parts?[3] ?? string.Empty);
                        unit.WpnCost = int.Parse(parts?[4] ?? string.Empty);
                        unit.ArmourCost = int.Parse(parts?[5] ?? string.Empty);
                        unit.CustomCost = int.Parse(parts?[6] ?? string.Empty);
                        unit.CustomLimit = int.Parse(parts?[7] ?? string.Empty);
                        unit.CustomIncrease = int.Parse(parts?[8] ?? string.Empty);
                        break;

                    case "stat_stl":
                        AssignComments(identifier, unit);
                        unit.Stat_stl = int.Parse(parts?[1] ?? string.Empty);
                        break;

                    case "armour_ug_levels":
                        AssignComments(identifier, unit);
                        if (parts != null)
                        {
                            var lvlbase = 0;
                            var lvlone = 0;
                            var lvltwo = 0;
                            var lvlthree = 0;
                            foreach (var level in parts[1..])
                            {
                                var lvlint = int.Parse(level ?? string.Empty);
                                if (string.IsNullOrWhiteSpace(unit.ArmourlvlBase))
                                {
                                    lvlbase = lvlint;
                                    unit.ArmourlvlBase = lvlbase.ToString();
                                }
                                else if (lvlbase == lvlint)
                                {
                                    unit.ArmourlvlBase += ", " + lvlint;
                                }
                                else if (string.IsNullOrWhiteSpace(unit.ArmourlvlOne))
                                {
                                    lvlone = lvlint;
                                    unit.ArmourlvlOne = lvlone.ToString();
                                }
                                else if (lvlone == lvlint)
                                {
                                    unit.ArmourlvlOne += ", " + lvlint;
                                }
                                else if (string.IsNullOrWhiteSpace(unit.ArmourlvlTwo))
                                {
                                    lvltwo = lvlint;
                                    unit.ArmourlvlTwo = lvltwo.ToString();
                                }
                                else if (lvltwo == lvlint)
                                {
                                    unit.ArmourlvlTwo += ", " + lvlint;
                                }
                                else if (string.IsNullOrWhiteSpace(unit.ArmourlvlThree))
                                {
                                    lvlthree = lvlint;
                                    unit.ArmourlvlThree = lvlthree.ToString();
                                }
                                else if (lvlthree == lvlint)
                                {
                                    unit.ArmourlvlThree += ", " + lvlint;
                                }
                            }
                        }

                        break;

                    case "armour_ug_models":
                        AssignComments(identifier, unit);
                        foreach (var model in parts?[1..]!)
                        {
                            if (string.IsNullOrWhiteSpace(unit.ArmourModelBase))
                            {
                                unit.ArmourModelBase = model?.ToLower();
                                UsedModels.Add(unit.ArmourModelBase);
                            }
                            else if (string.IsNullOrWhiteSpace(unit.ArmourModelOne))
                            {
                                unit.ArmourModelOne = model?.ToLower();
                                UsedModels.Add(unit.ArmourModelOne);
                            }
                            else if (string.IsNullOrWhiteSpace(unit.ArmourModelTwo))
                            {
                                unit.ArmourModelTwo = model?.ToLower();
                                UsedModels.Add(unit.ArmourModelTwo);
                            }
                            else if (string.IsNullOrWhiteSpace(unit.ArmourModelThree))
                            {
                                unit.ArmourModelThree = model?.ToLower();
                                UsedModels.Add(unit.ArmourModelThree);
                            }
                        }
                        unit.Armour_ug_models = parts?[1..];
                        break;

                    case "ownership":
                        AssignComments(identifier, unit);
                        foreach (var faction in parts?[1..]!)
                        {
                            if (faction != null) unit.Ownership.Add(faction);
                        }
                        break;

                    case "era 0":
                        AssignComments(identifier, unit);
                        foreach (var faction in parts?[1..]!)
                        {
                            if (faction != null) unit.EraZero.Add(faction);
                        }
                        break;

                    case "era 1":
                        AssignComments(identifier, unit);
                        foreach (var faction in parts?[1..]!)
                        {
                            if (faction != null) unit.EraOne.Add(faction);
                        }
                        break;

                    case "era 2":
                        AssignComments(identifier, unit);
                        foreach (var faction in parts?[1..]!)
                        {
                            if (faction != null) unit.EraTwo.Add(faction);
                        }
                        break;

                    case "recruit_priority_offset":
                        AssignComments(identifier, unit);
                        unit.Recruit_priority_offset = float.Parse(parts?[1] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                        break;

                    case "info_pic_dir":
                        AssignComments(identifier, unit);
                        unit.Info_dict = parts?[1];
                        break;

                    case "card_pic_dir":
                        AssignComments(identifier, unit);
                        unit.Card_dict = parts?[1];
                        break;

                    case "crusading_upkeep_modifier":
                        AssignComments(identifier, unit);
                        unit.CrusadeUpkeep = float.Parse(parts?[1] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                        break;
                }
            }
            catch (Exception e)
            {
                ErrorDb.AddError(e.Message + " " + identifier, _lineNum.ToString(), _fileName);
                Console.WriteLine(e);
                Console.WriteLine(identifier);
                Console.WriteLine(@"on line: " + _lineNum);
                Console.WriteLine(@"====================================================================================");
            }
        }
    }

}