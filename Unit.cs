using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModdingTool
{
    internal class Unit
    {
        #region Private fields
        private string unit_type;
        private string unit_name;
        private string unit_descr;
        private string unit_descrShort;
        private string unit_card;
        private string unit_cardInfo;
        private string unit_dictionary;
        private string unit_category;
        private string unit_class;
        private string unit_voice_type;
        private string unit_accent;
        private string unit_bannerFaction;
        private string unit_bannerHoly;
        private string unit_soldier;
        private int unit_soldierCount;
        private int unit_extrasCount;
        private double unit_mass;
        private string unit_officer1;
        private string unit_officer2;
        private string unit_officer3;
        private string unit_ship;
        private string unit_engine;
        private string unit_animal;
        private string unit_mount;
        private string[] unit_mount_effect;
        private string[] unit_attributes;
        private double unit_spacing_width;
        private double unit_spacing_depth;
        private double unit_spacing_width_loose;
        private double unit_spacing_depth_loose;
        private int unit_spacing_ranks;
        private string unit_formation_style;
        private string unit_special_formation;
        private int unit_hitpoints;
        private int unit_mount_hitpoints;
        private int unit_pri_attack;
        private int unit_pri_charge;
        private string unit_pri_projectile;
        private int unit_pri_range;
        private int unit_pri_ammunition;
        private string unit_pri_weapon_type;
        private string unit_pri_tech_type;
        private string unit_pri_damage_type;
        private string unit_pri_sound_type;
        private string unit_pri_fire_type;
        private int unit_pri_att_delay;
        private double unit_pri_skel_factor;
        private string[] unit_pri_attr;
        private int unit_sec_attack;
        private int unit_sec_charge;
        private string unit_sec_projectile;
        private int unit_sec_range;
        private int unit_sec_ammunition;
        private string unit_sec_weapon_type;
        private string unit_sec_tech_type;
        private string unit_sec_damage_type;
        private string unit_sec_sound_type;
        private string unit_sec_fire_type;
        private int unit_sec_att_delay;
        private double unit_sec_skel_factor;
        private string[] unit_sec_attr;
        private int unit_ter_attack;
        private int unit_ter_charge;
        private string unit_ter_projectile;
        private int unit_ter_range;
        private int unit_ter_ammunition;
        private string unit_ter_weapon_type;
        private string unit_ter_tech_type;
        private string unit_ter_damage_type;
        private string unit_ter_sound_type;
        private string unit_ter_fire_type;
        private int unit_ter_att_delay;
        private double unit_ter_skel_factor;
        private string[] unit_ter_attr;
        private int unit_pri_armour;
        private int unit_pri_defense;
        private int unit_pri_shield;
        private string unit_pri_defSound;
        private int unit_sec_armour;
        private int unit_sec_defense;
        private string unit_sec_defSound;
        private int unit_stat_heat;
        private int unit_stat_scrub;
        private int unit_stat_forest;
        private int unit_stat_snow;
        private int unit_stat_sand;
        private int unit_morale;
        private string unit_discipline;
        private string unit_training;
        private int unit_stat_charge_dist;
        private int unit_stat_fire_delay;
        private int unit_stat_food;
        private int unit_stat_food_sec;
        private int unit_recruitTime;
        private int unit_recruitCost;
        private int unit_upkeep;
        private int unit_wpnCost;
        private int unit_armourCost;
        private int unit_customCost;
        private int unit_customLimit;
        private int unit_customIncrease;
        private int[] unit_armour_ug_levels;
        private string[] unit_armour_ug_models;
        private string[] unit_ownership;
        private float unit_recruit_priority_offset;
        private float unit_moveSpeed;
        private string unit_info_dict;
        private string unit_card_dict;
        private float unit_crusadeUpkeep;
        private float unit_radius;
        private float unit_height;
        private bool unit_lockMorale;


        #endregion

        #region Public properties

        public string Unit_name { get => unit_name; set => unit_name = value; }
        public string Unit_type { get => unit_type; set => unit_type = value; }
        public string Unit_dictionary { get => unit_dictionary; set => unit_dictionary = value; }
        public string Unit_category { get => unit_category; set => unit_category = value; }
        public string Unit_class { get => unit_class; set => unit_class = value; }
        public string Unit_voice_type { get => unit_voice_type; set => unit_voice_type = value; }
        public string Unit_soldier { get => unit_soldier; set => unit_soldier = value; }
        public int Unit_soldierCount { get => unit_soldierCount; set => unit_soldierCount = value; }
        public int Unit_extrasCount { get => unit_extrasCount; set => unit_extrasCount = value; }
        public double Unit_mass { get => unit_mass; set => unit_mass = value; }
        public string Unit_officer1 { get => unit_officer1; set => unit_officer1 = value; }
        public string Unit_officer2 { get => unit_officer2; set => unit_officer2 = value; }
        public string Unit_officer3 { get => unit_officer3; set => unit_officer3 = value; }
        public string Unit_ship { get => unit_ship; set => unit_ship = value; }
        public string Unit_engine { get => unit_engine; set => unit_engine = value; }
        public string Unit_animal { get => unit_animal; set => unit_animal = value; }
        public string Unit_mount { get => unit_mount; set => unit_mount = value; }
        public string[] Unit_mount_effect { get => unit_mount_effect; set => unit_mount_effect = value; }
        public string[] Unit_attributes { get => unit_attributes; set => unit_attributes = value; }
        public double Unit_spacing_width { get => unit_spacing_width; set => unit_spacing_width = value; }
        public double Unit_spacing_depth { get => unit_spacing_depth; set => unit_spacing_depth = value; }
        public double Unit_spacing_width_loose { get => unit_spacing_width_loose; set => unit_spacing_width_loose = value; }
        public double Unit_spacing_depth_loose { get => unit_spacing_depth_loose; set => unit_spacing_depth_loose = value; }
        public string Unit_special_formation { get => unit_special_formation; set => unit_special_formation = value; }
        public int Unit_hitpoints { get => unit_hitpoints; set => unit_hitpoints = value; }
        public int Unit_mount_hitpoints { get => unit_mount_hitpoints; set => unit_mount_hitpoints = value; }
        public int Unit_pri_attack { get => unit_pri_attack; set => unit_pri_attack = value; }
        public int Unit_pri_charge { get => unit_pri_charge; set => unit_pri_charge = value; }
        public string Unit_pri_projectile { get => unit_pri_projectile; set => unit_pri_projectile = value; }
        public int Unit_pri_range { get => unit_pri_range; set => unit_pri_range = value; }
        public int Unit_pri_ammunition { get => unit_pri_ammunition; set => unit_pri_ammunition = value; }
        public string Unit_pri_weapon_type { get => unit_pri_weapon_type; set => unit_pri_weapon_type = value; }
        public string Unit_pri_tech_type { get => unit_pri_tech_type; set => unit_pri_tech_type = value; }
        public string Unit_pri_damage_type { get => unit_pri_damage_type; set => unit_pri_damage_type = value; }
        public string Unit_pri_sound_type { get => unit_pri_sound_type; set => unit_pri_sound_type = value; }
        public int Unit_pri_att_delay { get => unit_pri_att_delay; set => unit_pri_att_delay = value; }
        public double Unit_pri_skel_factor { get => unit_pri_skel_factor; set => unit_pri_skel_factor = value; }
        public string[] Unit_pri_attr { get => unit_pri_attr; set => unit_pri_attr = value; }
        public int Unit_sec_attack { get => unit_sec_attack; set => unit_sec_attack = value; }
        public int Unit_sec_charge { get => unit_sec_charge; set => unit_sec_charge = value; }
        public string Unit_sec_projectile { get => unit_sec_projectile; set => unit_sec_projectile = value; }
        public int Unit_sec_range { get => unit_sec_range; set => unit_sec_range = value; }
        public int Unit_sec_ammunition { get => unit_sec_ammunition; set => unit_sec_ammunition = value; }
        public string Unit_sec_weapon_type { get => unit_sec_weapon_type; set => unit_sec_weapon_type = value; }
        public string Unit_sec_tech_type { get => unit_sec_tech_type; set => unit_sec_tech_type = value; }
        public string Unit_sec_damage_type { get => unit_sec_damage_type; set => unit_sec_damage_type = value; }
        public string Unit_sec_sound_type { get => unit_sec_sound_type; set => unit_sec_sound_type = value; }
        public int Unit_sec_att_delay { get => unit_sec_att_delay; set => unit_sec_att_delay = value; }
        public double Unit_sec_skel_factor { get => unit_sec_skel_factor; set => unit_sec_skel_factor = value; }
        public string[] Unit_sec_attr { get => unit_sec_attr; set => unit_sec_attr = value; }
        public int Unit_ter_attack { get => unit_ter_attack; set => unit_ter_attack = value; }
        public int Unit_ter_charge { get => unit_ter_charge; set => unit_ter_charge = value; }
        public string Unit_ter_projectile { get => unit_ter_projectile; set => unit_ter_projectile = value; }
        public int Unit_ter_range { get => unit_ter_range; set => unit_ter_range = value; }
        public int Unit_ter_ammunition { get => unit_ter_ammunition; set => unit_ter_ammunition = value; }
        public string Unit_ter_weapon_type { get => unit_ter_weapon_type; set => unit_ter_weapon_type = value; }
        public string Unit_ter_tech_type { get => unit_ter_tech_type; set => unit_ter_tech_type = value; }
        public string Unit_ter_damage_type { get => unit_ter_damage_type; set => unit_ter_damage_type = value; }
        public string Unit_ter_sound_type { get => unit_ter_sound_type; set => unit_ter_sound_type = value; }
        public int Unit_ter_att_delay { get => unit_ter_att_delay; set => unit_ter_att_delay = value; }
        public double Unit_ter_skel_factor { get => unit_ter_skel_factor; set => unit_ter_skel_factor = value; }
        public string[] Unit_ter_attr { get => unit_ter_attr; set => unit_ter_attr = value; }
        public int Unit_pri_armour { get => unit_pri_armour; set => unit_pri_armour = value; }
        public string Unit_bannerFaction { get => unit_bannerFaction; set => unit_bannerFaction = value; }
        public string Unit_bannerHoly { get => unit_bannerHoly; set => unit_bannerHoly = value; }
        public string Unit_formation_style { get => unit_formation_style; set => unit_formation_style = value; }
        public int Unit_pri_defense { get => unit_pri_defense; set => unit_pri_defense = value; }
        public int Unit_pri_shield { get => unit_pri_shield; set => unit_pri_shield = value; }
        public string Unit_pri_defSound { get => unit_pri_defSound; set => unit_pri_defSound = value; }
        public int Unit_sec_armour { get => unit_sec_armour; set => unit_sec_armour = value; }
        public int Unit_sec_defense { get => unit_sec_defense; set => unit_sec_defense = value; }
        public string Unit_sec_defSound { get => unit_sec_defSound; set => unit_sec_defSound = value; }
        public int Unit_stat_heat { get => unit_stat_heat; set => unit_stat_heat = value; }
        public int Unit_stat_scrub { get => unit_stat_scrub; set => unit_stat_scrub = value; }
        public int Unit_stat_forest { get => unit_stat_forest; set => unit_stat_forest = value; }
        public int Unit_stat_snow { get => unit_stat_snow; set => unit_stat_snow = value; }
        public int Unit_stat_sand { get => unit_stat_sand; set => unit_stat_sand = value; }
        public int Unit_morale { get => unit_morale; set => unit_morale = value; }
        public string Unit_discipline { get => unit_discipline; set => unit_discipline = value; }
        public string Unit_training { get => unit_training; set => unit_training = value; }
        public int Unit_stat_charge_dist { get => unit_stat_charge_dist; set => unit_stat_charge_dist = value; }
        public int Unit_stat_fire_delay { get => unit_stat_fire_delay; set => unit_stat_fire_delay = value; }
        public int Unit_stat_food { get => unit_stat_food; set => unit_stat_food = value; }
        public int Unit_stat_food_sec { get => unit_stat_food_sec; set => unit_stat_food_sec = value; }
        public int Unit_recruitTime { get => unit_recruitTime; set => unit_recruitTime = value; }
        public int Unit_recruitCost { get => unit_recruitCost; set => unit_recruitCost = value; }
        public int Unit_upkeep { get => unit_upkeep; set => unit_upkeep = value; }
        public int Unit_wpnCost { get => unit_wpnCost; set => unit_wpnCost = value; }
        public int Unit_armourCost { get => unit_armourCost; set => unit_armourCost = value; }
        public int Unit_customCost { get => unit_customCost; set => unit_customCost = value; }
        public int Unit_customLimit { get => unit_customLimit; set => unit_customLimit = value; }
        public int Unit_customIncrease { get => unit_customIncrease; set => unit_customIncrease = value; }
        public float Unit_moveSpeed { get => unit_moveSpeed; set => unit_moveSpeed = value; }
        public int[] Unit_armour_ug_levels { get => unit_armour_ug_levels; set => unit_armour_ug_levels = value; }
        public string[] Unit_armour_ug_models { get => unit_armour_ug_models; set => unit_armour_ug_models = value; }
        public string[] Unit_ownership { get => unit_ownership; set => unit_ownership = value; }
        public float Unit_recruit_priority_offset { get => unit_recruit_priority_offset; set => unit_recruit_priority_offset = value; }
        public string Unit_info_dict { get => unit_info_dict; set => unit_info_dict = value; }
        public string Unit_card_dict { get => unit_card_dict; set => unit_card_dict = value; }
        public float Unit_crusadeUpkeep { get => unit_crusadeUpkeep; set => unit_crusadeUpkeep = value; }
        public float Unit_radius { get => unit_radius; set => unit_radius = value; }
        public float Unit_height { get => unit_height; set => unit_height = value; }
        public string Unit_accent { get => unit_accent; set => unit_accent = value; }
        public int Unit_spacing_ranks { get => unit_spacing_ranks; set => unit_spacing_ranks = value; }
        public bool Unit_lockMorale { get => unit_lockMorale; set => unit_lockMorale = value; }
        public string Unit_pri_fire_type { get => unit_pri_fire_type; set => unit_pri_fire_type = value; }
        public string Unit_sec_fire_type { get => unit_sec_fire_type; set => unit_sec_fire_type = value; }
        public string Unit_ter_fire_type { get => unit_ter_fire_type; set => unit_ter_fire_type = value; }
        public string Unit_descr { get => unit_descr; set => unit_descr = value; }
        public string Unit_descrShort { get => unit_descrShort; set => unit_descrShort = value; }
        public string Unit_card { get => unit_card; set => unit_card = value; }


        #endregion



    }
}
