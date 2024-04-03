// Ignore Spelling: Modding Edu

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using static ModdingTool.Globals;

namespace ModdingTool
{
    internal class ParseHelpers
    {

        public static List<string> CommentCache = new();
        public static string CommentCacheInLine = "";

        public static string RemoveComment(string line)
        {
            if (!line.Contains(';'))
            {
                return line;
            }
            else if(!line.Contains("ai_unit_value") && !line.Contains("value_per"))
            {
                CommentCacheInLine = line[line.IndexOf(';')..];
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

            //some first edu lines have multiple words in the first part
            if (firstParts[0].Equals("banner"))
            {
                var bannerSplit = firstParts[1].Split(delimitersWhite, 2, StringSplitOptions.RemoveEmptyEntries);
                firstParts[0] = "banner " + bannerSplit[0];
                firstParts[1] = bannerSplit[1];
            }
            if (firstParts[0].Equals("era"))
            {
                var eraSplit = firstParts[1].Split(delimitersWhite, 2, StringSplitOptions.RemoveEmptyEntries);
                firstParts[0] = "era " + eraSplit[0];
                firstParts[1] = eraSplit[1];
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

        private static string[] CurlySplitter(string line)
        {
            char[] deliminators = { '{', '}' };
            var splitLine = line.Split(deliminators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return splitLine;
        }

        public static string[]? LocalTextLineCleaner(string line, int num, string filename)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return null;
            }
            if (line.StartsWith('¬'))
            {
                return null;
            }
            if (!line.Contains('{') || !line.Contains('}'))
            {
                ErrorDb.AddError("Unrecognized content", num.ToString(), filename);
                return null;
            }
            var newLine = line.Trim();
            return CurlySplitter(newLine);
        }

        public static string[]? FileReader(string filePath, string filename, Encoding encoding)
        {
            string[]? lines;
            try
            {
                lines = File.ReadAllLines(ModPath + filePath, encoding);
            }
            catch (Exception e)
            {
                ErrorDb.AddError("Error reading " + filename);
                ErrorDb.AddError(e.Message);
                return null;
            }
            return lines;
        }
        
        public static string[]? FileReaderNonMod(string filePath, string filename, Encoding encoding)
        {
            string[]? lines;
            try
            {
                lines = File.ReadAllLines(filePath, encoding);
            }
            catch (Exception e)
            {
                ErrorDb.AddError("Error reading " + filename);
                ErrorDb.AddError(e.Message);
                return null;
            }
            return lines;
        }

        public static double ParseDouble(string? value)
        {
            return double.Parse(value ?? string.Empty, CultureInfo.InvariantCulture.NumberFormat);
        }

        public static string? CleanLine(string line)
        {
            if (line.StartsWith(';') && !line.Contains("ai_unit_value") && !line.Contains("value_per"))
            {
                CommentCache.Add(line);
                return null;
            }
            var newline = RemoveComment(line);
            return string.IsNullOrWhiteSpace(newline) ? null : newline.Trim();
        }
    }
}