﻿using Pfim;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;
using static ModdingTool.Globals;
using ImageFormat = Pfim.ImageFormat;

namespace ModdingTool.View.InterfaceData;

public class UnitTab : ITab.Tab
{
    public static Dictionary<string, string> UnitUiText { get; set; } = new Dictionary<string, string>()
        {
            { "Name", "Localized Name" },
            { "Type", "EDU Type" },
            { "Dictionary", "Dictionary Name" },
            { "Category", "Category" },
            { "Class_type", "Class" },
            { "Voice_type", "Voice type" },
            { "Soldier", "Soldier Animation Model" },
            { "SoldierCount", "Soldier Count" },
            { "ExtrasCount", "Extras Count" },
            { "Mass", "Mass" },
            { "Officer1", "Officer" },
            { "Officer2", "Officer" },
            { "Officer3", "Officer" },
            { "Ship", "Ship" },
            { "Engine", "Engine" },
            { "Animal", "Animal" },
            { "Mount", "Mount" },
            { "Stat_stl", "Minimum Soldiers" },
            { "Mounted_engine", "Mounted engine" },
            { "Mount_effect", "Mount effects" },
            { "Attributes", "Attributes" },
            { "Spacing_width", "Spacing width" },
            { "Spacing_depth", "Spacing depth" },
            { "Spacing_width_loose", "Spacing width (loose)" },
            { "Spacing_depth_loose", "Spacing depth (loose)" },
            { "Special_formation", "Special_formation" },
            { "Hitpoints", "Hitpoints" },
            { "Mount_hitpoints", "Mount hitpoints" },
            { "Pri_attack", "Pri Attack" },
            { "Pri_charge", "Pri Charge" },
            { "Pri_projectile", "Pri Projectile" },
            { "Pri_range", "Pri Range" },
            { "Pri_ammunition", "Pri Ammunition" },
            { "Pri_weapon_type", "Pri Weapon Type" },
            { "Pri_tech_type", "Pri Tech Type" },
            { "Pri_damage_type", "Pri Damage Type" },
            { "Pri_sound_type", "Pri Sound Type" },
            { "Pri_att_delay", "Pri Attack Delay" },
            { "Pri_skel_factor", "Pri Skeleton Factor" },
            { "Pri_attr", "Pri Attack Attributes" },
            { "Sec_attack", "Sec Attack" },
            { "Sec_charge", "Sec Charge" },
            { "Sec_projectile", "Sec Projectile" },
            { "Sec_range", "Sec Range" },
            { "Sec_ammunition", "Sec Ammunition" },
            { "Sec_weapon_type", "Sec Weapon Type" },
            { "Sec_tech_type", "Sec Tech Type" },
            { "Sec_damage_type", "Sec Damage Type" },
            { "Sec_sound_type", "Sec Sound Type" },
            { "Sec_att_delay", "Sec Attack Delay" },
            { "Sec_skel_factor", "Sec Skeleton Factor" },
            { "Sec_attr", "Sec Attack Attributes" },
            { "Ter_attack", "Ter Attack" },
            { "Ter_charge", "Ter Charge" },
            { "Ter_projectile", "Ter Projectile" },
            { "Ter_range", "Ter Range" },
            { "Ter_ammunition", "Ter Ammunition" },
            { "Ter_weapon_type", "Ter Weapon Type" },
            { "Ter_tech_type", "Ter Tech Type" },
            { "Ter_damage_type", "Ter Damage Type" },
            { "Ter_sound_type", "Ter Sound Type" },
            { "Ter_att_delay", "Ter Attack Delay" },
            { "Ter_skel_factor", "Ter Skeleton Factor" },
            { "Ter_attr", "Tertiary Attack Attributes" },
            { "Pri_armour", "Primary Armour" },
            { "Pri_defense", "Primary Defense Skill" },
            { "Pri_shield", "Primary Shield" },
            { "Pri_defSound", "Primary Armour Sound" },
            { "Sec_armour", "Secondary Armour" },
            { "Sec_defense", "Secondary Defense Skill" },
            { "Sec_defSound", "Secondary Armour Sound" },
            { "BannerFaction", "Banner Faction" },
            { "BannerHoly", "Banner Faction Holy" },
            { "Formation_style", "Formation Style" },
            { "Stat_heat", "Heat Penalty" },
            { "Stat_scrub", "Scrubs Modifier" },
            { "Stat_forest", "Forest Modifier" },
            { "Stat_snow", "Snow Modifier" },
            { "Stat_sand", "Desert Modifier" },
            { "Morale", "Morale" },
            { "Discipline", "Discipline" },
            { "Training", "Training" },
            { "Stat_charge_dist", "Charge Distance" },
            { "Stat_fire_delay", "Fire Delay" },
            { "Stat_food", "Food" },
            { "Stat_food_sec", "Food Alt" },
            { "RecruitTime", "Recruitment Time" },
            { "RecruitCost", "Recruitment Cost" },
            { "Upkeep", "Upkeep" },
            { "WpnCost", "Weapon Upgrade Cost" },
            { "ArmourCost", "Armour Upgrade Cost" },
            { "CustomCost", "Custom Battle Cost" },
            { "CustomLimit", "Custom Battle Limit" },
            { "CustomIncrease", "Custom Battle Limit Cost Increase" },
            { "MoveSpeed", "Movement Speed Modifier" },
            { "Armour_ug_levels", "Armour Upgrade Levels" },
            { "ArmourlvlBase", "Base Armour Level" },
            { "ArmourlvlOne", "Upgrade Level One" },
            { "ArmourlvlTwo", "Upgrade Level Two" },
            { "ArmourlvlThree", "Upgrade Level Three" },
            { "Armour_ug_models", "Armour Upgrade Models" },
            { "ArmourModelBase", "Base Model" },
            { "ArmourModelOne", "Upgrade Model 1" },
            { "ArmourModelTwo", "Upgrade Model 2" },
            { "ArmourModelThree", "Upgrade Model 3" },
            { "Ownership", "Ownership Factions" },
            { "EraZero", "Era 0" },
            { "EraOne", "Era 1" },
            { "EraTwo", "Era 2" },
            { "Recruit_priority_offset", "Recruitment Priority Modifier" },
            { "Info_dict", "Info Card Faction" },
            { "Card_dict", "Unit Card Faction" },
            { "CrusadeUpkeep", "Crusading Upkeep Modifier" },
            { "Radius", "Radius" },
            { "Height", "Height" },
            { "Accent", "Accent" },
            { "Spacing_ranks", "Ranks" },
            { "LockMorale", "Lock Morale" },
            { "Pri_fire_type", "Primary Fire Type" },
            { "Sec_fire_type", "Secondary Fire Type" },
            { "Ter_fire_type", "Tertiary Fire Type" },
            { "Descr", "Unit Description" },
            { "DescrShort", "Unit Short Description" },
            { "Card", "Unit Card" },
            { "Mercenary_unit", "Is a Mercenary Unit" },
            { "General_unit", "Is a General's Unit" },
            { "Edu_index", "Edu Index" },
            {"AIUnitValue", "AI Unit Value"},
            { "CardInfo", "Unit Info Card" }
        };
    public Unit SelectedUnit { get; set; }
    public static string[] Categories { get; set; } = new string[] { "infantry", "cavalry", "siege", "handler", "ship", "non_combatant" };
    public static string[] Classes { get; set; } = new string[] { "light", "heavy", "missile", "spearmen", "skirmish" };
    public static string[] DamageTypes { get; set; } = new string[] { "piercing", "slashing", "blunt", "fire" };
    public static string[] WeaponTypes { get; set; } = new string[] { "melee", "thrown", "missile", "siege_missile", "no" };
    public static string[] SoundTypes { get; set; } = new string[] { "none", "knife", "mace", "axe", "sword", "spear" };
    public static string[] VoiceTypes { get; set; } = new string[] { "General", "Heavy", "Light", "Female", "Medium" };
    public static List<string> Factions { get; set; }
    public static string[] ModelEntries { get; set; }
    public static string[] MountEntries { get; set; }
    public static string[] ProjectileEntries { get; set; }
    public static string PriAnimation { get; set; }
    public static string SecAnimation { get; set; }

