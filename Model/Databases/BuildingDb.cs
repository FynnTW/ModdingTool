using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using static ModdingTool.Globals;

namespace ModdingTool.Databases;

public partial class BuildingDb
{
        private Dictionary<string, Building> Buildings { get; set; } = new();
        
        public void Add(Building building)
        {
                if (Buildings.TryAdd(building.Name, building)) return;
                ErrorDb.AddError("Duplicate entry " + building.Name);
                Buildings[building.Name] = building;
        }
        
        public int GetCount() => Buildings.Count;
        
        public Dictionary<string, Building> GetBuildings() => Buildings;
        
        public bool Contains(string name) => Buildings.ContainsKey(name);
        
        public Building? Get(string key) => Buildings.GetValueOrDefault(key);
        
        public Building? GetByIndex(int index)
        {
                if (index < 0 || index >= Buildings.Count)
                        return null;
                return Buildings.ElementAt(index).Value;
        }
        
        public void ImportJson(string fileName)
        {
                var jsonString = File.ReadAllText(fileName);
                var imported = JsonConvert.DeserializeObject<Dictionary<string, Building>>(jsonString);
                if (imported == null)
                {
                        ErrorDb.AddError("Imported JSON is null");
                        return;
                }
                Buildings = new Dictionary<string, Building>();
                foreach (var entry in imported)
                        Add(entry.Value);
        }
        
        public void ExportJson() =>
                File.WriteAllText(@"edb.json", JsonConvert.SerializeObject(Buildings, Formatting.Indented));
        
        public List<string> GetNames() => new(Buildings.Keys);
        
        public void Remove(string name)
        {
                if (!Buildings.ContainsKey(name)) return;
                var model = Get(name);
                if (model == null) return;
                Buildings.Remove(name);
        }

        public void WriteFile()
        {
                
        }
        
        private enum BracketType
        {
                Building,
                Levels,
                Level,
                Upgrades,
                Plugins,
                Capabilities,
                FactionCapabilities,
                None
        }

