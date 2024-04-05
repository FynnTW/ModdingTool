// Ignore Spelling: Edu

using ModdingTool.View.InterfaceData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using ModdingTool.View.UserControls;
using static ModdingTool.Globals;
using static ModdingTool.ParseHelpers;

namespace ModdingTool
{
    internal static class EduParser
    {
        private const string UnitCardPath = "\\data\\ui\\units";
        private const string UnitInfoCardPath = "\\data\\ui\\unit_info";
        private const string FactionSymbolPath = "\\data\\ui\\faction_symbols";
        private static int s_lineNum;
        private static string s_fileName = "";

        private static readonly List<string> EduEndComments = new();

        public static void WriteEdu()
        {
            if (!Directory.Exists(ModPath + "\\ModdingToolBackup\\data"))
                Directory.CreateDirectory(ModPath + "\\ModdingToolBackup\\data");
            if (!Directory.Exists(ModPath + "\\ModdingToolBackup\\data\\text"))
                Directory.CreateDirectory(ModPath + "\\ModdingToolBackup\\data\\text");
            var backupName = "export_descr_unit_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt";
            File.Copy(ModPath + "/data/export_descr_unit.txt", ModPath + "/ModdingToolBackup/data/" + backupName, true);
            backupName = "export_units_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt";
            File.Copy(ModPath + "/data/text/export_units.txt", ModPath + "/ModdingToolBackup/data/text/" + backupName, true);
            
            if (UnitDataBase.Values.Count(x => x.IsEopUnit == false) > 500)
            {
                ErrorDb.AddError("Exceeded unit limit!");
                var window = Application.Current.MainWindow;
                if (window != null)
                {
                    var statusBar = window.FindName("StatusBarLive") as StatusBarCustom;
                    statusBar?.SetStatusModPath("Exceeded unit limit!");
                }
            }
            var newEdu = UnitDataBase.Values.Where(x => x.IsEopUnit == false).Aggregate("", (current, unit) => current + unit.WriteEntry());
            newEdu += EduEndComments.Aggregate("", (current, comment) => current + (comment + "\n"));
            var contentWithLfFixed = newEdu.Replace("\n", "\r\n").Replace("\r\r\n", "\r\n");
            File.WriteAllText(ModPath + @"/data/export_descr_unit.txt", contentWithLfFixed, Encoding.UTF8);
            var dicEntries = new List<string>();
            var newEu = "";
            foreach (var unit in UnitDataBase.Values.Where(unit => !string.IsNullOrWhiteSpace(unit.Dictionary)))
            {
                if (unit.Dictionary != null && dicEntries.Contains(unit.Dictionary))
                {
                    ErrorDb.AddError("Same Dictionary names for unit " + unit.Type + " are you sure these units should use the same localisation?");
                    continue;
                }
                if (unit.Dictionary != null) dicEntries.Add(unit.Dictionary);
                newEu += WriteEuEntry(unit);
            }

            newEu += "\u00ac-----\n";
            File.WriteAllText(ModPath + @"/data/text/export_units.txt", newEu, Encoding.Unicode);
            foreach (var unit in UnitDataBase.Values.Where(x => x.IsEopUnit))
            {
                var path = unit.FilePath;
                if (string.IsNullOrWhiteSpace(path))
                {
                    path = Globals.ModOptionsInstance.EopDirectories.First();
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        ErrorDb.AddError("No EOP directory found to export unit");
                        continue;
                    }
                }
                var newEduEop = unit.WriteEntry();
                File.WriteAllText(path, newEduEop, encoding: Encoding.UTF8);
            }
            
        }

        private static string WriteEuEntry(Unit unit)
        {
            var entry = "\u00ac-----\n";
            entry += "{" + unit.Dictionary + "}" + unit.LocalizedName + "\n";
            entry += "{" + unit.Dictionary + "_descr}" + unit.Descr + "\n";
            entry += "{" + unit.Dictionary + "_descr_short}" + unit.DescrShort + "\n";
            return entry;
        }


