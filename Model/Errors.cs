﻿using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.Notifications;

namespace ModdingTool;

public class Errors
{
    private List<string> ErrorList { get; set; }

    public Errors()
    {
        ErrorList = new List<string>();
    }

    public void AddError(string error)
    {
        if (!Globals.IsParsing)
            new ToastContentBuilder().AddText("Error").AddText(error).Show();
        ErrorList.Add(error);
        Console.WriteLine(error);
        Console.WriteLine(@"====================================================================================");
    }

    public void AddError(string error, string line, string file)
    {
        ErrorList.Add($@" {error} at line: {line} in file: {file}");
        Console.WriteLine($@" {error} at line: {line} in file: {file}");
        Console.WriteLine(@"====================================================================================");
    }

    public void ClearErrors()
    {
        ErrorList.Clear();
    }

    public List<string> GetErrors()
    {
        return ErrorList;
    }
}