using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using static ModdingTool.Globals;

namespace ModdingTool.Databases;

public partial class BattleModelDb
{
    /// <summary>
    /// Gets or sets the dictionary of BattleModels, where the key is the name of the BattleModel and the value is the BattleModel itself.
    /// </summary>
    private Dictionary<string, BattleModel> BattleModels { get; set; } = new();

    /// <summary>
    /// A list of names of the BattleModels that are used in the game files.
    /// </summary>
    public List<string> UsedModels = new();
    
    /// <summary>
    /// Adds a new BattleModel to the BattleModels dictionary.
    /// </summary>
    /// <param name="battleModel">The BattleModel to be added.</param>
    /// <remarks>
    /// If a BattleModel with the same name already exists in the dictionary, an error is logged and the new BattleModel is not added.
    /// </remarks>
    public void Add(BattleModel battleModel)
    {
        if (BattleModels.TryAdd(battleModel.Name, battleModel)) return;
        ErrorDb.AddError("Duplicate entry " + battleModel.Name);
        BattleModels[battleModel.Name] = battleModel;
    }
    
    /// <summary>
    /// Retrieves the count of BattleModels in the BattleModels dictionary.
    /// </summary>
    /// <returns>
    /// The count of BattleModels in the BattleModels dictionary.
    /// </returns>
    public int GetCount() => BattleModels.Count;
    
    /// <summary>
    /// Retrieves the BattleModels dictionary.
    /// </summary>
    /// <returns>
    /// The BattleModels dictionary.
    /// </returns>
    public Dictionary<string, BattleModel> GetBattleModels() => BattleModels;
    
    /// <summary>
    /// Checks if the BattleModels dictionary contains a specific key.
    /// </summary>
    /// <param name="name">The key to locate in the BattleModels dictionary.</param>
    /// <returns>
    /// true if the BattleModels dictionary contains an element with the specified key; otherwise, false.
    /// </returns>
    public bool Contains(string name) => BattleModels.ContainsKey(name);
    
    /// <summary>
    /// Gets the value associated with the specified key or the default value for the type of the value parameter.
    /// </summary>
    /// <param name="key">The key of the value to get.</param>
    /// <returns>
    /// The value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter.
    /// </returns>
    public BattleModel? Get(string key) => BattleModels.GetValueOrDefault(key);
    
    /// <summary>
    /// Retrieves the BattleModel at the specified index in the BattleModels dictionary.
    /// </summary>
    /// <param name="index">The zero-based index of the BattleModel to get.</param>
    /// <returns>
    /// The BattleModel at the specified index in the BattleModels dictionary, if the index is valid; otherwise, null.
    /// </returns>
    public BattleModel? GetByIndex(int index)
    {
        if (index < 0 || index >= BattleModels.Count)
            return null;
        return BattleModels.ElementAt(index).Value;
    }
    
    /// <summary>
    /// Imports a JSON file and deserializes it into the BattleModels dictionary.
    /// </summary>
    /// <param name="fileName">The name of the JSON file to import.</param>
    /// <remarks>
    /// If the imported JSON is null, an error is logged and the method returns.
    /// Otherwise, the BattleModels dictionary is cleared and populated with the imported data.
    /// </remarks>
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
    
    /// <summary>
    /// Exports the BattleModels dictionary to a JSON file.
    /// </summary>
    /// <remarks>
    /// The BattleModels dictionary is serialized into a JSON string and written to a file named "bmdb.json".
    /// The JSON is formatted with indented formatting for readability.
    /// </remarks>
    public void ExportJson() =>
        File.WriteAllText(@"bmdb.json", JsonConvert.SerializeObject(BattleModels, Formatting.Indented));
    
    /// <summary>
    /// Retrieves a list of the names of all BattleModels in the BattleModels dictionary.
    /// </summary>
    /// <returns>
    /// A new list containing the names of all BattleModels in the BattleModels dictionary.
    /// </returns>
    public List<string> GetNames() => new(BattleModels.Keys);

