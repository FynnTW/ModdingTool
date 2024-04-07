using Pfim;
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

public partial class UnitTab : Tab
{
    public static Dictionary<string, string> UnitUiText { get; set; } = new Dictionary<string, string>()
        {
            { "LocalizedName", "Localized Name" },
            { "Type", "EDU Type" },
            { "Dictionary", "Dictionary Name" },
            { "Category", "Category" },
            { "ClassType", "Class" },
            { "VoiceType", "Voice type" },
            { "Accent", "Accent" },
            { "BannerFaction", "Banner Faction" },
            { "BannerUnit", "Banner Unit" },
            { "BannerMain", "Banner Main" },
            { "BannerSecondary", "Banner Secondary" },
            { "BannerHoly", "Banner Holy" },
            { "Soldier", "Soldier Animation Model" },
            { "SoldierCount", "Soldier Count" },
            { "ExtrasCount", "Extras Count" },
            { "Mass", "Mass" },
            { "Radius", "Radius" },
            { "Height", "Height" },
            { "Officer1", "Officer" },
            { "Officer2", "Officer" },
            { "Officer3", "Officer" },
            { "MountedEngine", "Mounted engine" },
            { "Mount", "Mount" },
            { "Ship", "Ship" },
            { "Engine", "Engine" },
            { "Animal", "Animal" },
            { "StatStl", "Minimum Soldiers" },
            { "MountEffect", "Mount effects" },
            { "Attributes", "Attributes" },
            { "SpacingWidth", "Spacing width" },
            { "SpacingDepth", "Spacing depth" },
            { "SpacingWidthLoose", "Spacing width (loose)" },
            { "SpacingDepthLoose", "Spacing depth (loose)" },
            { "SpecialFormation", "Special_formation" },
            { "HitPoints", "Hit points" },
            { "MountHitPoints", "Mount hit points" },
            { "PriAttack", "Pri Attack" },
            { "PriCharge", "Pri Charge" },
            { "PriProjectile", "Pri Projectile" },
            { "PriRange", "Pri Range" },
            { "PriAmmunition", "Pri Ammunition" },
            { "PriWeaponType", "Pri Weapon Type" },
            { "PriTechType", "Pri Tech Type" },
            { "PriDamageType", "Pri Damage Type" },
            { "PriSoundType", "Pri Sound Type" },
            { "PriAttDelay", "Pri Attack Delay" },
            { "PriSkelFactor", "Pri Skeleton Factor" },
            { "PriAttr", "Pri Attack Attributes" },
            { "SecAttack", "Sec Attack" },
            { "SecCharge", "Sec Charge" },
            { "SecProjectile", "Sec Projectile" },
            { "SecRange", "Sec Range" },
            { "SecAmmunition", "Sec Ammunition" },
            { "SecWeaponType", "Sec Weapon Type" },
            { "SecTechType", "Sec Tech Type" },
            { "SecDamageType", "Sec Damage Type" },
            { "SecSoundType", "Sec Sound Type" },
            { "SecAttDelay", "Sec Attack Delay" },
            { "SecSkelFactor", "Sec Skeleton Factor" },
            { "SecAttr", "Sec Attack Attributes" },
            { "TerAttack", "Ter Attack" },
            { "TerCharge", "Ter Charge" },
            { "TerProjectile", "Ter Projectile" },
            { "TerRange", "Ter Range" },
            { "TerAmmunition", "Ter Ammunition" },
            { "TerWeaponType", "Ter Weapon Type" },
            { "TerTechType", "Ter Tech Type" },
            { "TerDamageType", "Ter Damage Type" },
            { "TerSoundType", "Ter Sound Type" },
            { "TerAttDelay", "Ter Attack Delay" },
            { "TerSkelFactor", "Ter Skeleton Factor" },
            { "TerAttr", "Tertiary Attack Attributes" },
            { "PriArmour", "Primary Armour" },
            { "PriDefense", "Primary Defense Skill" },
            { "PriShield", "Primary Shield" },
            { "PriDefSound", "Primary Armour Sound" },
            { "SecArmour", "Secondary Armour" },
            { "SecDefense", "Secondary Defense Skill" },
            { "SecDefSound", "Secondary Armour Sound" },
            { "FormationStyle", "Formation Style" },
            { "StatHeat", "Heat Penalty" },
            { "StatScrub", "Scrubs Modifier" },
            { "StatForest", "Forest Modifier" },
            { "StatSnow", "Snow Modifier" },
            { "StatSand", "Desert Modifier" },
            { "Morale", "Morale" },
            { "Discipline", "Discipline" },
            { "Training", "Training" },
            { "StatChargeDist", "Charge Distance" },
            { "StatFireDelay", "Fire Delay" },
            { "StatFood", "Food" },
            { "StatFoodSec", "Food Alt" },
            { "RecruitTime", "Recruitment Time" },
            { "RecruitCost", "Recruitment Cost" },
            { "Upkeep", "Upkeep" },
            { "WpnCost", "Weapon Upgrade Cost" },
            { "ArmourCost", "Armour Upgrade Cost" },
            { "CustomCost", "Custom Battle Cost" },
            { "CustomLimit", "Custom Battle Limit" },
            { "CustomIncrease", "Custom Battle Limit Cost Increase" },
            { "MoveSpeed", "Movement Speed Modifier" },
            { "ArmourUgLevels", "Armour Upgrade Levels" },
            { "ArmourlvlBase", "Base Armour Level" },
            { "ArmourlvlOne", "Upgrade Level One" },
            { "ArmourlvlTwo", "Upgrade Level Two" },
            { "ArmourlvlThree", "Upgrade Level Three" },
            { "ArmourUgModels", "Armour Upgrade Models" },
            { "ArmourModelBase", "Base Model" },
            { "ArmourModelOne", "Upgrade Model 1" },
            { "ArmourModelTwo", "Upgrade Model 2" },
            { "ArmourModelThree", "Upgrade Model 3" },
            { "Ownership", "Ownership Factions" },
            { "EraZero", "Era 0" },
            { "EraOne", "Era 1" },
            { "EraTwo", "Era 2" },
            { "RecruitPriorityOffset", "Recruitment Priority Modifier" },
            { "InfoDict", "Info Card Faction" },
            { "CardDict", "Unit Card Faction" },
            { "CrusadeUpkeep", "Crusading Upkeep Modifier" },
            { "SpacingRanks", "Ranks" },
            { "LockMorale", "Lock Morale" },
            { "PriFireType", "Primary Fire Type" },
            { "SecFireType", "Secondary Fire Type" },
            { "TerFireType", "Tertiary Fire Type" },
            { "Descr", "Unit Description" },
            { "DescrShort", "Unit Short Description" },
            { "Card", "Unit Card" },
            { "CardInfo", "Unit Info Card" },
            { "MercenaryUnit", "Is a Mercenary Unit" },
            { "GeneralUnit", "Is a General's Unit" },
            { "EduIndex", "Edu Index" },
            { "AiUnitValue", "AI Unit Value"},
            { "ValuePerUpkeep", "Value Per Upkeep"},
            { "ValuePerCost", "Value Per Cost"},
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
    public string MountEffectString { get; set; } = "";
    public List<string> FormationStylesX { get; set; } = new List<string> { "square", "horde", "phalanx" };
    public List<string> SpecialFormationStylesX { get; set; } = new List<string> { "wedge", "phalanx", "schiltrom", "shield_wall" };

    public override string ToString()
    {
        return "Units";
    }

    public UnitTab(string name)
    {
        Title = name;
        SelectedUnit = ModData.Units.Get(Title)!;
        Factions = FactionDataBase.Keys.ToList();
        Factions.Add("all");
        Factions.AddRange(CultureDataBase.Keys.ToList());
        ModelEntries = ModData.BattleModelDb.GetNames().ToArray();
        MountEntries = MountDataBase.Keys.ToArray();
        var projectilesList = ProjectileDataBase.Keys.ToList();
        projectilesList.Add("no");
        ProjectileEntries = projectilesList.ToArray();

        if (!ModOptionsInstance.DisableCardImages)
        {
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
        }


        foreach (var effect in SelectedUnit.MountEffect)
        {
            MountEffectString += effect;
            if (effect != SelectedUnit.MountEffect.Last())
            {
                MountEffectString += ", ";
            }
        }
    }



    private static BitmapImage TgaToImageSource(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            return null!;
        }
        Bitmap bitmap;
        var image = Pfimage.FromFile(source, new PfimConfig(bufferSize: 65536));

        PixelFormat format;
        switch (image.Format)
        {
            case ImageFormat.Rgb24:
                format = PixelFormat.Format24bppRgb;
                break;

            case ImageFormat.Rgba32:
                format = PixelFormat.Format32bppArgb;
                break;

            case ImageFormat.R5g5b5:
                format = PixelFormat.Format16bppRgb555;
                break;

            case ImageFormat.R5g6b5:
                format = PixelFormat.Format16bppRgb565;
                break;

            case ImageFormat.R5g5b5a1:
                format = PixelFormat.Format16bppArgb1555;
                break;

            case ImageFormat.Rgb8:
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
        var bitMapImage = new BitmapImage();
        bitMapImage.BeginInit();
        bitMapImage.StreamSource = memory;
        bitMapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitMapImage.EndInit();

        return bitMapImage;
    }
}