        public void ParseFiles()
        {
                var fileStream = new FileStream("\\data\\", "export_descr_buildings.txt");
                var valid = fileStream.ReadLines();
                if (!valid)
                        return;
                fileStream.LogStart();
                
                Buildings = new Dictionary<string, Building>();
                char[] delimitersWhite = { ' ', '\t' };
                char[] delimitersBrackets = { '{', '}' ,','};
                Stack<BracketType> bracketStack = new();
                var building = new Building();
                var nextBracket = BracketType.None;
                var currentBracket = BracketType.None;

               do {
                        //Console.WriteLine(line);
                        
                        var line = fileStream.GetNextCleanLine();
                        if (line == null)
                                break;
                        try
                        {
                                var parts = line.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries);
                                switch (parts[0])
                                {
                                        case "{":
                                                if (nextBracket == BracketType.None)
                                                {
                                                        Console.WriteLine(@"bracket error at:  " + fileStream.GetCurrentLineIndex);
                                                        break;
                                                }
                                                bracketStack.Push(nextBracket);
                                                nextBracket = BracketType.None;
                                                currentBracket = bracketStack.Peek();
                                                continue;
                                        case "}":
                                        {
                                                BracketType bType;
                                                try
                                                {
                                                        bType = bracketStack.Pop();
                                                        currentBracket = bracketStack.Count > 0 ? bracketStack.Peek() : BracketType.None;
                                                }
                                                catch (Exception e)
                                                {
                                                        Console.WriteLine(@"bracket error at:  " + fileStream.GetCurrentLineIndex);
                                                        Console.WriteLine(e);
                                                        throw;
                                                }
                                                if (bType == BracketType.Building)
                                                {
                                                        Add(building);
                                                        Console.WriteLine(building.Name);
                                                }
                                                continue;
                                        }
                                        case "building":
                                                nextBracket = BracketType.Building;
                                                building = new Building
                                                {
                                                        Name = parts[1]
                                                };
                                                if (building.IsTemple)
                                                        building.Classification = "religious";
                                                else if (building.IsHinterland)
                                                        building.Classification = "infrastructure";
                                                else if (building.IsConvert)
                                                        building.Classification = "convertion";
                                                else if (building.IsGuild)
                                                        building.Classification = "guild";
                                                break;
                                        case "levels":
                                                nextBracket = BracketType.Levels;
                                                building.LevelNames = parts[1..].ToList();
                                                break;
                                        case "upgrades":
                                                nextBracket = BracketType.Upgrades;
                                                break;
                                        case "plugins":
                                                nextBracket = BracketType.Plugins;
                                                break;
                                        case "capability":
                                                nextBracket = BracketType.Capabilities;
                                                break;
                                        case "faction_capability":
                                                nextBracket = BracketType.FactionCapabilities;
                                                break;
                                        case "convert_to":
                                                if (currentBracket == BracketType.Building)
                                                        building.ConvertTo = parts[1];
                                                else if (currentBracket == BracketType.Level)
                                                        building.Levels[^1].ConvertTo = parts[1];
                                                break;
                                        case "classification":
                                                building.Classification = parts[1];
                                                break;
                                        case "factions":
                                                if (parts[1].Equals("all"))
                                                {
                                                        building.Factions = FactionDataBase.Keys.ToList();
                                                        building.Cultures = CultureDataBase.Keys.ToList();
                                                }
                                                var partsComma = line.Split(delimitersBrackets, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var part in partsComma[1..])
                                                {
                                                        if (part.Equals("all"))
                                                        {
                                                                building.Factions = FactionDataBase.Keys.ToList();
                                                                building.Cultures = CultureDataBase.Keys.ToList();
                                                                break;
                                                        }
                                                        if (FactionDataBase.ContainsKey(part))
                                                        {
                                                                building.AddFaction(part);
                                                        }
                                                        else if (CultureDataBase.ContainsKey(part))
                                                        {
                                                                building.AddCulture(part);
                                                        }
                                                };
                                                break;
                                        case "religion":
                                                building.Religion = parts[1];
                                                break;
                                        case "material":
                                                building.Levels[^1].Material = parts[1];
                                                break;
                                        case "construction":
                                                building.Levels[^1].ConstructionTime = int.Parse(parts[1]);
                                                break;
                                        case "cost":
                                                building.Levels[^1].ConstructionCost = int.Parse(parts[1]);
                                                break;
                                        case "settlement_min":
                                                building.Levels[^1].SettlementMinLevel = parts[1];
                                                break;
                                                        
                                }
                                
                                if (currentBracket is BracketType.Capabilities or BracketType.FactionCapabilities)
                                {
                                        var cap = new Capability()
                                        {
                                                Type = parts[0]
                                        };
                                        if (parts.Length > 1)
                                        {
                                                if (line.Contains(" bonus "))
                                                {
                                                        cap.Bonus = true;
                                                }
                                                if (line.Contains(" requires "))
                                                {
                                                        var condition = line.Split(" requires ")[1];
                                                        cap.Condition = condition;
                                                }
                                                switch (cap.Type)
                                                {
                                                        case "recruit_pool":
                                                                var recSplit = line.Split('"');
                                                                cap.Unit = recSplit[1];
                                                                var newParts = recSplit[2].Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries);
                                                                cap.InitialPool = double.Parse(newParts[0], CultureInfo.InvariantCulture.NumberFormat);
                                                                cap.ReplenishmentRate = double.Parse(newParts[1], CultureInfo.InvariantCulture.NumberFormat);
                                                                cap.MaximumPool = double.Parse(newParts[2], CultureInfo.InvariantCulture.NumberFormat);
                                                                cap.StartingExperience = int.Parse(newParts[3]);
                                                                break;
                                                        case "agent":
                                                                cap.AgentType = parts[1];
                                                                break;
                                                        case "agent_limit":
                                                                cap.AgentType = parts[1];
                                                                cap.Value = int.Parse(cap.Bonus ? parts[3] : parts[2]);
                                                                break;
                                                        default:
                                                                cap.Value = (int)double.Parse(cap.Bonus ? parts[2] : parts[1]);
                                                                break;
                                                }
                                        }
                                        if (currentBracket == BracketType.Capabilities)
                                        {
                                                building.Levels[^1].AddCapability(cap);
                                        }
                                        else 
                                        {
                                                building.Levels[^1].AddFactionCapability(cap);
                                        }
                                }
                                
                                if (currentBracket == BracketType.Upgrades)
                                {
                                        var upg = new BuildingUpgrade()
                                        {
                                                Name = parts[0]
                                        };
                                        if (parts.Length > 1)
                                        {
                                             if (line.Contains(" requires "))
                                             {
                                                     var condition = line.Split(" requires ")[1];
                                                     upg.Condition = condition;
                                             }
                                        }
                                        building.Levels[^1].AddUpgrade(upg);
                                }
                                if (currentBracket == BracketType.Levels)
                                {
                                        if (building.LevelNames.Contains(parts[0]))
                                        {
                                                building.AddBuildingLevel(new BuildingLevel());
                                                nextBracket = BracketType.Level;
                                                building.Levels[^1].Name = parts[0];
                                                if (parts.Length > 1)
                                                {
                                                        switch (parts[1])
                                                        {
                                                                case "city":
                                                                        building.Levels[^1].AvailableCastle = false;
                                                                        break;
                                                                case "castle":
                                                                        building.Levels[^1].AvailableCity = false;
                                                                        break;
                                                        }
                                                        if (line.Contains(" requires "))
                                                        {
                                                                var condition = line.Split(" requires ")[1];
                                                                building.Levels[^1].Condition = condition;
                                                        }
                                                }
                                        }
                                }
                        }
                        catch (Exception e)
                        {
                                ErrorDb.AddError(e.ToString(), fileStream.GetCurrentLineIndex.ToString(), fileStream.GetFileName);
                        }
                } while (true);
                fileStream.LogEnd();
                ModData.Buildings.ExportJson();
        }

}