    /// <summary>
    /// Removes a BattleModel from the BattleModels dictionary.
    /// </summary>
    /// <param name="name">The name of the BattleModel to remove.</param>
    /// <remarks>
    /// If the BattleModels dictionary does not contain a BattleModel with the specified name, the method returns.
    /// If the BattleModel to be removed is still in use, an error is logged.
    /// </remarks>
    public void Remove(string name)
    {
        if (!BattleModels.ContainsKey(name)) return;
        var model = Get(name);
        if (model == null) return;
        if (model.ModelUsage.IsUsed())
            ErrorDb.AddError($@"Model entry {model.Name} was removed but still used in: {model.ModelUsage.GetUsageString()}");
        BattleModels.Remove(name);
    }

    /// <summary>.n
    /// Writes the BattleModels dictionary to the battlemodels.modeldb file in the mod folder.
    /// </summary>
    /// <remarks>
    /// This method first backs up the existing file, then constructs a new string representation of the BattleModels dictionary.
    /// The new string is written to the file "battle_models.modeldb" in the "data/unit_models" directory.
    /// The string is formatted with newline characters replaced by the appropriate line ending for the current platform.
    /// </remarks>
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
    
    /// <summary>
    /// Parses the battle_models.modeldb file and populates the BattleModels dictionary.
    /// </summary>
    /// <remarks>
    /// This method reads the battle_models.modeldb file and creates a new BattleModel for each entry in the file.
    /// Each BattleModel is populated with data from the file and added to the BattleModels dictionary.
    /// If the file cannot be read, the method returns.
    /// </remarks>
    public void ParseFile()
    {
        var fileStream = new FileStream("\\data\\unit_models\\", "battle_models.modeldb");
        var valid = fileStream.Read();
        if (!valid)
            return;
        fileStream.LogStart();

        //First entry in vanilla has extra integers
        var firstEntry = false;

        ////////Parse bmdb////////

        //serialization
        fileStream.SetStringPos(35);

        var numberOfEntries = fileStream.GetInt();
        FirstEntryPad(ref fileStream);

        //Loop through all entries
        for (var n = 0; n < numberOfEntries; n++)
        {
            //Create new entry
            var entry = new BattleModel
            {
                Name = fileStream.GetString().ToLower()
            };
            //check if there is a blank entry or default vanilla with extra integers
            switch (n)
            {
                case 0 when entry.Name.Equals("blank"):
                {
                    for (var i = 0; i < 39; i++)
                       fileStream.GetInt();
                    continue;
                }
                case 0 when !entry.Name.Equals("blank"):
                    firstEntry = true;
                    break;
            }

            //Get rest of entry
            entry.Scale = fileStream.GetFloat();
            if (firstEntry)
                FirstEntryPad(ref fileStream);
            entry.LodCount = fileStream.GetInt();
            if (firstEntry)
                FirstEntryPad(ref fileStream);
            entry.LodTable = new List<Lod>();
            for (var i = 0; i < entry.LodCount; i++)
                entry.AddLod(fileStream.GetString(), fileStream.GetInt(), i, entry.Name);
            if (firstEntry)
                FirstEntryPad(ref fileStream);
            entry.MainTexturesCount = fileStream.GetInt();
            if (firstEntry)
                FirstEntryPad(ref fileStream);
            for (var i = 0; i < entry.MainTexturesCount; i++)
            {
                var facName = fileStream.GetString().ToLower();
                if (FactionDataBase.ContainsKey(facName) || facName == "merc")
                {
                    entry.AddMainTexture(
                        facName, 
                        fileStream.GetString(), 
                        fileStream.GetString(),
                        fileStream.GetString(), entry.Name);
                }
                else
                {
                    fileStream.GetString();
                    fileStream.GetString();
                    fileStream.GetString();
                }
            }
            entry.MainTexturesCount = entry.MainTextures.Count;
            entry.AttachTexturesCount = fileStream.GetInt();
            for (var i = 0; i < entry.AttachTexturesCount; i++)
            {
                var facName = fileStream.GetString().ToLower();
                if (FactionDataBase.ContainsKey(facName) || facName == "merc")
                {
                    entry.AddAttachTexture(
                        facName, 
                        fileStream.GetString(), 
                        fileStream.GetString(),
                        fileStream.GetString(), entry.Name);
                }
                else
                {
                    fileStream.GetString();
                    fileStream.GetString();
                    fileStream.GetString();
                }
            }
            entry.AttachTexturesCount = entry.AttachTextures.Count;
            if (firstEntry)
                FirstEntryPad(ref fileStream);
            entry.MountTypeCount = fileStream.GetInt();
            if (firstEntry)
                FirstEntryPad(ref fileStream);
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
            if (firstEntry)
                FirstEntryPad(ref fileStream);
            entry.TorchIndex = fileStream.GetInt();
            entry.TorchBoneX = fileStream.GetFloat();
            entry.TorchBoneY = fileStream.GetFloat();
            entry.TorchBoneZ = fileStream.GetFloat();
            entry.TorchSpriteX = fileStream.GetFloat();
            entry.TorchSpriteY = fileStream.GetFloat();
            entry.TorchSpriteZ = fileStream.GetFloat();
            if (firstEntry)
                FirstEntryPad(ref fileStream);
            firstEntry = false;
            Add(entry);
            Console.WriteLine(entry.Name);
        }
        fileStream.LogEnd();
    }

