using System.Collections.Generic;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ModdingTool.ViewModel;

public partial class OptionsViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _useEop;
    [ObservableProperty]
    private bool _addUnitValues;
    [ObservableProperty]
    private bool _addValuePerCost;
    [ObservableProperty]
    private bool _addValuePerUpkeep;

    [ObservableProperty] 
    private List<string> _eopDirectories;
    
    public OptionsViewModel()
    {
        UseEop = Globals.GlobalOptionsInstance.UseEop;
        AddUnitValues = Globals.GlobalOptionsInstance.AddUnitValue;
        AddValuePerCost = Globals.GlobalOptionsInstance.AddUnitValuePerCost;
        AddValuePerUpkeep = Globals.GlobalOptionsInstance.AddUnitValuePerUpkeep;
        EopDirectories = Globals.ModOptionsInstance.EopDirectories;
        if (EopDirectories.Count == 0)
        {
            EopDirectories.Add(" ");
        }
    }

    [RelayCommand]
    public void SaveOptions()
    {
        Globals.GlobalOptionsInstance.UseEop = UseEop;
        Globals.GlobalOptionsInstance.AddUnitValue = AddUnitValues;
        Globals.GlobalOptionsInstance.AddUnitValuePerCost = AddValuePerCost;
        Globals.GlobalOptionsInstance.AddUnitValuePerUpkeep = AddValuePerUpkeep;
        Globals.ModOptionsInstance.EopDirectories = EopDirectories;
        Globals.SaveOptions();
    }

    [RelayCommand]
    public void BrowseDirectories()
    {
        var dialog = new System.Windows.Forms.FolderBrowserDialog();
        dialog.ShowDialog();
        EopDirectories.Add(dialog.SelectedPath);
        
    }
}