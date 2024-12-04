using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using ModdingTool.Databases;
using RGiesecke.DllExport;

namespace ModdingTool.API;
using NLua;

// ReSharper disable once InconsistentNaming
public static class LuaAPI
{
    private static Lua _luaState;

    static LuaAPI()
    {
        _luaState = new Lua();
        InitializeLuaState();
    }

    private static void InitializeLuaState()
    {
        // Make sure you load the CLR package to allow importing .NET namespaces
        _luaState.LoadCLRPackage();
        
        _luaState.RegisterFunction("GetModData", typeof(LuaAPI).GetMethod("GetModData"));
        _luaState.DoString(@"import ('ModdingTool', 'ModdingTool.Globals.Data')");
        
        _luaState.DoString(@"import ('ModdingTool', 'ModdingTool.Databases.BattleModelDb')");
        _luaState.DoString(@"import ('ModdingTool', 'ModdingTool.BattleModel')");
        
        _luaState.DoString(@"import ('ModdingTool', 'ModdingTool.Databases.UnitDb')");
        _luaState.DoString(@"import ('ModdingTool', 'ModdingTool.Unit')");
        
        _luaState.DoString(@"import ('ModdingTool', 'ModdingTool.Databases.BuildingDb')");
        _luaState.DoString(@"import ('ModdingTool', 'ModdingTool.Building')");
    }
    
    public static Globals.Data GetModData()
    {
        return Globals.ModData;
    }
    
    public static void ExecuteLuaScript(string luaScriptPath = "script.lua")
    {
        // Check if the file exists to avoid exceptions
        if (!File.Exists(luaScriptPath))
        {
            Console.WriteLine(@"Lua script file does not exist at the specified path.");
            return;
        }

        // Read the contents of the Lua script file
        var luaScript = File.ReadAllText(luaScriptPath);
        try
        {
            _luaState.DoString(luaScript);
        }
        catch (Exception e)
        {
            Console.WriteLine(@"An error occurred while executing the Lua script:");
            Console.WriteLine(e.Message);
        }
    }
    
}