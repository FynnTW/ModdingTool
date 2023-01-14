using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;
using Pfim;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Windows.Media.Imaging;

namespace ModdingTool
{
    /// <summary>
    /// Interaction logic for unitViewer.xaml
    /// </summary>
    public partial class unitViewer : Window
    {

        public ObservableCollection<string> unitList { get; set; }

        Unit selectedUnit = EDUParser.allUnits["Sellswords"];
        public unitViewer()
        {
            InitializeComponent();
            this.unitList = new ObservableCollection<string>(EDUParser.allUnits.Keys.ToList());
            this.DataContext = this;
        }

        private void unitPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string strItem = unitPicker.SelectedItem as string;
            selectedUnit = EDUParser.allUnits[strItem];
            infoCardImage.Source = tgaToImageSource(selectedUnit.Unit_cardInfo);
            unitCardImage.Source = tgaToImageSource(selectedUnit.Unit_card);
            localName.Text = selectedUnit.Unit_name;
            unitDescr.Text = selectedUnit.Unit_descr;
            unitShortDescr.Text = selectedUnit.Unit_descr;
            unitCategory.Text = selectedUnit.Unit_category;
            unitClass.Text = selectedUnit.Unit_class;
            unitVoice.Text = selectedUnit.Unit_voice_type;
            unitAccent.Text = selectedUnit.Unit_accent;
            unitSoldier.Text = selectedUnit.Unit_soldier;
            unitSoldierCount.Text = selectedUnit.Unit_soldierCount + "";
            unitExtrasCount.Text = selectedUnit.Unit_extrasCount + "";
            unitFormationWidth.Text = selectedUnit.Unit_spacing_width + "";
            unitRadius1.Text = selectedUnit.Unit_radius + "";
            unitHeight1.Text = selectedUnit.Unit_height + "";
            unitOfficer1.Text = selectedUnit.Unit_officer1 + "";
            unitOfficer2.Text = selectedUnit.Unit_officer2 + "";
            unitOfficer3.Text = selectedUnit.Unit_officer3 + "";
            unitShip.Text = selectedUnit.Unit_ship + "";
            unitEngine.Text = selectedUnit.Unit_engine + "";
            unitAnimal.Text = selectedUnit.Unit_animal + "";
            unitMount.Text = selectedUnit.Unit_mount + "";
            unitMountHP.Text = selectedUnit.Unit_mount_hitpoints + "";
            unitFormationHeight.Text = selectedUnit.Unit_spacing_depth + "";
            unitFormationWidthLoose.Text = selectedUnit.Unit_spacing_width_loose + "";
            unitFormationHeightLoose.Text = selectedUnit.Unit_spacing_depth_loose + "";
            unitMoveSpeed.Text = selectedUnit.Unit_moveSpeed + "";
            unitHitPoints.Text = selectedUnit.Unit_hitpoints + "";
            unitFormationStyle.Text = selectedUnit.Unit_formation_style + "";
            unitFormationRanks.Text = selectedUnit.Unit_spacing_ranks + "";
            unitSpecialFormation.Text = selectedUnit.Unit_special_formation + "";
            unitMass.Text = selectedUnit.Unit_mass + "";
            unitPriAttack.Text = selectedUnit.Unit_pri_attack + "";
            unitPriCharge.Text = selectedUnit.Unit_pri_charge + "";
            unitPriProjectile.Text = selectedUnit.Unit_pri_projectile + "";
            unitPriRange.Text = selectedUnit.Unit_pri_range + "";
            unitPriAmmo.Text = selectedUnit.Unit_pri_ammunition + "";
            unitPriFiringSound.Text = selectedUnit.Unit_pri_fire_type + "";
            unitSecAttack.Text = selectedUnit.Unit_sec_attack + "";
            unitSecCharge.Text = selectedUnit.Unit_sec_charge + "";
            unitSecProjectile.Text = selectedUnit.Unit_sec_projectile + "";
            unitSecRange.Text = selectedUnit.Unit_sec_range + "";
            unitSecAmmo.Text = selectedUnit.Unit_sec_ammunition + "";
            unitSecFiringSound.Text = selectedUnit.Unit_sec_fire_type + "";
            unitTerAttack.Text = selectedUnit.Unit_ter_attack + "";
            unitTerCharge.Text = selectedUnit.Unit_ter_charge + "";
            unitTerProjectile.Text = selectedUnit.Unit_ter_projectile + "";
            unitTerRange.Text = selectedUnit.Unit_ter_range + "";
            unitTerAmmo.Text = selectedUnit.Unit_ter_ammunition + "";
            unitTerFiringSound.Text = selectedUnit.Unit_ter_fire_type + "";
            unitPriArmour.Text = selectedUnit.Unit_pri_armour + "";
            unitPriDefense.Text = selectedUnit.Unit_pri_defense + "";
            unitPriShield.Text = selectedUnit.Unit_pri_shield + "";


        }

        BitmapImage tgaToImageSource(string source)
        {
            Bitmap bitmap;
            IImage image = Pfimage.FromFile(source);
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

                default:
                    var msg = $"{image.Format} is not recognized for Bitmap on Windows Forms. " +
                               "You'd need to write a conversion function to convert the data to known format";
                    var caption = "Unrecognized format";
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
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
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
