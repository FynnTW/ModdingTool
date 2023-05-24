using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;
using ModdingTool.View.UserControls;
using Pfim;
using ImageFormat = Pfim.ImageFormat;
using static ModdingTool.Globals;

namespace ModdingTool.View.InterfaceData;

public class UnitTab : ITab.Tab
{
    public Unit SelectedUnit { get; set; }
    public static string[] Categories { get; set; }
    public static string[] Classes { get; set; }
    public static string[] DamageTypes { get; set; }
    public static string[] WeaponTypes { get; set; }
    public static string[] SoundTypes { get; set; }
    public static string[] TechTypes { get; set; }
    public BitmapImage UnitImage { get; set; }
    public BitmapImage UnitInfoImage { get; set; }



    public Dictionary<string, string> UnitUiText { get; set; }

    public UnitTab()
    {
        UnitUiText = new Dictionary<string, string>()
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
            { "Mount_effect", "Mount effects" },
            { "Attributes", "Attributes" },
            { "Spacing_width", "Spacing width" },
            { "Spacing_depth", "Spacing depth" },
            { "Spacing_width_loose", "Spacing width (loose)" },
            { "Spacing_depth_loose", "Spacing depth (loose)" },
            { "Special_formation", "Special_formation" },
            { "Hitpoints", "Hitpoints" },
            { "Mount_hitpoints", "Mount hitpoints" },
            { "Pri_attack", "Primary Attack" },
            { "Pri_charge", "Primary Charge" },
            { "Pri_projectile", "Primary Projectile" },
            { "Pri_range", "Primary Range" },
            { "Pri_ammunition", "Primary Ammunition" },
            { "Pri_weapon_type", "Primary Weapon Type" },
            { "Pri_tech_type", "Primary Tech Type" },
            { "Pri_damage_type", "Primary Damage Type" },
            { "Pri_sound_type", "Primary Sound Type" },
            { "Pri_att_delay", "Primary Attack Delay" },
            { "Pri_skel_factor", "Primary Skeleton Factor" },
            { "Pri_attr", "Primary Attack Attributes" },
            { "Sec_attack", "Secondary Attack" },
            { "Sec_charge", "Secondary Charge" },
            { "Sec_projectile", "Secondary Projectile" },
            { "Sec_range", "Secondary Range" },
            { "Sec_ammunition", "Secondary Ammunition" },
            { "Sec_weapon_type", "Secondary Weapon Type" },
            { "Sec_tech_type", "Secondary Tech Type" },
            { "Sec_damage_type", "Secondary Damage Type" },
            { "Sec_sound_type", "Secondary Sound Type" },
            { "Sec_att_delay", "Secondary Attack Delay" },
            { "Sec_skel_factor", "Secondary Skeleton Factor" },
            { "Sec_attr", "Secondary Attack Attributes" },
            { "Ter_attack", "Tertiary Attack" },
            { "Ter_charge", "Tertiary Charge" },
            { "Ter_projectile", "Tertiary Projectile" },
            { "Ter_range", "Tertiary Range" },
            { "Ter_ammunition", "Tertiary Ammunition" },
            { "Ter_weapon_type", "Tertiary Weapon Type" },
            { "Ter_tech_type", "Tertiary Tech Type" },
            { "Ter_damage_type", "Tertiary Damage Type" },
            { "Ter_sound_type", "Tertiary Sound Type" },
            { "Ter_att_delay", "Tertiary Attack Delay" },
            { "Ter_skel_factor", "Tertiary Skeleton Factor" },
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
            { "Armour_ug_models", "Armour Upgrade Models" },
            { "Ownership", "Ownership Factions" },
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
            { "Edu_index", "Edu Index: " },
            { "CardInfo", "Unit Info Card" }
        };
        Categories = new string[] { "infantry", "cavalry", "siege", "handler", "ship", "non_combatant" };
        Classes = new string[] { "light", "heavy", "missile", "spearmen", "skirmish" };
        DamageTypes = new string[] { "piercing", "slashing", "blunt", "fire" };
        WeaponTypes = new string[] { "melee", "thrown", "missile", "siege_missile" };
        SoundTypes = new string[] { "none", "knife", "mace", "axe", "sword", "spear" };
        TechTypes = new string[] { "melee_simple", "missile_mechanical", "melee_blade", "missile_gunpowder", "artillery_mechanical", "artillery_gunpowder" };
        SelectedUnit = AllUnits[Name];
        UnitInfoImage = TgaToImageSource(SelectedUnit.CardInfo);
        UnitImage = TgaToImageSource(SelectedUnit.Card);
    }



    private static BitmapImage TgaToImageSource(string source)
    {
        Bitmap bitmap;
        var image = Pfimage.FromFile(source);
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
