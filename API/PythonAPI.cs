using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ModdingTool.API;
using Python.Runtime;

public static class PythonAPI
{
    /*
    static PythonAPI()
    {
        var pythonDir = Path.Combine(Environment.CurrentDirectory, @"python");
        Runtime.PythonDLL = Path.Combine(pythonDir, "python312.dll"); 
        PythonEngine.PythonHome = pythonDir;
        PythonEngine.PythonPath = $"{pythonDir}/Lib/site-packages{Path.PathSeparator}" + 
                                  $"{pythonDir}/Lib{Path.PathSeparator}";
        /*
        dynamic mainModule = Py.Import("__main__");
            
        // Expose C# methods as Python functions in the main module
        mainModule.GetModel = new Func<string, BattleModel?>(GetModel);
        mainModule.GetModelByIndex = new Func<int, BattleModel?>(GetModelByIndex);
        mainModule.GetModelCount = new Func<int>(GetModelCount);
        */
    /*
    }

    public static void RunPythonScript(string scriptPath = @"script.py")
    {
        // Check if the file exists to avoid exceptions
        if (!File.Exists(scriptPath))
        {
            Console.WriteLine(@"Python script file does not exist at the specified path.");
            return;
        }

        // Read the contents of the Lua script file
        var pyScript = File.ReadAllText(scriptPath);
        if (!PythonEngine.IsInitialized)// Since using asp.net, we may need to re-initialize
        {
            PythonEngine.Initialize();
        }
        using (Py.GIL())
        {
            PythonEngine.Exec(pyScript);
            PythonEngine.Shutdown();
        }
        /*
        using var scope = Py.CreateScope();
        var scriptCompiled = PythonEngine.Compile(pyScript, scriptPath);
        scope.Execute(scriptCompiled);
        */
    /*
    }

    public static BattleModel? GetModel(string name)
    {
        return Globals.BattleModelDataBase.GetValueOrDefault(name);
    }

    public static BattleModel? GetModelByIndex(int index)
    {
        if (index < 0 || index >= Globals.BattleModelDataBase.Count)
            return null;
        return Globals.BattleModelDataBase.ElementAt(index).Value;
    }

    public static int GetModelCount()
    {
        return Globals.BattleModelDataBase.Count;
    }
*/
}