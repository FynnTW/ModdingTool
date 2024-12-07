namespace ModdingTool_CLI;

class Program
{
    static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine(@"Please provide a mod and commands. Use -help for more information.");
            return 1;
        }

        var exportJson = false;
        var saveFiles = false;
        var filesToSave = new List<string>();
        var argDict = new Dictionary<string, string>();
        
        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-mod":
                    if (i + 1 < args.Length)
                    {
                        argDict.Add("mod", args[i + 1]);
                    }
                    else
                    {
                        Console.WriteLine(@"Please provide a mod. Use -help for more information.");
                        return 1;
                    }
                    break;
                case "-lua":
                    if (i + 1 < args.Length)
                    {
                        argDict.Add("lua", args[i + 1]);
                    }
                    else
                    {
                        Console.WriteLine(@"Please provide a lua script. Use -help for more information.");
                        return 1;
                    }
                    break;
                case "-help":
                    Console.WriteLine(@"-mod <path to mod> : Required, specify the path to the mod");
                    Console.WriteLine(@"-lua <path to lua script> : Run a lua script");
                    Console.WriteLine(@"-export : Exports all json files");
                    Console.WriteLine(@"-save <type> : Saves the specified file type. Types are: all, edu, bmdb, edb. You can specify multiple types separated by spaces.");
                    Console.WriteLine(@"-import <type> <path to json file> : Imports a json file. Types are: bmdb, edu, edb");
                    Console.WriteLine(@"-help : Shows this help message");
                    return 0;
                case "-export":
                    exportJson = true;
                    break;
                case "-save":
                    saveFiles = true;
                    while (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                    {
                        filesToSave.Add(args[i + 1]);
                        i++;
                    }
                    break;
                case "-import":
                    if (i + 2 < args.Length)
                    {
                        argDict.Add("import", args[i + 1] + ";" + args[i + 2]);
                    }
                    else
                    {
                        Console.WriteLine(@"Error: Please provide a type and a file path to a json file. Types are: bmdb, edu, edb. Use -help for more information.");
                        return 1;
                    }
                    break;
            }
        }
        
        if (!argDict.TryGetValue("mod", out var value))
        {
            Console.WriteLine(@"Please provide a mod. Use -help for more information.");
            return 1;
        }
        
        ModdingTool.Globals.SetModPath(value);
        ModdingTool.Globals.LoadMod();
        if (argDict.TryGetValue("import", out var import))
        {
            var split = import.Split(";");
            switch (split[0])
            {
                case "bmdb":
                    ModdingTool.Globals.ModData.BattleModelDb.ImportJson(split[1]);
                    break;
                case "edu":
                    ModdingTool.Globals.ModData.Units.ImportJson(split[1]);
                    break;
                case "edb":
                    ModdingTool.Globals.ModData.Buildings.ImportJson(split[1]);
                    break;
                default:
                    Console.WriteLine(@"Error: Please provide a valid type. Types are: bmdb, edu, edb.  Use -help for more information.");
                    return 1;
            }
        }
        if (argDict.TryGetValue("lua", out var script))
        {
            ModdingTool.API.LuaAPI.ExecuteLuaScript(script);
        }
        if (exportJson)
        {
            ModdingTool.Globals.ModData.Units.ExportJson();
            ModdingTool.Globals.ModData.BattleModelDb.ExportJson();
            ModdingTool.Globals.ModData.Buildings.ExportJson();
        }
        if (saveFiles)
        {
            foreach (var file in filesToSave)
            {
                switch (file)
                {
                    case "all":
                        ModdingTool.Globals.ModData.Units.WriteFile();
                        ModdingTool.Globals.ModData.BattleModelDb.WriteFile();
                        ModdingTool.Globals.ModData.Buildings.WriteFile();
                        break;
                    case "edu":
                        ModdingTool.Globals.ModData.Units.WriteFile();
                        break;
                    case "bmdb":
                        ModdingTool.Globals.ModData.BattleModelDb.WriteFile();
                        break;
                    case "edb":
                        ModdingTool.Globals.ModData.Buildings.WriteFile();
                        break;
                    default:
                        Console.WriteLine(@"Error: Please provide a valid type to save. Types are: all, edu, bmdb, edb.  Use -help for more information.");
                        break;
                }
            }
        }
        
        
        return 0;
    }
}