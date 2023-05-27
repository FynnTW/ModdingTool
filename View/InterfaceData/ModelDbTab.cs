using System;
using System.Collections.Generic;
using System.Linq;
using static ModdingTool.Globals;

namespace ModdingTool.View.InterfaceData;

public class ModelDbTab : ITab.Tab
{
    public static Dictionary<string, string> BmdbUiText { get; set; } = new Dictionary<string, string>()
    {
        {"Name", "Name"},
        {"Scale", "Scale"},
        {"LodCount", "LOD Count: "},
        {"Mesh", "Mesh Path"},
        {"Distance", "LOD Distance"},
        {"Faction", "Faction"},
        {"TexturePath", "Texture Path"},
        {"Normal", "Normal Texture Path"},
        {"Sprite", "Sprite Path"},
        {"MountType", "Mount Type"},
        {"Primary_skeleton", "Primary Animation"},
        {"Secondary_skeleton", "Secondary Animation"},
        {"PriWeaponCount", "Primary Weapon Count"},
        {"PriWeapons", "Primary Weapon Animations"},
        {"SecWeaponCount", "Secondary Weapon Count"},
        {"SecWeapons", "Secondary Weapon Animations"},
        {"MainTexturesCount", "Main Textures Count"},
        {"AttachTexturesCount", "Attachment Textures Count"},
        {"TorchIndex", "Torch Bone Index"},
        {"TorchBoneX", "Torch Offset X"},
        {"TorchBoneY", "Torch Offset Y"},
        {"TorchBoneZ", "Torch Offset Z"},
        {"TorchspriteX", "Torch Sprite X"},
        {"TorchspriteY", "Torch Sprite Y"},
        {"TorchspriteZ", "Torch Sprite Z"},
    };
    public BattleModel SelectedModel { get; set; }
    public static string[] Factions { get; set; }
    public static string[] MountTypes { get; set; } = new string[] { "horse", "none", "elephant", "camel" };

    public ModelDbTab(string name)
    {
        Title = name;
        SelectedModel = ModelDb[Title];
        Factions = FactionDataBase.Keys.ToArray();
        Factions.Append("merc");
    }
}