using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using ModdingTool.API;
using ModdingTool.View.InterfaceData;
using ModdingTool.View.UserControls;

namespace ModdingTool
{
    public class Globals
    {
        public static string ModPath = null!;
        public static string GamePath = null!;
        public static string ModName = "";
        public static bool ModLoaded = false;
        public static bool IsParsing = false;
        public static int ProjectileDelayStandard = 0;
        public static int ProjectileDelayFlaming = 0;
        public static int ProjectileDelayGunpowder = 0;
        public static int StartingActionPoints = 250;
        public static string OpenListType { get; set; } = "";
        public static string OpenTabType { get; set; } = "";
        public static Dictionary<string, Unit> UnitDataBase = new();
        public static Dictionary<string, string> UnitNames = new();
        public static Dictionary<string, string?> UnitDescr = new();
        public static Dictionary<string, string?> UnitDescrShort = new();
        public static Dictionary<string, BattleModel> BattleModelDataBase = new();
        public static Dictionary<string, Faction> FactionDataBase = new();
        public static Dictionary<string, Culture> CultureDataBase = new();
        public static Dictionary<string, Projectile> ProjectileDataBase = new();
        public static Dictionary<string, Engine> EngineDataBase = new();
        public static Dictionary<string, Mount> MountDataBase = new();
        public static Dictionary<string, string> ExpandedEntries = new();
        public static Dictionary<string, CharacterType> CharacterTypes = new();
        public static List<string> UsedModels = new();
        public static List<string> UsedMounts = new();
        public static readonly Errors ErrorDb = new();
        public static GlobalOptions GlobalOptionsInstance = new();

        public static ModOptions ModOptionsInstance = new();

        public class GlobalOptions
        {
            public bool UseEop = false;
            public string StartMod { get; set; }  = "";
            public bool AddUnitValuePerUpkeep { get; set; } = true;
            public bool AddUnitValuePerCost { get; set; } = true;
            public bool AddUnitValue { get; set; } = true;
        }
        public class ModOptions
        {
            public List<string> EopDirectories = new();
            public bool DisableCardImages { get; set; } = false;
        }

        public static event EventHandler<EventArgs>? ModLoadedEvent;

        public static void Print(string message)
        {
            Console.WriteLine(message);
        } 

        public static void PrintFinal()
        {
            Console.WriteLine(@"Done");
        }

        public static void PrintInt(int statement)
        {
            Console.WriteLine(statement);
        }
        
        public static void SetModPath(string path)
        {
            ModPath = path;
            var dirInfo = new DirectoryInfo(ModPath);
            if (!dirInfo.Exists)
            {
                ErrorDb.AddError("Mod path does not exist");
                return;
            }
            ModName = dirInfo.Name;
            if (dirInfo.Parent is { Parent: not null })
                GamePath = dirInfo.Parent.Parent.FullName;
            GlobalOptionsInstance.StartMod = ModPath;
        }
        
        public static void ModLoadedTrigger()
        {
            GlobalOptionsInstance.StartMod = ModPath;
            SaveOptions();
            ModLoadedEvent?.Invoke(null, EventArgs.Empty);
        }

        public static void ImportJson(string fileName)
        {
            var jsonString = File.ReadAllText(fileName);
            var imported = JsonConvert.DeserializeObject<Dictionary<string, BattleModel>>(jsonString);
            if (imported == null)
            {
                ErrorDb.AddError("Imported JSON is null");
                return;
            }
            BattleModelDataBase = new Dictionary<string, BattleModel>();
            foreach (var entry in imported)
            {
                BattleModelDataBase.Add(entry.Key, entry.Value);
            }
        }

        public static void WriteBmdb()
        {
            var newBmdb = "";
            newBmdb += "22 serialization::archive 3 0 0 0 0 ";
            newBmdb += (BattleModelDataBase.Count + 1) + " 0 0\n";
            newBmdb += "5 blank 0 0 0 1 0 0 0 0 0 0 1 0 0 0 0 0 0 0 0 0 1 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0\n";
            newBmdb = BattleModelDataBase.Aggregate(newBmdb, (current, entry) => current + entry.Value.WriteEntry());
            File.WriteAllText(@"battle_models.modeldb", newBmdb);
        }

        public static void ExportJson()
        {
            File.WriteAllText(@"bmdb.json", JsonConvert.SerializeObject(BattleModelDataBase));
        }

