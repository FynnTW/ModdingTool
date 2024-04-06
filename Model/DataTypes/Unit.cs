using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static ModdingTool.Globals;

namespace ModdingTool
{
    public class Unit : GameType
    {
        private static readonly List<string> EduIdentifiers = new()
        {
            "type",
            "dictionary",
            "category",
            "class",
            "voice_type",
            "accent",
            "banner faction",
            "banner unit",
            "banner holy",
            "banner main",
            "banner secondary",
            "banner mini",
            "soldier",
            "officer",
            "mounted_engine",
            "mount",
            "ship",
            "engine",
            "animal",
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
            "stat_stl",
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
        public static string[] Categories { get; set; } = { "infantry", "cavalry", "siege", "handler", "ship", "non_combatant" };
        public static string[] Classes { get; set; } = { "light", "heavy", "missile", "spearmen", "skirmish" };
        public static string[] DamageTypes { get; set; } = { "piercing", "slashing", "blunt", "fire" };
        public static string[] WeaponTypes { get; set; } = { "melee", "thrown", "missile", "siege_missile", "no" };
        public static string[] SoundTypes { get; set; } = { "none", "knife", "mace", "axe", "sword", "spear" };
        public static string[] VoiceTypes { get; set; } = { "general", "heavy", "medium", "light", "female", "general_1", "heavy_1", "medium_1", "light_1", "female_1" };
        
        #region Public properties
        
        private string _localizedName = "";
        private string _dictionary = "";
        private string _category = "infantry";
        private string _class = "light";
        private string _voiceType = "Light";
        private string _soldier = "";
        private string? _accent;
        private string? _bannerFaction = "main_infantry";
        private string? _bannerUnit;
        private string? _bannerMain;
        private string? _bannerSecondary;
        private string? _bannerHoly;
        private int _soldierCount = 4;
        private int _extrasCount;
        private double _mass = 1.0;
        private double _radius = 0.40;
        private double _height = 1.70;
        private string _officer1 = "";
        private string _officer2 = "";
        private string _officer3 = "";
        private string _mountedEngine = "";
        private string _mount = "";
        private string _ship = "";
        private string _engine = "";
        private string _animal = "";


        public string LocalizedName
        {
            get => _localizedName;
            set
            {
                AddChange(nameof(LocalizedName), _localizedName, value);
                _localizedName = value;
            }
        }

        public string Type
        {
            get => _name;
            set
            {
                var old = _name;
                if (ModData.Units.Contains(value))
                    ErrorDb.AddError($"Unit {value} already exists.");
                else
                {
                    _name = value;
                    if (string.IsNullOrWhiteSpace(old) || !ModData.Units.Contains(old)) return;
                    AddChange(nameof(Type), old, value);
                    var index = EduIndex;
                    ModData.Units.Remove(old);
                    ModData.Units.Add(this);
                    EduIndex = index;
                }
            }
        }

        public string? Dictionary
        {
            get => _dictionary;
            set
            {
                if (value == null) return;
                AddChange(nameof(Dictionary), _dictionary, value);
                _dictionary = value;
            }
        }

        public string? Category
        {
            get => _category;
            set
            {
                if (value == null) return;
                if (!Categories.Contains(value.ToLower()))
                {
                    ErrorDb.AddError($"Category {value} does not exist.");
                    return;
                }
                AddChange(nameof(Category), _category, value);
                _category = value;
            }
        }

        public string? ClassType
        {
            get => _class;
            set
            {
                if (value == null) return;
                if (!Classes.Contains(value.ToLower()))
                {
                    ErrorDb.AddError($"Class {value} does not exist.");
                    return;
                }
                AddChange(nameof(ClassType), _class, value);
                _class = value;
            }
        }
        public string? VoiceType 
        {
            get => _voiceType;
            set
            {
                if (value == null) return;
                if (!VoiceTypes.Contains(value.ToLower()))
                {
                    ErrorDb.AddError($"Voice Type {value} does not exist.");
                    return;
                }
                AddChange(nameof(ClassType), _voiceType, value);
                _voiceType = value;
            }
        }

        public string? Accent
        {
            get => _accent;
            set
            {
                if (value == null) return;
                AddChange(nameof(Accent), _accent ?? "", value);
                _accent = value;
            }
        }

        public string? BannerFaction
        {
            get => _bannerFaction;
            set
            {
                if (value == null) return;
                AddChange(nameof(BannerFaction), _bannerFaction ?? "", value);
                _bannerFaction = value;
            }
        } 
        public string? BannerUnit
        {
            get => _bannerUnit;
            set
            {
                if (value == null) return;
                AddChange(nameof(BannerUnit), _bannerUnit ?? "", value);
                _bannerUnit = value;
            }
        } 
        public string? BannerMain
        {
            get => _bannerMain;
            set
            {
                if (value == null) return;
                AddChange(nameof(BannerMain), _bannerMain ?? "", value);
                _bannerMain = value;
            }
        } 
        public string? BannerSecondary
        {
            get => _bannerSecondary;
            set
            {
                if (value == null) return;
                AddChange(nameof(BannerSecondary), _bannerSecondary ?? "", value);
                _bannerSecondary = value;
            }
        } 
        public string? BannerHoly 
        {
            get => _bannerHoly;
            set
            {
                if (value == null) return;
                AddChange(nameof(BannerHoly), _bannerHoly ?? "", value);
                _bannerHoly = value;
            }
        } 

        public string? Soldier
        {
            get => _soldier;
            set
            {
                if (value == null) return;
                if (!ModData.BattleModelDb.Contains(value))
                    ErrorDb.AddError($"Soldier {value} does not exist in battle models database.");
                AddChange(nameof(Soldier), _soldier, value);
                _soldier = value;
            }
        }

        public int SoldierCount
        {
            get => _soldierCount;
            set
            {
                if (Category == "non_combatant")
                {
                    if (value is 0 or > 300)
                        ErrorDb.AddError($"Invalid number of soldiers {value} for unit {Type}.");
                }
                else
                {
                    if (value is < 4 or > 100)
                        ErrorDb.AddError($"Invalid number of soldiers {value} for unit {Type}.");
                }
                AddChange(nameof(SoldierCount), _soldierCount, value);
                _soldierCount = value;
            }
        }

        public int ExtrasCount
        {
            get => _extrasCount;
            set
            {
                if (value is < 0 or > 300)
                    ErrorDb.AddError($"Invalid number of extras {value} for unit {Type}.");
                if (Category == "handler" && value / SoldierCount > 3)
                    ErrorDb.AddError($"Invalid number of animals {value} for handler unit {Type}.");
                AddChange(nameof(ExtrasCount), _extrasCount, value);
                _extrasCount = value;
            }
        }

        public double Mass
        {
            get => _mass;
            set
            {
                AddChange(nameof(Mass), _mass, value);
                _mass = value;
            }
        }

        public double Radius
        {
            get => _radius;
            set
            {
                AddChange(nameof(Radius), _radius, value);
                _radius = value;
            }
        }

        public double Height
        {
            get => _height;
            set
            {
                AddChange(nameof(Height), _height, value);
                _height = value;
            }
        }

        public string? Officer1
        {
            get => _officer1;
            set
            {
                if (value == null) return;
                if (!ModData.BattleModelDb.Contains(value))
                    ErrorDb.AddError($"Officer {value} does not exist in battle models database.");
                AddChange(nameof(Officer1), _officer1, value);
                _officer1 = value;
            }
        }

        public string? Officer2
        {
            get => _officer2;
            set
            {
                if (value == null) return;
                if (!ModData.BattleModelDb.Contains(value))
                    ErrorDb.AddError($"Officer {value} does not exist in battle models database.");
                AddChange(nameof(Officer2), _officer2, value);
                _officer2 = value;
            }
        }
        public string? Officer3
        {
            get => _officer3;
            set
            {
                if (value == null) return;
                if (!ModData.BattleModelDb.Contains(value))
                    ErrorDb.AddError($"Officer {value} does not exist in battle models database.");
                AddChange(nameof(Officer3), _officer3, value);
                _officer3 = value;
            }
        }
        public string? MountedEngine
        {
            get => _mountedEngine;
            set
            {
                if (value == null) return;
                AddChange(nameof(MountedEngine), _mountedEngine, value);
                _mountedEngine = value;
            }
        }

        public string? Mount
        {
            get => _mount;
            set
            {
                if (value == null) return;
                if (!MountDataBase.ContainsKey(value))
                    ErrorDb.AddError($"Mount {value} does not exist in mounts database.");
                AddChange(nameof(Mount), _mount, value);
                _mount = value;
            }
        }

        public string? Ship
        {
            get => _ship;
            set
            {
                if (value == null) return;
                AddChange(nameof(Ship), _ship, value);
                _ship = value;
            }
        }

        public string? Engine
        {
            get => _engine;
            set
            {
                if (value == null) return;
                AddChange(nameof(Engine), _engine, value);
                _engine = value;
            }
        }

        public string? Animal
        {
            get => _animal;
            set
            {
                if (value == null) return;
                AddChange(nameof(Animal), _animal, value);
                _animal = value;
            }
        }
        public List<string> MountEffect { get; set; } = new List<string>();

        public List<string> Attributes { get; set; } = new List<string>();

        public double SpacingWidth { get; set; } = 1.4;

        public double SpacingDepth { get; set; } = 1.4;

        public double SpacingWidthLoose { get; set; } = 2.4;

        public double SpacingDepthLoose { get; set; } = 2.4;

        public string? SpecialFormation { get; set; } = "";

        public int HitPoints { get; set; } = 1;

        public int MountHitPoints { get; set; } = 1;

        public int PriAttack { get; set; } = 1;

        public int PriCharge { get; set; } = 1;

        public string? PriProjectile { get; set; } = "";
        public int PriRange { get; set; } = 0;

        public int PriAmmunition { get; set; } = 0;

        public string? PriWeaponType { get; set; } = "melee";
        public string? PriTechType { get; set; } = "melee_blade";
        public string? PriDamageType { get; set; } = "blunt";
        public string? PriSoundType { get; set; } = "none";
        public int PriAttDelay { get; set; } = 0;

        public double PriSkelFactor { get; set; } = 1.0;

        public List<string>? PriAttr { get; set; } = new List<string>();

        public int SecAttack { get; set; } = 0;

        public int SecCharge { get; set; } = 0;

        public string? SecProjectile { get; set; } = "";
        public int SecRange { get; set; } = 0;

        public int SecAmmunition { get; set; } = 0;

        public string? SecWeaponType { get; set; } = "no";
        public string? SecTechType { get; set; } = "melee_simple";
        public string? SecDamageType { get; set; } = "blunt";
        public string? SecSoundType { get; set; } = "none";
        public int SecAttDelay { get; set; } = 0;

        public double SecSkelFactor { get; set; } = 1;

        public List<string>? SecAttr { get; set; } = new List<string>();

        public int TerAttack { get; set; } = 0;
        public int TerCharge { get; set; } = 0;
        public string? TerProjectile { get; set; } = "";
        public int TerRange { get; set; } = 0;
        public int TerAmmunition { get; set; } = 0;
        public string? TerWeaponType { get; set; } = "no";
        public string? TerTechType { get; set; } = "melee_simple";
        public string? TerDamageType { get; set; } = "blunt";
        public string? TerSoundType { get; set; } = "none";
        public int TerAttDelay { get; set; } = 0;
        public double TerSkelFactor { get; set; } = 0;
        public List<string>? TerAttr { get; set; } = new List<string>();

        public int PriArmour { get; set; } = 0;
        public string? FormationStyle { get; set; } = "square";
        public int PriDefense { get; set; } = 0;

        public int PriShield { get; set; } = 0;

        public string? PriDefSound { get; set; } = "flesh";
        public int SecArmour { get; set; } = 0;

        public int SecDefense { get; set; } = 0;

        public string? SecDefSound { get; set; } = "flesh";
        public int StatHeat { get; set; } = 0;

        public int StatScrub { get; set; } = 0;

        public int StatForest { get; set; } = 0;

        public int StatSnow { get; set; } = 0;

        public int StatSand { get; set; } = 0;

        public int Morale { get; set; } = 1;

        public string? Discipline { get; set; } = "normal";

        public string? Training { get; set; } = "untrained";

        public int StatChargeDist { get; set; } = 10;

        public int StatFireDelay { get; set; } = 0;

        public int StatFood { get; set; } = 60;

        public int StatFoodSec { get; set; } = 300;

        public int RecruitTime { get; set; } = 1;

        public int RecruitCost { get; set; } = 500;

        public int Upkeep { get; set; } = 200;

        public int WpnCost { get; set; } = 100;

        public int ArmourCost { get; set; } = 100;

        public int CustomCost { get; set; } = 500;

        public int CustomLimit { get; set; } = 3;

        public int CustomIncrease { get; set; } = 50;

        public double MoveSpeed { get; set; } = 1.0;
        public int StatStl { get; set; } = 0;

        public int[]? ArmourUgLevels { get; set; }

        public string? ArmourlvlBase { get; set; }

        public string? ArmourlvlOne { get; set; }

        public string? ArmourlvlTwo { get; set; }

        public string? ArmourlvlThree { get; set; }

        public string?[]? ArmourUgModels { get; set; }

        public string? ArmourModelBase { get; set; }

        public string? ArmourModelOne { get; set; }

        public string? ArmourModelTwo { get; set; }

        public string? ArmourModelThree { get; set; }

        public List<string> Ownership { get; set; } = new List<string>();

        public List<string> EraZero { get; set; } = new List<string>();

        public List<string> EraOne { get; set; } = new List<string>();

        public List<string> EraTwo { get; set; } = new List<string>();

        public double RecruitPriorityOffset { get; set; } = 0;

        public string? InfoDict { get; set; }

        public string? CardDict { get; set; }

        public double CrusadeUpkeep { get; set; } = 1.0;
        public int SpacingRanks { get; set; }

        public bool LockMorale { get; set; } = false;

        public string? PriFireType { get; set; } = "";
        public string? SecFireType { get; set; } = "";
        public string? TerFireType { get; set; } = "";
        public string? Descr { get; set; } = "";
        public string? DescrShort { get; set; } = "";
        public string Card { get; set; } = "";
        public bool MercenaryUnit { get; set; } = false;
        public bool GeneralUnit { get; set; } = false;
        public bool IsEopUnit { get; set; } = false;
        public string FilePath { get; set; } = "";
        public int EduIndex { get; set; }

        public double AiUnitValue { get; set; }
        
        public double ValuePerCost => Math.Round(AiUnitValue / RecruitCost, 2);

        public double ValuePerUpkeep => Math.Round(AiUnitValue / Upkeep, 2);

        public string CardInfo { get; set; } = "";
        public string FactionSymbol { get; set; } = "";

        #endregion Public properties

        public static Unit? CloneUnit(string name, string localizedName, Unit unit)
        {
            if (ModData.Units.Contains(name))
                return null;
            var newUnit = (Unit)unit.MemberwiseClone();
            newUnit.MountEffect = new List<string>(unit.MountEffect);
            newUnit.Attributes = new List<string>(unit.Attributes);
            newUnit.Comments = new Dictionary<string, List<string>>(unit.Comments);
            newUnit.CommentsInLine = new Dictionary<string, string>(unit.CommentsInLine);
            newUnit.Ownership = new List<string>(unit.Ownership);
            newUnit.EraZero = new List<string>(unit.EraZero);
            newUnit.EraOne = new List<string>(unit.EraOne);
            newUnit.EraTwo = new List<string>(unit.EraTwo);
            newUnit.PriAttr = unit.PriAttr != null ? new List<string>(unit.PriAttr) : new List<string>();
            newUnit.SecAttr = unit.SecAttr != null ? new List<string>(unit.SecAttr) : new List<string>();
            newUnit.TerAttr = unit.TerAttr != null ? new List<string>(unit.TerAttr) : new List<string>();
            newUnit.LocalizedName = localizedName;
            newUnit.Type = name;
            newUnit.Dictionary = name.Replace(" ", "_").ToLower();
            newUnit.Card = name.Replace(" ", "_").ToLower();
            newUnit.CardInfo = name.Replace(" ", "_").ToLower();
            return newUnit;
        }

        public static string WriteEuEntry(Unit unit)
        {
            var entry = "\u00ac-----\n";
            entry += "{" + unit.Dictionary + "}" + unit.LocalizedName + "\n";
            entry += "{" + unit.Dictionary + "_descr}" + unit.Descr + "\n";
            entry += "{" + unit.Dictionary + "_descr_short}" + unit.DescrShort + "\n";
            return entry;
        }

        public override string GetTypeTextField(string identifier)
        {
            return identifier switch
            {
                "type" => identifier + GetTabLength(identifier) + Type,
                "dictionary" => identifier + GetTabLength(identifier) + Dictionary,
                "category" => identifier + GetTabLength(identifier) + Category,
                "class" => identifier + GetTabLength(identifier) + ClassType,
                "voice_type" => identifier + GetTabLength(identifier) + VoiceType,
                "accent" => ConditionalString(Accent, identifier + GetTabLength(identifier) + Accent),
                "banner faction" => identifier + GetTabLength(identifier) + BannerFaction,
                "banner holy" => identifier + GetTabLength(identifier) + BannerHoly,
                "soldier" => identifier + GetTabLength(identifier) 
                                        + Soldier + ", " 
                                        + SoldierCount + ", " 
                                        + ExtrasCount + ", " 
                                        + FormatFloat(Mass)
                                        + (FloatNotEqual(Radius, 0.40f) ? ", " + FormatFloat(Radius) : "")
                                        + (FloatNotEqual(Height, 1.70f) 
                                            ? FloatNotEqual(Radius, 0.40f) 
                                            ? ", " + FormatFloat(Height) 
                                            : ", 0.40, " + FormatFloat(Height) 
                                            : ""),
                "officer" => ConditionalString(Officer1, identifier + GetTabLength(identifier) + Officer1)
                             + ConditionalString(Officer2, "\n" + identifier + GetTabLength(identifier) + Officer2)
                             + ConditionalString(Officer3, "\n" + identifier + GetTabLength(identifier) + Officer3),
                "ship" => ConditionalString(Ship, identifier + GetTabLength(identifier) + Ship),
                "engine" => ConditionalString(Engine, identifier + GetTabLength(identifier) + Engine),
                "animal" => ConditionalString(Animal, identifier + GetTabLength(identifier) + Animal),
                "mount" => ConditionalString(Mount, identifier + GetTabLength(identifier) + Mount),
                "mounted_engine" => ConditionalString(MountedEngine, identifier + GetTabLength(identifier) + MountedEngine),
                "mount_effect" => ConditionalString(MakeCommaString(MountEffect), identifier + GetTabLength(identifier) + MakeCommaString(MountEffect)),
                "attributes" => ConditionalString(MakeCommaString(Attributes), identifier + GetTabLength(identifier) + MakeCommaString(Attributes)),
                "move_speed_mod" => FloatNotEqual(MoveSpeed, 1.0) ? identifier + GetTabLength(identifier) + FormatFloat(MoveSpeed) : "",
                "formation" => identifier + GetTabLength(identifier) + FormatFloat(SpacingWidth) + ", " 
                               + FormatFloat(SpacingDepth)+ ", " 
                               + FormatFloat(SpacingWidthLoose) + ", " 
                               + FormatFloat(SpacingDepthLoose) + ", " 
                               + SpacingRanks + ", " 
                               + FormationStyle
                               + ConditionalString(SpecialFormation, ", "  + SpecialFormation),
                "stat_health" => identifier + GetTabLength(identifier) + HitPoints + ", " + MountHitPoints,
                "stat_pri" => identifier + GetTabLength(identifier) + PriAttack + ", " 
                              + PriCharge + ", "
                              + PriProjectile + ", "
                              + PriRange + ", "
                              + PriAmmunition + ", "
                              + PriWeaponType + ", "
                              + PriTechType + ", "
                              + PriDamageType + ", "
                              + PriSoundType + ", "
                              + ConditionalString(PriFireType, ", "  + PriFireType)
                              + PriAttDelay + ", "
                              + FormatFloat(PriSkelFactor),
                "stat_pri_attr" => identifier + GetTabLength(identifier) + (PriAttr is { Count: > 0 } ? MakeCommaString(PriAttr) : "no"),
                "stat_sec" => 
                    SecWeaponType == "no" || string.IsNullOrWhiteSpace(SecWeaponType) ? 
                        identifier + GetTabLength(identifier) + "0, 0, no, 0, 0, no, melee_simple, blunt, none, 0, 1\n" + "stat_sec_attr\t\t\t" + "no" :
                    identifier + GetTabLength(identifier) + SecAttack + ", " 
                              + SecCharge + ", "
                              + SecProjectile + ", "
                              + SecRange + ", "
                              + SecAmmunition + ", "
                              + SecWeaponType + ", "
                              + SecTechType + ", "
                              + SecDamageType + ", "
                              + SecSoundType + ", "
                              + ConditionalString(SecFireType, ", "  + SecFireType)
                              + SecAttDelay + ", "
                              + FormatFloat(SecSkelFactor) + "\n" 
                              + "stat_sec_attr\t\t\t" + (SecAttr is { Count: > 0 } ? MakeCommaString(SecAttr) : "no"),
                "stat_ter" => 
                    TerWeaponType == "no" || string.IsNullOrWhiteSpace(TerWeaponType) ? "" :
                    identifier + GetTabLength(identifier) + TerAttack + ", " 
                              + TerCharge + ", "
                              + TerProjectile + ", "
                              + TerRange + ", "
                              + TerAmmunition + ", "
                              + TerWeaponType + ", "
                              + TerTechType + ", "
                              + TerDamageType + ", "
                              + TerSoundType + ", "
                              + ConditionalString(TerFireType, ", "  + TerFireType)
                              + TerAttDelay + ", "
                              + FormatFloat(TerSkelFactor) + "\n" 
                              + "stat_ter_attr\t\t\t" + (TerAttr is { Count: > 0 } ? MakeCommaString(TerAttr) : "no"),
                "stat_pri_armour" => identifier + GetTabLength(identifier) + PriArmour + ", "
                                     + PriDefense + ", "
                                     + PriShield + ", "
                                     + PriDefSound,
                "stat_sec_armour" => identifier + GetTabLength(identifier) + SecArmour + ", "
                                     + SecDefense + ", "
                                     + SecDefSound,
                "stat_heat" => identifier + GetTabLength(identifier) + StatHeat,
                "stat_ground" => identifier + GetTabLength(identifier) + StatScrub + ", "
                                     + StatSand + ", "
                                     + StatForest + ", "
                                     + StatSnow,
                "stat_mental" => identifier + GetTabLength(identifier) + Morale + ", "
                                     + Discipline + ", "
                                     + Training
                                     + (LockMorale ? ", lock_morale" : ""),
                "stat_charge_dist" => identifier + GetTabLength(identifier) + StatChargeDist,
                "stat_fire_delay" => identifier + GetTabLength(identifier) + StatFireDelay,
                "stat_food" => identifier + GetTabLength(identifier) + StatFood + ", "+ StatFoodSec,
                "stat_cost" => identifier + GetTabLength(identifier) + RecruitTime + ", " 
                               + RecruitCost + ", " 
                               + Upkeep + ", " 
                               + WpnCost + ", " 
                               + ArmourCost + ", " 
                               + CustomCost + ", " 
                               + CustomLimit + ", " 
                               + CustomIncrease,
                "stat_stl" => StatStl > 0 ? identifier + GetTabLength(identifier) + StatStl : "",
                "armour_ug_levels" => identifier + GetTabLength(identifier) + ArmourlvlBase 
                                      + ConditionalString(ArmourlvlOne, ", "  + ArmourlvlOne)
                                      + ConditionalString(ArmourlvlTwo, ", "  + ArmourlvlTwo)
                                      + ConditionalString(ArmourlvlThree, ", "  + ArmourlvlThree),
                "armour_ug_models" => identifier + GetTabLength(identifier) + ArmourModelBase 
                                      + ConditionalString(ArmourModelOne, ", "  + ArmourModelOne)
                                      + ConditionalString(ArmourModelTwo, ", "  + ArmourModelTwo)
                                      + ConditionalString(ArmourModelThree, ", "  + ArmourModelThree),
                "ownership" => ConditionalString(MakeCommaString(Ownership), identifier + GetTabLength(identifier) + MakeCommaString(Ownership)),
                "era 0" => ConditionalString(MakeCommaString(EraZero), identifier + GetTabLength(identifier) + MakeCommaString(EraZero)),
                "era 1" => ConditionalString(MakeCommaString(EraOne), identifier + GetTabLength(identifier) + MakeCommaString(EraOne)),
                "era 2" => ConditionalString(MakeCommaString(EraTwo), identifier + GetTabLength(identifier) + MakeCommaString(EraTwo)),
                "info_pic_dir" => ConditionalString(InfoDict, identifier + GetTabLength(identifier) + InfoDict),
                "card_pic_dir" => ConditionalString(CardDict, identifier + GetTabLength(identifier) + CardDict),
                "crusading_upkeep_modifier" => FloatNotEqual(CrusadeUpkeep, 1.0) ? identifier + GetTabLength(identifier) + FormatFloat(CrusadeUpkeep) : "",
                "recruit_priority_offset" => identifier + GetTabLength(identifier) + RecruitPriorityOffset,
                _ => "",
            };
        }

        public string WriteEntry()
        {
            var entry = "";
            foreach (var id in EduIdentifiers)
            {
                CommentsInLine.TryAdd(id, "");
                entry += GetTextField(id);
                
            }if (!IsEopUnit)
            {
                if (GlobalOptionsInstance.AddUnitValue)
                {
                    entry += ";ai_unit_value\t\t\t" + FormatFloat(AiUnitValue) + "\n";
                }
                if (GlobalOptionsInstance.AddUnitValuePerCost)
                {
                    entry += ";value_per_cost\t\t\t" + FormatFloat(ValuePerCost) + "\n";
                }
                if (GlobalOptionsInstance.AddUnitValuePerUpkeep)
                {
                    entry += ";value_per_upkeep\t\t" + FormatFloat(ValuePerUpkeep) + "\n";
                }
            }
            entry += "\n";
            entry += "\n";
            entry += "\n";
            return entry;
        }


        public double CalculateUnitValue()
        {
            double attackValue = 0;
            var hasMissileModifier = 1.0;
            if (Category != null)
            {
                if ((PriAttr != null && PriAttr.Contains("prec")) || string.IsNullOrWhiteSpace(PriProjectile))
                {
                    var attackStat = string.IsNullOrWhiteSpace(PriProjectile) ? PriAttack : SecAttack + 2.0;
                    var chargeStat = string.IsNullOrWhiteSpace(PriProjectile) ? PriCharge / 2.0 : SecCharge / 2.0;
                    attackValue = attackStat + chargeStat;
                }
                else
                {
                    double rangeModifier = PriRange <= 200 ? PriRange * 0.02 : 4.0;
                    attackValue = PriAttack * 0.5 * rangeModifier + PriAttack * 0.5;
                    if (PriAttr != null && PriAttr.Contains("ap"))
                    {
                        attackValue += 5.0;
                        if (PriTechType is "missile_gunpowder")
                            attackValue += 5.0;
                    }
                    hasMissileModifier = 0.8;
                }
            }
            double statsTotal = attackValue + (4 * PriArmour) + (2 * PriDefense) + (2 * PriShield);

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
                            var rangeModifier = SecRange <= 210 ? SecRange * 0.01429 : 3.0;
                            attackValue = SecAttack * 1.5 * rangeModifier + SecAttack * 1.5;
                            if (SecAttr != null)
                            {
                                if (SecAttr.Contains("ap"))
                                    attackValue += 3.0;
                                if (SecAttr.Contains("launching"))
                                    attackValue += 3.0;
                                if (!SecAttr.Contains("area"))
                                {
                                    if (SecAttr.Contains("bp"))
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
                                var mountType = MountDataBase[Mount].mount_class;
                                switch (mountType)
                                {
                                    case "horse":
                                    case "camel":
                                        statsTotal += 4.0;
                                        break;
                                    case "elephant":
                                        {
                                            extrasStats = SecAttack + (SecCharge / 2.0);
                                            if (SecAttr != null)
                                            {
                                                if (SecAttr.Contains("ap"))
                                                    extrasStats += 5.0;
                                                if (SecAttr.Contains("launching"))
                                                    extrasStats += 1.0;
                                                if (SecAttr.Contains("launching"))
                                                    extrasStats += 2.0;
                                            }

                                            extrasStats = extrasStats * (ExtrasCount * MountHitPoints) * moraleModifier;
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
            var aiUnitValue = ((SoldierCount * HitPoints + officerCount * (HitPoints + 1)) * (moraleModifier * statsTotal) + extrasStats) * 0.45;
            return aiUnitValue;
        }
        
        
        private const string UnitCardPath = "\\data\\ui\\units";
        private const string UnitInfoCardPath = "\\data\\ui\\unit_info";
        private const string FactionSymbolPath = "\\data\\ui\\faction_symbols";

        public string[] GetCards()
        {
            var unitCard = "";
            var cardSearchFactions = Ownership;
            var infoCardSearchFactions = Ownership;
            
            if (Ownership.Contains("all"))
            {
                cardSearchFactions = FactionDataBase.Keys.ToList();
                infoCardSearchFactions = FactionDataBase.Keys.ToList();
            }

            if (MercenaryUnit)
            {
                cardSearchFactions = new List<string> { "mercs" };
                infoCardSearchFactions = new List<string> { "merc" };
            }

            if (CardDict != null)
            {
                cardSearchFactions = new List<string> { CardDict };
            }
            if (InfoDict != null)
            {
                infoCardSearchFactions = new List<string> { InfoDict };
            }

            foreach (var faction in cardSearchFactions)
            {
                var cardPath = ModPath + UnitCardPath + "\\";
                cardPath += faction;
                if (!(Directory.Exists(cardPath)))
                {
                    ErrorDb.AddError($@"Card path not found {cardPath}");
                    continue;
                }

                if (File.Exists(cardPath + "\\#" + Dictionary + ".tga"))
                {
                    unitCard = cardPath + "\\#" + Dictionary + ".tga";
                }
                else
                {
                    ErrorDb.AddError($@"no unit card found for unit: {Type} in faction: {faction}");
                }
            }

            var unitInfoCard = "";
            foreach (var faction in infoCardSearchFactions)
            {
                var cardInfoPath = ModPath + UnitInfoCardPath + "\\";
                cardInfoPath += faction;
                if (!(Directory.Exists(cardInfoPath)))
                {
                    ErrorDb.AddError($@"Info Card path not found {cardInfoPath}");
                    continue;
                }

                if (File.Exists(cardInfoPath + "\\" + Dictionary + "_info.tga"))
                {
                    unitInfoCard = cardInfoPath + "\\" + Dictionary + "_info.tga";
                }
                else
                {
                    ErrorDb.AddError($@"no unit info card found for unit: {Type} in faction: {faction}");
                }
            }

            // Faction Symbols
            var factionSymbol = "";
            var factionSymbolPath = ModPath + FactionSymbolPath + "\\";

            // If the unit is slave only, just set to slave and move on
            if (Ownership is ["slave"])
            {
                factionSymbolPath += "slave";
            }
            else switch (Ownership.Count)
            {
                // Otherwise, add the first non-slave faction
                case 1:
                    factionSymbolPath += Ownership[0];
                    break;
                case > 1:
                {
                    foreach (var faction in Ownership.Where(faction => faction != "slave"))
                    {
                        factionSymbolPath += faction;
                        break;
                    }
                    break;
                }
            }

            if (File.Exists(factionSymbolPath + ".tga"))
            {
                factionSymbol = factionSymbolPath + ".tga";
            }

            if (unitCard.Equals(""))
            {
                ErrorDb.AddError($@"!!no unit card at all found for unit: {Type}");
            }
            if (unitInfoCard.Equals(""))
            {
                ErrorDb.AddError($@"!!no unit info card at all found for unit: {Type}");
            }
            return new[] { unitCard, unitInfoCard, factionSymbol };
        }

    }
}