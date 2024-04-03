using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using static ModdingTool.Globals;
using static ModdingTool.ParseHelpers;

namespace ModdingTool
{
    internal class BmdbParser
    {
        private static string _fileName = "";

        //Position in Bmdb
        private static int _stringPos;

        //Lenght of next string
        private static int _nextLenght;
        private static int _lineNum;

        //bmdb loaded as string
        private static string? _bmdb = null!;

        public static void ParseBmdb()
        {
            _fileName = "battle_models.modeldb";
            Console.WriteLine($@"start parse {_fileName}");

            //Try read file
            _bmdb = BmdbReader("\\data\\unit_models\\battle_models.modeldb", _fileName, Encoding.UTF8);
            if (_bmdb == null) return;

            //First entry in vanilla has extra ints
            var firstEntry = false;

            ////////Parse bmdb////////

            //serialization
            _stringPos = 35;

            var numberOfEntries = GetInt();
            // ReSharper disable once NotAccessedVariable
            var pad = GetInt();
            // ReSharper disable once RedundantAssignment
            pad = GetInt();

            //Loop through all entries
            for (var n = 0; n < numberOfEntries; n++)
            {

                //failsafe for out of bounds
                if (_stringPos >= _bmdb.Length - 1)
                {
                    ErrorDb.AddError("Bmdb parsing error", _stringPos.ToString(), _fileName); continue;
                }

                //Create new entry
                var entry = new BattleModel
                {
                    Name = GetString().ToLower()
                };

                //check if there is a blank entry or default vanilla with extra ints
                switch (n)
                {
                    case 0 when entry.Name.Equals("blank"):
                        {
                            for (var i = 0; i < 39; i++)
                            {
                                // ReSharper disable once RedundantAssignment
                                pad = GetInt();
                            }
                            continue;
                        }
                    case 0 when !entry.Name.Equals("blank"):
                        firstEntry = true;
                        break;
                }

                //Get rest of entry
                entry.Scale = GetFloat();
                if (firstEntry) { pad = GetInt(); pad = GetInt(); }
                entry.LodCount = GetInt();
                if (firstEntry) { pad = GetInt(); pad = GetInt(); }
                entry.LodTable = new List<Lod>();
                for (var i = 0; i < entry.LodCount; i++)
                {
                    entry.LodTable.Add(new Lod
                    {
                        Mesh = GetString(),
                        Distance = GetInt()
                    });
                }
                if (firstEntry) { pad = GetInt(); pad = GetInt(); }
                entry.MainTexturesCount = GetInt();
                if (firstEntry) { pad = GetInt(); pad = GetInt(); }
                for (var i = 0; i < entry.MainTexturesCount; i++)
                {
                    var facname = GetString().ToLower();
                    if (FactionDataBase.ContainsKey(facname) || facname == "merc")
                    {
                        entry.MainTextures.Add(new Texture
                        {
                            Faction = facname,
                            TexturePath = GetString(),
                            Normal = GetString(),
                            Sprite = GetString()
                        });
                    }
                    else
                    {
                        var texpad = GetString();
                        texpad = GetString();
                        texpad = GetString();
                    }
                }
                entry.MainTexturesCount = entry.MainTextures.Count;
                entry.AttachTexturesCount = GetInt();
                for (var i = 0; i < entry.AttachTexturesCount; i++)
                {
                    var facname = GetString().ToLower();
                    if (FactionDataBase.ContainsKey(facname) || facname == "merc")
                    {
                        entry.AttachTextures.Add(new Texture
                        {
                            Faction = facname,
                            TexturePath = GetString(),
                            Normal = GetString(),
                            Sprite = GetString()
                        });
                    }
                    else
                    {
                        // ReSharper disable once NotAccessedVariable
                        var texpad = GetString();
                        texpad = GetString();
                        texpad = GetString();
                    }
                }
                entry.AttachTexturesCount = entry.AttachTextures.Count;
                if (firstEntry) { pad = GetInt(); pad = GetInt(); }
                entry.MountTypeCount = GetInt();
                if (firstEntry) { pad = GetInt(); pad = GetInt(); }
                entry.Animations = new List<Animation>();
                for (var i = 0; i < entry.MountTypeCount; i++)
                {
                    entry.Animations.Add(new Animation
                    {
                        MountType = GetString().ToLower(),
                        Primary_skeleton = GetString(),
                        Secondary_skeleton = GetString(),
                        PriWeaponCount = GetInt()
                    });
                    entry.Animations[i].PriWeapons = new List<string>();
                    for (var j = 0; j < entry.Animations[i].PriWeaponCount; j++)
                    {
                        entry.Animations[i].PriWeapons.Add(GetString());
                    }

                    if (entry.Animations[i].PriWeapons.Count > 0)
                    {
                        entry.Animations[i].PriWeaponOne = entry.Animations[i].PriWeapons[0];
                        if (entry.Animations[i].PriWeapons.Count > 1)
                        {
                            entry.Animations[i].PriWeaponTwo = entry.Animations[i].PriWeapons[1];
                        }
                    }
                    entry.Animations[i].SecWeaponCount = GetInt();
                    entry.Animations[i].SecWeapons = new List<string>();
                    for (var j = 0; j < entry.Animations[i].SecWeaponCount; j++)
                    {
                        entry.Animations[i].SecWeapons.Add(GetString());
                    }
                    if (entry.Animations[i].SecWeapons.Count <= 0) continue;
                    entry.Animations[i].SecWeaponOne = entry.Animations[i].SecWeapons[0];
                    if (entry.Animations[i].SecWeapons.Count > 1)
                    {
                        entry.Animations[i].SecWeaponTwo = entry.Animations[i].SecWeapons[1];
                    }
                }
                if (firstEntry) { pad = GetInt(); pad = GetInt(); }
                entry.TorchIndex = GetInt();
                entry.TorchBoneX = GetFloat();
                entry.TorchBoneY = GetFloat();
                entry.TorchBoneZ = GetFloat();
                entry.TorchspriteX = GetFloat();
                entry.TorchspriteY = GetFloat();
                entry.TorchspriteZ = GetFloat();
                if (firstEntry) { pad = GetInt(); pad = GetInt(); }
                firstEntry = false;
                if (BattleModelDataBase.ContainsKey(entry.Name))
                {
                    ErrorDb.AddError($"Duplicate entry {entry.Name} in {_fileName}");
                    Console.WriteLine(@"====================================================================================");
                }
                BattleModelDataBase[entry.Name] = entry;
                Console.WriteLine(entry.Name);
            }
            Console.WriteLine($@"end parse {_fileName}");
        }

