using System;
using log4net;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using Newtonsoft.Json;
using ModdingTool.View.UserControls;

namespace ModdingTool
{
    public class Globals
    {
        public static string ModPath = null!;
        public static bool ModLoaded = false;
        public static int ProjectileDelayStandard = 0;
        public static int ProjectileDelayFlaming = 0;
        public static int ProjectileDelayGunpowder = 0;
        public static Dictionary<string, Unit> AllUnits = new();
        public static Dictionary<string, string> UnitNames = new();
        public static Dictionary<string, string?> UnitDescr = new();
        public static Dictionary<string, string?> UnitDescrShort = new();
        public static Dictionary<string, BattleModel> ModelDb = new();
        public static Dictionary<string, Faction> FactionDataBase = new();
        public static Dictionary<string, Culture> CultureDataBase = new();
        public static Dictionary<string, string> ExpandedEntries = new();
        // ReSharper disable once InconsistentNaming
        public static Errors ErrorDB = new();

        public static void Print(string message)
        {
            Console.WriteLine(message);
        }
        public static void PrintFinal()
        {
            Console.WriteLine("Done");
        }

        public static void ImportJson(string fileName)
        {
            string jsonString = File.ReadAllText(fileName);
            ModelDb = JsonConvert.DeserializeObject<Dictionary<string, BattleModel>>(jsonString);
        }

        public static void WriteBMDB()
        {
            var newBmdb = "";
            newBmdb += "22 serialization::archive 3 0 0 0 0 ";
            newBmdb += (ModelDb.Count + 1) + " 0 0\n";
            newBmdb += "5 blank 0 0 0 1 0 0 0 0 0 0 1 0 0 0 0 0 0 0 0 0 1 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0\n";
            newBmdb = ModelDb.Aggregate(newBmdb, (current, entry) => current + entry.Value.WriteEntry(entry.Value));
            System.IO.File.WriteAllText(@"battle_models.modeldb", newBmdb);
        }

        public static void ExportJson()
        {
            System.IO.File.WriteAllText(@"bmdb.json", JsonConvert.SerializeObject(ModelDb));
        }

        public static void ParseFiles()
        {
            FactionParser.ParseExpanded();
            FactionParser.ParseCultures();
            FactionParser.ParseSmFactions();
            BmdbParser.ParseBmdb();
            EduParser.ParseEdu();
        }

        public static void Startauto()
        {
            MainWindow window = (ModdingTool.MainWindow)App.Current.MainWindow;
            var menubar = window.FindName("MenuBarCustom") as ModdingTool.View.UserControls.Menubar;
            ModPath = "E:\\SteamLibrary\\steamapps\\common\\Medieval II Total War\\mods\\Divide_and_Conquer";
            menubar.LoadMod();
        }

        public static void PrintInt(int statement)
        {
            Console.WriteLine(statement);
        }

    }
}