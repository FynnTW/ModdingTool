using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.WindowsAPICodePack.Dialogs;
using ModdingTool.API;
using ModdingTool.View;
using ModdingTool.View.UserControls;
using static ModdingTool.Globals;

namespace ModdingTool.ViewModel;

public partial class MenuBarViewModel : ObservableObject
{
    [ObservableProperty]
    private string _exportedFilePath = string.Empty;
    
    
    
    [RelayCommand]
    private static async Task OnOpenCustomMessageBox(object sender)
    {
        var uiMessageBox = new Wpf.Ui.Controls.MessageBox
        {
            Title = "WPF UI Message Box",
            Content =
                "Never gonna give you up, never gonna let you down Never gonna run around and desert you Never gonna make you cry, never gonna say goodbye",
        };

        var result = await uiMessageBox.ShowDialogAsync();
    }
    
    [RelayCommand]
    private static void ExportJson()
    {
        if (!ModLoaded) return;
        var file = "";
        switch (OpenListType)
        {
            case "Units":
                ModData.Units.ExportJson();
                file = "edu.json";
                break;
            case "Model Entries":
                ModData.BattleModelDb.ExportJson();
                file = "bmdb.json";
                break;
            default:
                new ToastContentBuilder().AddText("Data type Not implemented!").Show();
                return;
        }
        var uiMessageBox = new Wpf.Ui.Controls.MessageBox
        {
            Title = "Done!",
            Content = System.AppDomain.CurrentDomain.BaseDirectory + file,
            IsPrimaryButtonEnabled = true,
            PrimaryButtonText = "Open",
        };
        var result = uiMessageBox.ShowDialogAsync();
        if (result.Result == Wpf.Ui.Controls.MessageBoxResult.Primary)
            Process.Start(new ProcessStartInfo(System.AppDomain.CurrentDomain.BaseDirectory + file) { UseShellExecute = true });
    }
    
    [RelayCommand]
    private static void SaveFile()
    {
        if (!ModLoaded) return;
        switch (OpenListType)
        {
            case "Units":
                ModData.Units.WriteFile();
                break;
            case "Model Entries":
                ModData.BattleModelDb.WriteFile();
                break;
            default:
                new ToastContentBuilder().AddText("Data type Not implemented!").Show();
                break;
        }
    }
    
    [RelayCommand]
    private static void LoadFile()
    {
        if (!ModLoaded) return;
        IsParsing = true;
        switch (OpenListType)
        {
            case "Units":
                ModData.Units.ParseFile();
                break;
            case "Model Entries":
                ModData.BattleModelDb.ParseFile();
                break;
            default:
                new ToastContentBuilder().AddText("Data type Not implemented!").Show();
                break;
        }
        IsParsing = false;
    }
        
    [RelayCommand]
    private static void OpenErrorLog()
    {
        var logWindow = new ErrorLog();
        logWindow.Show();
        logWindow.WriteErrors();
    }

    [RelayCommand]
    private static void OpenOptions()
    {
        var optionsWindow = new OptionsWindow();
        optionsWindow.Show();
    }

    [RelayCommand]
    private static void RunScript()
    {
            
        var dialog = new OpenFileDialog
        {
            Filter = @"Lua script files (*.lua)|*.lua",
            Title = @"Please select a LUA script file"
        };
        dialog.ShowDialog();
        var filename = dialog.FileName;
        LuaAPI.ExecuteLuaScript(filename);
    }

    [RelayCommand]
    private static void OpenChangeLog()
    {
        var changeWindow = new ChangesWindow();
        changeWindow.Show();
    }

    [RelayCommand]
    private static void OpenGitHub() =>
        Process.Start(new ProcessStartInfo("https://github.com/FynnTW/ModdingTool#readme") { UseShellExecute = true });
    
    [RelayCommand]
    private static void ImportJson()
    {
        if (!ModLoaded) return;
        var dialog = new OpenFileDialog
        {
            Filter = @"Json files (*.json)|*.json",
            InitialDirectory = ModPath,
            Title = $@"Please select a {OpenListType} file to import"
        };
        dialog.ShowDialog();
        var filename = dialog.FileName;
        if (string.IsNullOrWhiteSpace(filename)) return;
        switch (OpenListType)
        {
            case "Units":
                ModData.Units.ImportJson(filename);
                break;
            case "Model Entries":
                ModData.BattleModelDb.ImportJson(filename);
                break;
            default:
                new ToastContentBuilder().AddText("Data type Not implemented!").Show();
                break;
        }
    }
    
    [RelayCommand]
    private static void LoadMod()
    {
        var dialog = new CommonOpenFileDialog();
        dialog.IsFolderPicker = true;
        var result = dialog.ShowDialog();
        if (result != CommonFileDialogResult.Ok) return;
        if (dialog.FileName != null) 
            SetModPath(dialog.FileName);
        Globals.LoadMod();
        ModLoaded = true;
    }
}