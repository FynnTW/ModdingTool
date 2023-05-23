using System;
using log4net;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace ModdingTool
{
    public class Globals
    {
        public static string ModPath = null!;
        public static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static int ProjectileDelayStandard = 0;
        public static int ProjectileDelayFlaming = 0;
        public static int ProjectileDelayGunpowder = 0;
        public static Dictionary<string, Unit> AllUnits = new();
        public static Dictionary<string, string> UnitNames = new();
        public static Dictionary<string, string?> UnitDescr = new();
        public static Dictionary<string, string?> UnitDescrShort = new();
        public static Dictionary<string, BattleModel> ModelDb = new();
        public static Dictionary<string, BattleModel> ModelDb_import = new();
        public static Dictionary<string, Faction> AllFactions = new();
        public static Dictionary<string, Culture> AllCultures = new();
        public static Dictionary<string, string> ExpandedEntries = new();

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
            string newBmdb = "";
            newBmdb += "22 serialization::archive 3 0 0 0 0 ";
            newBmdb += (ModelDb.Count + 1) + " 0 0\n";
            newBmdb += "5 blank 0 0 0 1 0 0 0 0 0 0 1 0 0 0 0 0 0 0 0 0 1 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0\n";
            foreach (KeyValuePair<string, BattleModel> entry in ModelDb)
            {
                newBmdb += entry.Value.WriteEntry(entry.Value);
            }
            System.IO.File.WriteAllText(@"battle_models.modeldb", newBmdb);
        }

        public static void ExportJson()
        {
            System.IO.File.WriteAllText(@"bmdb.json", JsonConvert.SerializeObject(ModelDb));
        }

        public static void PrintInt(int statement)
        {
            Console.WriteLine(statement);
        }

    }
}