        private static string FormatFloat(double input)
        {
            if (Math.Abs(input - 1) < 0.01)
            {
                return "1";
            }
            return input == 0 ? "0" : input.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private static string FormatFloatSingle(double input)
        {
            if (Math.Abs(input - 1) < 0.01)
            {
                return "1";
            }
            return input == 0 ? "0" : input.ToString("0.0", CultureInfo.InvariantCulture);
        }

        private static void ParseEduEntry(IEnumerable<string> lines, bool isEop = false, string filePath = "")
        {
            //Make first entry
            var newUnit = new Unit
            {
                IsEopUnit = isEop
            };
            if (isEop)
                newUnit.FilePath = filePath;
            var first = true;

            //Reset line counter and EDU index tracker
            var index = 0;
            s_lineNum = 0;

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
                var lineParts = SplitEduLine(newline);
                if (lineParts is { Length: < 2 })
                {
                    //Should be something wrong with line if you hit this
                    ErrorDb.AddError("Unrecognized content", s_lineNum.ToString(), s_fileName);
                    continue;
                }

                //Entry is completed, next entry is starting
                if (lineParts != null && (bool)lineParts[0]?.Equals("type"))
                {
                    if (!first)
                    {
                        newUnit.EduIndex = index;
                        var cards = GetCards(newUnit.Dictionary, newUnit.Ownership, newUnit.CardDict, newUnit.InfoDict, newUnit.MercenaryUnit);
                        newUnit.Card = cards[0];
                        newUnit.CardInfo = cards[1];
                        newUnit.FactionSymbol = cards[2];

                        if (newUnit.Type != null)
                        {
                            AddUnit(newUnit);
                        }

                        index++;
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
                AssignFields(newUnit, lineParts);
            }

            //Add last unit
            newUnit.EduIndex = index;
            AddUnit(newUnit);

            EduEndComments.AddRange(CommentCache);
            CommentCache.Clear();
        }

        public static void ParseEdu()
        {
            s_fileName = "export_descr_unit.txt";
            Console.WriteLine($@"start parse {s_fileName}");

            //Try read file
            var lines = FileReader("\\data\\export_descr_unit.txt", s_fileName, Encoding.UTF8);
            if (lines == null) { return; }

            //Initialize Global Unit Database
            UnitDataBase = new Dictionary<string, Unit>();

            ParseEduEntry(lines);
            
            Console.WriteLine($@"end parse {s_fileName}");

            foreach (var file in ModOptionsInstance.EopDirectories.Where(Directory.Exists).SelectMany(path => 
                         Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories)))
            {
                s_fileName = file.Split('/', '\\').Last();
                Console.WriteLine($@"start parse {s_fileName}");
                lines = FileReaderNonMod(file, s_fileName, Encoding.UTF8);
                if (lines == null) { continue; }
                ParseEduEntry(lines, true, file);
                Console.WriteLine($@"end parse {s_fileName}");
            }

        }

        public static void AddUnit(Unit unit)
        {
            if (unit.Type == null) return;
            unit.AiUnitValue = Math.Round(unit.CalculateUnitValue());
            UnitDataBase.Add(unit.Type, unit);
            Console.WriteLine(unit.Type);

            foreach (var faction in unit.Ownership.Where(_ => true))
            {
                if (faction == "all")
                {
                    foreach (var fac in FactionDataBase)
                    {
                        fac.Value.Unit_ownership.Add(unit.Type);
                    }
                }
                else if (FactionDataBase.Keys.Contains(faction))
                {
                    FactionDataBase[faction].Unit_ownership.Add(unit.Type);
                }
                else if (CultureDataBase.Keys.Contains(faction))
                {
                    foreach (var fac in CultureDataBase.SelectMany(cult => cult.Value.Factions))
                    {
                        FactionDataBase[fac].Unit_ownership.Add(unit.Type);
                    }
                }
                else
                {
                    ErrorDb.AddError($@"Faction not found {faction}", s_lineNum.ToString(), s_fileName);
                    Console.WriteLine(@"faction not found: " + faction);
                }
            }
        }

        public static void ParseEu()
        {
            s_fileName = "export_units.txt";
            Console.WriteLine($@"start parse {s_fileName}");

            //Try read file
            var lines = FileReader("\\data\\text\\export_units.txt", s_fileName, Encoding.Unicode);
            if (lines == null) { return; }

            //Reset line counter
            s_lineNum = 0;

            //Initialize Global names and descriptions database
            UnitNames = new Dictionary<string, string>();
            UnitDescr = new Dictionary<string, string?>();
            UnitDescrShort = new Dictionary<string, string?>();

            //Loop through lines
            foreach (var line in lines)
            {
                //Increase line counter
                s_lineNum++;
                //Clean up line
                var parts = LocalTextLineCleaner(line, s_lineNum, s_fileName);
                if (parts != null)
                    AddUnitStringEntry(parts);

            }
            Console.WriteLine($@"end parse {s_fileName}");
        }

        private static string[] GetCards(string? unit, List<string> factions, string? cardPicDir, string? infoPicDir, bool merc)
        {
            var unitCard = "";
            var cardSearchFactions = factions;
            var infoCardSearchFactions = factions;
            
            if (factions.Contains("all"))
            {
                cardSearchFactions = FactionDataBase.Keys.ToList();
                infoCardSearchFactions = FactionDataBase.Keys.ToList();
            }

            if (merc)
            {
                cardSearchFactions = new List<string> { "mercs" };
                infoCardSearchFactions = new List<string> { "merc" };
            }

            if (cardPicDir != null)
            {
                cardSearchFactions = new List<string> { cardPicDir };
            }
            if (infoPicDir != null)
            {
                infoCardSearchFactions = new List<string> { infoPicDir };
            }

            foreach (var faction in cardSearchFactions)
            {
                var cardPath = ModPath + UnitCardPath + "\\";
                cardPath += faction;
                if (!(Directory.Exists(cardPath)))
                {
                    ErrorDb.AddError($@"Card path not found {cardPath}", s_lineNum.ToString(), s_fileName);
                    continue;
                }

                if (File.Exists(cardPath + "\\#" + unit + ".tga"))
                {
                    unitCard = cardPath + "\\#" + unit + ".tga";
                }
                else
                {
                    ErrorDb.AddError($@"no unit card found for unit: {unit} in faction: {faction}");
                }
            }

            var unitInfoCard = "";
            foreach (var faction in infoCardSearchFactions)
            {
                var cardInfoPath = ModPath + UnitInfoCardPath + "\\";
                cardInfoPath += faction;
                if (!(Directory.Exists(cardInfoPath)))
                {
                    ErrorDb.AddError($@"Info Card path not found {cardInfoPath}", s_lineNum.ToString(), s_fileName);
                    continue;
                }

                if (File.Exists(cardInfoPath + "\\" + unit + "_info.tga"))
                {
                    unitInfoCard = cardInfoPath + "\\" + unit + "_info.tga";
                }
                else
                {
                    ErrorDb.AddError($@"no unit info card found for unit: {unit} in faction: {faction}");
                }
            }

            // Faction Symbols
            var factionSymbol = "";
            var factionSymbolPath = ModPath + FactionSymbolPath + "\\";

            // If the unit is slave only, just set to slave and move on
            if (factions is ["slave"])
            {
                factionSymbolPath += "slave";
            }
            else switch (factions.Count)
            {
                // Otherwise, add the first non-slave faction
                case 1:
                    factionSymbolPath += factions[0];
                    break;
                case > 1:
                {
                    foreach (var faction in factions.Where(faction => faction != "slave"))
                    {
                        factionSymbolPath += faction;
                        break;
                    }
                    break;
                }
            }

            if (File.Exists(factionSymbolPath + ".tga"))
            {
                factionSymbol = factionSymbolPath + ".tga";
            }

            if (unitCard.Equals(""))
            {
                ErrorDb.AddError($@"!!no unit card at all found for unit: {unit}");
            }
            if (unitInfoCard.Equals(""))
            {
                ErrorDb.AddError($@"!!no unit info card at all found for unit: {unit}");
            }
            return new[] { unitCard, unitInfoCard, factionSymbol };
        }

        private static void AddUnitStringEntry(string?[] parts)
        {
            var identifier = parts[0];

            Console.WriteLine(identifier);

            var text = "";

            if (parts.Length > 1)
            {
                text = parts[1];
            }
            else
            {
                ErrorDb.AddError($@"Unit localization is empty {identifier}", s_lineNum.ToString(), s_fileName);
            }

            if (identifier != null && identifier.Contains("_descr_short"))
            {
                var split = identifier.Split("_descr_short", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                UnitDescrShort.TryAdd(split[0], text);
            }
            else if (identifier != null && identifier.Contains("_descr"))
            {
                var split = identifier.Split("_descr", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                UnitDescr.TryAdd(split[0], text);
            }
            else
            {
                if (text == null || identifier == null) return;
                UnitNames.TryAdd(identifier, text);
            }
        }

        private static void AssignComments(string identifier, Unit unit)
        {
            unit.Comments[identifier] = new List<string>();
            unit.Comments[identifier].AddRange(CommentCache);
            CommentCache.Clear();
            unit.CommentsInLine[identifier] = "";
            unit.CommentsInLine[identifier] = CommentCacheInLine;
            CommentCacheInLine = "";
        }

        private static void AssignFields(Unit unit, string?[]? parts)
        {
            var identifier = parts?[0];
            try
            {
                switch (identifier)
                {
                    case "type":
                        AssignComments(identifier, unit);
                        unit.Type = parts?[1]?.Trim();
                        break;

                    case "dictionary":
                        AssignComments(identifier, unit);
                        var dict = parts?[1];
                        if (dict != null)
                        {
                            unit.Dictionary = dict;
                            unit.LocalizedName = UnitNames[dict];
                            unit.Descr = UnitDescr[dict];
                            unit.DescrShort = UnitDescrShort[dict];
                        }
                        break;

                    case "category":
                        AssignComments(identifier, unit);
                        unit.Category = parts?[1]?.Trim();
                        break;

                    case "class":
                        AssignComments(identifier, unit);
                        unit.ClassType = parts?[1]?.Trim();
                        break;

                    case "voice_type":
                        AssignComments(identifier, unit);
                        unit.VoiceType = parts?[1]?.Trim();
                        break;

                    case "accent":
                        AssignComments(identifier, unit);
                        unit.Accent = parts?[1]?.Trim();
                        break;

                    case "banner faction":
                        AssignComments(identifier, unit);
                        unit.BannerFaction = parts?[1]?.Trim();
                        break;

                    case "banner holy":
                        AssignComments(identifier, unit);
                        unit.BannerHoly = parts?[1]?.Trim();
                        break;

                    case "soldier":
                        AssignComments(identifier, unit);
                        unit.Soldier = parts?[1]?.Trim().ToLower();
                        if (unit.Soldier != null) UsedModels.Add(unit.Soldier);
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
                        AssignComments(identifier, unit);
                        if (unit.Officer1 is "")
                        {
                            unit.Officer1 = parts?[1]?.Trim().ToLower();
                            if (unit.Officer1 != null) UsedModels.Add(unit.Officer1);
                            break;
                        }
                        if (unit.Officer2 is "")
                        {
                            unit.Officer2 = parts?[1]?.Trim().ToLower();
                            if (unit.Officer2 != null) UsedModels.Add(unit.Officer2);
                            break;
                        }
                        if (unit.Officer3 is "")
                        {
                            unit.Officer3 = parts?[1]?.Trim().ToLower();
                            if (unit.Officer3 != null) UsedModels.Add(unit.Officer3);
                        }
                        break;

                    case "ship":
                        AssignComments(identifier, unit);
                        unit.Ship = parts?[1]?.Trim().ToLower();
                        break;

                    case "mounted_engine":
                        AssignComments(identifier, unit);
                        unit.MountedEngine = parts?[1]?.Trim().ToLower();
                        break;

                    case "engine":
                        AssignComments(identifier, unit);
                        unit.Engine = parts?[1]?.Trim().ToLower();
                        break;

                    case "animal":
                        AssignComments(identifier, unit);
                        unit.Animal = parts?[1]?.Trim().ToLower();
                        break;

                    case "mount":
                        AssignComments(identifier, unit);
                        unit.Mount = parts?[1]?.Trim();
                        if (unit.Mount != null)
                        {
                            UsedModels.Add(MountDataBase[unit.Mount].model.Trim());
                            UsedMounts.Add(unit.Mount);
                        }

                        break;

                    case "mount_effect":
                        AssignComments(identifier, unit);
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
                        AssignComments(identifier, unit);
                        if (parts == null || parts.Length < 2)
                            break;
                        foreach (var attr in parts[1..])
                        {
                            if (attr == null) break;
                            unit.Attributes.Add(attr);
                            if (!UnitTab.AttributeTypes.Contains(attr))
                            {
                                UnitTab.AttributeTypes.Add(attr);
                            }
                            switch (attr)
                            {
                                case "mercenary_unit":
                                    unit.MercenaryUnit = true;
                                    continue;
                                case "general_unit":
                                    unit.GeneralUnit = true;
                                    continue;
                            }
                        }

                        break;

                    case "move_speed_mod":
                        AssignComments(identifier, unit);
                        unit.MoveSpeed = double.Parse(parts?[1] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                        break;

                    case "formation":
                        AssignComments(identifier, unit);
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
                        AssignComments(identifier, unit);
                        unit.HitPoints = int.Parse(parts?[1] ?? string.Empty);
                        unit.MountHitPoints = int.Parse(parts?[2] ?? string.Empty);
                        break;

                    case "stat_pri":
                        AssignComments(identifier, unit);
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
                        AssignComments(identifier, unit);
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
                        AssignComments(identifier, unit);
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
                        AssignComments(identifier, unit);
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
                        AssignComments(identifier, unit);
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
                        AssignComments(identifier, unit);
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
                        AssignComments(identifier, unit);
                        unit.PriArmour = int.Parse(parts?[1] ?? string.Empty);
                        unit.PriDefense = int.Parse(parts?[2] ?? string.Empty);
                        unit.PriShield = int.Parse(parts?[3] ?? string.Empty);
                        unit.PriDefSound = parts?[4];
                        break;

                    case "stat_sec_armour":
                        AssignComments(identifier, unit);
                        unit.SecArmour = int.Parse(parts?[1] ?? string.Empty);
                        unit.SecDefense = int.Parse(parts?[2] ?? string.Empty);
                        unit.SecDefSound = parts?[3];
                        break;

                    case "stat_heat":
                        AssignComments(identifier, unit);
                        unit.StatHeat = int.Parse(parts?[1] ?? string.Empty);
                        break;

                    case "stat_ground":
                        AssignComments(identifier, unit);
                        unit.StatScrub = int.Parse(parts?[1] ?? string.Empty);
                        unit.StatSand = int.Parse(parts?[2] ?? string.Empty);
                        unit.StatForest = int.Parse(parts?[3] ?? string.Empty);
                        unit.StatSnow = int.Parse(parts?[4] ?? string.Empty);
                        break;

                    case "stat_mental":
                        AssignComments(identifier, unit);
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
                        AssignComments(identifier, unit);
                        unit.StatChargeDist = int.Parse(parts?[1] ?? string.Empty);
                        break;

                    case "stat_fire_delay":
                        AssignComments(identifier, unit);
                        unit.StatFireDelay = int.Parse(parts?[1] ?? string.Empty);
                        break;

                    case "stat_food":
                        AssignComments(identifier, unit);
                        unit.StatFood = int.Parse(parts?[1] ?? string.Empty);
                        unit.StatFoodSec = int.Parse(parts?[2] ?? string.Empty);
                        break;

                    case "stat_cost":
                        AssignComments(identifier, unit);
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
                        AssignComments(identifier, unit);
                        unit.StatStl = int.Parse(parts?[1] ?? string.Empty);
                        break;

                    case "armour_ug_levels":
                        AssignComments(identifier, unit);
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
                        AssignComments(identifier, unit);
                        foreach (var model in parts?[1..]!)
                        {
                            if (string.IsNullOrWhiteSpace(unit.ArmourModelBase))
                            {
                                unit.ArmourModelBase = model?.ToLower();
                                if (unit.ArmourModelBase != null) UsedModels.Add(unit.ArmourModelBase);
                            }
                            else if (string.IsNullOrWhiteSpace(unit.ArmourModelOne))
                            {
                                unit.ArmourModelOne = model?.ToLower();
                                if (unit.ArmourModelOne != null) UsedModels.Add(unit.ArmourModelOne);
                            }
                            else if (string.IsNullOrWhiteSpace(unit.ArmourModelTwo))
                            {
                                unit.ArmourModelTwo = model?.ToLower();
                                if (unit.ArmourModelTwo != null) UsedModels.Add(unit.ArmourModelTwo);
                            }
                            else if (string.IsNullOrWhiteSpace(unit.ArmourModelThree))
                            {
                                unit.ArmourModelThree = model?.ToLower();
                                if (unit.ArmourModelThree != null) UsedModels.Add(unit.ArmourModelThree);
                            }
                        }
                        unit.ArmourUgModels = parts[1..];
                        break;

                    case "ownership":
                        AssignComments(identifier, unit);
                        foreach (var faction in parts?[1..]!)
                        {
                            if (faction != null) unit.Ownership.Add(faction);
                        }
                        break;

                    case "era 0":
                        AssignComments(identifier, unit);
                        foreach (var faction in parts?[1..]!)
                        {
                            if (faction != null) unit.EraZero.Add(faction);
                        }
                        break;

                    case "era 1":
                        AssignComments(identifier, unit);
                        foreach (var faction in parts?[1..]!)
                        {
                            if (faction != null) unit.EraOne.Add(faction);
                        }
                        break;

                    case "era 2":
                        AssignComments(identifier, unit);
                        foreach (var faction in parts?[1..]!)
                        {
                            if (faction != null) unit.EraTwo.Add(faction);
                        }
                        break;

                    case "recruit_priority_offset":
                        AssignComments(identifier, unit);
                        unit.RecruitPriorityOffset = float.Parse(parts?[1] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                        break;

                    case "info_pic_dir":
                        AssignComments(identifier, unit);
                        unit.InfoDict = parts?[1];
                        break;

                    case "card_pic_dir":
                        AssignComments(identifier, unit);
                        unit.CardDict = parts?[1];
                        break;

                    case "crusading_upkeep_modifier":
                        AssignComments(identifier, unit);
                        unit.CrusadeUpkeep = float.Parse(parts?[1] ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
                        break;
                }
            }
            catch (Exception e)
            {
                ErrorDb.AddError(e.Message + " " + identifier, s_lineNum.ToString(), s_fileName);
                Console.WriteLine(e);
                Console.WriteLine(identifier);
                Console.WriteLine(@"on line: " + s_lineNum);
                Console.WriteLine(@"====================================================================================");
            }
        }
    }

}