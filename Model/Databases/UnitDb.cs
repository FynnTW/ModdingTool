using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using ModdingTool.View.InterfaceData;
using ModdingTool.View.UserControls;
using Newtonsoft.Json;
using static ModdingTool.Globals;

namespace ModdingTool.Databases;
/// <summary>
/// The UnitDb class represents a database of units in a game.
/// It contains methods for adding units, checking if a unit exists, retrieving units, and importing/exporting units from/to JSON.
/// It also contains methods for writing the database and reading the database from game files.
/// </summary>
public class UnitDb
{
    /// <summary>
    /// A dictionary that holds all the units in the game, with the unit type as the key and the unit object as the value.
    /// </summary>
    private Dictionary<string, Unit> Units { get; set; } = new();

    /// <summary>
    /// A dictionary that holds the names of the units, with the unit type as the key and the unit name as the value.
    /// </summary>
    private Dictionary<string, string> _unitNames = new();

    /// <summary>
    /// A dictionary that holds the descriptions of the units, with the unit type as the key and the unit description as the value.
    /// </summary>
    private Dictionary<string, string?> _unitDescr = new();

    /// <summary>
    /// A dictionary that holds the short descriptions of the units, with the unit type as the key and the short unit description as the value.
    /// </summary>
    private Dictionary<string, string?> _unitDescrShort = new();
    
    /// <summary>
    /// A list that holds the comments at the end of the EDU file.
    /// </summary>
    /// <remarks>
    /// These comments are typically used for providing additional information about the EDU file.
    /// They are not used by the game engine and are only for human readers.
    /// </remarks>
    private readonly List<string> _eduEndComments = new();
    
    /// <summary>
    /// Adds a new unit to the Units DB.
    /// </summary>
    /// <param name="unit">The unit to be added.</param>
    /// <remarks>
    /// Sets EDU Index, calculates Ai Unit Value and
    /// adds to Units DB with EDU type as key.
    /// </remarks>
    public void Add(Unit unit)
    {
        unit.EduIndex = Units.Count;
        unit.AiUnitValue = Math.Round(unit.CalculateUnitValue());
        Units.Add(unit.Type, unit);
    }
    /// <summary>
    /// Retrieves the dictionary of units in the game.
    /// </summary>
    /// <returns>A dictionary where the key is the unit type and the value is the unit object.</returns>
    public Dictionary<string, Unit> GetUnits() => Units;
    
    /// <summary>
    /// Checks if a unit with the given name exists in the game.
    /// </summary>
    /// <param name="name">The name of the unit to check.</param>
    /// <returns>True if a unit with the given name exists, false otherwise.</returns>
    public bool Contains(string name) => Units.ContainsKey(name);

    /// <summary>
    /// Retrieves a unit from the Units dictionary based on the unit type.
    /// </summary>
    /// <param name="type">The EDU type of the unit to retrieve.</param>
    /// <returns>The unit object if found, null otherwise.</returns>
    public Unit? Get(string type)
        => Units.GetValueOrDefault(type);
    
    /// <summary>
    /// Retrieves the total count of units in the Units dictionary.
    /// </summary>
    /// <returns>The count of units in the Units dictionary.</returns>
    public int GetUnitCount() => Units.Count;
    
    /// <summary>
    /// Retrieves a unit from the Units dictionary based on its index.
    /// </summary>
    /// <param name="index">The index of the unit in the Units dictionary.</param>
    /// <returns>The unit object if the index is valid, null otherwise.</returns>
    public Unit? GetByIndex(int index)
    {
        if (index < 0 || index >= Units.Count)
            return null;
        return Units.ElementAt(index).Value;
    }
    
    /// <summary>
    /// Imports a JSON file and deserializes it into a dictionary of units.
    /// </summary>
    /// <param name="fileName">The name of the JSON file to import.</param>
    /// <remarks>
    /// This method reads the JSON file, deserializes it into a dictionary where the key is the unit type and the value is the unit object.
    /// If the deserialized object is null, an error is added to the ErrorDb.
    /// If the deserialized object is not null, the Units dictionary is cleared and the units from the deserialized object are added to it.
    /// </remarks>
    public void ImportJson(string fileName)
    {
        var jsonString = File.ReadAllText(fileName);
        var imported = JsonConvert.DeserializeObject<Dictionary<string, Unit>>(jsonString);
        if (imported == null)
        {
            ErrorDb.AddError("Imported JSON is null");
            return;
        }
        Units = new Dictionary<string, Unit>();
        foreach (var entry in imported)
            Add(entry.Value);
    }
    
