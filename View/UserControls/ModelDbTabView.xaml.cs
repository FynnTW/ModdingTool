﻿using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static ModdingTool.Globals;

namespace ModdingTool.View.UserControls
{
    /// <summary>
    /// Interaction logic for ModelDbTabView.xaml
    /// </summary>
    public partial class ModelDbTabView : UserControl
    {
        public ModelDbTabView()
        {
            InitializeComponent();
        }

        private void BrowseMesh_OnClick(object sender, RoutedEventArgs e)
        {
            // Cast the sender as a Button
            Button button = sender as Button;
            if (button != null)
            {
                DataGridRow row = FindVisualParent<DataGridRow>(button);
                if (row != null)
                {
                    if (row.Item is not LOD dataItem)
                    {
                        dataItem = new LOD();
                    };
                    var dialog = new OpenFileDialog();
                    dialog.Filter = "Mesh files (*.mesh)|*.mesh";
                    dialog.Title = "Please select a mesh file";
                    dialog.ShowDialog();
                    var filename = dialog.FileName;
                    if (filename == "") return;
                    string removeString = ModPath + "\\data\\";
                    filename = filename.Replace(removeString, "");
                    filename = filename.Replace("\\", "/");
                    dataItem.Mesh = filename;
                    row.Item = dataItem;
                    var lods = LodGrid.ItemsSource as List<LOD>;
                    lods?.Add(dataItem);
                    LodGrid.ItemsSource = lods;
                    LodGrid.Items.Refresh();
                }
            }
        }

        public static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            UIElement parent = element;
            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }

            return null;
        }

        private void openBrowseTextureDialog(object sender, bool main)
        {
            if (sender is not Button button) return;

            var row = FindVisualParent<DataGridRow>(button);
            var parent = row.Parent as DataGrid;
            var removeString = ModPath + "\\data\\";


            if (row.Item is not Texture dataItem)
            {
                dataItem = new Texture();
            };

            var dialog = new OpenFileDialog
            {
                Filter = "Texture files (*.texture)|*.texture",
                Title = "Please select a texture file"
            };
            dialog.ShowDialog();
            var filename = dialog.FileName;

            var dialogNormal = new OpenFileDialog
            {
                Filter = "Texture files (*.texture)|*.texture",
                Title = "Please select a normal texture file"
            };
            dialogNormal.ShowDialog();
            var filenameNormal = dialogNormal.FileName;

            var dialogSprite = new OpenFileDialog
            {
                Filter = "Sprite files (*.spr)|*.spr",
                Title = "Please select a sprite"
            };
            dialogSprite.ShowDialog();
            var filenameSprite = dialogSprite.FileName;

            if (filename != "")
            {
                filename = filename.Replace(removeString, "");
                filename = filename.Replace("\\", "/");
                dataItem.TexturePath = filename;
            }
            if (filenameNormal != "")
            {
                filenameNormal = filenameNormal.Replace(removeString, "");
                filenameNormal = filenameNormal.Replace("\\", "/");
                dataItem.Normal = filenameNormal;
            }
            if (filenameSprite != "")
            {
                filenameSprite = filenameSprite.Replace(removeString, "");
                filenameSprite = filenameSprite.Replace("\\", "/");
                dataItem.Sprite = filenameSprite;
            }
            row.Item = dataItem;
            if (main)
            {
                var mainTextures = MainTextureGrid.ItemsSource as List<Texture>;
                mainTextures?.Add(dataItem);
                MainTextureGrid.ItemsSource = mainTextures;
            }
            else
            {
                var attachTextures = AttachTextureGrid.ItemsSource as List<Texture>;
                attachTextures?.Add(dataItem);
                AttachTextureGrid.ItemsSource = attachTextures;
            }
            MainTextureGrid.Items.Refresh();
            AttachTextureGrid.Items.Refresh();
        }

        private void BrowseTexture_OnClick(object sender, RoutedEventArgs e)
        {
            openBrowseTextureDialog(sender, true);
        }
        private void BrowseTextureAttach_OnClick(object sender, RoutedEventArgs e)
        {
            openBrowseTextureDialog(sender, false);
        }

        private void AnimationGrid_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid == null) return;
            foreach (var item in dataGrid.Items)
            {
                if (item is not Animation animation) continue;
                animation.PriWeapons ??= new List<string>();
                animation.SecWeapons ??= new List<string>();
            }
        }
    }
}