        public static void ClearDatabases()
        {
            UnitDataBase.Clear();
            UnitNames.Clear();
            UnitDescr.Clear();
            UnitDescrShort.Clear();
            BattleModelDataBase.Clear();
            FactionDataBase.Clear();
            CultureDataBase.Clear();
            ProjectileDataBase.Clear();
            EngineDataBase.Clear();
            MountDataBase.Clear();
            ExpandedEntries.Clear();
            CharacterTypes.Clear();
            UsedModels.Clear();
            UsedMounts.Clear();
            UnitTab.AttributeTypes = new List<string>
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
            ModOptionsInstance.EopDirectories.Clear();
            ProjectileDelayStandard = 0;
            ProjectileDelayFlaming = 0;
            ProjectileDelayGunpowder = 0;
            StartingActionPoints = 250;
        }

        public static void ParseFiles()
        {
            IsParsing = true;
            ClearDatabases();
            ErrorDb.ClearErrors();
            FactionParser.ParseExpanded();
            FactionParser.ParseCultures();
            FactionParser.ParseSmFactions();
            BmdbParser.ParseBmdb();
            EduParser.ParseEu();
            MountParser.Parse();
            EduParser.ParseEdu();
            CharacterTypesParser.parseCharacterTypes();
            BmdbParser.checkModelUsage();
            ProjectileParser.ParseProjectiles();
            UsedModels = UsedModels.Distinct().ToList();
            foreach (var entry in BattleModelDataBase.Where(entry => !UsedModels.Contains(entry.Key)))
            {
                Print(entry.Key);
                ErrorDb.AddError("Model " + entry.Key + " is not used");
                BattleModelDataBase.Remove(entry.Key);
            }
            UsedMounts = UsedMounts.Distinct().ToList();
            foreach (var entry in MountDataBase.Where(entry => !UsedMounts.Contains(entry.Key)))
            {
                Print(entry.Key);
                ErrorDb.AddError("Mount " + entry.Key + " is not used");
            }
            IsParsing = false;
            //FileRemover.CheckFiles();
        }

        public static void AutoStart()
        {
            if (Application.Current.MainWindow == null)
            {
                return;
            }
            var window = (MainWindow)Application.Current.MainWindow;
            var menubar = window.FindName("MenuBarCustom") as View.UserControls.Menubar;
            ModPath = "E:\\SteamLibrary\\steamapps\\common\\Medieval II Total War\\mods\\ago_beta";
            menubar?.LoadMod();
        }

        public static void LoadOptions()
        {
            if (!File.Exists("config/globalConfig.json"))
                File.WriteAllText("config/globalConfig.json", JsonConvert.SerializeObject(GlobalOptionsInstance, Formatting.Indented));
            else
            {
                var jsonString = File.ReadAllText("config/globalConfig.json");
                GlobalOptionsInstance = JsonConvert.DeserializeObject<GlobalOptions>(jsonString) ?? new GlobalOptions();
            }
            if (string.IsNullOrEmpty(ModName)) return;
            var configFileName = ModName + ".json";
            if (!File.Exists("config/" + configFileName))
                File.WriteAllText("config/" + configFileName, JsonConvert.SerializeObject(ModOptionsInstance, Formatting.Indented));
            else
            {
                var jsonString = File.ReadAllText("config/" + configFileName);
                ModOptionsInstance = JsonConvert.DeserializeObject<ModOptions>(jsonString) ?? new ModOptions();
            }
        }

        public static void SaveOptions()
        {
            if (File.Exists("config/globalConfig.json"))
                File.Delete("config/globalConfig.json");
            File.WriteAllText("config/globalConfig.json", JsonConvert.SerializeObject(GlobalOptionsInstance));
            if (string.IsNullOrEmpty(ModName)) return;
            var configFileName = ModName + ".json";
            if (File.Exists("config/" + configFileName))
                File.Delete("config/" + configFileName);
            File.WriteAllText("config/" + configFileName, JsonConvert.SerializeObject(ModOptionsInstance));
        }

        public static void AppStarted()
        {
            if (!Directory.Exists("config"))
                Directory.CreateDirectory("config");
            LoadOptions();
            if (!Directory.Exists("changelogs"))
                Directory.CreateDirectory("changelogs");
            if (!Directory.Exists(GlobalOptionsInstance.StartMod)) return;
            SetModPath(GlobalOptionsInstance.StartMod);
            var window = (MainWindow)Application.Current.MainWindow;
            var menubar = window.FindName("MenuBarCustom") as Menubar;
            menubar?.LoadMod();
        }
    }
}