    /// <summary>
    /// Exports the Units dictionary to a JSON file.
    /// </summary>
    /// <remarks>
    /// This method serializes the Units dictionary into a JSON string and writes it to a file named "edu.json".
    /// </remarks>
    public void ExportJson() =>
        File.WriteAllText(@"edu.json", JsonConvert.SerializeObject(Units, Formatting.Indented));
    
    /// <summary>
    /// Retrieves a list of the names of all units in the Units dictionary.
    /// </summary>
    /// <returns>A list of unit names.</returns>
    public List<string> GetNames() => new(Units.Keys);
    
    /// <summary>
    /// Removes a unit from the Units dictionary.
    /// </summary>
    public void Remove(string name) => Units.Remove(name);
    
    /// <summary>
    /// Writes the current state of the Units database to files.
    /// </summary>
    /// <remarks>
    /// This method performs several operations:<br></br>
    /// 1. Backs up the existing files.<br></br>
    /// 2. Checks if the number of non-EOP units exceeds the limit of 500.<br></br>
    /// 3. Writes the non-EOP units to the "export_descr_unit.txt" file.<br></br>
    /// 4. Writes the unit localization to the "export_units.txt" file.<br></br>
    /// 5. Writes each EOP unit to its own file.<br></br>
    /// </remarks>
    public void WriteFile()
    {
        BackupFile("\\data\\", "export_descr_unit.txt");
        BackupFile("\\data\\text\\", "export_units.txt");
        if (Units.Values.Count(x => x.IsEopUnit == false) > 500)
        {
            ErrorDb.AddError("Exceeded unit limit!");
            var window = Application.Current.MainWindow;
            if (window != null)
            {
                var statusBar = window.FindName("StatusBarLive") as StatusBarCustom;
                statusBar?.SetStatusModPath("Exceeded unit limit!");
            }
        }
        var newEdu = Units.Values.Where(x => x.IsEopUnit == false).Aggregate("", (current, unit) => current + unit.WriteEntry());
        newEdu += _eduEndComments.Aggregate("", (current, comment) => current + (comment + "\n"));
        var contentWithLfFixed = newEdu.Replace("\n", "\r\n").Replace("\r\r\n", "\r\n");
        File.WriteAllText(ModPath + @"/data/export_descr_unit.txt", contentWithLfFixed);
        var dicEntries = new List<string>();
        var newEu = "";
        foreach (var unit in Units.Values.Where(unit => !string.IsNullOrWhiteSpace(unit.Dictionary)))
        {
            if (unit.Dictionary != null && dicEntries.Contains(unit.Dictionary))
            {
                ErrorDb.AddError("Same Dictionary names for unit " + unit.Type + " are you sure these units should use the same localisation?");
                continue;
            }
            if (unit.Dictionary != null) dicEntries.Add(unit.Dictionary);
            newEu += Unit.WriteEuEntry(unit);
        }

        newEu += "\u00ac-----\n";
        File.WriteAllText(ModPath + @"/data/text/export_units.txt", newEu.Replace("\n", "\r\n").Replace("\r\r\n", "\r\n"), Encoding.Unicode);
        foreach (var unit in Units.Values.Where(x => x.IsEopUnit))
        {
            var path = unit.FilePath;
            if (string.IsNullOrWhiteSpace(path))
            {
                path = ModOptionsInstance.EopDirectories.First();
                if (string.IsNullOrWhiteSpace(path))
                {
                    ErrorDb.AddError("No EOP directory found to export unit");
                    continue;
                }
            }
            var newEduEop = unit.WriteEntry();
            File.WriteAllText(path, newEduEop.Replace("\n", "\r\n").Replace("\r\r\n", "\r\n"));
        }
    }

