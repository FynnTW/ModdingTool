﻿using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using static ModdingTool.Globals;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ModdingTool.View.UserControls
{
    /// <summary>
    /// Interaction logic for Menubar.xaml
    /// </summary>
    public partial class Menubar : UserControl
    {
        public Menubar()
        {
            InitializeComponent();
        }



        private void Export_Click(object sender, RoutedEventArgs e)
        {
            if (!ModLoaded) return;
            ExportJson();
        }

        private void openViewer_Click(object sender, RoutedEventArgs e)
        {
            //var view = new UnitViewer();
            //view.Show();
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            if (!ModLoaded) return;
            var dialog = new OpenFileDialog();
            dialog.Filter = "Json files (*.json)|*.json";
            dialog.InitialDirectory = @"C:\";
            dialog.Title = "Please select a file to import";
            dialog.ShowDialog();
            var filename = dialog.FileName;
            if (filename == "") return;
            ImportJson(filename);
        }

        private void WriteBMDB_Click(object sender, RoutedEventArgs e)
        {
            if (!ModLoaded) return;
            WriteBMDB();
        }

        private void loadMod_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            var result = dialog.ShowDialog();
            if (result != CommonFileDialogResult.Ok) return;
            ModPath = dialog.FileName;
            loadMod();
            Print("Mod path set to: " + ModPath);
            ModLoaded = true;
        }

        private void loadMod()
        {
            parseFiles();
            Window window = Window.GetWindow(this);
            var statusBar = window.FindName("StatusBarLive") as StatusBarCustom;
            statusBar.SetStatusModPath(ModPath);
            var dataPickerBox = window.FindName("DataListLive") as DataList;
            dataPickerBox.InitItems();
        }

        private void parseFiles()
        {
            BmdbParser.ParseBmdb();
            factionParser.parseSMFactions();
            EduParser.ParseEdu();
        }
    }
}
