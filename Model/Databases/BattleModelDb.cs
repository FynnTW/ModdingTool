using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using static ModdingTool.Globals;

namespace ModdingTool.Databases;

public partial class BattleModelDb
{
    private Dictionary<string, BattleModel> BattleModels { get; set; } = new();
    public List<string> UsedModels = new();
    
    public void Add(BattleModel battleModel)
    {
        BattleModels.Add(battleModel.Name, battleModel);
    }
    
    public Dictionary<string, BattleModel> GetBattleModels() => BattleModels;
    
    public bool Contains(string name) => BattleModels.ContainsKey(name);
    
    public BattleModel? Get(string key) => BattleModels.GetValueOrDefault(key);
    
    public BattleModel? GetByIndex(int index)
    {
        if (index < 0 || index >= BattleModels.Count)
            return null;
        return BattleModels.ElementAt(index).Value;
    }
    
    public void ImportJson(string fileName)
    {
        var jsonString = File.ReadAllText(fileName);
        var imported = JsonConvert.DeserializeObject<Dictionary<string, BattleModel>>(jsonString);
        if (imported == null)
        {
            ErrorDb.AddError("Imported JSON is null");
            return;
        }
        BattleModels = new Dictionary<string, BattleModel>();
        foreach (var entry in imported)
            Add(entry.Value);
    }
    
    public void ExportJson() =>
        File.WriteAllText(@"bmdb.json", JsonConvert.SerializeObject(BattleModels));
    
    public List<string> GetNames() => new(BattleModels.Keys);
    public void Remove(string name) => BattleModels.Remove(name);

    public void WriteFile()
    {
        BackupFile("\\data\\unit_models\\", "battle_models.modeldb");
        var newBmdb = "";
        newBmdb += "22 serialization::archive 3 0 0 0 0 ";
        newBmdb += BattleModels.Count + 1 + " 0 0\n";
        newBmdb += "5 blank 0 0 0 1 0 0 0 0 0 0 1 0 0 0 0 0 0 0 0 0 1 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0\n";
        newBmdb = BattleModels.Aggregate(newBmdb, (current, entry) => current + entry.Value.WriteEntry());
        File.WriteAllText(ModPath + @"/data/unit_models/battle_models.modeldb", newBmdb.Replace("\n", "\r\n").Replace("\r\r\n", "\r\n"));
    }
    
