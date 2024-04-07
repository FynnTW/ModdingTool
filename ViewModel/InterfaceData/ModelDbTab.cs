using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Python.Runtime;
using static ModdingTool.Globals;

namespace ModdingTool.View.InterfaceData;

public partial class ModelDbTab : Tab
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

    [ObservableProperty] 
    private BattleModel _selectedModel = new();

    [ObservableProperty] 
    private List<string> _factions;

    [ObservableProperty] 
    private float _scale;
    public static IEnumerable<string> MountTypes { get; set; } = BattleModel.MountTypes;
    
    [RelayCommand]
    private void BrowseMesh(Lod lod)
    {
        var dialog = new OpenFileDialog
        {
            Filter = "Mesh files (*.mesh)|*.mesh",
            Title = "Please select a mesh file"
        };
        dialog.ShowDialog();
        var filename = dialog.FileName;
        if (filename == "") return;
        var removeString = ModPath + "\\data\\";
        filename = filename.Replace(removeString, "");
        filename = filename.Replace("\\", "/");
        lod.Mesh = filename;
    }

    public ModelDbTab(string name)
    {
        Factions = FactionDataBase.Keys.ToList();
        Factions.Add("merc");
        Title = name;
        SelectedModel = ModData.BattleModelDb.Get(Title)!;
        Scale = SelectedModel.Scale;
    }
}