    public static string[] SoundTypesDef { get; set; } = new string[]
    {
        "flesh", "leather", "ground", "building", "metal"
    };
    public static List<string> AttributeTypes { get; set; } = new List<string>
    {
        "can_withdraw",
        "can_sap",
        "hide_long_grass",
        "hide_anywhere",
        "sea_faring",
        "gunpowder_unit",
        "screeching_women",
        "druid",
        "cantabrian_circle",
        "is_peasant",
        "no_custom",
        "start_not_skirmishing",
        "fire_by_rank",
        "gunpowder_artillery_unit",
        "command",
        "free_upkeep_unit",
        "heavy",
        "hardy",
        "mercenary_unit",
        "frighten_foot",
        "frighten_mounted",
        "very_hardy",
        "slave",
        "power_charge",
        "hide_forest",
        "can_horde",
        "can_swim",
        "can_formed_charge",
        "can_feign_rout",
        "can_run_amok",
        "warcry",
        "stakes",
        "general_unit",
        "general_unit_upgrade",
        "legionary_name",
        "wagon_fort",
        "cannot_skirmish",
        "hide_improved_forest"
    };
    public static string[] DisciplineTypes { get; set; } = new string[]
    {
        "impetuous", "normal", "disciplined", "berserker","low"
    };
    public static string[] TrainedTypes { get; set; } = new string[]
    {
        "trained", "highly_trained", "untrained"
    };
    public static string[] TechTypes { get; set; } = new string[] { "melee_simple", "missile_mechanical", "melee_blade", "missile_gunpowder", "artillery_mechanical", "artillery_gunpowder" };
    public static List<string> FormationStyles { get; set; } = new List<string> { "square", "horde", "phalanx" };
    public static List<string> SpecialFormationStyles { get; set; } = new List<string> { "wedge", "phalanx", "schiltrom", "shield_wall" };
    public static List<string> AttackAttr { get; set; } = new List<string> { "spear", "light_spear", "prec", "ap", "bp", "area", "fire", "launching", "thrown", "short_pike", "long_pike", "spear_bonus_12", "spear_bonus_10", "spear_bonus_8", "spear_bonus_6", "spear_bonus_4" };
    public BitmapImage UnitImage { get; set; }
    public BitmapImage UnitInfoImage { get; set; }
    public BitmapImage FactionSymbolImage { get; set; }
    public string mountEffectString { get; set; } = "";
    public List<string> FormationStylesX { get; set; } = new List<string> { "square", "horde", "phalanx" };
    public List<string> SpecialFormationStylesX { get; set; } = new List<string> { "wedge", "phalanx", "schiltrom", "shield_wall" };



