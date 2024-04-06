using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ModdingTool.Globals;
using static ModdingTool.ParseHelpers;

namespace ModdingTool
{
    public class CharacterTypesParser
    {
        //Parsing line number and file name used for error logging
        private static int _lineNum;
        private static string _fileName = "";
        private static string active_faction = "";
        private static int stratmodel_id = 0;

        public static void parseCharacterTypes()
        {
            _fileName = "descr_character.txt";
            Console.WriteLine($@"start parse {_fileName}");

            StartingActionPoints = -1;

            var lines = FileReader("\\data\\descr_character.txt", _fileName, Encoding.Default);
            if (lines == null)
            {
                return;
            } //something very wrong if you hit this

            //Reset line counter
            _lineNum = 0;

            //Initialize Global Mounts Database
            CharacterTypes = new Dictionary<string, CharacterType>();

            //Create new Mount for first entry
            var chartype = new CharacterType();
            var first = true;

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

                //Split line into parts
                var parts = LineSplitterCharacterTypes(newline);
                if (parts.Length < 1)
                {
                    //Should be something wrong with line if you hit this
                    ErrorDb.AddError("Unrecognized content", _lineNum.ToString(), _fileName);
                    continue;
                }

                if (StartingActionPoints == -1 && parts[0].Equals("starting_action_points"))
                {
                    StartingActionPoints = int.Parse(parts[1]);
                    continue;
                }

                //Entry is completed, next entry is starting
                if (parts[0].Equals("type"))
                {
                    //Add new faction if it is not the first time we hit this
                    if (!first)
                    {
                        AddCharacterType(chartype);
                    }

                    first = false;
                    chartype = new CharacterType();
                }

                //Fill out the faction field the line is about
                AssignFields(chartype, parts);
            }

            //Add last faction
            AddCharacterType(chartype);

            if (StartingActionPoints == -1)
            {
                StartingActionPoints = 250;
            }

            //Reset Line Counter
            Console.WriteLine($@"end parse {_fileName}");
            _lineNum = 0;

        }

        private static void AddCharacterType(CharacterType chartype)
        {
            if (CharacterTypes.ContainsKey(chartype.type))
            {
                ErrorDb.AddError("Character Type already exists", _lineNum.ToString(), _fileName);
                return;
            }

            CharacterTypes.Add(chartype.type, chartype);
            Console.WriteLine(chartype.type);
        }

        private static string[] LineSplitterCharacterTypes(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                ErrorDb.AddError("Warning empty line send on", _lineNum.ToString(), _fileName);
                return Array.Empty<string>();
            }

            //split by comma
            char[] delimiters = { ',' };
            var lineParts = line.Split(delimiters,
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            //Make sure there are no empty entries
            if (lineParts.Length == 0)
            {
                return lineParts;
            }

            //split first part by white space (no comma between identifier and value)
            char[] delimitersWhite = { ' ', '\t' };
            var firstParts = lineParts[0].Split(delimitersWhite, 2,
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            //concat first part with rest of line
            lineParts = firstParts.Concat(lineParts[1..]).ToArray();
            return lineParts;
        }

        private static void AssignFields(CharacterType chartype, string[] parts)
        {
            //name of the field
            var identifier = parts[0];

            var value = "";
            //first value, usually all needed
            if (parts.Length > 1)
            {
                value = parts[1];
            }

            char[] delimitersWhite = { ' ', '\t' };

            try
            {
                switch (identifier)
                {
                    case "type":
                        chartype.type = value;
                        break;
                    case "actions":
                        foreach (var action in parts[1..])
                        {
                            chartype.actions.Add(action);
                        }
                        break;
                    case "starting_action_points":
                        chartype.starting_action_points = int.Parse(value);
                        break;
                    case "wage_base":
                        chartype.wage_base = int.Parse(value);
                        break;
                    case "faction":
                        active_faction = value;
                        stratmodel_id = 0;
                        FactionDataBase[active_faction].FactionCharacterTypes.Add(chartype.type, new FactionCharacter { name = chartype.type });
                        break;
                    case "dictionary":
                        FactionDataBase[active_faction].FactionCharacterTypes[chartype.type].dictionary = int.Parse(value);
                        break;
                    case "battle_model":
                        FactionDataBase[active_faction].FactionCharacterTypes[chartype.type].battle_model = value.Trim().ToLower();
                        ModData.BattleModelDb.UsedModels.Add(value.Trim().ToLower());
                        break;
                    case "battle_equip":
                        FactionDataBase[active_faction].FactionCharacterTypes[chartype.type].battle_equip = parts[1..].ToString();
                        break;
                    case "strat_model":
                        FactionDataBase[active_faction].FactionCharacterTypes[chartype.type].models[stratmodel_id] = value;
                        stratmodel_id++;
                        break;
                }

            }
            catch (Exception e)
            {
                ErrorDb.AddError(e.Message, _lineNum.ToString(), _fileName);
                Console.WriteLine(e);
                Console.WriteLine(identifier);
                Console.WriteLine(@"on line: " + _lineNum);
                Console.WriteLine(
                    @"====================================================================================");
            }
        }
    }
}
