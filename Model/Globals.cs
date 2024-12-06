using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using ModdingTool.API;
using ModdingTool.Databases;
using ModdingTool.ViewModel.InterfaceData;
using ModdingTool.View.UserControls;
using Application = System.Windows.Application;

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
        //public static Dictionary<string, Unit> UnitDataBase = new();
        //public static Dictionary<string, BattleModel> BattleModelDataBase = new();
        public static Dictionary<string, Faction> FactionDataBase = new();
        public static Dictionary<string, Culture> CultureDataBase = new();
        public static Dictionary<string, Projectile> ProjectileDataBase = new();
        public static Dictionary<string, Engine> EngineDataBase = new();
        public static Dictionary<string, Mount> MountDataBase = new();
        public static Dictionary<string, string> ExpandedEntries = new();
        public static Dictionary<string, CharacterType> CharacterTypes = new();
        public static List<string> UsedMounts = new();
        public static readonly Errors ErrorDb = new();
        public static GlobalOptions GlobalOptionsInstance = new();
        public static Data ModData = new();

        public class Data
        {
            public BattleModelDb BattleModelDb { get; } = new();
            public UnitDb Units { get; } = new();
            public BuildingDb Buildings { get; } = new();
        }

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
        /// <summary>
        /// Sets the mod path and updates related global variables.
        /// </summary>
        /// <param name="path">The path to the mod.</param>
        /// <remarks>
        /// This method sets the ModPath to the provided path, checks if the directory exists, and if it does, 
        /// it sets the ModName to the directory's name and the GamePath to the grandparent directory's full name.
        /// If the directory does not exist, it logs an error and returns.
        /// Regardless, it sets the StartMod of the GlobalOptionsInstance to the ModPath.
        /// </remarks>
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
            var window = (MainWindow)Application.Current.MainWindow!;
            var statusBar = window.FindName("StatusBarLive") as StatusBarCustom;
            statusBar?.SetStatusModPath(ModPath);
            Print("Mod path set to: " + ModPath);
        }
        
        public static void LoadMod()
        {
            LoadOptions();
            ParseFiles();
            ModLoadedTrigger();
        }

        private static void ModLoadedTrigger()
        {
            GlobalOptionsInstance.StartMod = ModPath;
            SaveOptions();
            ModLoadedEvent?.Invoke(null, EventArgs.Empty);
            ModLoaded = true;
        }
        
        public static void BackupFile(string path, string fileName)
        {
            if (!Directory.Exists(ModPath + "\\ModdingToolBackup" + path))
                Directory.CreateDirectory(ModPath + "\\ModdingToolBackup" + path);  
            var backupName = fileName + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt";
            File.Copy(ModPath + path + fileName, ModPath + "/ModdingToolBackup" + path + backupName, true);
        }
        
        public static void ClearDatabases()
        {
            ModData = new Data();
            FactionDataBase.Clear();
            CultureDataBase.Clear();
            ProjectileDataBase.Clear();
            EngineDataBase.Clear();
            MountDataBase.Clear();
            ExpandedEntries.Clear();
            CharacterTypes.Clear();
            UsedMounts.Clear();
            ProjectileDelayStandard = 0;
            ProjectileDelayFlaming = 0;
            ProjectileDelayGunpowder = 0;
            StartingActionPoints = 250;
        }
        /// <summary>
        /// Parses the files related to the game mod.
        /// </summary>
        /// <remarks>
        /// This method sets the IsParsing flag to true, clears all databases and errors, and then parses various game data files.
        /// It parses expanded factions, cultures, factions, battle models, unit entries, mounts, character types, and projectiles.
        /// It also checks for model usage and removes unused models and mounts from the respective databases.
        /// After all parsing and cleaning operations are done, it sets the IsParsing flag back to false.
        /// </remarks>
        public static void ParseFiles()
        {
            IsParsing = true;
            ClearDatabases();
            ErrorDb.ClearErrors();
            FactionParser.ParseExpanded();
            FactionParser.ParseCultures();
            FactionParser.ParseSmFactions();
            ModData.BattleModelDb.ParseFile();
            MountParser.Parse();
            ModData.Units.ParseFile();
            ModData.Buildings.ParseFiles();
            CharacterTypesParser.parseCharacterTypes();
            ModData.BattleModelDb.CheckModelUsage();
            ProjectileParser.ParseProjectiles();
            IsParsing = false;
            //FileRemover.CheckFiles();
        }
        /// <summary>
        /// Loads the global and mod-specific options from their respective JSON files.
        /// </summary>
        /// <remarks>
        /// This method first checks if the global configuration file exists. If it does not, it creates a new file and writes the current GlobalOptionsInstance into it.
        /// If the file does exist, it reads the file and deserializes the JSON into a GlobalOptions object, which is then assigned to the GlobalOptionsInstance.
        /// The method then checks if the ModName is not null or empty. If it is, the method returns and does not proceed to load the mod-specific options.
        /// If the ModName is not null or empty, it checks if the mod-specific configuration file exists. If it does not, it creates a new file and writes the current ModOptionsInstance into it.
        /// If the file does exist, it reads the file and deserializes the JSON into a ModOptions object, which is then assigned to the ModOptionsInstance.
        /// </remarks>
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
            LoadOptions();
            LoadMod();
        }
    }
}