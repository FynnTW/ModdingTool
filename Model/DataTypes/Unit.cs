using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static ModdingTool.Globals;

namespace ModdingTool
{
    /// <summary>
    /// Represents a unit in the game. A unit is a type of game entity that has various properties and behaviors.
    /// This class inherits from the GameType class.
    /// </summary>
    public class Unit : GameType
    {
        // Static members for various categorizations and definitions related to game units.
        #region strings
        
        private const string UnitCardPath = "\\data\\ui\\units";
        private const string UnitInfoCardPath = "\\data\\ui\\unit_info";
        private const string FactionSymbolPath = "\\data\\ui\\faction_symbols";
        /// <summary>
        /// A list of identifiers used in the EDU file in the game.
        /// These identifiers are used to define various properties of a unit in the game.
        /// </summary>
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
        /// <summary>
        /// An array of strings representing the different categories a unit can belong to.
        /// </summary>
        public static readonly string[] Categories = { "infantry", "cavalry", "siege", "handler", "ship", "non_combatant" };

        /// <summary>
        /// An array of strings representing the different classes a unit can belong to.
        /// </summary>
        public static readonly string[] Classes = { "light", "heavy", "missile", "spearmen", "skirmish" };

        /// <summary>
        /// An array of strings representing the different types of damage a unit can inflict.
        /// </summary>
        public static readonly string[] DamageTypes = { "piercing", "slashing", "blunt", "fire" };

        /// <summary>
        /// An array of strings representing the different types of weapons a unit can use.
        /// </summary>
        public static readonly string[] WeaponTypes = { "melee", "thrown", "missile", "siege_missile", "no" };

        /// <summary>
        /// An array of strings representing the different types of sounds a unit can make.
        /// </summary>
        public static readonly string[] SoundTypes = { "none", "knife", "mace", "axe", "sword", "spear" };

        /// <summary>
        /// An array of strings representing the different types of voices a unit can have.
        /// </summary>
        public static readonly string[] VoiceTypes = { "general", "heavy", "medium", "light", "female", "general_1", "heavy_1", "medium_1", "light_1", "female_1" };

        /// <summary>
        /// An array of strings representing the different types of sound effects a unit can make.
        /// </summary>
        public static readonly string[] SoundTypesDef = { "flesh", "leather", "ground", "building", "metal" };
        
        /// <summary>
        /// An array of strings representing the different technology types a unit can have.
        /// </summary>
        public static readonly string[] TechTypes = { "melee_simple", "missile_mechanical", "melee_blade", "missile_gunpowder", "artillery_mechanical", "artillery_gunpowder" };

        /// <summary>
        /// A list of strings representing the different formation styles a unit can adopt.
        /// </summary>
        public static readonly List<string> FormationStyles = new (){ "square", "horde", "phalanx" };

        /// <summary>
        /// A list of strings representing the different special formation styles a unit can adopt.
        /// </summary>
        public static readonly List<string> SpecialFormationStyles = new (){ "wedge", "phalanx", "schiltrom", "shield_wall" };

        /// <summary>
        /// A list of strings representing the different attack attributes a unit can have.
        /// </summary>
        public static readonly List<string> AttackAttr = new (){ "spear", "light_spear", "prec", "ap", "bp", "area", "fire", "launching", "thrown", "short_pike", "long_pike", "spear_bonus_12", "spear_bonus_10", "spear_bonus_8", "spear_bonus_6", "spear_bonus_4" };

        /// <summary>
        /// An array of strings representing the different discipline types a unit can have.
        /// </summary>
        public static readonly string[] DisciplineTypes = { "impetuous", "normal", "disciplined", "berserker","low" };

        /// <summary>
        /// An array of strings representing the different training levels a unit can have.
        /// </summary>
        private static readonly string[] TrainedTypes = { "trained", "highly_trained", "untrained" };
        #endregion
        
        #region fields
        
        private string _localizedName = "";
        private string _dictionary = "";
        private string _category = "infantry";
        private string _class = "light";
        private string _voiceType = "Light";
        private string _soldier = "";
        private BattleModel? _soldierEntry = null;
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
        private BattleModel? _officer1Entry = null;
        private string _officer2 = "";
        private BattleModel? _officer2Entry = null;
        private string _officer3 = "";
        private BattleModel? _officer3Entry = null;
        private string _mountedEngine = "";
        private string _mount = "";
        private string _ship = "";
        private string _engine = "";
        private string _animal = "";
        private double _spacingWidth = 1.4;
        private double _spacingDepth = 1.4;
        private double _spacingWidthLoose = 2.4;
        private double _spacingDepthLoose = 2.4;
        private string? _specialFormation;
        private int _hitPoints;
        private int _mountHitPoints;
        private int _priAttack;
        private int _priCharge;
        private int _secAttack;
        private string? _priProjectile;
        private int _priRange;
        private int _priAmmunition;
        private string? _priWeaponType;
        private string? _priTechType = "melee_blade";
        private string? _priDamageType = "blunt";
        private string? _priSoundType = "none";
        private int _priAttDelay;
        private double _priSkelFactor = 1.0;
        private int _secCharge;
        private string? _secProjectile = "";
        private int _secRange;
        private int _secAmmunition;
        private string? _secWeaponType = "no";
        private string? _secTechType = "melee_simple";
        private string? _secDamageType = "blunt";
        private string? _secSoundType = "none";
        private int _secAttDelay;
        private double _secSkelFactor = 1;
        private int _terAttack;
        private int _terCharge;
        private string? _terProjectile = "";
        private int _terRange;
        private int _terAmmunition;
        private string? _terWeaponType = "no";
        private string? _terTechType = "melee_simple";
        private string? _terDamageType = "blunt";
        private string? _terSoundType = "none";
        private int _terAttDelay;
        private double _terSkelFactor;
        private int _priArmour;
        private string? _formationStyle = "square";
        private int _priDefense;
        private int _priShield;
        private string? _priDefSound = "flesh";
        private int _secArmour;
        private int _secDefense;
        private string? _secDefSound = "flesh";
        private int _statHeat;
        private int _statScrub;
        private int _statForest;
        private int _statSnow;
        private int _statSand;
        private int _morale = 1;
        private string? _discipline = "normal";
        private string? _training = "untrained";
        private int _statChargeDist = 10;
        private int _statFireDelay;
        private int _statFood = 60;
        private int _statFoodSec = 300;
        private int _recruitTime = 1;
        private int _recruitCost = 500;
        private int _upkeep = 200;
        private int _wpnCost = 100;
        private int _armourCost = 100;
        private int _customCost = 500;
        private int _customLimit = 3;
        private int _customIncrease = 50;
        private double _moveSpeed = 1.0;
        private int _statStl;
        private string? _armourlvlBase;
        private string? _armourlvlOne;
        private string? _armourlvlTwo;
        private string? _armourlvlThree;
        private string _armourModelBase = "";
        private BattleModel? _armourModelBaseEntry = null;
        private string _armourModelOne = "";
        private BattleModel? _armourModelOneEntry = null;
        private string _armourModelTwo = "";
        private BattleModel? _armourModelTwoEntry = null;
        private string _armourModelThree = "";
        private BattleModel? _armourModelThreeEntry = null;
        private double _recruitPriorityOffset;
        private string? _infoDict;
        private string? _cardDict;
        private double _crusadeUpkeep = 1.0;
        private int _spacingRanks;
        private bool _lockMorale;
        private string? _priFireType = "";
        private string? _secFireType = "";
        private string? _terFireType = "";
        private string? _descr = "";
        private string? _descrShort = "";

        #endregion fields

        #region Properties
        /// <summary>
        /// Gets or sets the localized name of the unit. This is the name that will be displayed in the game.
        /// </summary>
        public string LocalizedName
        {
            get => _localizedName;
            set
            {
                AddChange(nameof(LocalizedName), _localizedName, value);
                _localizedName = value;
                NotifyPropertyChanged();
            }
        }
        
        /// <summary>
        /// Gets or sets the type of the unit. This is the unique identifier for the unit in the game.
        /// </summary>
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
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the dictionary entry for the unit. This is used for localization and is typically the unit's name with spaces replaced by underscores and all lowercase.
        /// </summary>
        public string? Dictionary
        {
            get => _dictionary;
            set
            {
                if (value == null) return;
                AddChange(nameof(Dictionary), _dictionary, value);
                _dictionary = value;
                NotifyPropertyChanged();
            }
        }
        
