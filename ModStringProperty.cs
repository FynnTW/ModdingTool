namespace ModdingTool;

public class ModStringProperty : IProperty
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Type { get; set; }
    public string DefaultValue { get; set; }
    public string[]? AllowedValues { get; set; }

    public ModStringProperty(string name, string displayName, string type, string defaultValue, string[]? allowedValues)
    {
        Name = name;
        DisplayName = displayName;
        Type = type;
        DefaultValue = defaultValue;
        AllowedValues = allowedValues;
    }
}

public class ModNumberProperty : IProperty
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Type { get; set; }
    public int DefaultValue { get; set; }
    public int[]? AllowedValues { get; set; }
}

public class ModFloatProperty : IProperty
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Type { get; set; }
    public float DefaultValue { get; set; }
    public float[]? AllowedValues { get; set; }
}