    /// <summary>
    /// Parses an EDU entry from a file stream and adds it to the Units dictionary.
    /// </summary>
    /// <param name="stream">The file stream to parse the EDU entry from.</param>
    /// <param name="isEop">A boolean indicating whether the unit is an EOP unit.</param>
    /// <param name="filePath">The file path of the EOP unit, if applicable.</param>
    /// <remarks>
    /// This method reads lines from the file stream and splits them into parts.
    /// It then checks if the first part of the line is "type", which indicates the start of a new unit entry.
    /// If it is, it adds the current unit to the Units dictionary and starts a new unit.
    /// It then assigns the fields of the unit based on the parts of the line.
    /// After all lines have been read, it adds the last unit to the Units dictionary.
    /// It also adds any comments from the file stream to the _eduEndComments list.
    /// </remarks>
    private void ParseEduEntry(FileStream stream, bool isEop = false, string filePath = "")
    {
        //Make first entry
        var newUnit = new Unit
        {
            IsEopUnit = isEop
        };
        if (isEop)
            newUnit.FilePath = filePath;
        var first = true;

        for (var i = 0; i < stream.GetLineNum() - 1; i++)
        {
            var line = stream.GetCleanLine();
            if (string.IsNullOrWhiteSpace(line))
                continue;
            var lineParts = SplitEduLine(line);
            if (lineParts is { Length: < 2 })
            {
                //Should be something wrong with line if you hit this
                ErrorDb.AddError("Unrecognized content", stream.GetCurrentLineIndex.ToString(), stream.GetFileName);
                continue;
            }
            //Entry is completed, next entry is starting
            if (lineParts != null && (bool)lineParts[0]?.Equals("type"))
            {
                if (!first)
                {
                    newUnit.EduIndex = Units.Count;
                    var cards = newUnit.GetCards();
                    newUnit.Card = cards[0];
                    newUnit.CardInfo = cards[1];
                    newUnit.FactionSymbol = cards[2];
                    Add(newUnit);
                }
                first = false;
                newUnit = new Unit
                {
                    IsEopUnit = isEop
                };
                if (isEop)
                    newUnit.FilePath = filePath;
            }

            //assign field line is about
            AssignFields(newUnit, lineParts, stream);
        }

        //Add last unit
        Add(newUnit);
        
        if (!isEop)
            _eduEndComments.AddRange(stream.CommentCache);
    }
    
    /// <summary>
    /// Splits an EDU line into its constituent parts.
    /// </summary>
    /// <param name="line">The EDU line to split.</param>
    /// <returns>An array of strings where each string is a part of the EDU line, or null if the line is empty.</returns>
    /// <remarks>
    /// This method splits the line based on commas and whitespace.
    /// It also handles special cases where the first part of the line contains multiple words, such as "banner" and "era".
    /// After splitting the line, it trims each part to remove leading and trailing whitespace.
    /// </remarks>
    private static string?[]? SplitEduLine(string line)
    {
        if (line.Equals(""))
        {
            return null;
        }
        char[] delimiters = { ',' };
        char[] delimitersWhite = { ' ', '\t' };
        var lineParts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        var firstParts = lineParts[0].Split(delimitersWhite, 2, StringSplitOptions.RemoveEmptyEntries);

        //some first edu lines have multiple words in the first part
        if (firstParts[0].Equals("banner"))
        {
            var bannerSplit = firstParts[1].Split(delimitersWhite, 2, StringSplitOptions.RemoveEmptyEntries);
            firstParts[0] = "banner " + bannerSplit[0];
            firstParts[1] = bannerSplit[1];
        }
        if (firstParts[0].Equals("era"))
        {
            var eraSplit = firstParts[1].Split(delimitersWhite, 2, StringSplitOptions.RemoveEmptyEntries);
            firstParts[0] = "era " + eraSplit[0];
            firstParts[1] = eraSplit[1];
        }

        lineParts = firstParts.Concat(lineParts[1..]).ToArray();
        for (var i = 0; i < lineParts.Length; i++)
        {
            lineParts[i] = lineParts[i].Trim();
        }
        return lineParts;
    }
    
