﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;
using static ModdingTool.parseHelpers;
using static ModdingTool.Globals;
using System.Reflection;
using log4net;
using log4net.Config;

namespace ModdingTool
{
    internal class EDUParser
    {
        public static Dictionary<string, Unit> allUnits = new Dictionary<string, Unit>();

        private static string unitCardPath = "\\data\\ui\\units";
        private static string unitInfoCardPath = "\\data\\ui\\unit_info";

        public static Dictionary<string, string> unitNames = new Dictionary<string, string>();
        public static Dictionary<string, string> unitDescr = new Dictionary<string, string>();
        public static Dictionary<string, string> unitDescrShort = new Dictionary<string, string>();

        public static void parseEDU()
        {

            _log.Info("start parse edu");
            string[] lines = File.ReadAllLines(modPath + "\\data\\export_descr_unit.txt");

            Unit newUnit = new Unit();
            bool first = false;
            allUnits = new Dictionary<string, Unit>();

            int index = 0;

            foreach (string line in lines)
            {
                if (line.StartsWith(';'))
                {
                    continue;
                }
                string newline = removeComment(line);
                if (line.Equals(""))
                {
                    continue;
                }
                newline = newline.Trim();
                string[] lineParts = splitLine(newline);
                if (lineParts.Length < 2)
                {
                    continue;
                }
                if (lineParts[0].Equals("type"))
                {
                    if (first)
                    {
                        newUnit.Edu_index= index;
                        string[] cards = getCards(newUnit.Unit_dictionary, newUnit.Unit_ownership, newUnit.Unit_card_dict, newUnit.Unit_info_dict, newUnit.Mercenary_unit);
                        newUnit.Unit_card = cards[0];
                        newUnit.Unit_cardInfo = cards[1];
                        allUnits.Add(newUnit.Unit_type, newUnit);
                        _log.Info(newUnit.Unit_type);
                        foreach (string faction in newUnit.Unit_ownership)
                        {
                            factionParser.allFactions[faction].Unit_ownership.Add(newUnit);
                        }
                        index++;
                    }
                    first = true;
                    newUnit = new Unit();
                }
                if (first)
                {
                    assignFields(newUnit, lineParts);
                }
                //Console.WriteLine(lineParts[0]);
            }
            newUnit.Edu_index = index;
            allUnits.Add(newUnit.Unit_type, newUnit);
            _log.Info(newUnit.Unit_type);
            foreach (string faction in newUnit.Unit_ownership)
            {
                factionParser.allFactions[faction].Unit_ownership.Add(newUnit);
            }
            _log.Info("end parse edu");
        }

        public static void parseEU()
        {
            _log.Info("start parse export_units");
            string[] lines = File.ReadAllLines(modPath + "\\data\\text\\export_units.txt", Encoding.Unicode);
            unitNames = new Dictionary<string, string>();
            unitDescr = new Dictionary<string, string>();
            unitDescrShort = new Dictionary<string, string>();
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
                fillDicts(parts);


                //Console.WriteLine(line);
            }
            _log.Info("end parse export_units");
        }

        public static string[] stringSplitter(string line)
        {
            char[] deliminators = { '{', '}' };
            string[] splitted = line.Split(deliminators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return splitted;
        }

        public static string[] getCards(string unit, string[] factions, string card_pic_dir, string info_pic_dir, bool merc)
        {
            string unitCard = "";
            string[] cardSearchFactions = factions;
            string[] infoCardSearchFactions = factions;

            if (merc)
            {
                cardSearchFactions = new string[] { "mercs" };
                infoCardSearchFactions = new string[] { "merc" };
            }

            if (card_pic_dir != null)
            {
                cardSearchFactions = new string[] { card_pic_dir };
            }
            if (info_pic_dir != null)
            {
                infoCardSearchFactions = new string[] { info_pic_dir };
            }


            foreach (string faction in cardSearchFactions)
            {
                string cardPath = modPath + unitCardPath + "\\";
                cardPath += faction;
                if(!(Directory.Exists(cardPath))) 
                {
                    continue;
                }
                if (File.Exists(cardPath + "\\#" + unit + ".tga"))
                {
                    unitCard = cardPath + "\\#" + unit + ".tga";
                }
                else
                {
                    _log.Info("no unit card found for unit: " + unit + " in faction: " + faction);
                }
            }
            string unitInfoCard = "";
            foreach (string faction in infoCardSearchFactions)
            {
                string cardInfoPath = modPath + unitInfoCardPath + "\\";
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
                    _log.Info("no unit info card found for unit: " + unit + " in faction: " + faction);
                }
            }
            if (unitCard.Equals(""))
            {
                _log.Info("!!no unit card at all found for unit: " + unit);
            }
            if (unitInfoCard.Equals(""))
            {
                _log.Info("!!no unit info card at all found for unit: " + unit);
            }
            return new string[] { unitCard, unitInfoCard };
        }

