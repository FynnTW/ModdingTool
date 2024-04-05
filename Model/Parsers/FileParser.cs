using System;
using System.Collections.Generic;
using System.Text;

namespace ModdingTool;

public abstract class FileParser
{
    protected static int s_lineNum;
    protected static string s_fileName = "";
    
    protected static readonly List<string> EndComments = new();
    protected static readonly List<string> StartComments = new();
    protected static readonly List<string> Identifiers = new();
}