    /// <summary>
    /// Parses the game files to populate the Units dictionary.
    /// </summary>
    /// <remarks>
    /// This method performs several operations:<br></br>
    /// 1. Calls the ParseEu method to parse the "export_units.txt" file.<br></br>
    /// 2. Creates a FileStream for the "export_descr_unit.txt" file and reads its lines.<br></br>
    /// 3. If the lines are read successfully, initializes the Units dictionary.<br></br>
    /// 4. Calls the ParseEduEntry method to parse the EDU entries from the file stream and add them to the Units dictionary.<br></br>
    /// 5. For each file in the EOP directories, it creates a FileStream for the file and reads its lines.<br></br>
    /// 6. If the lines are read successfully, calls the ParseEduEntry method to parse the EDU entries from the file stream and add them to the Units dictionary.<br></br>
    /// </remarks>
    public void ParseFile()
    {
        ParseEu();
        var fileStream = new FileStream("\\data\\", "export_descr_unit.txt");
        var valid = fileStream.ReadLines();
        if (!valid)
            return;
        fileStream.LogStart();

        //Initialize Global Unit Database
        Units = new Dictionary<string, Unit>();

        ParseEduEntry(fileStream);
            
        fileStream.LogEnd();

        foreach (var file in ModOptionsInstance.EopDirectories.Where(Directory.Exists).SelectMany(path => 
                     Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories)))
        {
            var fileName = file.Split('/', '\\').Last();
            var path = file.Split('/', '\\').Reverse().Skip(1).Reverse().Aggregate("", (current, s) => current + s + "\\");
            fileStream = new FileStream(path, fileName);
            valid = fileStream.ReadLines();
            if (!valid)
                continue;
            fileStream.LogStart();
            ParseEduEntry(fileStream, true, path);
            fileStream.LogEnd();
        }

    }
    
    /// <summary>
    /// Parses the "export_units.txt" file to populate the unit names and descriptions dictionaries.
    /// </summary>
    /// <remarks>
    /// This method performs several operations:
    /// 1. Creates a FileStream for the "export_units.txt" file with Unicode encoding.
    /// 2. Reads the lines from the file stream. If the lines are not read successfully, it returns without doing anything.
    /// 3. Logs the start of the operation.
    /// 4. Initializes the _unitNames, _unitDescr, and _unitDescrShort dictionaries.
    /// 5. For each line in the file stream, it cleans the line and splits it into parts.
    /// 6. If the parts are not null, it calls the AddUnitStringEntry method to add the unit string entry to the appropriate dictionary.
    /// 7. Logs the end of the operation.
    /// </remarks>
    private void ParseEu()
    {
        var fileStream = new FileStream("\\data\\text\\" , "export_units.txt")
        {
            Encoding = Encoding.Unicode
        };
        var success = fileStream.ReadLines();
        if (!success) return;
        fileStream.LogStart();
        //Initialize Global names and descriptions database
        _unitNames = new Dictionary<string, string>();
        _unitDescr = new Dictionary<string, string?>();
        _unitDescrShort = new Dictionary<string, string?>();

        for (var i = 0; i < fileStream.GetLineNum() - 1; i++)
        {
            var line = fileStream.GetLine();
            var parts = fileStream.LocalLineCleaner(line);
            if (parts != null)
                AddUnitLocalizationEntry(parts, fileStream);
        }
            
        fileStream.LogEnd();
    }
    
    
    /// <summary>
    /// A list of strings representing the different attributes a unit can have in the game.
    /// These attributes can define various behaviors and characteristics of a unit.
    /// </summary>
    public List<string> AttributeTypes { get; set; } = new()
    {
        "can_withdraw",
        "can_sap",
        "hide_long_grass",
        "hide_anywhere",
        "sea_faring",
        "gunpowder_unit",
        "screeching_women",
        "druid",
        "cantabrian_circle",
        "is_peasant",
        "no_custom",
        "start_not_skirmishing",
        "fire_by_rank",
        "gunpowder_artillery_unit",
        "command",
        "free_upkeep_unit",
        "heavy",
        "hardy",
        "mercenary_unit",
        "frighten_foot",
        "frighten_mounted",
        "very_hardy",
        "slave",
        "power_charge",
        "hide_forest",
        "can_horde",
        "can_swim",
        "can_formed_charge",
        "can_feign_rout",
        "can_run_amok",
        "warcry",
        "stakes",
        "general_unit",
        "general_unit_upgrade",
        "legionary_name",
        "wagon_fort",
        "cannot_skirmish",
        "hide_improved_forest"
    };

    /// <summary>
    /// Adds a unit localization entry to the appropriate dictionary.
    /// </summary>
    /// <param name="parts">A list of strings where each string is a part of the unit localization entry.</param>
    /// <param name="stream">The file stream from which the unit localization entry was read.</param>
    /// <remarks>
    /// This method performs several operations:
    /// 1. Retrieves the identifier and text from the parts list.
    /// 2. If the text is null or empty, it adds an error to the ErrorDb.
    /// 3. If the identifier contains "_descr_short", it splits the identifier and adds the entry to the _unitDescrShort dictionary.
    /// 4. If the identifier contains "_descr", it splits the identifier and adds the entry to the _unitDescr dictionary.
    /// 5. If the identifier does not contain "_descr_short" or "_descr", it adds the entry to the _unitNames dictionary.
    /// </remarks>
    private void AddUnitLocalizationEntry(IReadOnlyList<string?> parts, FileStream stream)
    {
        var identifier = parts[0];

        Console.WriteLine(identifier);

        var text = "";

        if (parts.Count > 1)
        {
            text = parts[1];
        }
        else
        {
            ErrorDb.AddError($@"Unit localization is empty {identifier}", stream.GetCurrentLineIndex.ToString(), stream.GetFileName);
        }

        if (identifier != null && identifier.Contains("_descr_short"))
        {
            var split = identifier.Split("_descr_short", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            _unitDescrShort.TryAdd(split[0], text);
        }
        else if (identifier != null && identifier.Contains("_descr"))
        {
            var split = identifier.Split("_descr", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            _unitDescr.TryAdd(split[0], text);
        }
        else
        {
            if (text == null || identifier == null) return;
            _unitNames.TryAdd(identifier, text);
        }
    }
    /// <summary>
    /// Assigns the fields of a unit based on the parts of a line from a file stream.
    /// </summary>
    /// <param name="unit">The unit to assign the fields to.</param>
    /// <param name="parts">An array of strings where each string is a part of a line from the file stream.</param>
    /// <param name="stream">The file stream to assign the fields from.</param>
    /// <remarks>
    /// This method performs several operations:
    /// 1. Retrieves the identifier from the parts array.
    /// 2. Depending on the identifier, assigns the corresponding field of the unit.
    /// 3. If an exception occurs during the assignment of the fields, it adds an error to the ErrorDb and logs the exception, identifier, and current line index.
    /// </remarks>
    private void AssignFields(Unit unit, string?[]? parts, FileStream stream)
    {
        var identifier = parts?[0];
        try
        {
            switch (identifier)
            {
                case "type":
                    unit.Type = parts?[1]?.Trim() ?? string.Empty;
                    break;

                case "dictionary":
                    var dict = parts?[1];
                    if (dict != null)
                    {
                        unit.Dictionary = dict;
                        unit.LocalizedName = _unitNames[dict];
                        unit.Descr = _unitDescr[dict];
                        unit.DescrShort = _unitDescrShort[dict];
                    }
                    break;

                case "category":
                    unit.Category = parts?[1]?.Trim();
                    break;

                case "class":
                    unit.ClassType = parts?[1]?.Trim();
                    break;

                case "voice_type":
                    unit.VoiceType = parts?[1]?.Trim();
                    break;

                case "accent":
                    unit.Accent = parts?[1]?.Trim();
                    break;

                case "banner faction":
                    unit.BannerFaction = parts?[1]?.Trim();
                    break;

                case "banner holy":
                    unit.BannerHoly = parts?[1]?.Trim();
                    break;

                case "soldier":
                    unit.Soldier = parts?[1]?.Trim().ToLower();
                    if (unit.Soldier != null) ModData.BattleModelDb.UsedModels.Add(unit.Soldier);
                    unit.SoldierCount = int.Parse(parts?[2] ?? string.Empty);
                    unit.ExtrasCount = int.Parse(parts?[3] ?? string.Empty);
                    unit.Mass = double.Parse(parts?[4] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                    if (parts is { Length: > 5 })
                    {
                        unit.Radius = double.Parse(parts[5] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                        if (parts.Length > 6)
                        {
                            unit.Height = double.Parse(parts[6] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                        }
                    }
                    break;

                case "officer":
                    if (unit.Officer1 is "")
                    {
                        unit.Officer1 = parts?[1]?.Trim().ToLower();
                        if (unit.Officer1 != null) ModData.BattleModelDb.UsedModels.Add(unit.Officer1);
                        break;
                    }
                    if (unit.Officer2 is "")
                    {
                        unit.Officer2 = parts?[1]?.Trim().ToLower();
                        if (unit.Officer2 != null) ModData.BattleModelDb.UsedModels.Add(unit.Officer2);
                        break;
                    }
                    if (unit.Officer3 is "")
                    {
                        unit.Officer3 = parts?[1]?.Trim().ToLower();
                        if (unit.Officer3 != null) ModData.BattleModelDb.UsedModels.Add(unit.Officer3);
                    }
                    break;

                case "ship":
                    unit.Ship = parts?[1]?.Trim().ToLower();
                    break;

                case "mounted_engine":
                    unit.MountedEngine = parts?[1]?.Trim().ToLower();
                    break;

                case "engine":
                    unit.Engine = parts?[1]?.Trim().ToLower();
                    break;

                case "animal":
                    unit.Animal = parts?[1]?.Trim().ToLower();
                    break;

                case "mount":
                    unit.Mount = parts?[1]?.Trim();
                    if (unit.Mount != null)
                    {
                        ModData.BattleModelDb.UsedModels.Add(MountDataBase[unit.Mount].model.Trim());
                        UsedMounts.Add(unit.Mount);
                    }

                    break;

                case "mount_effect":
                    if (parts == null || parts.Length < 2)
                    {
                        break;
                    }
                    foreach (var attr in parts[1..])
                    {
                        if (attr == null) break;
                        unit.MountEffect.Add(attr);
                    }
                    break;

                case "attributes":
                    if (parts == null || parts.Length < 2)
                        break;
                    foreach (var attr in parts[1..])
                    {
                        if (attr == null) break;
                        if (attr.Contains(','))
                        {
                            var split = attr.Split(',');
                            foreach (var s in split)
                            {
                                unit.AddAttribute(s);
                                switch (attr)
                                {
                                    case "mercenary_unit":
                                        unit.MercenaryUnit = true;
                                        continue;
                                    case "general_unit":
                                        continue;
                                }
                            }
                        }
                        else
                        {
                            unit.AddAttribute(attr);
                        }
                        switch (attr)
                        {
                            case "mercenary_unit":
                                unit.MercenaryUnit = true;
                                continue;
                            case "general_unit":
                                continue;
                        }
                    }

                    break;

                case "move_speed_mod":
                    unit.MoveSpeed = double.Parse(parts?[1] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                    break;

                case "formation":
                    unit.SpacingWidth = double.Parse(parts?[1] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                    unit.SpacingDepth = double.Parse(parts?[2] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                    unit.SpacingWidthLoose = double.Parse(parts?[3] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                    unit.SpacingDepthLoose = double.Parse(parts?[4] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                    unit.SpacingRanks = int.Parse(parts?[5] ?? string.Empty);
                    if (parts is { Length: > 6 })
                    {
                        unit.FormationStyle = parts[6];
                        if (parts.Length > 7)
                        {
                            unit.SpecialFormation = parts[7];
                        }
                    }
                    break;

                case "stat_health":
                    unit.HitPoints = int.Parse(parts?[1] ?? string.Empty);
                    unit.MountHitPoints = int.Parse(parts?[2] ?? string.Empty);
                    break;

                case "stat_pri":
                    unit.PriAttack = int.Parse(parts?[1] ?? string.Empty);
                    unit.PriCharge = int.Parse(parts?[2] ?? string.Empty);
                    unit.PriProjectile = parts?[3];
                    unit.PriRange = int.Parse(parts?[4] ?? string.Empty);
                    unit.PriAmmunition = int.Parse(parts?[5] ?? string.Empty);
                    unit.PriWeaponType = parts?[6];
                    unit.PriTechType = parts?[7];
                    unit.PriDamageType = parts?[8];
                    unit.PriSoundType = parts?[9];
                    if (parts is { Length: > 12 })
                    {
                        unit.PriFireType = parts[10];
                        unit.PriAttDelay = int.Parse(parts[11] ?? string.Empty);
                        unit.PriSkelFactor = double.Parse(parts[12] ?? string.Empty);
                    }
                    else
                    {
                        unit.PriAttDelay = int.Parse(parts?[10] ?? string.Empty);
                        unit.PriSkelFactor = double.Parse(parts?[11] ?? string.Empty);
                    }
                    break;

                case "stat_pri_attr":
                    if (parts?.Length < 2)
                    {
                        break;
                    }
                    foreach (var part in parts?[1..]!)
                    {
                        if (part != null) unit.PriAttr?.Add(part);
                    }
                    break;

                case "stat_sec":
                    unit.SecAttack = int.Parse(parts?[1] ?? string.Empty);
                    unit.SecCharge = int.Parse(parts?[2] ?? string.Empty);
                    unit.SecProjectile = parts?[3];
                    unit.SecRange = int.Parse(parts?[4] ?? string.Empty);
                    unit.SecAmmunition = int.Parse(parts?[5] ?? string.Empty);
                    unit.SecWeaponType = parts?[6];
                    unit.SecTechType = parts?[7];
                    unit.SecDamageType = parts?[8];
                    unit.SecSoundType = parts?[9];
                    if (parts is { Length: > 12 })
                    {
                        unit.SecFireType = parts[10];
                        unit.SecAttDelay = int.Parse(parts[11] ?? string.Empty);
                        unit.SecSkelFactor = double.Parse(parts[12] ?? string.Empty);
                    }
                    else
                    {
                        unit.SecAttDelay = int.Parse(parts?[10] ?? string.Empty);
                        unit.SecSkelFactor = double.Parse(parts?[11] ?? string.Empty);
                    }
                    break;

                case "stat_sec_attr":
                    if (parts?.Length < 2)
                    {
                        break;
                    }
                    foreach (var part in parts?[1..]!)
                    {
                        if (part != null) unit.SecAttr?.Add(part);
                    }
                    break;

                case "stat_ter":
                    unit.TerAttack = int.Parse(parts?[1] ?? string.Empty);
                    unit.TerCharge = int.Parse(parts?[2] ?? string.Empty);
                    unit.TerProjectile = parts?[3];
                    unit.TerRange = int.Parse(parts?[4] ?? string.Empty);
                    unit.TerAmmunition = int.Parse(parts?[5] ?? string.Empty);
                    unit.TerWeaponType = parts?[6];
                    unit.TerTechType = parts?[7];
                    unit.TerDamageType = parts?[8];
                    unit.TerSoundType = parts?[9];
                    if (parts is { Length: > 12 })
                    {
                        unit.TerFireType = parts[10];
                        unit.TerAttDelay = int.Parse(parts[11] ?? string.Empty);
                        unit.TerSkelFactor = double.Parse(parts[12] ?? string.Empty);
                    }
                    else
                    {
                        unit.TerAttDelay = int.Parse(parts?[10] ?? string.Empty);
                        unit.TerSkelFactor = double.Parse(parts?[11] ?? string.Empty);
                    }
                    break;

                case "stat_ter_attr":
                    if (parts?.Length < 2)
                    {
                        break;
                    }
                    foreach (var part in parts?[1..]!)
                    {
                        if (part != null) unit.TerAttr?.Add(part);
                    }
                    break;

                case "stat_pri_armour":
                    unit.PriArmour = int.Parse(parts?[1] ?? string.Empty);
                    unit.PriDefense = int.Parse(parts?[2] ?? string.Empty);
                    unit.PriShield = int.Parse(parts?[3] ?? string.Empty);
                    unit.PriDefSound = parts?[4];
                    break;

                case "stat_sec_armour":
                    unit.SecArmour = int.Parse(parts?[1] ?? string.Empty);
                    unit.SecDefense = int.Parse(parts?[2] ?? string.Empty);
                    unit.SecDefSound = parts?[3];
                    break;

                case "stat_heat":
                    unit.StatHeat = int.Parse(parts?[1] ?? string.Empty);
                    break;

                case "stat_ground":
                    unit.StatScrub = int.Parse(parts?[1] ?? string.Empty);
                    unit.StatSand = int.Parse(parts?[2] ?? string.Empty);
                    unit.StatForest = int.Parse(parts?[3] ?? string.Empty);
                    unit.StatSnow = int.Parse(parts?[4] ?? string.Empty);
                    break;

                case "stat_mental":
                    unit.Morale = int.Parse(parts?[1] ?? string.Empty);
                    unit.Discipline = parts?[2];
                    unit.Training = parts?[3];
                    if (parts is { Length: > 4 } && parts[4] is "lock_morale")
                    {
                        unit.LockMorale = true;
                    }
                    else
                    {
                        unit.LockMorale = false;
                    }
                    break;

                case "stat_charge_dist":
                    unit.StatChargeDist = int.Parse(parts?[1] ?? string.Empty);
                    break;

                case "stat_fire_delay":
                    unit.StatFireDelay = int.Parse(parts?[1] ?? string.Empty);
                    break;

                case "stat_food":
                    unit.StatFood = int.Parse(parts?[1] ?? string.Empty);
                    unit.StatFoodSec = int.Parse(parts?[2] ?? string.Empty);
                    break;

                case "stat_cost":
                    unit.RecruitTime = int.Parse(parts?[1] ?? string.Empty);
                    unit.RecruitCost = int.Parse(parts?[2] ?? string.Empty);
                    unit.Upkeep = int.Parse(parts?[3] ?? string.Empty);
                    unit.WpnCost = int.Parse(parts?[4] ?? string.Empty);
                    unit.ArmourCost = int.Parse(parts?[5] ?? string.Empty);
                    unit.CustomCost = int.Parse(parts?[6] ?? string.Empty);
                    unit.CustomLimit = int.Parse(parts?[7] ?? string.Empty);
                    unit.CustomIncrease = int.Parse(parts?[8] ?? string.Empty);
                    break;

                case "stat_stl":
                    unit.StatStl = int.Parse(parts?[1] ?? string.Empty);
                    break;

                case "armour_ug_levels":
                    if (parts != null)
                    {
                        var baseLvl = 0;
                        var lvlOne = 0;
                        var lvlTwo = 0;
                        var lvlThree = 0;
                        foreach (var level in parts[1..])
                        {
                            var lvl = int.Parse(level ?? string.Empty);
                            if (string.IsNullOrWhiteSpace(unit.ArmourlvlBase))
                            {
                                baseLvl = lvl;
                                unit.ArmourlvlBase = baseLvl.ToString();
                            }
                            else if (baseLvl == lvl)
                            {
                                unit.ArmourlvlBase += ", " + lvl;
                            }
                            else if (string.IsNullOrWhiteSpace(unit.ArmourlvlOne))
                            {
                                lvlOne = lvl;
                                unit.ArmourlvlOne = lvlOne.ToString();
                            }
                            else if (lvlOne == lvl)
                            {
                                unit.ArmourlvlOne += ", " + lvl;
                            }
                            else if (string.IsNullOrWhiteSpace(unit.ArmourlvlTwo))
                            {
                                lvlTwo = lvl;
                                unit.ArmourlvlTwo = lvlTwo.ToString();
                            }
                            else if (lvlTwo == lvl)
                            {
                                unit.ArmourlvlTwo += ", " + lvl;
                            }
                            else if (string.IsNullOrWhiteSpace(unit.ArmourlvlThree))
                            {
                                lvlThree = lvl;
                                unit.ArmourlvlThree = lvlThree.ToString();
                            }
                            else if (lvlThree == lvl)
                            {
                                unit.ArmourlvlThree += ", " + lvl;
                            }
                        }
                    }

                    break;

                case "armour_ug_models":
                    foreach (var model in parts?[1..]!)
                    {
                        if (string.IsNullOrWhiteSpace(unit.ArmourModelBase))
                        {
                            unit.ArmourModelBase = model?.ToLower();
                            if (unit.ArmourModelBase != null) ModData.BattleModelDb.UsedModels.Add(unit.ArmourModelBase);
                        }
                        else if (string.IsNullOrWhiteSpace(unit.ArmourModelOne))
                        {
                            unit.ArmourModelOne = model?.ToLower();
                            if (unit.ArmourModelOne != null) ModData.BattleModelDb.UsedModels.Add(unit.ArmourModelOne);
                        }
                        else if (string.IsNullOrWhiteSpace(unit.ArmourModelTwo))
                        {
                            unit.ArmourModelTwo = model?.ToLower();
                            if (unit.ArmourModelTwo != null) ModData.BattleModelDb.UsedModels.Add(unit.ArmourModelTwo);
                        }
                        else if (string.IsNullOrWhiteSpace(unit.ArmourModelThree))
                        {
                            unit.ArmourModelThree = model?.ToLower();
                            if (unit.ArmourModelThree != null) ModData.BattleModelDb.UsedModels.Add(unit.ArmourModelThree);
                        }
                    }

                    break;

                case "ownership":
                    foreach (var faction in parts?[1..]!)
                    {
                        if (faction != null) unit.Ownership.Add(faction);
                    }
                    break;

                case "era 0":
                    foreach (var faction in parts?[1..]!)
                    {
                        if (faction != null) unit.EraZero.Add(faction);
                    }
                    break;

                case "era 1":
                    foreach (var faction in parts?[1..]!)
                    {
                        if (faction != null) unit.EraOne.Add(faction);
                    }
                    break;

                case "era 2":
                    foreach (var faction in parts?[1..]!)
                    {
                        if (faction != null) unit.EraTwo.Add(faction);
                    }
                    break;

                case "recruit_priority_offset":
                    unit.RecruitPriorityOffset = float.Parse(parts?[1] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                    break;

                case "info_pic_dir":
                    unit.InfoDict = parts?[1];
                    break;

                case "card_pic_dir":
                    unit.CardDict = parts?[1];
                    break;

                case "crusading_upkeep_modifier":
                    unit.CrusadeUpkeep = float.Parse(parts?[1] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                    break;
            }
            if (identifier != null)
                unit.AssignComments(identifier, ref stream.CommentCache, ref stream.CommentCacheInLine);
        }
        catch (Exception e)
        {
            ErrorDb.AddError(e.Message + " " + identifier, stream.GetCurrentLineIndex.ToString(), stream.GetFileName);
            Console.WriteLine(e);
            Console.WriteLine(identifier);
            Console.WriteLine(@"on line: " + stream.GetCurrentLineIndex.ToString());
            Console.WriteLine(@"====================================================================================");
        }
    }
}