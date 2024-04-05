using System;
using System.IO;

namespace ModdingTool;

public abstract class GameType
{
    private string DataType => GetType().Name;

    protected string _name = "";
    
    public static bool FileExistsData(string path, string name)
    {
        if (string.IsNullOrWhiteSpace(path)) return false;
        if (File.Exists(Globals.ModPath + "/data/" + path) ||
            File.Exists(Globals.GamePath + "/data/" + path)) return true;
        Globals.ErrorDb.AddError($"[{name}] WARNING! File {path} does not exist.");
        return false;
    }
        
    protected void AddChange(string attribute, string oldValue, string newValue)
    {
        if (Globals.IsParsing) return;
        if (oldValue == newValue) return;
        Changes.AddChange(attribute, DataType, _name, oldValue, newValue);
    }
    protected void AddChange(string attribute, int oldValue, int newValue)
    {
        if (Globals.IsParsing) return;
        if (oldValue == newValue) return;
        Changes.AddChange(attribute, DataType, _name, oldValue.ToString(), newValue.ToString());
    }
    protected static string FormatFloat(float input)
    {
        if (Math.Abs(input - 1) < 0.001)
            return "1";
        return input == 0 ? "0" : input.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
    }
    protected void AddChange(string attribute, float oldValue, float newValue)
    {
        if (Globals.IsParsing) return;
        var oldV = FormatFloat(oldValue);
        var newV = FormatFloat(newValue);
        if (oldV == newV) return;
        Changes.AddChange(attribute, DataType, _name, oldV, newV);
    }
}