    public UnitTab(string name)
    {
        Title = name;
        SelectedUnit = UnitDataBase[Title];
        Factions = FactionDataBase.Keys.ToList();
        Factions.Add("all");
        Factions.AddRange(CultureDataBase.Keys.ToList());
        ModelEntries = BattleModelDataBase.Keys.ToArray();
        MountEntries = MountDataBase.Keys.ToArray();
        var projectilesList = ProjectileDataBase.Keys.ToList();
        projectilesList.Add("no");
        ProjectileEntries = projectilesList.ToArray();

        try
        {
            UnitInfoImage = TgaToImageSource(SelectedUnit.CardInfo);
        }
        catch (Exception e)
        {
            ErrorDb.AddError("Error reading " + SelectedUnit.CardInfo);
            ErrorDb.AddError(e.Message);
        }
        try
        {
            UnitImage = TgaToImageSource(SelectedUnit.Card);
        }
        catch (Exception e)
        {
            ErrorDb.AddError("Error reading " + SelectedUnit.Card);
            ErrorDb.AddError(e.Message);
        }
        try
        {
            FactionSymbolImage = TgaToImageSource(SelectedUnit.FactionSymbol);
        }
        catch (Exception e)
        {
            ErrorDb.AddError("Error reading " + SelectedUnit.FactionSymbol);
            ErrorDb.AddError(e.Message);
        }


        foreach (var effect in SelectedUnit.Mount_effect)
        {
            mountEffectString += effect;
            if (effect != SelectedUnit.Mount_effect.Last())
            {
                mountEffectString += ", ";
            }
        }
    }



    private static BitmapImage TgaToImageSource(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            return null;
        }
        Bitmap bitmap;
        var image = Pfimage.FromFile(source, new PfimConfig(bufferSize: 65536));

        PixelFormat format;
        switch (image.Format)
        {
            case Pfim.ImageFormat.Rgb24:
                format = PixelFormat.Format24bppRgb;
                break;

            case Pfim.ImageFormat.Rgba32:
                format = PixelFormat.Format32bppArgb;
                break;

            case Pfim.ImageFormat.R5g5b5:
                format = PixelFormat.Format16bppRgb555;
                break;

            case Pfim.ImageFormat.R5g6b5:
                format = PixelFormat.Format16bppRgb565;
                break;

            case Pfim.ImageFormat.R5g5b5a1:
                format = PixelFormat.Format16bppArgb1555;
                break;

            case Pfim.ImageFormat.Rgb8:
                format = PixelFormat.Format8bppIndexed;
                break;

            case ImageFormat.Rgba16:
            default:
                var msg = $"{image.Format} is not recognized for Bitmap on Windows Forms. " +
                           "You'd need to write a conversion function to convert the data to known format";
                const string caption = "Unrecognized format";
                MessageBox.Show(msg, caption);
                format = PixelFormat.Format32bppArgb;
                break;
        }
        var handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
        try
        {
            var data = Marshal.UnsafeAddrOfPinnedArrayElement(image.Data, 0);
            bitmap = new Bitmap(image.Width, image.Height, image.Stride, format, data);
            bitmap.MakeTransparent();
        }
        finally
        {
            handle.Free();
        }

        using var memory = new MemoryStream();
        bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
        memory.Position = 0;
        var bitmapimage = new BitmapImage();
        bitmapimage.BeginInit();
        bitmapimage.StreamSource = memory;
        bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapimage.EndInit();

        return bitmapimage;
    }
}
