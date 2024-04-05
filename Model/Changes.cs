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
    
    public static void AddChange(string attribute, string dataType, string dataId, string oldValue, string newValue)
    {
        ChangeList.Add(new Change()
        {
            Attribute = attribute,
            DataType = dataType,
            DataId = dataId,
            OldValue = oldValue,
            NewValue = newValue,
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
        
        public override string ToString()
        {
            return $@"{Time} - Changed [{DataType} - {DataId}]: [{Attribute}] from {OldValue} to {NewValue}";
        }

        public void Undo()
        {
            dynamic? dataBase = DataType switch
            {
                "Unit" => Globals.UnitDataBase,
                "BattleModel" => Globals.BattleModelDataBase,
                "Lod" => Globals.BattleModelDataBase,
                "Texture" => Globals.BattleModelDataBase,
                "Animation" => Globals.BattleModelDataBase,
                "Mount" => Globals.MountDataBase,
                "Projectile" => Globals.ProjectileDataBase,
                "Faction" => Globals.FactionDataBase,
                "Culture" => Globals.CultureDataBase,
                _ => null
            };
            if (dataBase == null) return;
            dynamic? data = dataBase[DataId];
            if (data == null) return;
            try
            {
                if (DataId is "Lod" or "Texture" or "Animation")
                    return;
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