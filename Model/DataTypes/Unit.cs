﻿using System;
using System.Collections.Generic;

namespace ModdingTool
{
    public class Unit
    {
        #region Private fields

        #endregion Private fields


        #region Public properties

        public string Name { get; set; } = "";
        public string? Type { get; set; } = "";
        public string? Dictionary { get; set; } = "";
        public string? Category { get; set; } = "infantry";
        public string? Class_type { get; set; } = "light";
        public string? Voice_type { get; set; } = "";
        public string? Soldier { get; set; } = "";
        public int SoldierCount { get; set; } = 0;
        public int ExtrasCount { get; set; } = 0;
        public double Mass { get; set; } = 1.0;
        public string? Officer1 { get; set; } = "";
        public string? Officer2 { get; set; } = "";
        public string? Officer3 { get; set; } = "";
        public string? Ship { get; set; } = "";
        public string? Engine { get; set; } = "";
        public string? Animal { get; set; } = "";
        public string? Mount { get; set; } = "";
        public string? Mounted_engine { get; set; } = "";
        public List<string> Mount_effect { get; set; } = new List<string>();

        public List<string> Attributes { get; set; } = new List<string>();

        public double Spacing_width { get; set; } = 1.4;

        public double Spacing_depth { get; set; } = 1.4;

        public double Spacing_width_loose { get; set; } = 2.4;

        public double Spacing_depth_loose { get; set; } = 2.4;

        public string? Special_formation { get; set; } = "";

        public int Hitpoints { get; set; } = 1;

        public int Mount_hitpoints { get; set; } = 1;

        public int Pri_attack { get; set; } = 1;

        public int Pri_charge { get; set; } = 1;

        public string? Pri_projectile { get; set; } = "";
        public int Pri_range { get; set; } = 0;

        public int Pri_ammunition { get; set; } = 0;

        public string? Pri_weapon_type { get; set; } = "melee";
        public string? Pri_tech_type { get; set; } = "melee_blade";
        public string? Pri_damage_type { get; set; } = "blunt";
        public string? Pri_sound_type { get; set; } = "none";
        public int Pri_att_delay { get; set; } = 0;

        public double Pri_skel_factor { get; set; } = 1.0;

        public List<string>? Pri_attr { get; set; } = new List<string>();

        public int Sec_attack { get; set; } = 0;

        public int Sec_charge { get; set; } = 0;

        public string? Sec_projectile { get; set; } = "";
        public int Sec_range { get; set; } = 0;

        public int Sec_ammunition { get; set; } = 0;

        public string? Sec_weapon_type { get; set; } = "no";
        public string? Sec_tech_type { get; set; } = "melee_simple";
        public string? Sec_damage_type { get; set; } = "blunt";
        public string? Sec_sound_type { get; set; } = "none";
        public int Sec_att_delay { get; set; } = 0;

        public double Sec_skel_factor { get; set; } = 1;

        public List<string>? Sec_attr { get; set; } = new List<string>();

        public int Ter_attack { get; set; } = 0;
        public int Ter_charge { get; set; } = 0;
        public string? Ter_projectile { get; set; } = "";
        public int Ter_range { get; set; } = 0;
        public int Ter_ammunition { get; set; } = 0;
        public string? Ter_weapon_type { get; set; } = "no";
        public string? Ter_tech_type { get; set; } = "melee_simple";
        public string? Ter_damage_type { get; set; } = "blunt";
        public string? Ter_sound_type { get; set; } = "none";
        public int Ter_att_delay { get; set; } = 0;
        public double Ter_skel_factor { get; set; } = 0;
        public List<string>? Ter_attr { get; set; } = new List<string>();

        public int Pri_armour { get; set; } = 0;

        public string? BannerFaction { get; set; } = "main_infantry";
        public string? BannerHoly { get; set; } = "crusade";
        public string? Formation_style { get; set; } = "square";
        public int Pri_defense { get; set; } = 0;

        public int Pri_shield { get; set; } = 0;

        public string? Pri_defSound { get; set; } = "flesh";
        public int Sec_armour { get; set; } = 0;

        public int Sec_defense { get; set; } = 0;

        public string? Sec_defSound { get; set; } = "flesh";
        public int Stat_heat { get; set; } = 0;

        public int Stat_scrub { get; set; } = 0;

        public int Stat_forest { get; set; } = 0;

        public int Stat_snow { get; set; } = 0;

        public int Stat_sand { get; set; } = 0;

        public int Morale { get; set; } = 1;

        public string? Discipline { get; set; } = "normal";

        public string? Training { get; set; } = "untrained";

        public int Stat_charge_dist { get; set; } = 10;

        public int Stat_fire_delay { get; set; } = 0;

        public int Stat_food { get; set; } = 60;

        public int Stat_food_sec { get; set; } = 300;

        public int RecruitTime { get; set; } = 1;

        public int RecruitCost { get; set; } = 500;

        public int Upkeep { get; set; } = 200;

        public int WpnCost { get; set; } = 100;

        public int ArmourCost { get; set; } = 100;

        public int CustomCost { get; set; } = 500;

        public int CustomLimit { get; set; } = 3;

        public int CustomIncrease { get; set; } = 50;

        public double MoveSpeed { get; set; } = 1.0;
        public int Stat_stl { get; set; } = 0;

        public int[]? Armour_ug_levels { get; set; }

        public string? ArmourlvlBase { get; set; }

        public string? ArmourlvlOne { get; set; }

        public string? ArmourlvlTwo { get; set; }

        public string? ArmourlvlThree { get; set; }

        public string?[]? Armour_ug_models { get; set; }

        public string? ArmourModelBase { get; set; }

        public string? ArmourModelOne { get; set; }

