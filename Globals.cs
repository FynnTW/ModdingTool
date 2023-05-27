using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;

namespace ModdingTool
{
    public class Globals
    {
        public static string ModPath = null!;
        public static bool ModLoaded = false;
        public static int ProjectileDelayStandard = 0;
        public static int ProjectileDelayFlaming = 0;
        public static int ProjectileDelayGunpowder = 0;
        public static Dictionary<string, Unit> UnitDataBase = new();
        public static Dictionary<string, string> UnitNames = new();
        public static Dictionary<string, string?> UnitDescr = new();
        public static Dictionary<string, string?> UnitDescrShort = new();
        public static Dictionary<string, BattleModel> BattleModelDataBase = new();
        public static Dictionary<string, Faction> FactionDataBase = new();
        public static Dictionary<string, Culture> CultureDataBase = new();
        public static Dictionary<string, string> ExpandedEntries = new();
        public static readonly Errors ErrorDb = new();

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
            newBmdb = BattleModelDataBase.Aggregate(newBmdb, (current, entry) => current + entry.Value.WriteEntry(entry.Value));
            File.WriteAllText(@"battle_models.modeldb", newBmdb);
        }

        public static void ExportJson()
        {
            File.WriteAllText(@"bmdb.json", JsonConvert.SerializeObject(BattleModelDataBase));
        }

        public static void ParseFiles()
        {
            ErrorDb.ClearErrors();
            FactionParser.ParseExpanded();
            FactionParser.ParseCultures();
            FactionParser.ParseSmFactions();
            BmdbParser.ParseBmdb();
            EduParser.ParseEu();
            EduParser.ParseEdu();
        }

        public static void AutoStart()
        {
            if (Application.Current.MainWindow == null)
            {
                return;
            }
            var window = (MainWindow)Application.Current.MainWindow;
            var menubar = window.FindName("MenuBarCustom") as View.UserControls.Menubar;
            ModPath = "E:\\SteamLibrary\\steamapps\\common\\Medieval II Total War\\mods\\Divide_and_Conquer";
            menubar?.LoadMod();
        }

    }
}