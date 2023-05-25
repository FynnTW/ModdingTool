using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using static ModdingTool.Globals;

namespace ModdingTool
{
    internal class BmdbParser
    {
        private static int _stringPos;
        private static int _nextLenght;
        private static string _bmdb = null!;

        public static void ParseBmdb()
        {
            Console.WriteLine(@"start parse bmdb");
            _bmdb = File.ReadAllText(ModPath + "\\data\\unit_models\\battle_models.modeldb", Encoding.UTF8);

            _stringPos = 0;
            _stringPos += 35; //serialization

            var numberOfEntries = GetInt();
            // ReSharper disable once NotAccessedVariable
            var pad = GetInt();
            // ReSharper disable once RedundantAssignment
            pad = GetInt();

            for (var n = 0; n < numberOfEntries; n++)
            {

                if (_stringPos >= _bmdb.Length - 1) continue;
                var entry = new BattleModel
                {
                    Name = GetString()
                };
                if (n == 0 && entry.Name.Equals("blank"))
                {
                    for (var i = 0; i < 39; i++)
                    {
                        // ReSharper disable once RedundantAssignment
                        pad = GetInt();
                    }
                    continue;
                }
                entry.Scale = GetFloat();
                entry.LodCount = GetInt();
                entry.LodTable = new List<LOD>();
                for (var i = 0; i < entry.LodCount; i++)
                {
                    entry.LodTable.Add(new LOD
                    {
                        Mesh = GetString(),
                        Distance = GetInt()
                    });
                }
                entry.MainTexturesCount = GetInt();
                for (var i = 0; i < entry.MainTexturesCount; i++)
                {
                    var facname = GetString();
                    entry.MainTextures.Add(new Texture
                    {
                        Faction = facname,
                        TexturePath = GetString(),
                        Normal = GetString(),
                        Sprite = GetString()
                    });
                }
                entry.AttachTexturesCount = GetInt();
                for (var i = 0; i < entry.AttachTexturesCount; i++)
                {
                    var facname = GetString();
                    entry.AttachTextures.Add(new Texture
                    {
                        Faction = facname,
                        TexturePath = GetString(),
                        Normal = GetString(),
                        Sprite = GetString()
                    });
                }
                entry.MountTypeCount = GetInt();
                entry.Animations = new List<Animation>();
                for (var i = 0; i < entry.MountTypeCount; i++)
                {
                    entry.Animations.Add(new Animation
                    {
                        MountType = GetString(),
                        Primary_skeleton = GetString(),
                        Secondary_skeleton = GetString(),
                        PriWeaponCount = GetInt()
                    });
                    entry.Animations[i].PriWeapons = new List<string>();
                    for (var j = 0; j < entry.Animations[i].PriWeaponCount; j++)
                    {
                        entry.Animations[i].PriWeapons.Add(GetString());
                    }
                    entry.Animations[i].SecWeaponCount = GetInt();
                    entry.Animations[i].SecWeapons = new List<string>();
                    for (var j = 0; j < entry.Animations[i].SecWeaponCount; j++)
                    {
                        entry.Animations[i].SecWeapons.Add(GetString());
                    }
                }
                entry.TorchIndex = GetInt();
                entry.TorchBoneX = GetFloat();
                entry.TorchBoneY = GetFloat();
                entry.TorchBoneZ = GetFloat();
                entry.TorchspriteX = GetFloat();
                entry.TorchspriteY = GetFloat();
                entry.TorchspriteZ = GetFloat();
                if (ModelDb.ContainsKey(entry.Name))
                {
                    Console.WriteLine(@"Duplicate entry");
                    Console.WriteLine(@"====================================================================================");
                }
                ModelDb[entry.Name] = entry;
                Console.WriteLine(entry.Name);
            }
            Console.WriteLine(@"end parse bmdb");
        }

        public static string GetString()
        {
            if (_stringPos >= _bmdb.Length - 1) return "";
            AdvancePos();
            var lenght = GetInt();
            _stringPos += GetNextNonWhite();
            if (lenght <= 0) return "";
            var returnString = _bmdb.Substring(_stringPos, lenght).Trim();
            _stringPos += lenght;
            return returnString;
        }

        public static int GetInt()
        {
            if (_stringPos >= _bmdb.Length - 1) return 0;
            AdvancePos();
            var returnValue = int.Parse(_bmdb.Substring(_stringPos, _nextLenght).Trim());
            _stringPos += _nextLenght - 1;
            return returnValue;
        }

        public static float GetFloat()
        {
            if (_stringPos >= _bmdb.Length - 1) return 0;
            AdvancePos();
            if (_stringPos >= _bmdb.Length - 1) return 0;
            var stringLen = _bmdb.Length - 1;
            var len = Math.Min((stringLen) - _stringPos, _nextLenght);
            var substr = _bmdb.Substring(_stringPos, len).Trim();
            var returnValue = float.Parse(substr, CultureInfo.InvariantCulture.NumberFormat);
            _stringPos += _nextLenght - 1;
            return returnValue;
        }

        public static void AdvancePos()
        {
            if (_stringPos >= _bmdb.Length - 1) return;
            _stringPos += GetNextNonWhite();
            var end = _stringPos + GetNextWhite() + 1;
            _nextLenght = end - _stringPos;
        }

        public static int GetNextNonWhite()
        {
            if (_stringPos >= _bmdb.Length - 1) return 0;
            var stringLen = _bmdb.Length - 1;
            var len = Math.Min((stringLen) - _stringPos, 100);
            var subline = _bmdb.Substring(_stringPos, len);
            var match = Regex.Match(subline, @"\S");
            return match.Success ? match.Index : _stringPos;
        }

        public static int GetNextWhite()
        {
            if (_stringPos >= _bmdb.Length - 1) return 0;
            var stringLen = _bmdb.Length - 1;
            var len = Math.Min((stringLen) - _stringPos, 100);
            if (len <= 0) return _stringPos;
            var subline = _bmdb.Substring(_stringPos, len);
            var match = Regex.Match(subline, @"\s");
            return match.Success ? match.Index : _stringPos;
        }
    }
}