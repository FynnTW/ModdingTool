using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ModdingTool;

public abstract class GameType : INotifyPropertyChanged 
{

    public event PropertyChangedEventHandler? PropertyChanged;  

    // This method is called by the Set accessor of each property.  
    // The CallerMemberName attribute that is applied to the optional propertyName  
    // parameter causes the property name of the caller to be substituted as an argument.  
    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")  
    {  
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }  
    
    private string DataType => GetType().Name;

    protected string _name = "";

    protected static bool FileExistsData(string path, string name)
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
    protected void AddChangeList(string attribute, string oldValue, string newValue, string listIndex, string listType)
    {
        if (Globals.IsParsing) return;
        if (oldValue == newValue) return;
        Changes.AddChange(attribute, DataType, _name, oldValue, newValue, listIndex, listType);
    }
        
    protected void AddChange(string attribute, bool oldValue, bool newValue)
    {
        if (Globals.IsParsing) return;
        if (oldValue == newValue) return;
        Changes.AddChange(attribute, DataType, _name, oldValue.ToString(), newValue.ToString());
    }
    protected void AddChangeList(string attribute, bool oldValue, bool newValue, string listIndex, string listType)
    {
        if (Globals.IsParsing) return;
        if (oldValue == newValue) return;
        Changes.AddChange(attribute, DataType, _name, oldValue.ToString(), newValue.ToString(), listIndex, listType);
    }
    
    protected void AddChange(string attribute, int oldValue, int newValue)
    {
        if (Globals.IsParsing) return;
        if (oldValue == newValue) return;
        Changes.AddChange(attribute, DataType, _name, oldValue.ToString(), newValue.ToString());
    }
    protected void AddChangeList(string attribute, int oldValue, int newValue, string listIndex, string listType)
    {
        if (Globals.IsParsing) return;
        if (oldValue == newValue) return;
        Changes.AddChange(attribute, DataType, _name, oldValue.ToString(), newValue.ToString(), listIndex, listType);
    }
    protected static string FormatFloat(float input)
    {
        if (Math.Abs(input - Math.Round(input)) < 0.001)
            return Math.Round(input).ToString(CultureInfo.InvariantCulture);

        var formattedInput = input.ToString("0.##", CultureInfo.InvariantCulture);

        return formattedInput;
    }
    protected static string FormatFloat(double input)
    {
        if (Math.Abs(input - Math.Round(input)) < 0.001)
            return Math.Round(input).ToString(CultureInfo.InvariantCulture);

        var formattedInput = input.ToString("0.##", CultureInfo.InvariantCulture);

        return formattedInput;
    }
    protected void AddChange(string attribute, float oldValue, float newValue)
    {
        if (Globals.IsParsing) return;
        var oldV = FormatFloat(oldValue);
        var newV = FormatFloat(newValue);
        if (oldV == newV) return;
        Changes.AddChange(attribute, DataType, _name, oldV, newV);
    }
    protected void AddChangeList(string attribute, float oldValue, float newValue, string listIndex, string listType)
    {
        if (Globals.IsParsing) return;
        var oldV = FormatFloat(oldValue);
        var newV = FormatFloat(newValue);
        if (oldV == newV) return;
        Changes.AddChange(attribute, DataType, _name, oldV, newV, listIndex, listType);
    }
    protected void AddChange(string attribute, double oldValue, double newValue)
    {
        if (Globals.IsParsing) return;
        var oldV = FormatFloat(oldValue);
        var newV = FormatFloat(newValue);
        if (oldV == newV) return;
        Changes.AddChange(attribute, DataType, _name, oldV, newV);
    }
    protected void AddChangeList(string attribute, double oldValue, double newValue, string listIndex, string listType)
    {
        if (Globals.IsParsing) return;
        var oldV = FormatFloat(oldValue);
        var newV = FormatFloat(newValue);
        if (oldV == newV) return;
        Changes.AddChange(attribute, DataType, _name, oldV, newV, listIndex, listType);
    }
    
    public static bool FloatNotEqual(float a, float b) => Math.Abs(a - b) > 0.001;
    protected static bool FloatNotEqual(double a, double b) => Math.Abs(a - b) > 0.001;

    protected Dictionary<string, List<string>> Comments { get; set; } = new();

    protected static string ConditionalString(string? text, string returnText = "") => 
        string.IsNullOrWhiteSpace(text) ? "" : string.IsNullOrWhiteSpace(returnText) ? text : returnText;

    protected virtual string GetTypeTextField(string identifier) => "";


    public void AssignComments(string identifier, ref List<string> commentCache, ref string commentCacheInLine)
    {
        Comments[identifier] = new List<string>();
        Comments[identifier].AddRange(commentCache);
        commentCache.Clear();
        CommentsInLine[identifier] = "";
        CommentsInLine[identifier] = commentCacheInLine;
        commentCacheInLine = "";
    }

    protected string GetTextField(string identifier)
    {
        var text = "";
        text += AddComment(identifier);
        text += GetTypeTextField(identifier);
        if (string.IsNullOrWhiteSpace(text)) return "";
        return string.IsNullOrWhiteSpace(CommentsInLine[identifier]) ? 
            text + "\n" : text + " " + CommentsInLine[identifier] + "\n";
    }

    public static string FormatFloatSingle(double input)
    {
        if (Math.Abs(input - 1) < 0.01)
        {
            return "1";
        }
        return input == 0 ? "0" : input.ToString("0.0", CultureInfo.InvariantCulture);
    }

    protected static string MakeCommaString(List<string> list)
    {
        var commaString = "";
        if (list.Count == 0) return commaString;
        foreach (var element in list)
        {
            commaString += element;
            if (element != list.Last())
            {
                commaString += ", ";
            }
        }
        return commaString;
    }

    protected static string GetTabLength(string text) 
        => text.Length switch
            {
                0 => "",
                < 4 => "\t\t\t\t\t\t",
                < 8 => "\t\t\t\t\t",
                < 12 => "\t\t\t\t",
                < 16 => "\t\t\t",
                < 20 => "\t\t",
                _ => "\t",
            };


    protected Dictionary<string, string> CommentsInLine { get; set; } = new();

    private string AddComment(string identifier)
    {
        if (!Comments.TryGetValue(identifier, out var value)) return "";
        if (value.Count == 0) return "";
        var comment = value.Aggregate("", (current, c) => current + (c + "\n"));
        return comment;
    }
}