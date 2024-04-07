using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace ModdingTool;

public static class Changes
{
    private static List<Change> ChangeList { get; set; } = new();
    
    public static List<Change> GetChanges()
    {
        return ChangeList;
    }
    
    public static void ClearChanges()
    {
        ChangeList.Clear();
    }
    
    public static List<string> GetChangesString()
    {
        return new List<string>(ChangeList.ConvertAll(x => x.ToString()));
    }
    
    public static void AddChange(string attribute, string dataType, string dataId, string oldValue, string newValue, string listIndex = "", string listType = "")
    {
        ChangeList.Add(new Change()
        {
            Attribute = attribute,
            DataType = dataType,
            DataId = dataId,
            OldValue = oldValue,
            NewValue = newValue,
            ListIndex = listIndex,
            ListType = listType,
            IsList = !string.IsNullOrWhiteSpace(listIndex),
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        });
    }

    public class Change
    {
        public string Attribute { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public string DataId { get; set; } = string.Empty;
        public string OldValue { get; set; } = string.Empty;
        public string NewValue { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public string ListIndex { get; set; } = string.Empty;
        public string ListType { get; set; } = string.Empty;
        public bool IsList { get; set; } = false;
        
        public override string ToString()
        {
            return IsList ? 
                $@"{Time} - Changed [{DataType} - {DataId}]: [{Attribute}] from {OldValue} to {NewValue} at index {ListIndex} in list {ListType}" : 
                $@"{Time} - Changed [{DataType} - {DataId}]: [{Attribute}] from {OldValue} to {NewValue}";
        }

        public void Undo()
        {
            dynamic? dataBase = DataType switch
            {
                "Unit" => Globals.ModData.Units.GetUnits(),
                "BattleModel" => Globals.ModData.BattleModelDb.GetBattleModels(),
                "Lod" => Globals.ModData.BattleModelDb.GetBattleModels(),
                "Texture" => Globals.ModData.BattleModelDb.GetBattleModels(),
                "Animation" => Globals.ModData.BattleModelDb.GetBattleModels(),
                "Mount" => Globals.MountDataBase,
                "Projectile" => Globals.ProjectileDataBase,
                "Faction" => Globals.FactionDataBase,
                "Culture" => Globals.CultureDataBase,
                _ => null
            };
            if (dataBase == null) return;
            if (string.IsNullOrWhiteSpace(DataId))
            {
                Debug.WriteLine("DataId is null or empty");
                return;
            }
            dynamic? data = dataBase[DataId];
            if (data == null) return;
            try
            {
                if (DataType is "Lod" or "Texture" or "Animation")
                {
                    switch (DataType)
                    {
                        case "Lod":
                            if (Globals.ModData.BattleModelDb.Get(DataId) == null) return;
                            data = Globals.ModData.BattleModelDb.Get(DataId)!.LodTable[int.Parse(ListIndex)];
                            break;
                        case "Texture":
                            return;
                            break;
                        case "Animation":
                            return;
                            break;
                    }
                }
                Type type = data.GetType();
                if (type == null) return;
                var property = type.GetProperty(Attribute);
                if (property != null)
                    property.SetValue(data, Convert.ChangeType(OldValue, property.PropertyType));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}