    /// <summary>
    /// Skips two integers in the FileStream.
    /// </summary>
    /// <param name="stream">The FileStream to read from.</param>
    /// <remarks>
    /// This method is used to skip over two integers in the FileStream during the parsing of the first entry in the battle_models.modeldb file.
    /// </remarks>
    private static void FirstEntryPad(ref FileStream stream)
    {
        stream.GetInt();
        stream.GetInt();
    }

    /// <summary>
    /// Checks the usage of models and mounts in the game files.
    /// </summary>
    /// <remarks>
    /// This method checks the usage of models and mounts in the campaign_script.txt and descr_strat.txt files.
    /// It then removes duplicates from the list of used models and mounts.
    /// For each model and mount in the BattleModels and MountDataBase dictionaries that is not used, an error is logged.
    /// </remarks>
    public void CheckModelUsage()
    {
        CheckUseInFile("\\data\\world\\maps\\campaign\\imperial_campaign\\", "campaign_script.txt");
        CheckUseInFile("\\data\\world\\maps\\campaign\\imperial_campaign\\", "descr_strat.txt");
        
        UsedModels = UsedModels.Distinct().ToList();
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
    
    /// <summary>
    /// Checks the usage of models in a specified file.
    /// </summary>
    /// <param name="path">The path of the file to check.</param>
    /// <param name="filename">The name of the file to check.</param>
    /// <remarks>
    /// This method reads the specified file line by line and checks each line for the presence of a model name.
    /// If a model name is found, it is added to the list of used models and its usage is logged.
    /// If a model name is not found in the BattleModels dictionary, an error is logged.
    /// </remarks>
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
            var key = matchString.ToLower().Trim();
            if (BattleModels.ContainsKey(key))
            {
                var isCampaignScript = filename.Contains("campaign_script");
                if (isCampaignScript)
                    BattleModels[key].ModelUsage.CampaignScriptLines.Add(line);
                else
                    BattleModels[key].ModelUsage.DescrStratLines.Add(line);
                UsedModels.Add(matchString.ToLower().Trim());
            }
            else
            {
                ErrorDb.AddError("Model " + matchString + " not found");
            }
        }

        fileStream.LogEnd();
    }
    
    [GeneratedRegex("battle_model\\s+(\\S*)\\s")]
    private static partial Regex BattleModelRegex1();
}