using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static ModdingTool.Globals;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Windows.Shapes;
using Microsoft.VisualBasic;
using System.Windows.Media.Media3D;

namespace ModdingTool
{
    internal class BMDBParser
    {

        private static int stringPos;
        private static int nextLenght;
        private static string bmdb;
        public static Dictionary<string, BattleModel> ModelDB = new Dictionary<string, BattleModel>();


        public static void parseBMDB()
        {
            _log.Info("start parse bmdb");
            bmdb = File.ReadAllText(modPath + "\\data\\unit_models\\battle_models.modeldb", Encoding.UTF8);

            stringPos = 0;
            stringPos += 35; //serialization

            int numberOfEntries = getInt();
            int pad = getInt();
            pad = getInt();

            for (int n = 0; n < numberOfEntries; n++)
            {
                BattleModel entry = new BattleModel();
                entry.Name = getString();
                entry.Scale = getFloat();
                entry.LodCount= getInt();
                entry.LodTable = new LOD[entry.LodCount];
                for (int i = 0; i < entry.LodCount;i++)
                {
                    entry.LodTable[i] = new LOD();
                    entry.LodTable[i].Mesh = getString();
                    entry.LodTable[i].Distance = getInt();
                }
                entry.MainTexturesCount = getInt();
                for (int i = 0; i < entry.MainTexturesCount; i++)
                {
                    string facname = getString();
                    entry.MainTextures[facname] = new Texture();
                    entry.MainTextures[facname].TexturePath = getString();
                    entry.MainTextures[facname].Normal = getString();
                    entry.MainTextures[facname].Sprite = getString();
                }
                entry.AttachTexturesCount = getInt();
                for (int i = 0; i < entry.AttachTexturesCount; i++)
                {
                    string facname = getString();
                    entry.AttachTextures[facname] = new Texture();
                    entry.AttachTextures[facname].TexturePath = getString();
                    entry.AttachTextures[facname].Normal = getString();
                    entry.AttachTextures[facname].Sprite = getString();
                }
                entry.MountTypeCount= getInt();
                entry.Animations = new Animation[entry.MountTypeCount];
                for (int i = 0; i < entry.MountTypeCount; i++)
                {
                    entry.Animations[i] = new Animation();
                    entry.Animations[i].MountType = getString();
                    entry.Animations[i].Primary_skeleton = getString();
                    entry.Animations[i].Secondary_skeleton = getString();
                    entry.Animations[i].PriWeaponCount = getInt();
                    entry.Animations[i].PriWeapons = new string[entry.Animations[i].PriWeaponCount];
                    for (int j = 0; j < entry.Animations[i].PriWeaponCount; j++)
                    {
                        entry.Animations[i].PriWeapons[j] = getString();
                    }
                    entry.Animations[i].SecWeaponCount = getInt();
                    entry.Animations[i].SecWeapons = new string[entry.Animations[i].SecWeaponCount];
                    for (int j = 0; j < entry.Animations[i].SecWeaponCount; j++)
                    {
                        entry.Animations[i].SecWeapons[j] = getString();
                    }
                }
                entry.TorchIndex = getInt();
                entry.TorchBoneX = getFloat();
                entry.TorchBoneY = getFloat();
                entry.TorchBoneZ = getFloat();
                entry.TorchspriteX = getFloat();
                entry.TorchspriteY = getFloat();
                entry.TorchspriteZ = getFloat();
                ModelDB[entry.Name] = entry;
                _log.Info(entry.Name);

            }
            _log.Info("end parse bmdb");

        }

        public static string getString()
        {
            advancePos();
            int lenght = getInt();
            stringPos += getNextNonWhite();
            if (lenght > 0)
            {
                string returnString = bmdb.Substring(stringPos, lenght).Trim();
                stringPos += lenght;
                return returnString;
            }
            return "";
        }

        public static int getInt()
        {
            advancePos();
            int returnValue = int.Parse(bmdb.Substring(stringPos, nextLenght).Trim());
            stringPos += nextLenght - 1;
            return returnValue;
        }

        public static float getFloat()
        {
            advancePos();
            int stringLen = bmdb.Length - 1;
            int len = Math.Min((bmdb.Length - 1) - stringPos, nextLenght);
            float returnValue = float.Parse(bmdb.Substring(stringPos, len).Trim());
            stringPos += nextLenght - 1;
            return returnValue;
        }

        public static void advancePos()
        {
            stringPos += getNextNonWhite();
            int end = stringPos + getNextWhite() + 1;
            nextLenght =  end - stringPos;
        }

        public static int getNextNonWhite()
        {
            int stringLen = bmdb.Length - 1;
            int len = Math.Min((bmdb.Length - 1) - stringPos, 100);
            string subline = bmdb.Substring(stringPos, len);
            var match = Regex.Match(subline, @"\S");
            if (match.Success)
            {
                return match.Index;
            }
            else
            {
                return stringPos;
            }
        }
        public static int getNextWhite()
        {
            int stringLen = bmdb.Length - 1;
            int len = Math.Min((bmdb.Length - 1) - stringPos, 100);
            string subline = bmdb.Substring(stringPos, len);
            var match = Regex.Match(subline, @"\s");
            if (match.Success)
            {
                return match.Index;
            }
            else
            {
                return stringPos;
            }
        }


    }
}
