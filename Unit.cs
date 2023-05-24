using System;
using System.Collections;
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
        public string? Category { get; set; } = "";
        public string? Class_type { get; set; } = "";
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
        public string?[]? Mount_effect { get; set; }

        public string?[]? Attributes { get; set; }

        public double Spacing_width { get; set; }

        public double Spacing_depth { get; set; }

        public double Spacing_width_loose { get; set; }

        public double Spacing_depth_loose { get; set; }

        public string? Special_formation { get; set; } = "";
        public int Hitpoints { get; set; }

        public int Mount_hitpoints { get; set; }

        public int Pri_attack { get; set; }

        public int Pri_charge { get; set; }

        public string? Pri_projectile { get; set; } = "";
        public int Pri_range { get; set; }

        public int Pri_ammunition { get; set; }

        public string? Pri_weapon_type { get; set; } = "";
        public string? Pri_tech_type { get; set; } = "";
        public string? Pri_damage_type { get; set; } = "";
        public string? Pri_sound_type { get; set; } = "";
        public int Pri_att_delay { get; set; }

        public double Pri_skel_factor { get; set; }

        public string?[]? Pri_attr { get; set; }

        public int Sec_attack { get; set; }

        public int Sec_charge { get; set; }

        public string? Sec_projectile { get; set; } = "";
        public int Sec_range { get; set; }

        public int Sec_ammunition { get; set; }

        public string? Sec_weapon_type { get; set; } = "";
        public string? Sec_tech_type { get; set; } = "";
        public string? Sec_damage_type { get; set; } = "";
        public string? Sec_sound_type { get; set; } = "";
        public int Sec_att_delay { get; set; }

        public double Sec_skel_factor { get; set; }

        public string?[]? Sec_attr { get; set; }

        public int Ter_attack { get; set; } = 0;
        public int Ter_charge { get; set; } = 0;
        public string? Ter_projectile { get; set; } = "";
        public int Ter_range { get; set; } = 0;
        public int Ter_ammunition { get; set; } = 0;
        public string? Ter_weapon_type { get; set; } = "";
        public string? Ter_tech_type { get; set; } = "";
        public string? Ter_damage_type { get; set; } = "";
        public string? Ter_sound_type { get; set; } = "";
        public int Ter_att_delay { get; set; } = 0;
        public double Ter_skel_factor { get; set; } = 0;
        public string?[]? Ter_attr { get; set; }

        public int Pri_armour { get; set; }

        public string? BannerFaction { get; set; } = "";
        public string? BannerHoly { get; set; } = "";
        public string? Formation_style { get; set; } = "";
        public int Pri_defense { get; set; }

        public int Pri_shield { get; set; }

        public string? Pri_defSound { get; set; } = "";
        public int Sec_armour { get; set; }

        public int Sec_defense { get; set; }

        public string? Sec_defSound { get; set; } = "";
        public int Stat_heat { get; set; }

        public int Stat_scrub { get; set; }

        public int Stat_forest { get; set; }

        public int Stat_snow { get; set; }

        public int Stat_sand { get; set; }

        public int Morale { get; set; }

        public string? Discipline { get; set; }

        public string? Training { get; set; }

        public int Stat_charge_dist { get; set; }

        public int Stat_fire_delay { get; set; }

        public int Stat_food { get; set; }

        public int Stat_food_sec { get; set; }

        public int RecruitTime { get; set; }

        public int RecruitCost { get; set; }

        public int Upkeep { get; set; }

        public int WpnCost { get; set; }

        public int ArmourCost { get; set; }

        public int CustomCost { get; set; }

        public int CustomLimit { get; set; }

        public int CustomIncrease { get; set; }

        public float MoveSpeed { get; set; }

        public int[]? Armour_ug_levels { get; set; }

        public string?[]? Armour_ug_models { get; set; }

        public string?[]? Ownership { get; set; }

        public float Recruit_priority_offset { get; set; }

        public string? Info_dict { get; set; }

        public string? Card_dict { get; set; }

        public float CrusadeUpkeep { get; set; }

        public float Radius { get; set; }

        public float Height { get; set; }

        public string? Accent { get; set; } = "";
        public int Spacing_ranks { get; set; }

        public bool LockMorale { get; set; }

        public string? Pri_fire_type { get; set; } = "";
        public string? Sec_fire_type { get; set; } = "";
        public string? Ter_fire_type { get; set; } = "";
        public string? Descr { get; set; } = "";
        public string? DescrShort { get; set; } = "";
        public string Card { get; set; } = "";
        public bool Mercenary_unit { get; set; } = false;
        public bool General_unit { get; set; } = false;
        public int Edu_index { get; set; }

        public string CardInfo { get; set; } = "";

        #endregion Public properties

        public Dictionary<string, IProperty> UnitPropsDictionary = new()
        {
        };

    }
}