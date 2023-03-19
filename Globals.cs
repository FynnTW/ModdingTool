using System;
using log4net;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;

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
            Console.WriteLine("Done");
        }
        public static void PrintInt(int statement)
        {
            Console.WriteLine(statement);
        }

    }
}