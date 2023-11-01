using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using static ModdingTool.Globals;

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
            ExportJson();
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
            ImportJson(filename);
        }

        private void WriteBMDB_Click(object sender, RoutedEventArgs e)
        {
            if (!ModLoaded) return;
            WriteBmdb();
            popupText.Text = System.AppDomain.CurrentDomain.BaseDirectory + "battle_models.modeldb";
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
            if (dialog.FileName != null) ModPath = dialog.FileName;
            LoadMod();
            Print("Mod path set to: " + ModPath);
            ModLoaded = true;
        }

        public void LoadMod()
        {
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
            EduParser.WriteEdu();
            popupText.Text = System.AppDomain.CurrentDomain.BaseDirectory + "export_descr_unit.txt";
            donePopup.IsOpen = true;
        }

        private void ErrorLog_Click(object sender, RoutedEventArgs e)
        {
            var logWindow = new ErrorLog();
            logWindow.Show();
            logWindow.WriteErrors();
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
    }
}
