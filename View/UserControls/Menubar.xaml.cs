using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using ModToolLib;
using ModdingTool.API;
using static ModdingTool.Globals;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using UserControl = System.Windows.Controls.UserControl;

namespace ModdingTool.View.UserControls
{
    /// <summary>
    /// Interaction logic for Menubar.xaml
    /// </summary>
    public partial class Menubar : UserControl
    {
        public string exportedFilePath { get; set; }
        public Menubar()
        {
            InitializeComponent();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            if (!ModLoaded) return;
            popupText.Text = System.AppDomain.CurrentDomain.BaseDirectory + "battle_models.json";
            ModData.BattleModelDb.ExportJson();
            donePopup.IsOpen = true;
        }

        private void openViewer_Click(object sender, RoutedEventArgs e)
        {
            //var view = new UnitViewer();
            //view.Show();
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            if (!ModLoaded) return;
            var dialog = new OpenFileDialog
            {
                Filter = "Json files (*.json)|*.json",
                InitialDirectory = @"C:\",
                Title = "Please select a file to import"
            };
            dialog.ShowDialog();
            var filename = dialog.FileName;
            if (filename == "") return;
            ModData.BattleModelDb.ImportJson(filename);
        }

        private void WriteBMDB_Click(object sender, RoutedEventArgs e)
        {
            if (!ModLoaded) return;
            ModData.BattleModelDb.WriteFile();
            popupText.Text = ModPath + @"/data/unit_models/battle_models.modeldb";
            donePopup.IsOpen = true;
        }

        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            donePopup.IsOpen = false;
        }

        private void loadMod_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            var result = dialog.ShowDialog();
            if (result != CommonFileDialogResult.Ok) return;
            if (dialog.FileName != null) 
                SetModPath(dialog.FileName);
            LoadMod();
            Print("Mod path set to: " + ModPath);
            ModLoaded = true;
        }

        public void LoadMod()
        {
            LoadOptions();
            ParseFiles();
            var window = Window.GetWindow(this);
            if (window != null)
            {
                var statusBar = window.FindName("StatusBarLive") as StatusBarCustom;
                statusBar?.SetStatusModPath(ModPath);
            }
            var dataPickerBox = window?.FindName("DataListLive") as DataList;
            ModLoadedTrigger();
        }

        private void WriteEDU_Click(object sender, RoutedEventArgs e)
        {
            if (!ModLoaded) return;
            ModData.Units.WriteFile();
            popupText.Text = ModPath + @"/data/export_descr_unit.txt";
            donePopup.IsOpen = true;
        }

        private void ErrorLog_Click(object sender, RoutedEventArgs e)
        {
            var logWindow = new ErrorLog();
            logWindow.Show();
            logWindow.WriteErrors();
            var classs = new Class1();
            Print(classs.Add(1, 1).ToString());
        }

        private void ChangeLog_Click(object sender, RoutedEventArgs e)
        {
            var changeWindow = new ChangesWindow();
            changeWindow.Show();
        }
        
        private void ClosePopupOpen_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo(popupText.Text) { UseShellExecute = true });
            donePopup.IsOpen = false;
        }

        private void Open_GitHub_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo("https://github.com/FynnTW/ModdingTool#readme") { UseShellExecute = true });
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            var optionsWindow = new OptionsWindow();
            optionsWindow.Show();
        }

        private void RunScript_Click(object sender, RoutedEventArgs e)
        {
            
            var dialog = new OpenFileDialog
            {
                Filter = "Lua script files (*.lua)|*.lua",
                Title = "Please select a LUA script file"
            };
            dialog.ShowDialog();
            var filename = dialog.FileName;
            LuaAPI.ExecuteLuaScript(filename);
        }

        /*
        private void RunScriptPy_Click(object sender, RoutedEventArgs e)
        {
            PythonAPI.RunPythonScript();
        }
        */
    }
}
