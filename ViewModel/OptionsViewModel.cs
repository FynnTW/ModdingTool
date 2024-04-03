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
    private List<string> _eopDirectories;
    
    public OptionsViewModel()
    {
        UseEop = Globals.GlobalOptionsInstance.UseEop;
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