        /// <summary>
        /// Gets or sets the category of the unit. This is a string representing the category a unit belongs to.
        /// The setter validates the input against a predefined list of categories. If the category does not exist, an error is logged.
        /// </summary>
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
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the class type of the unit. This is a string representing the class a unit belongs to.
        /// The setter validates the input against a predefined list of classes. If the class does not exist, an error is logged.
        /// </summary>
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
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the voice type of the unit. This is a string representing the voice type a unit has.
        /// The setter validates the input against a predefined list of voice types. If the voice type does not exist, an error is logged.
        /// </summary>
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
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the accent of the unit. This is a string representing the accent a unit has.
        /// The setter validates the input and logs changes.
        /// </summary>
        public string? Accent
        {
            get => _accent;
            set
            {
                if (value == null) return;
                AddChange(nameof(Accent), _accent ?? "", value);
                _accent = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the banner faction of the unit. This is a string representing the faction banner a unit has.
        /// The setter validates the input and logs changes.
        /// </summary>
        public string? BannerFaction
        {
            get => _bannerFaction;
            set
            {
                if (value == null) return;
                AddChange(nameof(BannerFaction), _bannerFaction ?? "", value);
                _bannerFaction = value;
                NotifyPropertyChanged();
            }
        } 

        /// <summary>
        /// Gets or sets the banner unit of the unit. This is a string representing the unit banner a unit has.
        /// The setter validates the input and logs changes.
        /// </summary>
        public string? BannerUnit
        {
            get => _bannerUnit;
            set
            {
                if (value == null) return;
                AddChange(nameof(BannerUnit), _bannerUnit ?? "", value);
                _bannerUnit = value;
                NotifyPropertyChanged();
            }
        } 

        /// <summary>
        /// Gets or sets the main banner of the unit. This is a string representing the main banner a unit has.
        /// The setter validates the input and logs changes.
        /// </summary>
        public string? BannerMain
        {
            get => _bannerMain;
            set
            {
                if (value == null) return;
                AddChange(nameof(BannerMain), _bannerMain ?? "", value);
                _bannerMain = value;
                NotifyPropertyChanged();
            }
        } 

        /// <summary>
        /// Gets or sets the secondary banner of the unit. This is a string representing the secondary banner a unit has.
        /// The setter validates the input and logs changes.
        /// </summary>
        public string? BannerSecondary
        {
            get => _bannerSecondary;
            set
            {
                if (value == null) return;
                AddChange(nameof(BannerSecondary), _bannerSecondary ?? "", value);
                _bannerSecondary = value;
                NotifyPropertyChanged();
            }
        } 

        /// <summary>
        /// Gets or sets the holy banner of the unit. This is a string representing the holy banner a unit has.
        /// The setter validates the input and logs changes.
        /// </summary>
        public string? BannerHoly 
        {
            get => _bannerHoly;
            set
            {
                if (value == null) return;
                AddChange(nameof(BannerHoly), _bannerHoly ?? "", value);
                _bannerHoly = value;
                NotifyPropertyChanged();
            }
        }
        
        
        /// <summary>
        /// Gets or sets the soldier model for the unit. This is a string representing the soldier model a unit uses.
        /// The setter validates the input against the battle models database. If the model does not exist, an error is logged.
        /// </summary>
        public string Soldier
        {
            get => _soldierEntry != null ? _soldierEntry.Name : _soldier;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;
                var old = _soldierEntry != null ? _soldierEntry.Name : _soldier;
                if (old == value)
                    return;
                AddChange(nameof(Soldier), old, value);
                _soldierEntry = ModData.BattleModelDb.Get(value.ToLower());
                _soldier = value;
                NotifyPropertyChanged();
                if (old != value)
                    UpdateModelUsage(old, value);
            }
        }

        /// <summary>
        /// Gets or sets the soldier count for the unit. This is an integer representing the number of soldiers in a unit.
        /// The setter validates the input. If the unit category is "non_combatant", the soldier count must be between 0 and 300. For other categories, the soldier count must be between 4 and 100. If the count is not within these ranges, an error is logged.
        /// </summary>
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
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the extras count for the unit. This is an integer representing the number of extras in a unit.
        /// The setter validates the input. The extras count must be between 0 and 300. If the unit category is "handler", the number of animals (extras count divided by soldier count) must not exceed 3. If these conditions are not met, an error is logged.
        /// </summary>
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
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the mass of the unit. This is a double representing the mass of a unit.
        /// </summary>
        public double Mass
        {
            get => _mass;
            set
            {
                if (value < 0)
                    value = 1.0;
                AddChange(nameof(Mass), _mass, value);
                _mass = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the radius of the unit. This is a double representing the radius of a unit.
        /// </summary>
        public double Radius
        {
            get => _radius;
            set
            {
                if (value < 0)
                    value = 0.4;
                AddChange(nameof(Radius), _radius, value);
                _radius = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the height of the unit. This is a double representing the height of a unit.
        /// </summary>
        public double Height
        {
            get => _height;
            set
            {
                if (value < 0)
                    value = 1.7;
                AddChange(nameof(Height), _height, value);
                _height = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the first officer model for the unit. This is a string representing the first officer model a unit uses.
        /// The setter validates the input against the battle models database. If the model does not exist, an error is logged.
        /// </summary>
        public string Officer1
        {
            get => _officer1Entry != null ? _officer1Entry.Name : _officer1;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    value = string.Empty;
                var old = Officer1;
                if (old == value)
                    return;
                AddChange(nameof(Officer1), old, value);
                _officer1Entry = value != string.Empty ? ModData.BattleModelDb.Get(value.ToLower()) : null;
                _officer1 = value;
                NotifyPropertyChanged();
                UpdateModelUsage(old, value);
            }
        }

        /// <summary>
        /// Gets or sets the second officer model for the unit. This is a string representing the second officer model a unit uses.
        /// The setter validates the input against the battle models database. If the model does not exist, an error is logged.
        /// </summary>
        public string Officer2
        {
            get => _officer2Entry != null ? _officer2Entry.Name : _officer2;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    value = string.Empty;
                var old = Officer2;
                if (old == value)
                    return;
                AddChange(nameof(Officer2), old, value);
                _officer2Entry = value != string.Empty ? ModData.BattleModelDb.Get(value.ToLower()) : null;
                _officer2 = value;
                NotifyPropertyChanged();
                UpdateModelUsage(old, value);
            }
        }

        /// <summary>
        /// Gets or sets the third officer model for the unit. This is a string representing the third officer model a unit uses.
        /// The setter validates the input against the battle models database. If the model does not exist, an error is logged.
        /// </summary>
        public string Officer3
        {
            get => _officer3Entry != null ? _officer3Entry.Name : _officer3;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    value = string.Empty;
                var old = Officer3;
                if (old == value)
                    return;
                AddChange(nameof(Officer3), old, value);
                _officer3Entry = value != string.Empty ? ModData.BattleModelDb.Get(value.ToLower()) : null;
                _officer3 = value;
                NotifyPropertyChanged();
                UpdateModelUsage(old, value);
            }
        }

        /// <summary>
        /// Gets or sets the mounted engine for the unit. This is a string representing the mounted engine a unit uses.
        /// </summary>
        public string? MountedEngine
        {
            get => _mountedEngine;
            set
            {
                if (value == null) return;
                AddChange(nameof(MountedEngine), _mountedEngine, value);
                _mountedEngine = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the mount for the unit. This is a string representing the mount a unit uses.
        /// The setter validates the input against the mounts database. If the mount does not exist, an error is logged.
        /// </summary>
        public string? Mount
        {
            get => _mount;
            set
            {
                if (value == null) return;
                if (!string.IsNullOrWhiteSpace(value) && !MountDataBase.ContainsKey(value))
                    ErrorDb.AddError($"Mount {value} does not exist in mounts database.");
                AddChange(nameof(Mount), _mount, value);
                _mount = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the ship for the unit. This is a string representing the ship a unit uses.
        /// </summary>
        public string? Ship
        {
            get => _ship;
            set
            {
                if (value == null) return;
                AddChange(nameof(Ship), _ship, value);
                _ship = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the engine for the unit. This is a string representing the engine a unit uses.
        /// </summary>
        public string? Engine
        {
            get => _engine;
            set
            {
                if (value == null) return;
                AddChange(nameof(Engine), _engine, value);
                _engine = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the animal for the unit. This is a string representing the animal a unit uses.
        /// </summary>
        public string? Animal
        {
            get => _animal;
            set
            {
                if (value == null) return;
                AddChange(nameof(Animal), _animal, value);
                _animal = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the mount effect for the unit. This is a list of strings representing the mount effects a unit has.
        /// </summary>
        public List<string> MountEffect { get; private set; } = new();

        /// <summary>
        /// Adds a new mount effect to the unit. If the effect already exists, it will not be added.
        /// </summary>
        /// <param name="effect">The mount effect to add.</param>
        public void AddMountEffect(string effect)
        {
            if (MountEffect.Contains(effect))
                return;
            MountEffect.Add(effect);
        }

        /// <summary>
        /// Gets or sets the attributes for the unit. This is a list of strings representing the attributes a unit has.
        /// </summary>
        public List<string> Attributes { get; set; } = new();

        /// <summary>
        /// Adds a new attribute to the unit. If the attribute already exists, it will not be added.
        /// If the attribute does not exist in the global attribute types, it will be added there as well.
        /// </summary>
        /// <param name="attribute">The attribute to add.</param>
        public void AddAttribute(string attribute)
        {
            if (Attributes.Contains(attribute))
                return;
            if (!ModData.Units.AttributeTypes.Contains(attribute))
                ModData.Units.AttributeTypes.Add(attribute);
            Attributes.Add(attribute);
        }

        /// <summary>
        /// Gets or sets the spacing width of the unit. This is a double representing the horizontal spacing between soldiers in a unit.
        /// The setter logs changes to the spacing width.
        /// </summary>
        public double SpacingWidth
        {
            get => _spacingWidth; 
            set 
            {
                AddChange(nameof(SpacingWidth), _spacingWidth, value);
                _spacingWidth = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the spacing depth of the unit. This is a double representing the vertical spacing between soldiers in a unit.
        /// The setter logs changes to the spacing depth.
        /// </summary>
        public double SpacingDepth
        {
            get => _spacingDepth;
            set
            {
                AddChange(nameof(SpacingDepth), _spacingDepth, value);
                _spacingDepth = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the loose spacing width of the unit. This is a double representing the horizontal spacing between soldiers in a unit when in loose formation.
        /// The setter logs changes to the loose spacing width.
        /// </summary>
        public double SpacingWidthLoose
        {
            get => _spacingWidthLoose;
            set
            {
                AddChange(nameof(SpacingWidthLoose), _spacingWidthLoose, value);
                _spacingWidthLoose = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the loose spacing depth of the unit. This is a double representing the vertical spacing between soldiers in a unit when in loose formation.
        /// The setter logs changes to the loose spacing depth.
        /// </summary>
        public double SpacingDepthLoose
        {
            get => _spacingDepthLoose;
            set
            {
                AddChange(nameof(SpacingDepthLoose), _spacingDepthLoose, value);
                _spacingDepthLoose = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the special formation of the unit. This is a string representing the special formation a unit can adopt.
        /// The setter validates the input against a predefined list of special formations. If the formation does not exist, an error is logged.
        /// </summary>
        public string? SpecialFormation
        {
            get => _specialFormation;
            set
            {
                if (value == null) return;
                if (!string.IsNullOrWhiteSpace(value) && !SpecialFormationStyles.Contains(value.ToLower()))
                {
                    ErrorDb.AddError($"Special formation {value} does not exist.");
                    return;
                }
                AddChange(nameof(SpecialFormation), _specialFormation ?? "", value);
                _specialFormation = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the hit points of the unit. This is an integer representing the health of a unit.
        /// The setter clamps the value between a predefined minimum and maximum, and logs changes to the hit points.
        /// </summary>
        public int HitPoints
        {
            get => _hitPoints;
            set
            {
                value = ClampUnitStat(value);
                AddChange(nameof(HitPoints), _hitPoints, value);
                _hitPoints = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the mount hit points of the unit. This is an integer representing the health of a unit's mount.
        /// The setter clamps the value between a predefined minimum and maximum, and logs changes to the mount hit points.
        /// </summary>
        public int MountHitPoints
        {
            get => _mountHitPoints;
            set
            {
                value = ClampUnitStat(value);
                AddChange(nameof(MountHitPoints), _mountHitPoints, value);
                _mountHitPoints = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the primary attack value of the unit. This is an integer representing the primary attack power of a unit.
        /// The setter clamps the value between a predefined minimum and maximum, and logs changes to the primary attack value.
        /// </summary>
        public int PriAttack
        {
            get => _priAttack;
            set
            {
                value = ClampUnitStat(value);
                AddChange(nameof(PriAttack), _priAttack, value);
                _priAttack = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the primary charge value of the unit. This is an integer representing the primary charge power of a unit.
        /// The setter clamps the value between a predefined minimum and maximum, and logs changes to the primary charge value.
        /// </summary>
        public int PriCharge
        {
            get => _priCharge;
            set
            {
                value = ClampUnitStat(value);
                AddChange(nameof(PriCharge), _priCharge, value);
                _priCharge = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the primary projectile type of the unit. This is a string representing the type of projectile used in a unit's primary attack.
        /// The setter validates the input and logs changes. If the value is null, it defaults to "no".
        /// </summary>
        public string? PriProjectile
        {
            get => _priProjectile;
            set
            {
                value ??= "no";
                AddChange(nameof(PriProjectile), _priProjectile ?? "", value);
                _priProjectile = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the primary range of the unit. This is an integer representing the range of a unit's primary attack.
        /// The setter logs changes to the primary range.
        /// </summary>
        public int PriRange
        {
            get => _priRange;
            set
            {
                AddChange(nameof(PriRange), _priRange, value);
                _priRange = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the primary ammunition of the unit. This is an integer representing the amount of ammunition for a unit's primary attack.
        /// The setter logs changes to the primary ammunition.
        /// </summary>
        public int PriAmmunition
        {
            get => _priAmmunition;
            set
            {
                AddChange(nameof(PriAmmunition), _priAmmunition, value);
                _priAmmunition = value;
                NotifyPropertyChanged();
            }
        }
        
        /// <summary>
        /// Gets or sets the primary weapon type of the unit. This is a string representing the type of weapon used in a unit's primary attack.
        /// The setter validates the input against a predefined list of weapon types. If the weapon type does not exist, an error is logged.
        /// </summary>
        public string? PriWeaponType
        {
            get => _priWeaponType;
            set
            {
                if (value == null) return;
                if (!WeaponTypes.Contains(value.ToLower()))
                {
                    ErrorDb.AddError($"Weapon type {value} does not exist.");
                    return;
                }
                AddChange(nameof(PriWeaponType), _priWeaponType ?? "", value);
                _priWeaponType = value;
                NotifyPropertyChanged();
            }
        }
        
        /// <summary>
        /// Gets or sets the primary technology type of the unit. This is a string representing the technology type of a unit's primary attack.
        /// The setter validates the input against a predefined list of technology types. If the technology type does not exist, an error is logged.
        /// </summary>
        public string? PriTechType
        {
            get => _priTechType;
            set
            {
                if (value == null) return;
                if (!TechTypes.Contains(value.ToLower()))
                {
                    ErrorDb.AddError($"Tech type {value} does not exist.");
                    return;
                }
                AddChange(nameof(PriTechType), _priTechType ?? "", value);
                _priTechType = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the primary damage type of the unit. This is a string representing the type of damage a unit's primary attack inflicts.
        /// The setter validates the input against a predefined list of damage types. If the damage type does not exist, an error is logged.
        /// </summary>
        public string? PriDamageType
        {
            get => _priDamageType;
            set
            {
                if (value == null) return;
                if (!DamageTypes.Contains(value.ToLower()))
                {
                    ErrorDb.AddError($"Damage type {value} does not exist.");
                    return;
                }
                AddChange(nameof(PriDamageType), _priDamageType ?? "", value);
                _priDamageType = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the primary sound type of the unit. This is a string representing the sound type of a unit's primary attack.
        /// The setter validates the input against a predefined list of sound types. If the sound type does not exist, an error is logged.
        /// </summary>
        public string? PriSoundType
        {
            get => _priSoundType;
            set
            {
                if (value == null) return;
                if (!SoundTypes.Contains(value.ToLower()))
                {
                    ErrorDb.AddError($"Sound type {value} does not exist.");
                    return;
                }
                AddChange(nameof(PriSoundType), _priSoundType ?? "", value);
                _priSoundType = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the primary attack delay of the unit. This is an integer representing the delay of a unit's primary attack.
        /// The setter logs changes to the primary attack delay.
        /// </summary>
        public int PriAttDelay
        {
            get => _priAttDelay;
            set
            {
                AddChange(nameof(PriAttDelay), _priAttDelay, value);
                _priAttDelay = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the primary skeleton factor of the unit. This is a double representing the skeleton factor of a unit's primary attack.
        /// The setter logs changes to the primary skeleton factor.
        /// </summary>
        public double PriSkelFactor
        {
            get => _priSkelFactor;
            set
            {
                AddChange(nameof(PriSkelFactor), _priSkelFactor, value);
                _priSkelFactor = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the primary attributes of the unit. This is a list of strings representing the attributes of a unit's primary attack.
        /// </summary>
        public List<string>? PriAttr { get; set; } = new ();

        /// <summary>
        /// Adds a new primary attribute to the unit. If the attribute already exists, it will not be added.
        /// If the attribute does not exist in the global attribute types, an error is logged.
        /// </summary>
        /// <param name="attribute">The attribute to add.</param>
        public void AddPriAttr(string attribute)
        {
            if (PriAttr != null && PriAttr.Contains(attribute))
                return;
            if (!AttackAttr.Contains(attribute))
            {
                ErrorDb.AddError($"Attack attribute {attribute} does not exist.");
                return;
            }
            PriAttr ??= new List<string>();
            PriAttr.Add(attribute);
        }

        /// <summary>
        /// Gets or sets the secondary attack value of the unit. This is an integer representing the secondary attack power of a unit.
        /// The setter clamps the value between a predefined minimum and maximum, and logs changes to the secondary attack value.
        /// </summary>
        public int SecAttack
        {
            get => _secAttack;
            set
            {
                value = ClampUnitStat(value);
                AddChange(nameof(SecAttack), _secAttack, value);
                _secAttack = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the secondary charge value of the unit. This is an integer representing the secondary charge power of a unit.
        /// The setter clamps the value between a predefined minimum and maximum, and logs changes to the secondary charge value.
        /// </summary>
        public int SecCharge
        {
            get => _secCharge;
            set
            {
                value = ClampUnitStat(value);
                AddChange(nameof(SecCharge), _secCharge, value);
                _secCharge = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the secondary projectile type of the unit. This is a string representing the type of projectile used in a unit's secondary attack.
        /// The setter validates the input and logs changes. If the value is null, it defaults to "no".
        /// </summary>
        public string? SecProjectile
        {
            get => _secProjectile;
            set
            {
                value ??= "no";
                AddChange(nameof(SecProjectile), _secProjectile ?? "", value);
                _secProjectile = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the secondary range of the unit. This is an integer representing the range of a unit's secondary attack.
        /// The setter logs changes to the secondary range.
        /// </summary>
        public int SecRange
        {
            get => _secRange;
            set
            {
                AddChange(nameof(SecRange), _secRange, value);
                _secRange = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the secondary ammunition of the unit. This is an integer representing the amount of ammunition for a unit's secondary attack.
        /// The setter logs changes to the secondary ammunition.
        /// </summary>
        public int SecAmmunition
        {
            get => _secAmmunition;
            set
            {
                AddChange(nameof(SecAmmunition), _secAmmunition, value);
                _secAmmunition = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the secondary weapon type of the unit. This is a string representing the type of weapon used in a unit's secondary attack.
        /// The setter validates the input against a predefined list of weapon types. If the weapon type does not exist, an error is logged.
        /// </summary>
        public string? SecWeaponType
        {
            get => _secWeaponType;
            set
            {
                if (value == null) return;
                if (!WeaponTypes.Contains(value.ToLower()))
                {
                    ErrorDb.AddError($"Weapon type {value} does not exist.");
                    return;
                }
                AddChange(nameof(SecWeaponType), _secWeaponType ?? "", value);
                _secWeaponType = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the secondary technology type of the unit. This is a string representing the technology type of a unit's secondary attack.
        /// The setter validates the input against a predefined list of technology types. If the technology type does not exist, an error is logged.
        /// </summary>
        public string? SecTechType
        {
            get => _secTechType;
            set
            {
                if (value == null) return;
                if (!TechTypes.Contains(value.ToLower()))
                {
                    ErrorDb.AddError($"Weapon type {value} does not exist.");
                    return;
                }
                AddChange(nameof(SecTechType), _secTechType ?? "", value);
                _secTechType = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the secondary damage type of the unit. This is a string representing the type of damage a unit's secondary attack inflicts.
        /// The setter validates the input against a predefined list of damage types. If the damage type does not exist, an error is logged.
        /// </summary>
        public string? SecDamageType
        {
            get => _secDamageType;
            set
            {
                if (value == null) return;
                if (!DamageTypes.Contains(value.ToLower()))
                {
                    ErrorDb.AddError($"Damage type {value} does not exist.");
                    return;
                }
                AddChange(nameof(SecDamageType), _secDamageType ?? "", value);
                _secDamageType = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the secondary sound type of the unit. This is a string representing the sound type of a unit's secondary attack.
        /// The setter validates the input against a predefined list of sound types. If the sound type does not exist, an error is logged.
        /// </summary>
        public string? SecSoundType
        {
            get => _secSoundType;
            set
            {
                if (value == null) return;
                if (!SoundTypes.Contains(value.ToLower()))
                {
                    ErrorDb.AddError($"Sound type {value} does not exist.");
                    return;
                }
                AddChange(nameof(SecSoundType), _secSoundType ?? "", value);
                _secSoundType = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the secondary attack delay of the unit. This is an integer representing the delay of a unit's secondary attack.
        /// The setter logs changes to the secondary attack delay.
        /// </summary>
        public int SecAttDelay
        {
            get => _secAttDelay;
            set
            {
                AddChange(nameof(SecAttDelay), _secAttDelay, value);
                _secAttDelay = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the secondary skeleton factor of the unit. This is a double representing the skeleton factor of a unit's secondary attack.
        /// The setter logs changes to the secondary skeleton factor.
        /// </summary>
        public double SecSkelFactor
        {
            get => _secSkelFactor;
            set
            {
                AddChange(nameof(SecSkelFactor), _secSkelFactor, value);
                _secSkelFactor = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the secondary attributes of the unit. This is a list of strings representing the attributes of a unit's secondary attack.
        /// </summary>
        public List<string>? SecAttr { get; set; } = new();

        /// <summary>
        /// Adds a new secondary attribute to the unit. If the attribute already exists, it will not be added.
        /// If the attribute does not exist in the global attribute types, an error is logged.
        /// </summary>
        /// <param name="attribute">The attribute to add.</param>
        public void AddSecAttr(string attribute)
        {
            if (SecAttr != null && SecAttr.Contains(attribute))
                return;
            if (!AttackAttr.Contains(attribute))
            {
                ErrorDb.AddError($"Attack attribute {attribute} does not exist.");
                return;
            }
            SecAttr ??= new List<string>();
            SecAttr.Add(attribute);
        }

        /// <summary>
        /// Gets or sets the tertiary attack value of the unit. This is an integer representing the tertiary attack power of a unit.
        /// The setter clamps the value between a predefined minimum and maximum, and logs changes to the tertiary attack value.
        /// </summary>
        public int TerAttack
        {
            get => _terAttack;
            set
            {
                value = ClampUnitStat(value);
                AddChange(nameof(TerAttack), _terAttack, value);
                _terAttack = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the tertiary charge value of the unit. This is an integer representing the tertiary charge power of a unit.
        /// The setter clamps the value between a predefined minimum and maximum, and logs changes to the tertiary charge value.
        /// </summary>
        public int TerCharge
        {
            get => _terCharge;
            set
            {
                value = ClampUnitStat(value);
                AddChange(nameof(TerCharge), _terCharge, value);
                _terCharge = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the tertiary projectile type of the unit. This is a string representing the type of projectile used in a unit's tertiary attack.
        /// The setter validates the input and logs changes. If the value is null, it defaults to "no".
        /// </summary>
        public string? TerProjectile
        {
            get => _terProjectile;
            set
            {
                value ??= "no";
                AddChange(nameof(TerProjectile), _terProjectile ?? "", value);
                _terProjectile = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the tertiary range of the unit. This is an integer representing the range of a unit's tertiary attack.
        /// The setter logs changes to the tertiary range.
        /// </summary>
        public int TerRange
        {
            get => _terRange;
            set
            {
                AddChange(nameof(TerRange), _terRange, value);
                _terRange = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the tertiary ammunition of the unit. This is an integer representing the amount of ammunition for a unit's tertiary attack.
        /// The setter logs changes to the tertiary ammunition.
        /// </summary>
        public int TerAmmunition
        {
            get => _terAmmunition;
            set
            {
                AddChange(nameof(TerAmmunition), _terAmmunition, value);
                _terAmmunition = value;
                NotifyPropertyChanged();
            }
        }
        
        /// <summary>
        /// Gets or sets the tertiary weapon type of the unit. This is a string representing the type of weapon used in a unit's tertiary attack.
        /// The setter validates the input against a predefined list of weapon types. If the weapon type does not exist, an error is logged.
        /// </summary>
        public string? TerWeaponType
        {
            get => _terWeaponType;
            set
            {
                if (value == null) return;
                if (!WeaponTypes.Contains(value.ToLower()))
                {
                    ErrorDb.AddError($"Weapon type {value} does not exist.");
                    return;
                }
                AddChange(nameof(TerWeaponType), _terWeaponType ?? "", value);
                _terWeaponType = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the tertiary technology type of the unit. This is a string representing the technology type of a unit's tertiary attack.
        /// The setter validates the input against a predefined list of technology types. If the technology type does not exist, an error is logged.
        /// </summary>
        public string? TerTechType
        {
            get => _terTechType;
            set
            {
                if (value == null) return;
                if (!TechTypes.Contains(value.ToLower()))
                {
                    ErrorDb.AddError($"Weapon type {value} does not exist.");
                    return;
                }
                AddChange(nameof(TerTechType), _terTechType ?? "", value);
                _terTechType = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the tertiary damage type of the unit. This is a string representing the type of damage a unit's tertiary attack inflicts.
        /// The setter validates the input against a predefined list of damage types. If the damage type does not exist, an error is logged.
        /// </summary>
        public string? TerDamageType
        {
            get => _terDamageType;
            set
            {
                if (value == null) return;
                if (!DamageTypes.Contains(value.ToLower()))
                {
                    ErrorDb.AddError($"Damage type {value} does not exist.");
                    return;
                }
                AddChange(nameof(TerDamageType), _terDamageType ?? "", value);
                _terDamageType = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the tertiary sound type of the unit. This is a string representing the sound type of a unit's tertiary attack.
        /// The setter validates the input against a predefined list of sound types. If the sound type does not exist, an error is logged.
        /// </summary>
        public string? TerSoundType
        {
            get => _terSoundType;
            set
            {
                if (value == null) return;
                if (!SoundTypes.Contains(value.ToLower()))
                {
                    ErrorDb.AddError($"Sound type {value} does not exist.");
                    return;
                }
                AddChange(nameof(TerSoundType), _terSoundType ?? "", value);
                _terSoundType = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the tertiary attack delay of the unit. This is an integer representing the delay of a unit's tertiary attack.
        /// The setter logs changes to the tertiary attack delay.
        /// </summary>
        public int TerAttDelay
        {
            get => _terAttDelay;
            set
            {
                AddChange(nameof(TerAttDelay), _terAttDelay, value);
                _terAttDelay = value;
                NotifyPropertyChanged();
            }
        }
        
        /// <summary>
        /// Gets or sets the tertiary skeleton factor of the unit. This is a double representing the skeleton factor of a unit's tertiary attack.
        /// The setter logs changes to the tertiary skeleton factor.
        /// </summary>
        public double TerSkelFactor
        {
            get => _terSkelFactor;
            set
            {
                AddChange(nameof(TerSkelFactor), _terSkelFactor, value);
                _terSkelFactor = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the tertiary attributes of the unit. This is a list of strings representing the attributes of a unit's tertiary attack.
        /// </summary>
        public List<string>? TerAttr { get; set; } = new();

        /// <summary>
        /// Adds a new tertiary attribute to the unit. If the attribute already exists, it will not be added.
        /// If the attribute does not exist in the global attribute types, an error is logged.
        /// </summary>
        /// <param name="attribute">The attribute to add.</param>
        public void AddTerAttr(string attribute)
        {
            // Check if the tertiary attribute list is not null and contains the attribute
            if (TerAttr != null && TerAttr.Contains(attribute))
                return;

            // Check if the attribute exists in the global attribute types
            if (!AttackAttr.Contains(attribute))
            {
                // Log an error if the attribute does not exist
                ErrorDb.AddError($"Attack attribute {attribute} does not exist.");
                return;
            }

            // Initialize the tertiary attribute list if it is null
            TerAttr ??= new List<string>();

            // Add the attribute to the tertiary attribute list
            TerAttr.Add(attribute);
        }
        
        /// <summary>
        /// Gets or sets the primary armour value of the unit. This is an integer representing the primary armour value of a unit.
        /// The setter clamps the value to a minimum of 0 using the ClampUnitStat method, logs changes to the primary armour value, and then sets the primary armour value.
        /// </summary>
        public int PriArmour
        {
            get => _priArmour;
            set
            {
                value = ClampUnitStat(value, 0);
                AddChange(nameof(PriArmour), _priArmour, value);
                _priArmour = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the formation style of the unit. This is a string representing the formation style a unit can adopt.
        /// The setter validates the input against a predefined list of formation styles. If the formation style does not exist, an error is logged.
        /// </summary>
        public string? FormationStyle
        {
            get => _formationStyle;
            set
            {
                if (value == null) return;
                if (!FormationStyles.Contains(value.ToLower()))
                {
                    ErrorDb.AddError($"Formation style {value} does not exist.");
                    return;
                }
                AddChange(nameof(FormationStyle), _formationStyle ?? "", value);
                _formationStyle = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the primary defense value of the unit. This is an integer representing the primary defense power of a unit.
        /// The setter clamps the value to a minimum of 0 using the ClampUnitStat method, logs changes to the primary defense value, and then sets the primary defense value.
        /// </summary>
        public int PriDefense
        {
            get => _priDefense;
            set
            {
                value = ClampUnitStat(value, 0);
                AddChange(nameof(PriDefense), _priDefense, value);
                _priDefense = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the primary shield value of the unit. This is an integer representing the primary shield power of a unit.
        /// The setter clamps the value to a minimum of 0 using the ClampUnitStat method, logs changes to the primary shield value, and then sets the primary shield value.
        /// </summary>
        public int PriShield
        {
            get => _priShield;
            set
            {
                value = ClampUnitStat(value, 0);
                AddChange(nameof(PriShield), _priShield, value);
                _priShield = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the primary defense sound type of the unit. This is a string representing the sound type of a unit's primary defense.
        /// The setter validates the input against a predefined list of sound types. If the sound type does not exist, an error is logged.
        /// </summary>
        public string? PriDefSound
        {
            get => _priDefSound;
            set
            {
                if (value == null) return;
                if (!SoundTypesDef.Contains(value.ToLower()))
                {
                    ErrorDb.AddError($"Defence sound {value} does not exist.");
                    return;
                }
                AddChange(nameof(PriDefSound), _priDefSound ?? "", value);
                _priDefSound = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the secondary armor value of the unit. This is an integer representing the secondary armor value of a unit.
        /// The setter clamps the value to a minimum of 0 using the ClampUnitStat method, logs changes to the secondary armor value, and then sets the secondary armor value.
        /// </summary>
        public int SecArmour
        {
            get => _secArmour;
            set
            {
                value = ClampUnitStat(value, 0);
                AddChange(nameof(SecArmour), _secArmour, value);
                _secArmour = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the secondary defense value of the unit. This is an integer representing the secondary defense power of a unit.
        /// The setter clamps the value to a minimum of 0 using the ClampUnitStat method, logs changes to the secondary defense value, and then sets the secondary defense value.
        /// </summary>
        public int SecDefense
        {
            get => _secDefense;
            set
            {
                value = ClampUnitStat(value, 0);
                AddChange(nameof(SecDefense), _secDefense, value);
                _secDefense = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the secondary defense sound type of the unit. This is a string representing the sound type of a unit's secondary defense.
        /// The setter validates the input against a predefined list of sound types. If the sound type does not exist, an error is logged.
        /// </summary>
        public string? SecDefSound
        {
            get => _secDefSound;
            set
            {
                if (value == null) return;
                if (!SoundTypesDef.Contains(value.ToLower()))
                {
                    ErrorDb.AddError($"Defence sound {value} does not exist.");
                    return;
                }
                AddChange(nameof(SecDefSound), _secDefSound ?? "", value);
                _secDefSound = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the heat resistance of the unit. This is an integer representing the unit's resistance to heat.
        /// The setter clamps the value to a minimum of 0 using the ClampUnitStat method, logs changes to the heat resistance, and then sets the heat resistance.
        /// </summary>
        public int StatHeat
        {
            get => _statHeat;
            set
            {
                value = ClampUnitStat(value, 0);
                AddChange(nameof(StatHeat), _statHeat, value);
                _statHeat = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the scrub resistance of the unit. This is an integer representing the unit's resistance to scrub.
        /// The setter clamps the value to a minimum of 0 using the ClampUnitStat method, logs changes to the scrub resistance, and then sets the scrub resistance.
        /// </summary>
        public int StatScrub
        {
            get => _statScrub;
            set
            {
                value = ClampUnitStat(value, 0);
                AddChange(nameof(StatScrub), _statScrub, value);
                _statScrub = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the forest resistance of the unit. This is an integer representing the unit's resistance to forest.
        /// The setter clamps the value to a minimum of 0 using the ClampUnitStat method, logs changes to the forest resistance, and then sets the forest resistance.
        /// </summary>
        public int StatForest
        {
            get => _statForest;
            set
            {
                value = ClampUnitStat(value, 0);
                AddChange(nameof(StatForest), _statForest, value);
                _statForest = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the snow resistance of the unit. This is an integer representing the unit's resistance to snow.
        /// The setter clamps the value to a minimum of 0 using the ClampUnitStat method, logs changes to the snow resistance, and then sets the snow resistance.
        /// </summary>
        public int StatSnow
        {
            get => _statSnow;
            set
            {
                value = ClampUnitStat(value, 0);
                AddChange(nameof(StatSnow), _statSnow, value);
                _statSnow = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the sand resistance of the unit. This is an integer representing the unit's resistance to sand.
        /// The setter clamps the value to a minimum of 0 using the ClampUnitStat method, logs changes to the sand resistance, and then sets the sand resistance.
        /// </summary>
        public int StatSand
        {
            get => _statSand;
            set
            {
                value = ClampUnitStat(value, 0);
                AddChange(nameof(StatSand), _statSand, value);
                _statSand = value;
                NotifyPropertyChanged();
            }
        }
        
        /// <summary>
        /// Gets or sets the morale of the unit. This is an integer representing the morale of a unit.
        /// The setter clamps the value to a minimum of 0 using the ClampUnitStat method, logs changes to the morale, and then sets the morale.
        /// </summary>
        public int Morale
        {
            get => _morale;
            set
            {
                value = ClampUnitStat(value, 0);
                AddChange(nameof(Morale), _morale, value);
                _morale = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the discipline of the unit. This is a string representing the discipline of a unit.
        /// The setter validates the input against a predefined list of discipline types. If the discipline type does not exist, an error is logged.
        /// </summary>
        public string? Discipline
        {
            get => _discipline;
            set
            {
                if (value == null) return;
                if (!DisciplineTypes.Contains(value.ToLower()))
                {
                    ErrorDb.AddError($"Discipline type {value} does not exist.");
                    return;
                }
                AddChange(nameof(Discipline), _discipline ?? "", value);
                _discipline = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the training of the unit. This is a string representing the training of a unit.
        /// The setter validates the input against a predefined list of training types. If the training type does not exist, an error is logged.
        /// </summary>
        public string? Training
        {
            get => _training;
            set
            {
                if (value == null) return;
                if (!TrainedTypes.Contains(value.ToLower()))
                {
                    ErrorDb.AddError($"Training type {value} does not exist.");
                    return;
                }
                AddChange(nameof(Training), _training ?? "", value);
                _training = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the charge distance of the unit. This is an integer representing the charge distance of a unit.
        /// The setter logs changes to the charge distance.
        /// </summary>
        public int StatChargeDist
        {
            get => _statChargeDist;
            set
            {
                AddChange(nameof(StatChargeDist), _statChargeDist, value);
                _statChargeDist = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the fire delay of the unit. This is an integer representing the fire delay of a unit.
        /// The setter logs changes to the fire delay.
        /// </summary>
        public int StatFireDelay
        {
            get => _statFireDelay;
            set
            {
                AddChange(nameof(StatFireDelay), _statFireDelay, value);
                _statFireDelay = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the food statistic of the unit. This is an integer representing the food statistic of a unit.
        /// The setter logs changes to the food statistic.
        /// </summary>
        public int StatFood
        {
            get => _statFood;
            set
            {
                AddChange(nameof(StatFood), _statFood, value);
                _statFood = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the food per second statistic of the unit. This is an integer representing the food per second statistic of a unit.
        /// The setter logs changes to the food per second statistic.
        /// </summary>
        public int StatFoodSec
        {
            get => _statFoodSec;
            set
            {
                AddChange(nameof(StatFoodSec), _statFoodSec, value);
                _statFoodSec = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the recruit time of the unit. This is an integer representing the time it takes to recruit a unit.
        /// The setter logs changes to the recruit time.
        /// </summary>
        public int RecruitTime
        {
            get => _recruitTime;
            set
            {
                AddChange(nameof(RecruitTime), _recruitTime, value);
                _recruitTime = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the recruit cost of the unit. This is an integer representing the cost to recruit a unit.
        /// The setter logs changes to the recruit cost.
        /// </summary>
        public int RecruitCost
        {
            get => _recruitCost;
            set
            {
                AddChange(nameof(RecruitCost), _recruitCost, value);
                _recruitCost = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the upkeep of the unit. This is an integer representing the upkeep cost of a unit.
        /// The setter logs changes to the upkeep.
        /// </summary>
        public int Upkeep
        {
            get => _upkeep;
            set
            {
                AddChange(nameof(Upkeep), _upkeep, value);
                _upkeep = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the weapon cost of the unit. This is an integer representing the cost of the unit's weapon.
        /// The setter logs changes to the weapon cost.
        /// </summary>
        public int WpnCost
        {
            get => _wpnCost;
            set
            {
                AddChange(nameof(WpnCost), _wpnCost, value);
                _wpnCost = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the armour cost of the unit. This is an integer representing the cost of the unit's armour.
        /// The setter logs changes to the armour cost.
        /// </summary>
        public int ArmourCost
        {
            get => _armourCost;
            set
            {
                AddChange(nameof(ArmourCost), _armourCost, value);
                _armourCost = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the custom cost of the unit. This is an integer representing the custom cost of the unit.
        /// The setter logs changes to the custom cost.
        /// </summary>
        public int CustomCost
        {
            get => _customCost;
            set
            {
                AddChange(nameof(CustomCost), _customCost, value);
                _customCost = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the custom limit of the unit. This is an integer representing the custom limit of the unit.
        /// The setter logs changes to the custom limit.
        /// </summary>
        public int CustomLimit
        {
            get => _customLimit;
            set
            {
                AddChange(nameof(CustomLimit), _customLimit, value);
                _customLimit = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the custom increase of the unit. This is an integer representing the custom increase of the unit.
        /// The setter logs changes to the custom increase.
        /// </summary>
        public int CustomIncrease
        {
            get => _customIncrease;
            set
            {
                AddChange(nameof(CustomIncrease), _customIncrease, value);
                _customIncrease = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the move speed of the unit. This is a double representing the move speed of a unit.
        /// The setter logs changes to the move speed.
        /// </summary>
        public double MoveSpeed
        {
            get => _moveSpeed;
            set
            {
                AddChange(nameof(MoveSpeed), _moveSpeed, value);
                _moveSpeed = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the stat_stl statistic of the unit. This is the minimum numbers of soldiers that need to exist for the unit to not be killed off.
        /// The setter logs changes to the stl statistic.
        /// </summary>
        public int StatStl
        {
            get => _statStl;
            set
            {
                AddChange(nameof(StatStl), _statStl, value);
                _statStl = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the base armour levels of the unit. This is a string representing the base armour level a unit has.
        /// The setter validates the input, logs changes to the base armour level, and then sets the base armour level.
        /// </summary>
        public string? ArmourlvlBase
        {
            get => _armourlvlBase;
            set
            {
                if (value == null) return;
                AddChange(nameof(ArmourlvlBase), _armourlvlBase ?? "", value);
                _armourlvlBase = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the first armour levels of the unit. This is a string representing the first armour level a unit has.
        /// The setter validates the input, logs changes to the first armour level, and then sets the first armour level.
        /// </summary>
        public string? ArmourlvlOne
        {
            get => _armourlvlOne;
            set
            {
                if (value == null) return;
                AddChange(nameof(ArmourlvlOne), _armourlvlOne ?? "", value);
                _armourlvlOne = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the second armour levels of the unit. This is a string representing the second armour level a unit has.
        /// The setter validates the input, logs changes to the second armour level, and then sets the second armour level.
        /// </summary>
        public string? ArmourlvlTwo
        {
            get => _armourlvlTwo;
            set
            {
                if (value == null) return;
                AddChange(nameof(ArmourlvlTwo), _armourlvlTwo ?? "", value);
                _armourlvlTwo = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the third armour levels of the unit. This is a string representing the third armour level a unit has.
        /// The setter validates the input, logs changes to the third armour level, and then sets the third armour level.
        /// </summary>
        public string? ArmourlvlThree
        {
            get => _armourlvlThree;
            set
            {
                if (value == null) return;
                AddChange(nameof(ArmourlvlThree), _armourlvlThree ?? "", value);
                _armourlvlThree = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the base armour model of the unit. This is a string representing the base armour model a unit has.
        /// The setter validates the input against the battle models database. If the model does not exist, an error is logged.
        /// </summary>
        public string ArmourModelBase
        {
            get
            {
                if (_armourModelBaseEntry != null)
                    _armourModelBase = _armourModelBaseEntry.Name;
                return _armourModelBase;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;
                var old = _armourModelBase;
                if (old == value)
                    return;
                AddChange(nameof(ArmourModelBase), old, value);
                _armourModelBaseEntry = value != string.Empty ? ModData.BattleModelDb.Get(value.ToLower()) : null;
                _armourModelBase = value;
                NotifyPropertyChanged();
                UpdateModelUsage(old, value);
            }
        }

        /// <summary>
        /// Gets or sets the first armour model of the unit. This is a string representing the first armour model a unit has.
        /// The setter validates the input against the battle models database. If the model does not exist, an error is logged.
        /// </summary>
        public string ArmourModelOne
        {
            get => _armourModelOneEntry != null ? _armourModelOneEntry.Name : _armourModelOne;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;
                var old = ArmourModelOne;
                if (old == value)
                    return;
                AddChange(nameof(ArmourModelOne), old, value);
                _armourModelOneEntry = value != string.Empty ? ModData.BattleModelDb.Get(value.ToLower()) : null;
                _armourModelOne = value;
                NotifyPropertyChanged();
                UpdateModelUsage(old, value);
            }
        }

        /// <summary>
        /// Gets or sets the second armour model of the unit. This is a string representing the second armour model a unit has.
        /// The setter validates the input against the battle models database. If the model does not exist, an error is logged.
        /// </summary>
        public string ArmourModelTwo
        {
            get => _armourModelTwoEntry != null ? _armourModelTwoEntry.Name : _armourModelTwo;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;
                var old = ArmourModelTwo;
                if (old == value)
                    return;
                AddChange(nameof(ArmourModelTwo), old, value);
                _armourModelTwoEntry = value != string.Empty ? ModData.BattleModelDb.Get(value.ToLower()) : null;
                _armourModelTwo = value;
                NotifyPropertyChanged();
                UpdateModelUsage(old, value);
            }
        }

        /// <summary>
        /// Gets or sets the third armour model of the unit. This is a string representing the third armour model a unit has.
        /// The setter validates the input against the battle models database. If the model does not exist, an error is logged.
        /// </summary>
        public string ArmourModelThree
        {
            get => _armourModelThreeEntry != null ? _armourModelThreeEntry.Name : _armourModelThree;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;
                var old = ArmourModelThree;
                if (old == value)
                    return;
                AddChange(nameof(ArmourModelThree), old, value);
                _armourModelThreeEntry = value != string.Empty ? ModData.BattleModelDb.Get(value.ToLower()) : null;
                _armourModelThree = value;
                NotifyPropertyChanged();
                UpdateModelUsage(old, value);
            }
        }

        /// <summary>
        /// Gets or sets the Ownership of the unit. This is a list of strings representing the factions that own the unit.
        /// </summary>
        public List<string> Ownership { get; set; } = new();

        /// <summary>
        /// Adds a new faction to the Ownership of the unit. If the faction already exists in the Ownership, it will not be added.
        /// </summary>
        /// <param name="faction">The faction to add.</param>
        public void AddOwnership(string faction)
        {
            if (Ownership.Contains(faction))
                return;
            Ownership.Add(faction);
        }

        /// <summary>
        /// Gets or sets the EraZero of the unit. This is a list of strings representing the factions that own the unit in EraZero.
        /// </summary>
        public List<string> EraZero { get; set; } = new List<string>();

        /// <summary>
        /// Adds a new faction to the EraZero of the unit. If the faction already exists in the EraZero, it will not be added.
        /// </summary>
        /// <param name="faction">The faction to add.</param>
        public void AddEraZero(string faction)
        {
            if (EraZero.Contains(faction))
                return;
            EraZero.Add(faction);
        }

        /// <summary>
        /// Gets or sets the EraOne of the unit. This is a list of strings representing the factions that own the unit in EraOne.
        /// </summary>
        public List<string> EraOne { get; set; } = new List<string>();

        /// <summary>
        /// Adds a new faction to the EraOne of the unit. If the faction already exists in the EraOne, it will not be added.
        /// </summary>
        /// <param name="faction">The faction to add.</param>
        public void AddEraOne(string faction)
        {
            if (EraOne.Contains(faction))
                return;
            EraOne.Add(faction);
        }

        /// <summary>
        /// Gets or sets the EraTwo of the unit. This is a list of strings representing the factions that own the unit in EraTwo.
        /// </summary>
        public List<string> EraTwo { get; set; } = new List<string>();

        /// <summary>
        /// Adds a new faction to the EraTwo of the unit. If the faction already exists in the EraTwo, it will not be added.
        /// </summary>
        /// <param name="faction">The faction to add.</param>
        public void AddEraTwo(string faction)
        {
            if (EraTwo.Contains(faction))
                return;
            EraTwo.Add(faction);
        }

        /// <summary>
        /// Gets or sets the RecruitPriorityOffset of the unit. This is a double representing the priority offset for recruiting the unit.
        /// The setter logs changes to the RecruitPriorityOffset.
        /// </summary>
        public double RecruitPriorityOffset
        {
            get => _recruitPriorityOffset;
            set
            {
                AddChange(nameof(RecruitPriorityOffset),_recruitPriorityOffset, value);
                _recruitPriorityOffset = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the InfoDict of the unit. This is a string representing the dictionary entry for the unit's information.
        /// The setter validates the input, logs changes to the InfoDict, and then sets the InfoDict.
        /// </summary>
        public string? InfoDict
        {
            get => _infoDict;
            set 
            {
                if (value == null) return;
                AddChange(nameof(InfoDict),_infoDict ?? "", value);
                _infoDict = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the CardDict of the unit. This is a string representing the dictionary entry for the unit's card.
        /// The setter validates the input, logs changes to the CardDict, and then sets the CardDict.
        /// </summary>
        public string? CardDict
        {
            get => _cardDict;
            set 
            {
                if (value == null) return;
                AddChange(nameof(CardDict),_cardDict ?? "", value);
                _cardDict = value;
                NotifyPropertyChanged();
            }
        }


        /// <summary>
        /// Gets or sets the CrusadeUpkeep value of the Unit.
        /// </summary>
        /// <remarks>
        /// The CrusadeUpkeep property represents the upkeep cost of the Unit during a crusade.
        /// </remarks>
        public double CrusadeUpkeep
        {
            get => _crusadeUpkeep;
            set
            {
                AddChange(nameof(CrusadeUpkeep),_crusadeUpkeep, value);
                _crusadeUpkeep = value;
                NotifyPropertyChanged();
            }
        }
            
        /// <summary>
        /// Gets or sets the SpacingRanks of the unit. This is an integer representing the number of ranks in the unit's formation.
        /// The setter logs changes to the SpacingRanks.
        /// </summary>
        public int SpacingRanks
        {
            get => _spacingRanks;
            set
            {
                AddChange(nameof(SpacingRanks),_spacingRanks, value);
                _spacingRanks = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the LockMorale of the unit. This is a boolean indicating whether the unit's morale is locked.
        /// The setter logs changes to the LockMorale.
        /// </summary>
        public bool LockMorale
        {
            get => _lockMorale;
            set
            {
                AddChange(nameof(LockMorale),_lockMorale, value);
                _lockMorale = value;
                NotifyPropertyChanged();
            }
        }
        
        /// <summary>
        /// Gets or sets the PriFireType of the unit. This is a string representing the type of fire used in a unit's primary attack.
        /// The setter logs changes to the PriFireType.
        /// </summary>
        public string? PriFireType
        {
            get => _priFireType;
            set
            {
                if (value == null) return;
                AddChange(nameof(PriFireType),_priFireType ?? "", value);
                _priFireType = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the SecFireType of the unit. This is a string representing the type of fire used in a unit's secondary attack.
        /// The setter logs changes to the SecFireType.
        /// </summary>
        public string? SecFireType
        {
            get => _secFireType;
            set
            {
                if (value == null) return;
                AddChange(nameof(SecFireType),_secFireType ?? "", value);
                _secFireType = value;
                NotifyPropertyChanged();
            }
        }
        
        /// <summary>
        /// Gets or sets the TerFireType of the unit. This is a string representing the type of fire used in a unit's tertiary attack.
        /// The setter logs changes to the TerFireType.
        /// </summary>
        public string? TerFireType
        {
            get => _terFireType;
            set
            {
                if (value == null) return;
                AddChange(nameof(TerFireType),_terFireType ?? "", value);
                _terFireType = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Descr of the unit. This is a string representing the description of the unit.
        /// The setter validates the input, logs changes to the Descr, and then sets the Descr.
        /// </summary>
        public string? Descr
        {
            get => _descr;
            set
            {
                if (value == null) return;
                AddChange(nameof(Descr),_descr ?? "", value);
                _descr = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the short description of the unit.
        /// </summary>
        public string? DescrShort
        {
            get => _descrShort;
            set
            {
                if (value == null) return;
                AddChange(nameof(DescrShort),_descrShort ?? "", value);
                _descrShort = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Represents a card for a unit in a game.
        /// </summary>
        public string Card { get; set; } = "";

        /// <summary>
        /// Gets or sets the MercenaryUnit property of the unit. This is a boolean indicating whether the unit is a mercenary.
        /// </summary>
        public bool MercenaryUnit { get; set; }

        /// <summary>
        /// Gets or sets the IsEopUnit property of the unit. This is a boolean indicating whether the unit is an EopUnit.
        /// </summary>
        public bool IsEopUnit { get; init; }

        /// <summary>
        /// Gets or sets the FilePath of the unit. This is a string representing the file path of the unit.
        /// </summary>
        public string FilePath { get; set; } = "";

        /// <summary>
        /// Gets or sets the EduIndex of the unit. This is an integer representing the education index of the unit.
        /// </summary>
        public int EduIndex { get; set; }

        /// <summary>
        /// Gets or sets the AiUnitValue of the unit. This is a double representing the AI unit value of the unit.
        /// </summary>
        public double AiUnitValue { get; set; }

        /// <summary>
        /// Gets the value per cost ratio of the unit.
        /// </summary>
        /// <remarks>
        /// The value per cost ratio is calculated as the AI unit value divided by the recruit cost.
        /// </remarks>
        public double ValuePerCost => Math.Round(AiUnitValue / RecruitCost, 2);

        /// <summary>
        /// The value of the unit divided by its upkeep cost.
        /// </summary>
        /// <remarks>
        /// This property calculates the value per upkeep by dividing the aiUnitValue property of the unit by its upkeep property. The result is rounded to two decimal places.
        /// </remarks>
        public double ValuePerUpkeep => Math.Round(AiUnitValue / Upkeep, 2);

        /// <summary>
        /// Gets or sets the card information picture for the unit.
        /// </summary>
        /// <value>
        /// The card information.
        /// </value>
        public string CardInfo { get; set; } = "";

        /// <summary>
        /// Gets or sets the symbol representing the faction.
        /// </summary>
        public string FactionSymbol { get; set; } = "";

        #endregion

        #region Methods

        /// <summary>
        /// Clamps the unit stat value between the specified minimum and maximum values.
        /// </summary>
        /// <param name="value">The value of the unit stat.</param>
        /// <param name="min">The minimum value the unit stat can have. Default is 1.</param>
        /// <param name="max">The maximum value the unit stat can have. Default is 63.</param>
        /// <returns>The clamped value of the unit stat.</returns>
        private static int ClampUnitStat(int value, int min = 1, int max = 63) => Math.Clamp(value, min, max);

        /// <summary>
        /// Checks if the given model name matches any of the unit's model properties.
        /// </summary>
        /// <param name="model">The name of the model to check.</param>
        /// <returns>True if the model name matches any of the unit's model properties, false otherwise.</returns>
        public bool ContainsModel(string model)
        {
            return ArmourModelBase == model 
                   || ArmourModelOne == model 
                   || ArmourModelTwo == model 
                   || ArmourModelThree == model
                   || Soldier == model
                   || Officer1 == model
                   || Officer2 == model
                   || Officer3 == model;
        }

        /// <summary>
        /// Updates the usage of a model in the unit.
        /// </summary>
        /// <param name="oldModelName">The name of the model to be replaced.</param>
        /// <param name="newModelName">The name of the model to replace with.</param>
        /// <remarks>
        /// If the new model exists, it is added to the unit's model usage.
        /// If the old model is not used by the unit anymore, it is removed from the unit's model usage.
        /// </remarks>
        public void UpdateModelUsage(string oldModelName, string newModelName)
        {
            var newModel = ModData.BattleModelDb.Get(newModelName.ToLower());
            if (!string.IsNullOrWhiteSpace(newModelName) && newModel == null)
                ErrorDb.AddError($"Model entry {newModelName} does not exist.");
            else if (newModel != null && !newModel.ModelUsage.Units.Contains(this))
                newModel.ModelUsage.Units.Add(this);
            if (string.IsNullOrWhiteSpace(oldModelName) ||
                !ModData.BattleModelDb.Contains(oldModelName.ToLower())) return;
            if (ContainsModel(oldModelName)) return;
            var oldModel = ModData.BattleModelDb.Get(oldModelName.ToLower());
            oldModel?.ModelUsage.Units.Remove(this);
        }

        /// <summary>
        /// Clones a Unit object with the specified name and localized name.
        /// </summary>
        /// <param name="name">The name of the new Unit object.</param>
        /// <param name="localizedName">The localized name of the new Unit object.</param>
        /// <param name="unit">The Unit object to clone.</param>
        /// <returns>A cloned Unit object with the specified name and localized name.</returns>
        public static Unit? CloneUnit(string name, string localizedName, Unit unit)
        {
            if (ModData.Units.Contains(name))
            {
                ErrorDb.AddError($"Unit {name} already exists.");
                return null;
            }
            var newUnit = (Unit)unit.MemberwiseClone();
            newUnit.MountEffect = new List<string>(unit.MountEffect);
            newUnit.Attributes = new List<string>(unit.Attributes);
            newUnit.Comments = new Dictionary<string, List<string>>();
            newUnit.CommentsInLine = new Dictionary<string, string>();
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
            newUnit.GetCards();
            return newUnit;
        }

        /// <summary>
        /// Generates an export_units entry string for the given Unit object.
        /// </summary>
        /// <param name="unit">The Unit object to generate the export_units entry for.</param>
        /// <returns>The generated export_units entry string.</returns>
        public static string WriteEuEntry(Unit unit)
        {
            var entry = "\u00ac-----\n";
            entry += "{" + unit.Dictionary + "}" + unit.LocalizedName + "\n";
            entry += "{" + unit.Dictionary + "_descr}" + unit.Descr + "\n";
            entry += "{" + unit.Dictionary + "_descr_short}" + unit.DescrShort + "\n";
            return entry;
        }
        /// <summary>
        /// Returns a string representation of the unit's attribute based on the provided identifier.
        /// </summary>
        /// <param name="identifier">The identifier for the attribute.</param>
        /// <returns>A string representation of the attribute.</returns>
        /// <remarks>
        /// This method uses a switch statement to determine which attribute to return based on the provided identifier.
        /// It handles every edu field.
        /// </remarks>
        protected override string GetTypeTextField(string identifier)
        {
            return identifier switch
            {
                "type" => identifier + GetTabLength(identifier) + Type,
                "dictionary" => identifier + GetTabLength(identifier) + Dictionary,
                "category" => identifier + GetTabLength(identifier) + Category,
                "class" => identifier + GetTabLength(identifier) + ClassType,
                "voice_type" => identifier + GetTabLength(identifier) + VoiceType,
                "accent" => ConditionalString(Accent, identifier + GetTabLength(identifier) + Accent),
                "banner faction" => ConditionalString(BannerFaction, identifier + GetTabLength(identifier) + BannerFaction),
                "banner holy" => ConditionalString(BannerHoly, identifier + GetTabLength(identifier) + BannerHoly),
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

        /// <summary>
        /// Generates a string representation of the unit's attributes for export.
        /// </summary>
        /// <returns>A string containing the unit's attributes formatted for export.</returns>
        /// <remarks>
        /// This method iterates over the EduIdentifiers of the unit, adding each attribute to the export string.
        /// If the unit is not an EopUnit, it also adds the AI unit value, value per cost, and value per upkeep to the export string.
        /// </remarks>
        public string WriteEntry()
        {
            var entry = "";
            // Iterate over the EduIdentifiers of the unit
            foreach (var id in EduIdentifiers)
            {
                // Try to add the identifier to the CommentsInLine dictionary
                CommentsInLine.TryAdd(id, "");
                // Add the attribute to the export string
                entry += GetTextField(id);
            }
            // If the unit is not an EopUnit, add additional attributes to the export string
            if (!IsEopUnit)
            {
                // If the AddUnitValue option is enabled, add the AI unit value to the export string
                if (GlobalOptionsInstance.AddUnitValue)
                {
                    entry += ";ai_unit_value\t\t\t" + FormatFloat(AiUnitValue) + "\n";
                }
                // If the AddUnitValuePerCost option is enabled, add the value per cost to the export string
                if (GlobalOptionsInstance.AddUnitValuePerCost)
                {
                    entry += ";value_per_cost\t\t\t" + FormatFloat(ValuePerCost) + "\n";
                }
                // If the AddUnitValuePerUpkeep option is enabled, add the value per upkeep to the export string
                if (GlobalOptionsInstance.AddUnitValuePerUpkeep)
                {
                    entry += ";value_per_upkeep\t\t" + FormatFloat(ValuePerUpkeep) + "\n";
                }
            }
            // Add line breaks to the end of the export string
            entry += "\n";
            entry += "\n";
            entry += "\n";
            // Return the export string
            return entry;
        }


        /// <summary>
        /// Calculates the unit value based on various attributes and statistics.
        /// </summary>
        /// <returns>The calculated unit value.</returns>
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
                    var rangeModifier = PriRange <= 200 ? PriRange * 0.02 : 4.0;
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
            var statsTotal = attackValue + (4 * PriArmour) + (2 * PriDefense) + (2 * PriShield);

            const double maxMorale = 12.0;
            const double adjustmentScale = 0.55;
            const double baseOffset = 0.25;

            var moraleValue = Math.Clamp(Morale, 0.0, maxMorale);
            var normalizedMorale = moraleValue / maxMorale;
            var adjustedMorale = normalizedMorale * adjustmentScale + baseOffset;
            var moraleEffect = 1.0 - adjustedMorale;

            var moraleModifier = 1.0 - (moraleEffect * hasMissileModifier);
            var extrasStats = 0.0;
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

        /// <summary>
        /// Retrieves the unit cards for the unit.
        /// </summary>
        /// <returns>An array of strings containing the paths to the unit card, unit info card, and faction symbol.</returns>
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
        #endregion
    }
}