        public static void fillDicts(string[] parts)
        {
            String identifier = parts[0];

            _log.Info(identifier);

            string text = "";

            if (parts.Length > 1)
            {
                text = parts[1];
            }

            if (identifier.Contains("_descr_short"))
            {
                string[] split = identifier.Split("_descr_short", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                unitDescrShort.Add(split[0], text);
            } else if (identifier.Contains("_descr"))
            {
                string[] split = identifier.Split("_descr", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                unitDescr.Add(split[0], text);
            } else
            {
                unitNames.Add(identifier, text);
            }
        }

        public static void assignFields(Unit unit, string[] parts)
        {
            String identifier = parts[0];

            switch (identifier)
            {
                case "type":
                    unit.Unit_type = parts[1].Trim();
                    break;
                case "dictionary":
                    string dict = parts[1];
                    unit.Unit_dictionary = dict;
                    unit.Unit_name = unitNames[dict];
                    unit.Unit_descr = unitDescr[dict];
                    unit.Unit_descrShort = unitDescrShort[dict];
                    break;
                case "category":
                    unit.Unit_category = parts[1].Trim();
                    break;
                case "class":
                    unit.Unit_class = parts[1].Trim();
                    break;
                case "voice_type":
                    unit.Unit_voice_type = parts[1].Trim();
                    break;
                case "accent":
                    unit.Unit_accent = parts[1].Trim();
                    break;
                case "banner faction":
                    unit.Unit_bannerFaction = parts[1].Trim();
                    break;
                case "banner holy":
                    unit.Unit_bannerHoly = parts[1].Trim();
                    break;
                case "soldier":
                    unit.Unit_soldier = parts[1].Trim();
                    unit.Unit_soldierCount = int.Parse(parts[2]);
                    unit.Unit_extrasCount = int.Parse(parts[3]);
                    unit.Unit_mass = float.Parse(parts[4]);
                    if (parts.Length > 5)
                    {
                        unit.Unit_radius = float.Parse(parts[5]);
                        if (parts.Length > 6)
                        {
                            unit.Unit_height = float.Parse(parts[6]);
                        }
                    }
                    break;
                case "officer":
                    if (unit.Unit_officer1.Equals(""))
                    {
                        unit.Unit_officer1 = parts[1].Trim();
                        break;
                    }
                    if (unit.Unit_officer2.Equals(""))
                    {
                        unit.Unit_officer2 = parts[1].Trim();
                        break;
                    }
                    if (unit.Unit_officer3.Equals(""))
                    {
                        unit.Unit_officer3 = parts[1].Trim();
                        break;
                    }
                    break;
                case "ship":
                    unit.Unit_ship = parts[1].Trim();
                    break;
                case "engine":
                    unit.Unit_engine = parts[1].Trim();
                    break;
                case "animal":
                    unit.Unit_animal = parts[1].Trim();
                    break;
                case "mount":
                    unit.Unit_mount = parts[1].Trim();
                    break;
                case "mount_effect":
                    unit.Unit_mount_effect = parts[1..];
                    break;
                case "attributes":
                    unit.Unit_attributes = parts[1..];
                    foreach (string attr in unit.Unit_attributes)
                    {
                        if (attr.Equals("mercenary_unit"))
                        {
                            unit.Mercenary_unit = true;
                            continue;
                        } else if (attr.Equals("general_unit"))
                        {
                            unit.General_unit = true;
                            continue;
                        }
                    }
                    break;
                case "move_speed_mod":
                    unit.Unit_moveSpeed = float.Parse(parts[1]);
                    break;
                case "formation":
                    unit.Unit_spacing_width = float.Parse(parts[1]);
                    unit.Unit_spacing_depth = float.Parse(parts[2]);
                    unit.Unit_spacing_width_loose = float.Parse(parts[3]);
                    unit.Unit_spacing_depth_loose = float.Parse(parts[4]);
                    unit.Unit_spacing_ranks = int.Parse(parts[5]);
                    if (parts.Length > 6)
                    {
                        unit.Unit_formation_style = parts[6];
                        if (parts.Length > 7)
                        {
                            unit.Unit_special_formation = parts[7];
                        }
                    }
                    break;
                case "stat_health":
                    unit.Unit_hitpoints = int.Parse(parts[1]);
                    unit.Unit_mount_hitpoints = int.Parse(parts[2]);
                    break;
                case "stat_pri":
                    unit.Unit_pri_attack = int.Parse(parts[1]);
                    unit.Unit_pri_charge = int.Parse(parts[2]);
                    unit.Unit_pri_projectile = parts[3];
                    unit.Unit_pri_range = int.Parse(parts[4]);
                    unit.Unit_pri_ammunition = int.Parse(parts[5]);
                    unit.Unit_pri_weapon_type = parts[6];
                    unit.Unit_pri_tech_type = parts[7];
                    unit.Unit_pri_damage_type = parts[8];
                    unit.Unit_pri_sound_type = parts[9];
                    if (parts.Length > 12)
                    {
                        unit.Unit_pri_fire_type = parts[10];
                        unit.Unit_pri_att_delay = int.Parse(parts[11]);
                        unit.Unit_pri_skel_factor = double.Parse(parts[12]);
                    }
                    else
                    {
                        unit.Unit_pri_att_delay = int.Parse(parts[10]);
                        unit.Unit_pri_skel_factor = double.Parse(parts[11]);
                    }
                    break;
                case "stat_pri_attr":
                    unit.Unit_pri_attr = parts[1..];
                    break;
                case "stat_sec":
                    unit.Unit_sec_attack = int.Parse(parts[1]);
                    unit.Unit_sec_charge = int.Parse(parts[2]);
                    unit.Unit_sec_projectile = parts[3];
                    unit.Unit_sec_range = int.Parse(parts[4]);
                    unit.Unit_sec_ammunition = int.Parse(parts[5]);
                    unit.Unit_sec_weapon_type = parts[6];
                    unit.Unit_sec_tech_type = parts[7];
                    unit.Unit_sec_damage_type = parts[8];
                    unit.Unit_sec_sound_type = parts[9];
                    if (parts.Length > 12)
                    {
                        unit.Unit_sec_fire_type = parts[10];
                        unit.Unit_sec_att_delay = int.Parse(parts[11]);
                        unit.Unit_sec_skel_factor = double.Parse(parts[12]);
                    }
                    else
                    {
                        unit.Unit_sec_att_delay = int.Parse(parts[10]);
                        unit.Unit_sec_skel_factor = double.Parse(parts[11]);
                    }
                    break;
                case "stat_sec_attr":
                    unit.Unit_sec_attr = parts[1..];
                    break;
                case "stat_ter":
                    unit.Unit_ter_attack = int.Parse(parts[1]);
                    unit.Unit_ter_charge = int.Parse(parts[2]);
                    unit.Unit_ter_projectile = parts[3];
                    unit.Unit_ter_range = int.Parse(parts[4]);
                    unit.Unit_ter_ammunition = int.Parse(parts[5]);
                    unit.Unit_ter_weapon_type = parts[6];
                    unit.Unit_ter_tech_type = parts[7];
                    unit.Unit_ter_damage_type = parts[8];
                    unit.Unit_ter_sound_type = parts[9];
                    if (parts.Length > 12)
                    {
                        unit.Unit_ter_fire_type = parts[10];
                        unit.Unit_ter_att_delay = int.Parse(parts[11]);
                        unit.Unit_ter_skel_factor = double.Parse(parts[12]);
                    } else
                    {
                        unit.Unit_ter_att_delay = int.Parse(parts[10]);
                        unit.Unit_ter_skel_factor = double.Parse(parts[11]);
                    }
                    break;
                case "stat_ter_attr":
                    unit.Unit_ter_attr = parts[1..];
                    break;
                case "stat_pri_armour":
                    unit.Unit_pri_armour = int.Parse(parts[1]);
                    unit.Unit_pri_defense = int.Parse(parts[2]);
                    unit.Unit_pri_shield = int.Parse(parts[3]);
                    unit.Unit_pri_defSound = parts[4];
                    break;
                case "stat_sec_armour":
                    unit.Unit_sec_armour = int.Parse(parts[1]);
                    unit.Unit_sec_defense = int.Parse(parts[2]);
                    unit.Unit_sec_defSound = parts[3];
                    break;
                case "stat_heat":
                    unit.Unit_stat_heat = int.Parse(parts[1]);
                    break;
                case "stat_ground":
                    unit.Unit_stat_scrub = int.Parse(parts[1]);
                    unit.Unit_stat_sand = int.Parse(parts[2]);
                    unit.Unit_stat_forest = int.Parse(parts[3]);
                    unit.Unit_stat_snow = int.Parse(parts[4]);
                    break;
                case "stat_mental":
                    unit.Unit_morale = int.Parse(parts[1]);
                    unit.Unit_discipline = parts[2];
                    unit.Unit_training = parts[3];
                    if (parts.Length > 4 && parts[4].Equals("lock_morale"))
                    {
                        unit.Unit_lockMorale = true;
                    } else {
                        unit.Unit_lockMorale = false;
                    }
                    break;
                case "stat_charge_dist":
                    unit.Unit_stat_charge_dist = int.Parse(parts[1]);
                    break;
                case "stat_fire_delay":
                    unit.Unit_stat_fire_delay = int.Parse(parts[1]);
                    break;
                case "stat_food":
                    unit.Unit_stat_food = int.Parse(parts[1]);
                    unit.Unit_stat_food_sec = int.Parse(parts[2]);
                    break;
                case "stat_cost":
                    unit.Unit_recruitTime = int.Parse(parts[1]);
                    unit.Unit_recruitCost = int.Parse(parts[2]);
                    unit.Unit_upkeep = int.Parse(parts[3]);
                    unit.Unit_wpnCost = int.Parse(parts[4]);
                    unit.Unit_armourCost = int.Parse(parts[5]);
                    unit.Unit_customCost = int.Parse(parts[6]);
                    unit.Unit_customLimit = int.Parse(parts[7]);
                    unit.Unit_customIncrease = int.Parse(parts[8]);
                    break;
                case "armour_ug_levels":
                    int[] levels = new int[parts[1..].Length];
                    for (int i = 0; i < parts[1..].Length;i++)
                    {
                        levels[i] = int.Parse(parts[i + 1]);
                    }
                    unit.Unit_armour_ug_levels = levels;
                    break;
                case "armour_ug_models":
                    unit.Unit_armour_ug_models = parts[1..];
                    break;
                case "ownership":
                    unit.Unit_ownership = parts[1..];
                    break;
                case "recruit_priority_offset":
                    unit.Unit_recruit_priority_offset = float.Parse(parts[1]);
                    break;
                case "info_pic_dir":
                    unit.Unit_info_dict = parts[1];
                    break;
                case "card_pic_dir":
                    unit.Unit_card_dict = parts[1];
                    break;
                case "crusading_upkeep_modifier":
                    unit.Unit_crusadeUpkeep = float.Parse(parts[1]);
                    break;

            }
        }
    }
}