        public string? ArmourModelTwo { get; set; }

        public string? ArmourModelThree { get; set; }

        public List<string> Ownership { get; set; } = new List<string>();

        public List<string> EraZero { get; set; } = new List<string>();

        public List<string> EraOne { get; set; } = new List<string>();

        public List<string> EraTwo { get; set; } = new List<string>();

        public double Recruit_priority_offset { get; set; } = 0;

        public string? Info_dict { get; set; }

        public string? Card_dict { get; set; }

        public double CrusadeUpkeep { get; set; } = 1.0;

        public double? Radius { get; set; } = 0.4;

        public double? Height { get; set; } = 1.7;

        public string? Accent { get; set; } = "";
        public int Spacing_ranks { get; set; }

        public bool LockMorale { get; set; } = false;

        public string? Pri_fire_type { get; set; } = "";
        public string? Sec_fire_type { get; set; } = "";
        public string? Ter_fire_type { get; set; } = "";
        public string? Descr { get; set; } = "";
        public string? DescrShort { get; set; } = "";
        public string Card { get; set; } = "";
        public bool Mercenary_unit { get; set; } = false;
        public bool General_unit { get; set; } = false;
        public int Edu_index { get; set; }

        public double AIUnitValue { get; set; } = 0;

        public string CardInfo { get; set; } = "";
        public string FactionSymbol { get; set; } = "";

        public Dictionary<string, List<string>> Comments { get; set; } = new();

        public Dictionary<string, string> CommentsInLine { get; set; } = new();

        #endregion Public properties


        public double CalculateUnitValue()
        {
            double attackValue = 0;
            double hasMissileModifier = 1.0;
            if (Category != null)
            {
                if ((Pri_attr != null && Pri_attr.Contains("prec")) || string.IsNullOrWhiteSpace(Pri_projectile))
                {
                    var attackStat = string.IsNullOrWhiteSpace(Pri_projectile) ? Pri_attack : Sec_attack + 2.0;
                    var chargeStat = string.IsNullOrWhiteSpace(Pri_projectile) ? Pri_charge / 2.0 : Sec_charge / 2.0;
                    attackValue = attackStat + chargeStat;
                }
                else
                {
                    double rangeModifier = Pri_range <= 200 ? Pri_range * 0.02 : 4.0;
                    attackValue = Pri_attack * 0.5 * rangeModifier + Pri_attack * 0.5;
                    if (Pri_attr != null && Pri_attr.Contains("ap"))
                    {
                        attackValue += 5.0;
                        if (Pri_tech_type is "missile_gunpowder")
                            attackValue += 5.0;
                    }
                    hasMissileModifier = 0.8;
                }
            }
            double statsTotal = attackValue + (4 * Pri_armour) + (2 * Pri_defense) + (2 * Pri_shield);

            const double MAX_MORALE = 12.0;
            const double ADJUSTMENT_SCALE = 0.55;
            const double BASE_OFFSET = 0.25;

            double moraleValue = Math.Clamp(Morale, 0.0, MAX_MORALE);
            double normalizedMorale = moraleValue / MAX_MORALE;
            double adjustedMorale = normalizedMorale * ADJUSTMENT_SCALE + BASE_OFFSET;
            double moraleEffect = 1.0 - adjustedMorale;

            double moraleModifier = 1.0 - (moraleEffect * hasMissileModifier);
            double extrasStats = 0.0;
            if (Category != null)
            {
                switch (Category)
                {
                    case "siege":
                        {
                            var rangeModifier = Sec_range <= 210 ? Sec_range * 0.01429 : 3.0;
                            attackValue = Sec_attack * 1.5 * rangeModifier + Sec_attack * 1.5;
                            if (Sec_attr != null)
                            {
                                if (Sec_attr.Contains("ap"))
                                    attackValue += 3.0;
                                if (Sec_attr.Contains("launching"))
                                    attackValue += 3.0;
                                if (!Sec_attr.Contains("area"))
                                {
                                    if (Sec_attr.Contains("bp"))
                                        attackValue *= 1.4;
                                }
                                else
                                    attackValue *= 2.5;
                            }
                            extrasStats = ExtrasCount * attackValue * moraleModifier;
                            //if (Pri_armour < 8)
                            //   statsTotal += 3.0;
                            break;
                        }
                    case "handler":
                        break;
                    case "cavalry":
                        {
                            if (Mount != null)
                            {
                                var mountType = Globals.MountDataBase[Mount].mount_class;
                                switch (mountType)
                                {
                                    case "horse":
                                    case "camel":
                                        statsTotal += 4.0;
                                        break;
                                    case "elephant":
                                        {
                                            extrasStats = Sec_attack + (Sec_charge / 2.0);
                                            if (Sec_attr != null)
                                            {
                                                if (Sec_attr.Contains("ap"))
                                                    extrasStats += 5.0;
                                                if (Sec_attr.Contains("launching"))
                                                    extrasStats += 1.0;
                                                if (Sec_attr.Contains("launching"))
                                                    extrasStats += 2.0;
                                            }

                                            extrasStats = extrasStats * (ExtrasCount * Mount_hitpoints) * moraleModifier;
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                }
            }

            var officerCount = 0;
            if (!string.IsNullOrWhiteSpace(Officer1))
                officerCount++;
            if (!string.IsNullOrWhiteSpace(Officer2))
                officerCount++;
            if (!string.IsNullOrWhiteSpace(Officer3))
                officerCount++;
            if (Attributes.Contains("general_unit"))
                officerCount++;
            var aiUnitValue = ((SoldierCount * Hitpoints + officerCount * (Hitpoints + 1)) * (moraleModifier * statsTotal) + extrasStats) * 0.45;
            return aiUnitValue;
        }

    }
}