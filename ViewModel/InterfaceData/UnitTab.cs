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
using ModdingTool.Databases;
using static ModdingTool.Globals;
using ImageFormat = Pfim.ImageFormat;

namespace ModdingTool.ViewModel.InterfaceData;

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
    public static string[] Categories => Unit.Categories;
    public static string[] Classes => Unit.Classes;
    public static string[] DamageTypes => Unit.DamageTypes;
    public static string[] WeaponTypes => Unit.WeaponTypes;
    public static string[] SoundTypes => Unit.SoundTypes;
    public static string[] VoiceTypes => Unit.VoiceTypes;
    public static List<string> Factions => new List<string> { "all", string.Empty }
        .Concat(FactionDataBase.Keys)
        .Concat(CultureDataBase.Keys)
        .ToList();
    public static List<string> ModelEntries => new(ModData.BattleModelDb.GetNames()) { string.Empty };
    public static List<string> MountEntries => new(MountDataBase.Keys.ToList()) { string.Empty };
    public static List<string> ProjectileEntries => new(ProjectileDataBase.Keys.ToList()) { "no", string.Empty };
    
    public static string PriAnimation { get; set; }
    public static string SecAnimation { get; set; }

    public static string[] SoundTypesDef => Unit.SoundTypesDef;
    public static List<string> AttributeTypes => ModData.Units.AttributeTypes;
    
    public static string[] DisciplineTypes => Unit.DisciplineTypes;
    public static string[] TrainedTypes => Unit.DisciplineTypes;
    public static string[] TechTypes  => Unit.TechTypes;
    public static List<string> FormationStyles => Unit.FormationStyles;
    public static List<string> SpecialFormationStyles => Unit.SpecialFormationStyles;
    public static List<string> AttackAttr => Unit.AttackAttr;
    public BitmapImage UnitImage { get; set; } = new();
    public BitmapImage UnitInfoImage { get; set; } = new();
    public BitmapImage FactionSymbolImage { get; set; } = new();
    public string MountEffectString => string.Join(", ", SelectedUnit.MountEffect);
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

        if (ModOptionsInstance.DisableCardImages) return;
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



    private static BitmapImage TgaToImageSource(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return null!;
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
