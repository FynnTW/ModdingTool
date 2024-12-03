using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using static ModdingTool.Globals;

namespace ModdingTool;

public partial class FileStream
{
    private int _stringPos = 0;
    private int _nextLength = 0;
    
    public List<string> CommentCache = new();
    public string CommentCacheInLine = "";
    
    private string FileName { get; set; }
    private int Line { get; set; } = 0;
    private string FilePath { get; set; }
    private string[] Lines { get; set; } = Array.Empty<string>();
    private string Text { get; set; } = "";
    
    public string GetFileName => FileName;
    public int GetCurrentLineIndex => Line;
    
    public Encoding Encoding { get; set; } = Encoding.UTF8;
    
    public FileStream(string filePath, string fileName)
    {
        FilePath = filePath;
        FileName = fileName;
    }
    
    public string? GetNextCleanLine()
    {
        var line = GetCleanLine();
        while(line == null && Line < Lines.Length)
            line = GetCleanLine();
        return line;
    }
    
    
    
    public string? GetCleanLine()
    {
        var line = GetLine();
        if (line.StartsWith(';') && !line.Contains("ai_unit_value") && !line.Contains("value_per"))
        {
            CommentCache.Add(line);
            return null;
        }
        var newline = RemoveComment(line);
        return string.IsNullOrWhiteSpace(newline) ? null : newline.Trim();
    }
    
    public string RemoveComment(string line)
    {
        if (!line.Contains(';'))
            return line;
        if(!line.Contains("ai_unit_value") && !line.Contains("value_per"))
            CommentCacheInLine = line[line.IndexOf(';')..];
        return line.StartsWith(';') ? "" : line[..line.IndexOf(';')];
    }
    
    public string[]? LocalLineCleaner(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            return null;
        if (line.StartsWith('¬'))
            return null;
        if (!line.Contains('{') || !line.Contains('}'))
        {
            ErrorDb.AddError("Unrecognized content", Line.ToString(), FileName);
            return null;
        }
        var newLine = line.Trim();
        return CurlySplitter(newLine);
    }
    
    private static string[] CurlySplitter(string line)
    {
        char[] deliminators = { '{', '}' };
        var splitLine = line.Split(deliminators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return splitLine;
    }
    
    public int GetLineNum() => Lines.Length;
    
    public string GetLine()
    {
        var line = Line >= Lines.Length ? "" : Lines[Line];
        Line++;
        return line;
    }

    public bool ReadLines()
    {
        try
        {
            Lines = File.ReadAllLines(ModPath + FilePath + FileName, Encoding);
        }
        catch (Exception e)
        {
            ErrorDb.AddError("Error reading " + FileName);
            ErrorDb.AddError(e.Message);
            return false;
        }
        return true;
    }

    private void AdvancePos()
    {
        if (string.IsNullOrWhiteSpace(Text)) return;
        if (_stringPos >= Text.Length - 1) return;
        _stringPos += GetNextNonWhite();
        var end = _stringPos + GetNextWhite() + 1;
        _nextLength = end - _stringPos;
    }

    private int GetNextNonWhite()
    {
        if (string.IsNullOrWhiteSpace(Text)) return 0;
        if (_stringPos >= Text.Length - 1) return 0;
        var stringLen = Text.Length - 1;
        var len = Math.Min(stringLen - _stringPos, 100);
        var subLine = Text.Substring(_stringPos, len);
        var match = NonWhite().Match(subLine);
        return match.Success ? match.Index : _stringPos;
    }

    private int GetNextWhite()
    {
        if (string.IsNullOrWhiteSpace(Text)) return 0;
        if (_stringPos >= Text.Length - 1) return 0;
        var stringLen = Text.Length - 1;
        var len = Math.Min(stringLen - _stringPos, 100);
        if (len <= 0) return 0;
        var subLine = Text.Substring(_stringPos, len);
        var match = White().Match(subLine);
        return match.Success ? match.Index : _stringPos;
    }

    public string GetString()
    {
        if (string.IsNullOrWhiteSpace(Text)) return "";
        if (_stringPos >= Text.Length - 1) return "";

        AdvancePos();

        var lenght = GetInt();
        _stringPos += GetNextNonWhite();

        if (lenght <= 0) return "";

        var returnString = Text.Substring(_stringPos, lenght).Trim();
        _stringPos += lenght;

        return returnString;
    }

    public int GetInt()
    {
        if (string.IsNullOrWhiteSpace(Text)) return 0;
        if (_stringPos >= Text.Length - 1) return 0;
        AdvancePos();
        var returnValue = int.Parse(Text.Substring(_stringPos, _nextLength).Trim());
        _stringPos += _nextLength - 1;
        return returnValue;
    }

    public float GetFloat()
    {
        if (string.IsNullOrWhiteSpace(Text)) return 0;
        if (_stringPos >= Text.Length - 1) return 0;
        AdvancePos();
        if (_stringPos >= Text.Length - 1) return 0;
        var stringLen = Text.Length - 1;
        var len = Math.Min((stringLen) - _stringPos, _nextLength);
        var substr = Text.Substring(_stringPos, len).Trim();
        var returnValue = float.Parse(substr, CultureInfo.InvariantCulture.NumberFormat);
        _stringPos += _nextLength - 1;
        return returnValue;
    }
    
    public bool Read()
    {
        try
        {
            Text = File.ReadAllText(ModPath + FilePath + FileName, Encoding);
        }
        catch (Exception e)
        {
            ErrorDb.AddError("Error reading " + FileName);
            ErrorDb.AddError(e.Message);
            return false;
        }
        return true;
    }
    
    public void SetStringPos(int pos) => _stringPos = pos;
    
    public void LogEnd()
    {
        Console.WriteLine($@"end parse {FileName}");
    }

    public void LogStart()
    {
        Console.WriteLine($@"start parse {FileName}");
    }
    
    private readonly List<string> _endComments = new();

    [GeneratedRegex("\\S")]
    private static partial Regex NonWhite();
    [GeneratedRegex("\\s")]
    private static partial Regex White();
}