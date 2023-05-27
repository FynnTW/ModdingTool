using System;
using System.IO;
using System.Linq;
using System.Text;
using static ModdingTool.Globals;
using System.Windows.Shapes;

namespace ModdingTool
{
    internal class ParseHelpers
    {
        public static string RemoveComment(string line)
        {
            if (!line.Contains(';'))
            {
                return line;
            }
            return line.StartsWith(';') ? "" : line[..line.IndexOf(';')];
        }

        public static string?[]? SplitEduLine(string line)
        {
            if (line.Equals(""))
            {
                return null;
            }
            char[] delimiters = { ',' };
            char[] delimitersWhite = { ' ', '\t' };
            var lineParts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            var firstParts = lineParts[0].Split(delimitersWhite, 2, StringSplitOptions.RemoveEmptyEntries);
            if (firstParts[0].Equals("banner"))
            {
                var bannersplit = firstParts[1].Split(delimitersWhite, 2, StringSplitOptions.RemoveEmptyEntries);
                firstParts[0] = "banner " + bannersplit[0];
                firstParts[1] = bannersplit[1];
            }
            if (firstParts[0].Equals("era"))
            {
                var bannersplit = firstParts[1].Split(delimitersWhite, 2, StringSplitOptions.RemoveEmptyEntries);
                firstParts[0] = "era " + bannersplit[0];
                firstParts[1] = bannersplit[1];
            }

            lineParts = firstParts.Concat(lineParts[1..]).ToArray();
            for (var i = 0; i < lineParts.Length; i++)
            {
                lineParts[i] = lineParts[i].Trim();
            }
            return lineParts;
        }

        public static bool ToBool(string value)
        {
            return value.Equals("yes");
        }

        public static string[] CurlySplitter(string line)
        {
            char[] deliminators = { '{', '}' };
            var splitted = line.Split(deliminators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return splitted;
        }

        public static string[]? FileReader(string filepath, string filename, Encoding encoding)
        {
            string[]? lines;
            try
            {
                lines = File.ReadAllLines(ModPath + filepath, encoding);
            }
            catch (Exception e)
            {
                ErrorDB.AddError("Error reading " + filename);
                ErrorDB.AddError("e");
                return null;
            }
            return lines;
        }

        public static string? CleanLine(string line)
        {
            if (line.StartsWith(';')) { return null; }
            var newline = RemoveComment(line);
            return string.IsNullOrWhiteSpace(newline) ? null : newline.Trim();
        }
    }
}