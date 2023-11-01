using System;
using System.Linq;

namespace ModdingTool;
using static ModdingTool.Globals;
public class EngineParser
{
    //Parsing line number and file name used for error logging
    private static int _lineNum;
    private static string _fileName = "";

    //public static void ParseEngines()
    //{
    //    _fileName = "descr_engines.txt";
    //    Console.WriteLine($@"start parse {_fileName}");
    //
    //
    //    var lines = FileReader("\\data\\descr_engines.txt", _fileName, Encoding.Default);
    //    if (lines == null) { return; } //something very wrong if you hit this
    //
    //    //Reset line counter
    //    _lineNum = 0;
    //
    //    //Initialize Global Engines Database
    //    EngineDataBase = new Dictionary<string, Engine>();
    //
    //    //Create new Engine for first entry
    //    var projectile = new Projectile();
    //    var first = true;
    //
    //    //Loop through lines
    //    foreach (var line in lines)
    //    {
    //        //Increase line counter
    //        _lineNum++;
    //
    //        //Remove Comments and Faulty lines
    //        var newline = CleanLine(line);
    //        if (string.IsNullOrWhiteSpace(newline))
    //        {
    //            continue;
    //        }
    //
    //        //Split line into parts
    //        var parts = LineSplitterEngines(newline);
    //        if (parts.Length < 1)
    //        {
    //            //Should be something wrong with line if you hit this
    //            ErrorDb.AddError("Unrecognized content", _lineNum.ToString(), _fileName);
    //            continue;
    //        }
    //
    //        //Entry is completed, next entry is starting
    //        if (parts[0].Equals("projectile"))
    //        {
    //            //Add new faction if it is not the first time we hit this
    //            if (!first)
    //            {
    //                AddProjectile(projectile);
    //            }
    //            first = false;
    //            projectile = new Projectile();
    //            stuck = false;
    //        }
    //
    //        //Fill out the faction field the line is about
    //        AssignFields(projectile, parts);
    //    }
    //
    //    //Add last faction
    //    AddProjectile(projectile);
    //
    //    //Reset Line Counter
    //    Console.WriteLine($@"end parse {_fileName}");
    //    _lineNum = 0;
    //
    //}

    private static string[] LineSplitterEngines(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            ErrorDb.AddError("Warning empty line send on", _lineNum.ToString(), _fileName);
            return Array.Empty<string>();
        }

        //split by comma
        char[] delimiters = { ',' };
        var lineParts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        //Make sure there are no empty entries
        if (lineParts.Length == 0)
        {
            return lineParts;
        }

        //split first part by white space (no comma between identifier and value)
        char[] delimitersWhite = { ' ', '\t' };
        var firstParts = lineParts[0].Split(delimitersWhite, 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        //concat first part with rest of line
        lineParts = firstParts.Concat(lineParts[1..]).ToArray();
        return lineParts;
    }
}
