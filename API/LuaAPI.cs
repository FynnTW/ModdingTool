using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

        // Expose the GetModel function to Lua
        _luaState.RegisterFunction("GetModel", null, typeof(LuaAPI).GetMethod("GetModel"));
        // Expose the GetModel function to Lua
        
        _luaState.RegisterFunction("GetModelByIndex", null, typeof(LuaAPI).GetMethod("GetModelByIndex"));
        
        _luaState.RegisterFunction("GetModelCount", null, typeof(LuaAPI).GetMethod("GetModelCount"));
        
        // You can also expose the BattleModel class to Lua
        // Note: Replace 'YourAssemblyName' and 'YourNamespace' with the actual values.
        _luaState.DoString(@"import ('ModdingTool', 'ModdingTool.BattleModel')");
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
    
    public static void DictionaryToLuaTable<T>(Dictionary<string, T> dictionary, string tableName)
    {
        // Create a new table in Lua with the name 'tableName'
        _luaState.NewTable(tableName);

        foreach (var pair in dictionary)
        {
            // For each key-value pair in the dictionary, insert it into the Lua table
            // NLua can handle basic data type conversions automatically
            _luaState[tableName + "." + pair.Key] = pair.Value;

            // If the value is also a dictionary, you could recursively call this method
            // to transform it into a nested Lua table.
            // You'd need to check if 'pair.Value' is a dictionary and call accordingly.
            // This is an advanced use case and requires careful management of table names.
        }
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