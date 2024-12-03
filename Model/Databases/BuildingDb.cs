using System;
using System.Collections.Generic;
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
                var line = fileStream.GetNextCleanLine();
                Stack<BracketType> bracketStack = new();
                var building = new Building();
                var nextBracket = BracketType.None;
                var currentBracket = BracketType.None;

                while (line != null)
                {
                        //Console.WriteLine(line);
                        var parts = line.Split(delimitersWhite);
                        switch (parts[0])
                        {
                                case "{":
                                        if (nextBracket == BracketType.None)
                                        {
                                                Console.WriteLine(@"bracket error at:  " + fileStream.GetCurrentLineIndex);
                                                break;
                                        }
                                        bracketStack.Push(nextBracket);
                                        currentBracket = bracketStack.Peek();
                                        break;
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
                                        }

                                        break;
                                }
                                case "building":
                                        nextBracket = BracketType.Building;
                                        building = new Building
                                        {
                                                Name = parts[1]
                                        };
                                        break;
                                case "levels":
                                        nextBracket = BracketType.Levels;
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
                        }
                        line = fileStream.GetNextCleanLine();
                }
                fileStream.LogEnd();
        }

}