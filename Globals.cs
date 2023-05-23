using System;
using log4net;
using System.Collections.Generic;
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
        public static Dictionary<string, Faction> AllFactions = new();
        public static Dictionary<string, Culture> AllCultures = new();
        public static Dictionary<string, string> ExpandedEntries = new();

        public static void Print(string message)
        {
            Console.WriteLine(message);
        }
        public static void PrintFinal()
        {
            System.IO.File.WriteAllText(@"units.json", JsonConvert.SerializeObject(AllUnits));
            System.IO.File.WriteAllText(@"bmdb.json", JsonConvert.SerializeObject(ModelDb));
            System.IO.File.WriteAllText(@"factions.json", JsonConvert.SerializeObject(AllFactions));
            System.IO.File.WriteAllText(@"cultures.json", JsonConvert.SerializeObject(AllCultures));
            Console.WriteLine("Done");
        }
        public static void PrintInt(int statement)
        {
            Console.WriteLine(statement);
        }

    }
}