        private static string GetString()
        {
            if (_bmdb == null) return "";
            if (_stringPos >= _bmdb.Length - 1) return "";

            AdvancePos();

            var lenght = GetInt();
            _stringPos += GetNextNonWhite();

            if (lenght <= 0) return "";

            var returnString = _bmdb.Substring(_stringPos, lenght).Trim();
            _stringPos += lenght;

            return returnString;
        }

        private static int GetInt()
        {
            if (_bmdb == null) return 0;
            if (_stringPos >= _bmdb.Length - 1) return 0;
            AdvancePos();
            var returnValue = int.Parse(_bmdb.Substring(_stringPos, _nextLenght).Trim());
            _stringPos += _nextLenght - 1;
            return returnValue;
        }

        private static float GetFloat()
        {
            if (_bmdb == null) return 0;
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

        private static void AdvancePos()
        {
            if (_bmdb == null) return;
            if (_stringPos >= _bmdb.Length - 1) return;
            _stringPos += GetNextNonWhite();
            var end = _stringPos + GetNextWhite() + 1;
            _nextLenght = end - _stringPos;
        }

        private static int GetNextNonWhite()
        {
            if (_bmdb == null) return 0;
            if (_stringPos >= _bmdb.Length - 1) return 0;
            var stringLen = _bmdb.Length - 1;
            var len = Math.Min((stringLen) - _stringPos, 100);
            var subline = _bmdb.Substring(_stringPos, len);
            var match = Regex.Match(subline, @"\S");
            return match.Success ? match.Index : _stringPos;
        }

        private static int GetNextWhite()
        {
            if (_bmdb == null) return 0;
            if (_stringPos >= _bmdb.Length - 1) return 0;
            var stringLen = _bmdb.Length - 1;
            var len = Math.Min((stringLen) - _stringPos, 100);
            if (len <= 0) return _stringPos;
            var subline = _bmdb.Substring(_stringPos, len);
            var match = Regex.Match(subline, @"\s");
            return match.Success ? match.Index : _stringPos;
        }

        private static string? BmdbReader(string filepath, string filename, Encoding encoding)
        {
            string text;
            try
            {
                text = File.ReadAllText(ModPath + filepath, encoding);
            }
            catch (Exception e)
            {
                ErrorDb.AddError("Error reading " + filename);
                ErrorDb.AddError(e.Message);
                return null;
            }
            return text;
        }

        public static void checkModelUsage()
        {
            _fileName = "campaign_script.txt";
            Console.WriteLine($@"start parse {_fileName}");


            var lines = FileReader("\\data\\world\\maps\\campaign\\imperial_campaign\\campaign_script.txt", _fileName, Encoding.Default);
            if (lines == null)
            {
                return;
            } //something very wrong if you hit this

            //Reset line counter
            _lineNum = 0;
            //Loop through lines
            foreach (var line in lines)
            {
                //Increase line counter
                _lineNum++;

                //Remove Comments and Faulty lines
                var newline = CleanLine(line);
                if (string.IsNullOrWhiteSpace(newline))
                {
                    continue;
                }


                var rx = new Regex(@"battle_model\s+(\S*)\s");
                var match = rx.Match(newline);
                if (match.Success)
                {
                    var matchstring = match.Groups[1].Value;
                    if (matchstring.Contains(','))
                    {
                        matchstring = matchstring.Split(',')[0];
                    }
                    UsedModels.Add(matchstring.ToLower().Trim());
                }

            }

            //Reset Line Counter
            Console.WriteLine($@"end parse {_fileName}");
            _lineNum = 0;


            _fileName = "descr_strat.txt";
            Console.WriteLine($@"start parse {_fileName}");
            //Reset line counter
            _lineNum = 0;
            lines = FileReader("\\data\\world\\maps\\campaign\\imperial_campaign\\descr_strat.txt", _fileName, Encoding.Default);
            if (lines == null)
            {
                return;
            } //something very wrong if you hit this

            //Reset line counter
            _lineNum = 0;
            //Loop through lines
            foreach (var line in lines)
            {
                //Increase line counter
                _lineNum++;

                //Remove Comments and Faulty lines
                var newline = CleanLine(line);
                if (string.IsNullOrWhiteSpace(newline))
                {
                    continue;
                }

                var rx = new Regex(@"battle_model\s+(\S*)\s");
                var match = rx.Match(newline);
                if (match.Success)
                {
                    var matchstring = match.Groups[1].Value;
                    if (matchstring.Contains(','))
                    {
                        matchstring = matchstring.Split(',')[0];
                    }
                    UsedModels.Add(matchstring.ToLower().Trim());
                }

            }

            //Reset Line Counter
            Console.WriteLine($@"end parse {_fileName}");
            _lineNum = 0;
            UsedModels.Add("dunforgoil_warlords");
            UsedModels.Add("dun_elite_horse");
            UsedModels.Add("frekkalingir");
            UsedModels.Add("dunland_banner");
            PrintFinal();
        }
    }
}