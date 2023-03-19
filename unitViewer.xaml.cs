using Pfim;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ImageFormat = Pfim.ImageFormat;
using static ModdingTool.Globals;

namespace ModdingTool
{
    /// <summary>
    /// Interaction logic for unitViewer.xaml
    /// </summary>
    public partial class UnitViewer : Window
    {
        public ObservableCollection<string?> UnitList { get; set; }

        private Unit selectedUnit = AllUnits["Sellswords"];

        public UnitViewer()
        {
            InitializeComponent();
            this.UnitList = new ObservableCollection<string?>(AllUnits.Keys.ToList());
            this.DataContext = this;
        }

        private void unitPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (unitPicker.SelectedItem is string strItem) selectedUnit = AllUnits[strItem];
            infoCardImage.Source = TgaToImageSource(selectedUnit.CardInfo);
            unitCardImage.Source = TgaToImageSource(selectedUnit.Card);
            localName.Text = selectedUnit.Name;
            unitDescr.Text = selectedUnit.Descr;
            unitShortDescr.Text = selectedUnit.Descr;
            unitCategory.Text = selectedUnit.Category;
            unitClass.Text = selectedUnit.Class_type;
            unitVoice.Text = selectedUnit.Voice_type;
            unitAccent.Text = selectedUnit.Accent;
            unitSoldier.Text = selectedUnit.Soldier;
            unitSoldierCount.Text = selectedUnit.SoldierCount + "";
            unitExtrasCount.Text = selectedUnit.ExtrasCount + "";
            unitFormationWidth.Text = selectedUnit.Spacing_width + "";
            unitRadius1.Text = selectedUnit.Radius + "";
            unitHeight1.Text = selectedUnit.Height + "";
            unitOfficer1.Text = selectedUnit.Officer1 + "";
            unitOfficer2.Text = selectedUnit.Officer2 + "";
            unitOfficer3.Text = selectedUnit.Officer3 + "";
            unitShip.Text = selectedUnit.Ship + "";
            unitEngine.Text = selectedUnit.Engine + "";
            unitAnimal.Text = selectedUnit.Animal + "";
            unitMount.Text = selectedUnit.Mount + "";
            unitMountHP.Text = selectedUnit.Mount_hitpoints + "";
            unitFormationHeight.Text = selectedUnit.Spacing_depth + "";
            unitFormationWidthLoose.Text = selectedUnit.Spacing_width_loose + "";
            unitFormationHeightLoose.Text = selectedUnit.Spacing_depth_loose + "";
            unitMoveSpeed.Text = selectedUnit.MoveSpeed + "";
            unitHitPoints.Text = selectedUnit.Hitpoints + "";
            unitFormationStyle.Text = selectedUnit.Formation_style + "";
            unitFormationRanks.Text = selectedUnit.Spacing_ranks + "";
            unitSpecialFormation.Text = selectedUnit.Special_formation + "";
            unitMass.Text = selectedUnit.Mass + "";
            unitPriAttack.Text = selectedUnit.Pri_attack + "";
            unitPriCharge.Text = selectedUnit.Pri_charge + "";
            unitPriProjectile.Text = selectedUnit.Pri_projectile + "";
            unitPriRange.Text = selectedUnit.Pri_range + "";
            unitPriAmmo.Text = selectedUnit.Pri_ammunition + "";
            unitPriFiringSound.Text = selectedUnit.Pri_fire_type + "";
            unitSecAttack.Text = selectedUnit.Sec_attack + "";
            unitSecCharge.Text = selectedUnit.Sec_charge + "";
            unitSecProjectile.Text = selectedUnit.Sec_projectile + "";
            unitSecRange.Text = selectedUnit.Sec_range + "";
            unitSecAmmo.Text = selectedUnit.Sec_ammunition + "";
            unitSecFiringSound.Text = selectedUnit.Sec_fire_type + "";
            unitTerAttack.Text = selectedUnit.Ter_attack + "";
            unitTerCharge.Text = selectedUnit.Ter_charge + "";
            unitTerProjectile.Text = selectedUnit.Ter_projectile + "";
            unitTerRange.Text = selectedUnit.Ter_range + "";
            unitTerAmmo.Text = selectedUnit.Ter_ammunition + "";
            unitTerFiringSound.Text = selectedUnit.Ter_fire_type + "";
            unitPriArmour.Text = selectedUnit.Pri_armour + "";
            unitPriDefense.Text = selectedUnit.Pri_defense + "";
            unitPriShield.Text = selectedUnit.Pri_shield + "";
        }

        private static BitmapImage TgaToImageSource(string source)
        {
            Bitmap bitmap;
            var image = Pfimage.FromFile(source);
            PixelFormat format;
            switch (image.Format)
            {
                case Pfim.ImageFormat.Rgb24:
                    format = PixelFormat.Format24bppRgb;
                    break;

                case Pfim.ImageFormat.Rgba32:
                    format = PixelFormat.Format32bppArgb;
                    break;

                case Pfim.ImageFormat.R5g5b5:
                    format = PixelFormat.Format16bppRgb555;
                    break;

                case Pfim.ImageFormat.R5g6b5:
                    format = PixelFormat.Format16bppRgb565;
                    break;

                case Pfim.ImageFormat.R5g5b5a1:
                    format = PixelFormat.Format16bppArgb1555;
                    break;

                case Pfim.ImageFormat.Rgb8:
                    format = PixelFormat.Format8bppIndexed;
                    break;

                case ImageFormat.Rgba16:
                default:
                    var msg = $"{image.Format} is not recognized for Bitmap on Windows Forms. " +
                               "You'd need to write a conversion function to convert the data to known format";
                    const string caption = "Unrecognized format";
                    MessageBox.Show(msg, caption);
                    format = PixelFormat.Format32bppArgb;
                    break;
            }
            var handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
            try
            {
                var data = Marshal.UnsafeAddrOfPinnedArrayElement(image.Data, 0);
                bitmap = new Bitmap(image.Width, image.Height, image.Stride, format, data);
                bitmap.MakeTransparent();
            }
            finally
            {
                handle.Free();
            }

            using var memory = new MemoryStream();
            bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
            memory.Position = 0;
            var bitmapimage = new BitmapImage();
            bitmapimage.BeginInit();
            bitmapimage.StreamSource = memory;
            bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapimage.EndInit();

            return bitmapimage;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {
        }
    }
}