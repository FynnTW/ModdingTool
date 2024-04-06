using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static ModdingTool.Globals;
using static ModdingTool.ParseHelpers;

namespace ModdingTool;

public abstract class FileParser
{
    protected static int s_lineNum;
    protected static string s_fileName = "";
    
    protected static readonly List<string> EndComments = new();
    protected static readonly List<string> StartComments = new();
    protected static readonly List<string> Identifiers = new();

    public static void BackupFile(string path, string fileName)
    {
        if (!Directory.Exists(ModPath + "\\ModdingToolBackup" + path))
            Directory.CreateDirectory(ModPath + "\\ModdingToolBackup" + path);  
        var backupName = fileName + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt";
        File.Copy(ModPath + path + fileName, ModPath + "/ModdingToolBackup" + path + backupName, true);
    }

}