    public void ParseFile()
    {
        var fileStream = new FileStream("\\data\\unit_models\\", "battle_models.modeldb");
        var valid = fileStream.Read();
        if (!valid)
            return;
        fileStream.LogStart();

        //First entry in vanilla has extra ints
        var firstEntry = false;

        ////////Parse bmdb////////

        //serialization
        fileStream.SetStringPos(35);

        var numberOfEntries = fileStream.GetInt();
        // ReSharper disable once NotAccessedVariable
        var pad = fileStream.GetInt();
        // ReSharper disable once RedundantAssignment
        pad = fileStream.GetInt();

        //Loop through all entries
        for (var n = 0; n < numberOfEntries; n++)
        {
            //Create new entry
            var entry = new BattleModel
            {
                Name = fileStream.GetString().ToLower()
            };
            //check if there is a blank entry or default vanilla with extra ints
            switch (n)
            {
                case 0 when entry.Name.Equals("blank"):
                {
                    for (var i = 0; i < 39; i++)
                    {
                        // ReSharper disable once RedundantAssignment
                        pad = fileStream.GetInt();
                    }
                    continue;
                }
                case 0 when !entry.Name.Equals("blank"):
                    firstEntry = true;
                    break;
            }

            //Get rest of entry
            entry.Scale = fileStream.GetFloat();
            if (firstEntry) { pad = fileStream.GetInt(); pad = fileStream.GetInt(); }
            entry.LodCount = fileStream.GetInt();
            if (firstEntry) { pad = fileStream.GetInt(); pad = fileStream.GetInt(); }
            entry.LodTable = new List<Lod>();
            for (var i = 0; i < entry.LodCount; i++)
            {
                entry.LodTable.Add(new Lod
                {
                    Mesh = fileStream.GetString(),
                    Distance = fileStream.GetInt(),
                    Name = entry.Name
                });
            }
            if (firstEntry) { pad = fileStream.GetInt(); pad = fileStream.GetInt(); }
            entry.MainTexturesCount = fileStream.GetInt();
            if (firstEntry) { pad = fileStream.GetInt(); pad = fileStream.GetInt(); }
            for (var i = 0; i < entry.MainTexturesCount; i++)
            {
                var facname = fileStream.GetString().ToLower();
                if (FactionDataBase.ContainsKey(facname) || facname == "merc")
                {
                    entry.MainTextures.Add(new Texture
                    {
                        Name = entry.Name,
                        Faction = facname,
                        TexturePath = fileStream.GetString(),
                        Normal = fileStream.GetString(),
                        Sprite = fileStream.GetString()
                    });
                }
                else
                {
                    var texpad = fileStream.GetString();
                    texpad = fileStream.GetString();
                    texpad = fileStream.GetString();
                }
            }
            entry.MainTexturesCount = entry.MainTextures.Count;
            entry.AttachTexturesCount = fileStream.GetInt();
            for (var i = 0; i < entry.AttachTexturesCount; i++)
            {
                var facname = fileStream.GetString().ToLower();
                if (FactionDataBase.ContainsKey(facname) || facname == "merc")
                {
                    entry.AttachTextures.Add(new Texture
                    {
                        Name = entry.Name,
                        Faction = facname,
                        TexturePath = fileStream.GetString(),
                        Normal = fileStream.GetString(),
                        Sprite = fileStream.GetString()
                    });
                }
                else
                {
                    // ReSharper disable once NotAccessedVariable
                    var texpad = fileStream.GetString();
                    texpad = fileStream.GetString();
                    texpad = fileStream.GetString();
                }
            }
            entry.AttachTexturesCount = entry.AttachTextures.Count;
            if (firstEntry) { pad = fileStream.GetInt(); pad = fileStream.GetInt(); }
            entry.MountTypeCount = fileStream.GetInt();
            if (firstEntry) { pad = fileStream.GetInt(); pad = fileStream.GetInt(); }
            entry.Animations = new List<Animation>();
            for (var i = 0; i < entry.MountTypeCount; i++)
            {
                entry.Animations.Add(new Animation
                {
                    Name = entry.Name,
                    MountType = fileStream.GetString().ToLower(),
                    PrimarySkeleton = fileStream.GetString(),
                    SecondarySkeleton = fileStream.GetString(),
                    PriWeaponCount = fileStream.GetInt()
                });
                entry.Animations[i].PriWeapons = new List<string>();
                for (var j = 0; j < entry.Animations[i].PriWeaponCount; j++)
                {
                    entry.Animations[i].PriWeapons.Add(fileStream.GetString());
                }

                if (entry.Animations[i].PriWeapons.Count > 0)
                {
                    entry.Animations[i].PriWeaponOne = entry.Animations[i].PriWeapons[0];
                    if (entry.Animations[i].PriWeapons.Count > 1)
                    {
                        entry.Animations[i].PriWeaponTwo = entry.Animations[i].PriWeapons[1];
                    }
                }
                entry.Animations[i].SecWeaponCount = fileStream.GetInt();
                entry.Animations[i].SecWeapons = new List<string>();
                for (var j = 0; j < entry.Animations[i].SecWeaponCount; j++)
                {
                    entry.Animations[i].SecWeapons.Add(fileStream.GetString());
                }
                if (entry.Animations[i].SecWeapons.Count <= 0) continue;
                entry.Animations[i].SecWeaponOne = entry.Animations[i].SecWeapons[0];
                if (entry.Animations[i].SecWeapons.Count > 1)
                {
                    entry.Animations[i].SecWeaponTwo = entry.Animations[i].SecWeapons[1];
                }
            }
            if (firstEntry) { pad = fileStream.GetInt(); pad = fileStream.GetInt(); }
            entry.TorchIndex = fileStream.GetInt();
            entry.TorchBoneX = fileStream.GetFloat();
            entry.TorchBoneY = fileStream.GetFloat();
            entry.TorchBoneZ = fileStream.GetFloat();
            entry.TorchSpriteX = fileStream.GetFloat();
            entry.TorchSpriteY = fileStream.GetFloat();
            entry.TorchSpriteZ = fileStream.GetFloat();
            if (firstEntry) { pad = fileStream.GetInt(); pad = fileStream.GetInt(); }
            firstEntry = false;
            if (BattleModels.ContainsKey(entry.Name))
            {
                ErrorDb.AddError($"Duplicate entry {entry.Name}");
                Console.WriteLine(@"====================================================================================");
            }
            Add(entry);
            Console.WriteLine(entry.Name);
        }
        fileStream.LogEnd();
    }
    
    public void CheckModelUsage()
    {
        CheckUseInFile("\\data\\world\\maps\\campaign\\imperial_campaign\\", "campaign_script.txt");
        CheckUseInFile("\\data\\world\\maps\\campaign\\imperial_campaign\\", "descr_strat.txt");
        
        UsedModels = ModData.BattleModelDb.UsedModels.Distinct().ToList();
        foreach (var entry in BattleModels.Where(entry => !UsedModels.Contains(entry.Key)))
        {
            Print(entry.Key);
            ErrorDb.AddError("Model " + entry.Key + " is not used");
            //BattleModels.Remove(entry.Key);
        }
        UsedMounts = UsedMounts.Distinct().ToList();
        foreach (var entry in MountDataBase.Where(entry => !UsedMounts.Contains(entry.Key)))
        {
            Print(entry.Key);
            ErrorDb.AddError("Mount " + entry.Key + " is not used");
        }
    }
    
    private void CheckUseInFile(string path, string filename)
    {
        var fileStream = new FileStream(path, filename);
        var valid = fileStream.ReadLines();
        if (!valid)
            return;
        fileStream.LogStart();

        for (var i = 0; i < fileStream.GetLineNum() - 1; i++)
        {
            var line = fileStream.GetCleanLine();
            if (string.IsNullOrWhiteSpace(line))
                continue;
            var rx = BattleModelRegex1();
            var match = rx.Match(line);
            if (!match.Success) continue;
            var matchString = match.Groups[1].Value;
            if (matchString.Contains(','))
            {
                matchString = matchString.Split(',')[0];
            }
            UsedModels.Add(matchString.ToLower().Trim());
        }

        fileStream.LogEnd();
    }
    
    [GeneratedRegex("battle_model\\s+(\\S*)\\s")]
    private static partial Regex BattleModelRegex1();
}