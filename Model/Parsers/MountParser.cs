using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModdingTool;
using static ModdingTool.Globals;
using static ModdingTool.ParseHelpers;

public class MountParser : FileParser
{

    //Parsing line number and file name used for error logging
    private static int rider = 0;
    
    public static void Parse()
    {
        s_fileName = "descr_mount.txt";
        Console.WriteLine($@"start parse {s_fileName}");
        
        var lines = FileReader("\\data\\descr_mount.txt", s_fileName, Encoding.Default);
        if (lines == null) { return; } //something very wrong if you hit this

        //Reset line counter
        s_lineNum = 0;

        //Initialize Global Mounts Database
        MountDataBase = new Dictionary<string, Mount>();

        //Create new Mount for first entry
        var mount = new Mount();
        var first = true;

        //Loop through lines
        foreach (var line in lines)
        {
            //Increase line counter
            s_lineNum++;

            //Remove Comments and Faulty lines
            var newline = CleanLine(line);
            if (string.IsNullOrWhiteSpace(newline))
            {
                continue;
            }

            //Split line into parts
            var parts = LineSplitterMounts(newline);
            if (parts.Length < 1)
            {
                //Should be something wrong with line if you hit this
                ErrorDb.AddError("Unrecognized content", s_lineNum.ToString(), s_fileName);
                continue;
            }

            //Entry is completed, next entry is starting
            if (parts[0].Equals("type"))
            {
                //Add new faction if it is not the first time we hit this
                if (!first)
                {
                    AddMount(mount);
                }
                first = false;
                mount = new Mount();
                rider = 0;
            }

            //Fill out the faction field the line is about
            AssignFields(mount, parts);
        }

        //Add last faction
        AddMount(mount);
        CommentCache.Clear();

        //Reset Line Counter
        Console.WriteLine($@"end parse {s_fileName}");
        s_lineNum = 0;

    }

    private static void AddMount(Mount mount)
    {
        if (MountDataBase.ContainsKey(mount.type))
        {
            ErrorDb.AddError("Mount name already exists", s_lineNum.ToString(), s_fileName);
            return;
        }
        MountDataBase.Add(mount.type, mount);
        Console.WriteLine(mount.type);
    }
    
    private static string[] LineSplitterMounts(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            ErrorDb.AddError("Warning empty line send on", s_lineNum.ToString(), s_fileName);
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

    private static void AssignFields(Mount mount, string[] parts)
    {
        //name of the field
        var identifier = parts[0];

        var value = "";
        //first value, usually all needed
        if (parts.Length > 1)
        {
            value = parts[1];
        }

        char[] delimitersWhite = { ' ', '\t' };

        try
        {
            switch (identifier)
            {
                case "type":
                    mount.type = value.Trim();
                    break;
                case "class":
                    mount.mount_class = value;
                    break;
                case "model":
                    mount.model = value.Trim().ToLower();
                    break;
                case "radius":
                    mount.radius = ParseDouble(value);
                    break;
                case "x_radius":
                    mount.x_radius = ParseDouble(value);
                    break;
                case "y_offset":
                    mount.y_offset = ParseDouble(value);
                    break;
                case "height":
                    mount.height = ParseDouble(value);
                    break;
                case "mass":
                    mount.mass = ParseDouble(value);
                    break;
                case "banner_height":
                    mount.banner_height = ParseDouble(value);
                    break;
                case "bouyancy_offset":
                    mount.bouyancy_offset = ParseDouble(value);
                    break;
                case "water_trail_effect":
                    mount.water_trail_effect = value;
                    break;
                case "root_node_height":
                    mount.root_node_height = ParseDouble(value);
                    break;
                case "rider_offset":
                    mount.rider_offsets.Add(new RiderOffset
                    {
                        id = rider,
                        x = ParseDouble(parts[1]),
                        y = ParseDouble(parts[2]),
                        z = ParseDouble(parts[3])
                    });
                    rider++;
                    break;
                case "attack_delay":
                    mount.attack_delay = ParseDouble(value);
                    break;
                case "dead_radius":
                    mount.dead_radius = ParseDouble(value);
                    break;
                case "tusk_z":
                    mount.tusk_z = ParseDouble(value);
                    break;
                case "tusk_radius":
                    mount.tusk_radius = ParseDouble(value);
                    break;
                case "riders":
                    mount.riders = int.Parse(value);
                    break;
            };

        }
        catch (Exception e)
        {
            ErrorDb.AddError(e.Message, s_lineNum.ToString(), s_fileName);
            Console.WriteLine(e);
            Console.WriteLine(identifier);
            Console.WriteLine(@"on line: " + s_lineNum);
            Console.WriteLine(@"====================================================================================");
        }
    }

}