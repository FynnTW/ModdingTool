using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ModdingTool
{
    internal class parseHelpers
    {
        static public string removeComment(string line)
        {
            if (!line.Contains(';'))
            {
                return line;
            }
            if (line.StartsWith(';'))
            {
                return "";
            }
            int commentPos = line.IndexOf(';');
            string newline = line[..commentPos];

            return newline;
        }

        static public string[] splitLine(string line) 
        {
            char[] delimiters = { ',' };
            char[] delimitersWhite = { ' ', '\t' };
            string[] lineParts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            string[] firstParts = lineParts[0].Split(delimitersWhite, 2, StringSplitOptions.RemoveEmptyEntries);
            if (firstParts[0].Equals("banner")) 
            {
                string[] bannersplit = firstParts[1].Split(delimitersWhite, 2, StringSplitOptions.RemoveEmptyEntries);
                firstParts[0] = "banner " + bannersplit[0];
                firstParts[1] = bannersplit[1];
            }

            lineParts = firstParts.Concat(lineParts[1..]).ToArray();
            for (int i = 0; i < lineParts.Length;i++)
            {
                lineParts[i] = lineParts[i].Trim();
            }
            return lineParts;
        }
    }
}