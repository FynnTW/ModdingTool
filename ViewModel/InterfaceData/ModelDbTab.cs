using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Python.Runtime;
using static ModdingTool.Globals;

namespace ModdingTool.ViewModel.InterfaceData;

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

    [ObservableProperty] 
    private bool _isMeshPopupOpen;

    [ObservableProperty] 
    private string _inputMeshPath;

    [ObservableProperty] 
    private string _inputMeshRange;
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

    [RelayCommand]
    private void AddMesh()
    {
        if (string.IsNullOrWhiteSpace(InputMeshPath)) return;
        if (string.IsNullOrWhiteSpace(InputMeshRange)) return;
        int range;
        try
        {
            range = int.Parse(InputMeshRange);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            range = 6400;
        }
        SelectedModel.AddLod(InputMeshPath, range, SelectedModel.LodCount, SelectedModel.Name);
        IsMeshPopupOpen = false;
    }

    [RelayCommand]
    private void CancelAddMesh()
    {
        IsMeshPopupOpen = false;
    }

    [RelayCommand]
    private void OpenMeshPopup()
    {
        IsMeshPopupOpen = true;
        if (SelectedModel.LodCount <= 0) return;
        InputMeshRange = (SelectedModel.LodTable[SelectedModel.LodCount - 1].Distance * 2).ToString();
        var oldLodString = "lod" + (SelectedModel.LodCount - 1);
        var newLodString = "lod" + SelectedModel.LodCount;
        InputMeshPath = SelectedModel.LodTable[SelectedModel.LodCount - 1].Mesh.Replace(oldLodString, newLodString);
    }

    public ModelDbTab(string name)
    {
        Factions = FactionDataBase.Keys.ToList();
        Factions.Add("merc");
        SelectedModel = ModData.BattleModelDb.Get(name)!;
        Title = name;
        SelectedModel.PropertyChanged += SelectedModel_PropertyChanged;
        IsMeshPopupOpen = false;
        InputMeshRange = "6400";
        InputMeshPath = "";
    }

    private void SelectedModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Name")
        {
            Title = SelectedModel.Name;
        }
    }
}